using UnityEngine;

public class TurnManager : MonoBehaviour
//es el árbitro del combate: decide de quién es el turno, prende y apaga lo que corresponde,
//y cuando se acaba el tiempo del enemigo le devuelve el turno al jugador.
{
    public CombatMenu playerTurn; 
    //Es el menú de combate del jugador (el que tiene los botones Attack, Item, etc.). Le pusimos
    //playerTurn porque, cuando ese menú está prendido, es el turno del jugador.

    public Player enemyTurn;
    //Es el Player de la escena de combate (el corazoncito que esquiva las balas). Se llama enemyTurn porque,
    //cuando el Player está prendido, es el turno del enemigo, que es cuando el jugador se mueve para esquivar.
    //El nombre confunde, pero no es otro personaje: es el mismo Player.

    public Transform combatBoxSpawner;
    //Es el lugar donde aparece el Player al empezar el turno del enemigo, algo como una “X”
    //marcada en el piso. Transform es “la posición, rotación y tamaño de un objeto”.

    [SerializeField] private float timeEnemyAttack = 10;
    //Es cuánto dura el turno del enemigo, en segundos. Empieza en 10.

    [SerializeField] private float timeRemaining;
    //Es el relojito de arena: cuántos segundos le quedan al turno del enemigo. Empieza vacío (en 0)
    //y se llena cuando empieza el turno. Está en el Inspector para que puedas espiar cuánto queda mientras probás.

    public EnemyCombatAttack enemyAttack;
    //Es el script que dispara las balas. TurnManager lo prende y lo apaga.

    //Nuevo
    [SerializeField] private CombatResult combatResult;
    //Para avisar que el jugador perdió. Se arrastra desde el Inspector.

    private void Start()
    {
        StartPlayerTurn();
        //Llama a la función que empieza el turno del jugador. Así, el combate siempre arranca con el jugador.
    }
    private void Update()
    {
        if (enemyTurn.enabled)
            //Pregunta: “¿el Player está prendido?”, o sea, “¿es el turno del enemigo?”.
        {
            //nuevo
            if (enemyTurn.life <= 0)
            //Pregunta: "¿al jugador se le acabó la vida?"
            {
                combatResult.Lose();
                return;
                //Corta el Update para que no siga con el reloj ni llame a Lose dos veces.
            }

            timeRemaining -= Time.deltaTime;
            //Gasta un poquito del relojito de arena.
            //Sumando muchos cuadritos, el reloj baja 1 por cada segundo real.

            if (timeRemaining <= 0)
            //Pregunta: “¿se acabó el tiempo?”. <= es “menor o igual que”. Si el reloj llegó a 0 o menos, se acabó.
            {
                StartPlayerTurn();
                //Si se acabó el tiempo, vuelve a empezar el turno del jugador.
            }
        }
    }
    public void StartPlayerTurn()
    //Crea la función StartPlayerTurn (“empezar el turno del jugador”).
    {
        enemyAttack.ClearBullets();
        //Barre todas las balas que quedaron en pantalla

        enemyAttack.enabled = false;
        //Apaga el disparador del enemigo. enabled = false es “apagá el interruptor”. Así deja de disparar.

        playerTurn.enabled = true;
        //Prende el menú de combate, y el jugador puede elegir su acción.

        enemyTurn.enabled = false;
        //Apaga el Player. Con el script apagado, no se mueve, porque su Update y su FixedUpdate dejan de funcionar.
        //Además deja de contar el reloj en Update, porque enemyTurn.enabled pasa a false.
    }

    public void StartEnemyTurn()
    //Crea la función StartEnemyTurn (“empezar el turno del enemigo”). La llama CombatMenu cuando el jugador ataca o usa una poción.
    {
        enemyAttack.enabled = true;
        //Prende el disparador. Ahora el enemigo empieza a tirar balas.

        timeRemaining = timeEnemyAttack;
        //Llena el relojito de arena: le copia al reloj la duración del turno (10 segundos).

        playerTurn.enabled = false;
        //Apaga el menú de combate, y el jugador ya no puede elegir.

        enemyTurn.enabled = true;
        //Prende el Player, y ahora sí se puede mover para esquivar. También hace que Update empiece a gastar el reloj.

        enemyTurn.transform.position = combatBoxSpawner.position;
        //Pone al Player en su lugar de inicio
    }
}
