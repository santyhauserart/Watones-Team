using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyPatrol : MonoBehaviour
#region Qué hace el script
//Este script es el guardia del mapa: camina de parada en parada, persigue al jugador si se le acerca
//y, si lo alcanza, lo lleva al combate anotando antes todo lo que hay que recordar.
#endregion
{
    [SerializeField] private Player player;
    #region A quién persigue
    #endregion
    [SerializeField] private Transform[] waypoints;
    #region Las paradas de la ruta
    //Son los puntos de la ruta del guardia, como las paradas de un colectivo. Se arrastran desde el Inspector, en orden.
    //[]: significa "una lista de varios".
    #endregion
    [SerializeField] private int currentIndex = 0;
    #region A qué parada va
    //Es el número de la parada a la que va ahora. Empieza en 0, que es la primera de la lista.
    #endregion
    [SerializeField] private float speed = 3f;
    #region Velocidad de caminar
    //Es qué tan rápido camina el guardia.
    #endregion
    [SerializeField] private float stopDistance = 0.1f;
    #region Distancia para "llegué"
    //Es qué tan cerca de la parada tiene que estar para decir "llegué".
    #endregion
    [SerializeField] private float waitRate = 1f;
    #region Cuánto espera en cada parada
    //Es cuántos segundos se queda esperando en cada parada antes de seguir.
    #endregion
    private float waitTime;
    #region Hasta qué hora espera
    //Es la hora hasta la que tiene que esperar. Es como un reloj despertador.
    #endregion
    private bool isChasing = false;
    #region ¿Está persiguiendo?
    //Es un cartelito de sí o no: "¿estoy persiguiendo al jugador?".
    #endregion
    [SerializeField] private float encounterDistance = 1f;
    #region Distancia para agarrarte
    //Es a qué distancia el guardia te agarra y empieza el combate. Más chico, más cerca tiene que estar.
    #endregion
    private bool inEncounter = false;
    #region ¿Ya te agarró?
    //Es un cartelito de sí o no: "¿ya te agarré?". Evita que te agarre muchas veces seguidas.
    #endregion
    [SerializeField] private int guardId;
    #region Número del guardia
    //Es el número de documento del guardia. Cada guardia tiene que tener uno distinto. Se pone en el Inspector.
    #endregion
    [SerializeField] private string combatScene = "CombatScene";
    #region Nombre de la escena de combate
    //Es el nombre de la escena a la que te lleva cuando te agarra.
    //string: un texto. = "CombatScene": empieza con ese nombre.
    //Tiene que estar escrito igual que la escena, y la escena tiene que estar en Build Profiles (Build Settings).
    #endregion

    private void Start()
    {
        Debug.Log(guardId + " / " + GameState.defeatedGuards.Contains(guardId));
        #region Mensaje para probar
        //guardId: el número del guardia. " / ": un texto fijo.
        //GameState.defeatedGuards.Contains(guardId): pregunta si este guardia está en la libretita de derrotados. Da True o False.
        //Ejemplo: "2 / False". Esta línea usa el script GameState, el pizarrón del juego.
        #endregion
        if (GameState.defeatedGuards.Contains(guardId))
        #region ¿Ya lo derrotaron?
        //Pregunta: "¿a este guardia ya lo derrotaron?".
        #endregion
        {
            Destroy(gameObject);
            return;
        }
        if (GameState.guardPositions.ContainsKey(guardId))
        #region ¿Hay posición anotada?
        //Pregunta: "¿hay una posición anotada para este guardia?".
        //GameState.guardPositions: el cuaderno de posiciones de los guardias, del script GameState.
        //.ContainsKey(guardId): "¿existe una página con el número de este guardia?".
        //Hay página si huiste de un combate con él.
        #endregion
        {
            transform.position = GameState.guardPositions[guardId];
            #region Volver a su lugar
            //Pone al guardia donde estaba parado cuando lo dejaste.
            #endregion
            currentIndex = GameState.guardWaypoints[guardId];
            #region Recordar su parada
            //Le devuelve la parada a la que iba, para que siga su camino y no empiece desde la primera.
            //GameState.guardWaypoints: el cuaderno de a qué parada iba cada guardia, del script GameState.
            #endregion
        }
    }
    private void Update()
    {
        if (isChasing)
        #region ¿Está persiguiendo?
        //Pregunta: "¿estoy persiguiendo al jugador?". Si sí, hace todo lo de adentro.
        #endregion
        {
            if (Time.time < GameState.encounterImmuneUntil) return;
            #region ¿Sigue dormido?
            //Pregunta: "¿sigo dormido por la huida del jugador?". Si sí, corta acá y no hace nada más en este cuadro.
            //Time.time: la hora de ahora. <: significa "menor que".
            //GameState.encounterImmuneUntil: la hora hasta la que duerme, anotada en GameState por CombatResult cuando huís.
            //return: "cortá acá y me voy". En el cuadro siguiente vuelve a preguntar.
            //Sin llaves: el if solo manda a ese return.
            #endregion
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;
            #region Flechita hacia el jugador
            //Calcula la flechita que va del guardia al jugador, para saber hacia dónde caminar.
            #endregion
            if (!inEncounter)
            #region ¿Todavía no te agarró?
            //Pregunta: "¿todavía NO te agarré?".
            #endregion
            {
                transform.Translate(playerDirection * speed * Time.deltaTime, Space.World);
                #region Caminar hacia el jugador
                //Mueve al guardia un pasito hacia el jugador.
                #endregion
            }
            float distancePlayer = Vector2.Distance(transform.position, player.transform.position);
            #region Medir cuánto lo separa
            //Mide cuánto lo separa del jugador. Es como medir con una regla.
            #endregion
            if (distancePlayer < encounterDistance && !inEncounter && Time.time >= GameState.encounterImmuneUntil)
            #region ¿Te alcanzó?
            //Pregunta: "¿estás lo bastante cerca, todavía no te agarré, y ya no estoy dormido?". Tienen que cumplirse las tres.
            #endregion
            {
                inEncounter = true;
                #region Marcar que te agarró
                //Cambia el cartelito a sí: ya te agarró. Así no te agarra otra vez en el cuadro siguiente.
                #endregion
                player.SetCanMove(false);
                #region Congelar al jugador
                //Congela al jugador para que no se escape mientras carga el combate.
                #endregion
                if (!GameState.encounterGuards.Contains(guardId))
                #region ¿Falta anotarlo?
                //Pregunta: "¿este guardia NO está ya anotado en la libretita del combate?".
                #endregion
                {
                    GameState.encounterGuards.Add(guardId);
                    #region Anotar al guardia
                    //Anota al guardia en la libretita del combate.
                    //Si ganás, CombatResult usa esa libretita para saber a cuál derrotar.
                    #endregion
                }
                GameState.playerPosition = player.transform.position;
                #region Anotar dónde estás
                //Anota dónde estaba parado el jugador, para ponerlo ahí al volver.
                #endregion
                GameState.hasSavedPosition = true;
                #region Avisar que hay posición
                //Cambia el cartelito del pizarrón a sí: "hay una posición anotada para usar".
                #endregion
                GameState.guardPositions[guardId] = transform.position;
                #region Anotar dónde está el guardia
                //Anota en el cuaderno dónde estaba parado este guardia, en la página con su número.
                #endregion
                GameState.guardWaypoints[guardId] = currentIndex;
                #region Anotar su parada
                //Anota a qué parada iba este guardia, en la página con su número.
                #endregion
                SceneFader.Load(combatScene);
                SceneManager.LoadScene(combatScene);
                #region Ir al combate
                //Te lleva a la escena del combate.
                #endregion
            }
        }
        else
        {
            Transform target = waypoints[currentIndex];
            #region Elegir la parada
            //Elige la parada a la que va y la guarda en una cajita llamada target ("objetivo").
            #endregion
            Vector2 waypointDirection = (target.position - transform.position).normalized;
            #region Flechita hacia la parada
            //Calcula la flechita que va del guardia a la parada.
            //Funciona igual que la de perseguir, pero apuntando a la parada y no al jugador.
            #endregion
            if (Time.time >= waitTime)
            #region ¿Terminó de esperar?
            //Pregunta: "¿ya terminé de esperar?".
            #endregion
            {
                transform.Translate(waypointDirection * speed * Time.deltaTime, Space.World);
                #region Caminar hacia la parada
                //Mueve al guardia un pasito hacia la parada. Funciona igual que al perseguir.
                #endregion
            }
            float distance = Vector2.Distance(transform.position, target.position);
            #region Medir cuánto falta
            //Mide cuánto le falta para llegar a la parada.
            #endregion
            if (distance < stopDistance)
            #region ¿Llegó a la parada?
            //Pregunta: "¿ya llegué a la parada?".
            #endregion
            {
                waitTime = Time.time + waitRate;
                #region Ponerse a esperar
                //Anota la hora hasta la que se queda esperando: la hora de ahora más waitRate segundos.
                #endregion
                currentIndex = (currentIndex + 1) % waypoints.Length;
                #region Elegir la próxima parada
                //Pasa a la parada siguiente de la lista, y al llegar a la última vuelve a la primera.
                #endregion
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            isChasing = true;
            #region Empezar a perseguir
            //Cambia el cartelito a sí: ahora el guardia persigue al jugador.
            #endregion
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            isChasing = false;
            #region Dejar de perseguir
            //Cambia el cartelito a no: el guardia deja de perseguir y vuelve a patrullar.
            #endregion
        }
    }
}
