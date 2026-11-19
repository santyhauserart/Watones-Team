using UnityEngine;

public class NPC2 : MonoBehaviour
#region Qué hace el script
//Este script es un compañero que sigue al jugador, como un patito yendo detrás de su mamá.
//No va hacia el jugador mismo: va hacia un punto que está justo detrás de él (targetPoint, que calcula el script Player).
//Cuando llega a ese punto, se queda quieto. Si el jugador se mueve, el NPC vuelve a caminar.
#endregion
{
    [SerializeField] private Player player;
    #region A quién sigue
    //Es el jugador al que sigue. Se arrastra desde el Inspector.
    #endregion
    [SerializeField] private float speed = 5f;
    #region Velocidad de caminar
    //Es qué tan rápido camina el NPC. Se puede cambiar en el Inspector.
    #endregion
    [SerializeField] private float stopDistance = 0.1f;
    #region Distancia para frenar
    //Es qué tan cerca del punto tiene que estar para decir "llegué" y quedarse quieto.
    #endregion
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.targetPoint);
        #region Medir cuánto falta
        //Mide cuánto lo separa del punto de atrás del jugador. Es como medir con una regla.
        //float distance: una cajita para un número con coma, llamada distance.
        //Vector2.Distance(a, b): la distancia entre dos puntos.
        //transform.position: dónde está el NPC. player.targetPoint: el punto detrás del jugador.
        //Un detalle: transform.position tiene tres números (x, y, z) y Unity lo achica solo a dos (x, y), porque el juego es en 2D.
        #endregion
        Vector2 playerDirection = ((Vector3)player.targetPoint - transform.position).normalized;
        #region Flechita hacia el punto
        //Calcula la flechita que va del NPC al punto de atrás del jugador, para saber hacia dónde caminar.
        //Vector2: una flechita con dos números (izquierda/derecha y arriba/abajo).
        //(Vector3)player.targetPoint: convierte el punto a tres números (x, y, z), para poder restarlo con transform.position, que tiene tres.
        //- transform.position: la resta de los dos lugares dice hacia dónde hay que ir.
        //.normalized: hace la flechita de tamaño 1, así solo dice la dirección y no la distancia.
        //Ojo con el nombre: playerDirection apunta al punto de atrás del jugador, no al jugador mismo.
        #endregion
        if (distance <= stopDistance)
        #region ¿Ya llegó?
        //Pregunta: "¿estoy lo bastante cerca del punto como para frenar?".
        #endregion
        {
            return;
            #region Quedarse quieto
            //Corta la función acá: lo que está abajo no se ejecuta, así que el NPC no camina en este cuadro.
            //return: "terminé, me voy". Sin nada al lado, es solo "cortá".
            //Se vuelve a preguntar en cada cuadro, así que si el jugador se aleja, el NPC arranca de nuevo.
            #endregion
        }
        transform.Translate(playerDirection * speed * Time.deltaTime, Space.World);
        #region Dar un pasito
        //Mueve al NPC un pasito hacia el punto.
        //transform.Translate(...): "mové este objeto".
        //playerDirection: hacia dónde. * speed: qué tan rápido.
        //* Time.deltaTime: lo que tardó el último cuadro del juego. Así camina igual de rápido en una compu rápida o lenta.
        //Space.World: "movete según el mapa, sin importar para dónde esté girado el NPC".
        #endregion
    }
}
