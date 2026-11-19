using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class Transition : MonoBehaviour
#region Qué hace el script
//Este script es una puerta mágica entre zonas del mapa.
//Cuando el jugador la toca, la pantalla se oscurece, el jugador aparece en otro lugar,
//la cámara se acomoda a la zona nueva y la pantalla se aclara.
#endregion
{
    [SerializeField] private Animator transitionAnimator;
    #region Animador de la transición
    //Es el que maneja la animación de oscurecer y aclarar la pantalla. Se arrastra desde el Inspector.
    //Animator: el tipo, "el que reproduce animaciones".
    #endregion
    [SerializeField] Transform spawn;
    #region Lugar de llegada
    //Es el lugar donde va a aparecer el jugador, como una "X" marcada en el piso. Se arrastra desde el Inspector.
    //No dice private, pero en C# eso cuenta como private.
    #endregion
    [SerializeField] MenuController menu;
    #region Menú del juego
    //Es el script del menú. Se apaga mientras dura la transición, así no podés abrirlo en el medio. Se arrastra desde el Inspector.
    #endregion
    [SerializeField] float fade = 0.5f;
    #region Tiempo de oscurecer
    //Es cuántos segundos se espera mientras la pantalla se oscurece, antes de mover al jugador.
    #endregion
    [SerializeField] CinemachineCamera cam;
    #region La cámara
    //Es la cámara que sigue al jugador. Se arrastra desde el Inspector.
    #endregion
    [SerializeField] PolygonCollider2D mapBoundary;
    #region Límite de la zona nueva
    //Es la "cerca invisible" de la zona a la que llegás: la cámara no puede salirse de ese dibujo. Se arrastra desde el Inspector.
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    #region Algo entró en la puerta
    //Unity llama a esta función cuando algo entra en la zona de la puerta.
    #endregion
    {
        if (collision.TryGetComponent<Player>(out Player player))
        #region ¿Entró el jugador?
        //Pregunta: "¿lo que entró es el jugador?".
        //collision.TryGetComponent<Player>: "intentá sacarle el script Player a lo que entró".
        //out Player player: "si lo encontrás, dejámelo en esta cajita llamada player".
        #endregion
        {
            StartCoroutine(FadeRoutine(player));
            #region Empezar la transición
            //Arranca la rutina de la transición (la de más abajo).
            //StartCoroutine: "empezá esta rutina, que puede ir esperando de a ratitos".
            //FadeRoutine(player): la rutina, y le pasamos el jugador que entró.
            //Es como apretar el botón de "play" en una película en vez de hacer todo de golpe.
            #endregion
        }
    }
    private IEnumerator FadeRoutine(Player player)
    #region La rutina de la transición
    //Es la rutina que hace toda la transición paso a paso.
    //private: nadie de afuera la llama.
    //IEnumerator: es el tipo de las rutinas, o sea, funciones que pueden pausarse con yield y seguir después.
    //FadeRoutine: es su nombre.
    //Player player: hay que decirle qué jugador mover.
    #endregion
    {
        player.enabled = false;
        #region Apagar al jugador
        //Apaga el script del jugador. Con el script apagado, no responde a los controles.
        #endregion
        menu.enabled = false;
        #region Apagar el menú
        //Apaga el menú, para que no se pueda abrir durante la transición.
        #endregion
        if (player.TryGetComponent(out Rigidbody2D rb))
        #region ¿Tiene cuerpo físico?
        //Pregunta: "¿el jugador tiene un Rigidbody2D (su cuerpo físico)?".
        #endregion
        {
            rb.linearVelocity = Vector2.zero;
            #region Frenar en seco
            //Pone la velocidad del jugador en cero, así no sigue deslizándose.
            //rb.linearVelocity: qué tan rápido va. Vector2.zero: la flechita (0, 0), o sea, quieto.
            #endregion
        }
        transitionAnimator.SetTrigger("StartTransition");
        #region Disparar "empezar"
        //Le dice al animador que dispare el gatillo (trigger) llamado "StartTransition".
        //SetTrigger: "tocá este timbre". El texto entre comillas tiene que escribirse igual que en el Animator.
        #endregion
        transitionAnimator.SetTrigger("EndTransition");
        #region Disparar "terminar"
        //Dispara otro gatillo, "EndTransition", que se usa para aclarar la pantalla.
        //Ojo: se tocan los dos timbres seguidos. Cómo se ve depende de cómo armaste el Animator.
        #endregion
        yield return new WaitForSeconds(fade);
        #region Esperar un ratito
        //Pausa la rutina durante "fade" segundos y después sigue en la línea de abajo.
        //yield return: "pausá acá y devolvé el control a Unity".
        //new WaitForSeconds(fade): "esperá tantos segundos". Es como contar hasta 0,5 antes de seguir.
        #endregion
        Vector3 oldPosition = player.transform.position;
        #region Anotar lugar viejo
        //Anota dónde estaba parado el jugador antes de moverlo.
        #endregion
        player.transform.position = spawn.position;
        #region Teletransportar al jugador
        //Pone al jugador en el lugar de llegada.
        #endregion
        CinemachineConfiner2D confiner = cam.GetComponent<CinemachineConfiner2D>();
        #region Agarrar el limitador
        //Le saca a la cámara su componente CinemachineConfiner2D, que es el que la mantiene dentro de una zona.
        #endregion
        float dampingOriginal = confiner.Damping;
        #region Guardar la suavidad
        //Anota cuánta suavidad tiene la cámara al moverse (Damping), para devolvérsela después.
        //float: número con coma.
        #endregion
        float slowingOriginal = confiner.SlowingDistance;
        #region Guardar la distancia de freno
        //Anota a qué distancia del borde la cámara empieza a frenar (SlowingDistance), para devolverla después.
        #endregion
        confiner.Damping = 0f;
        #region Quitar la suavidad
        //Pone la suavidad en 0, así la cámara salta de golpe y no viaja lento hasta el lugar nuevo.
        #endregion
        confiner.SlowingDistance = 0f;
        #region Quitar el freno
        //Pone la distancia de freno en 0, por el mismo motivo.
        #endregion
        confiner.BoundingShape2D = mapBoundary;
        #region Cambiar la cerca
        //Le dice a la cámara que ahora su cerca invisible es la de la zona nueva (mapBoundary).
        //BoundingShape2D: la forma que limita a la cámara.
        #endregion
        //nuevo:
        GameState.currentRoom = mapBoundary.name; //anotar la sala
        confiner.InvalidateBoundingShapeCache();
        #region Refrescar la cerca
        //Le avisa a la cámara que la cerca cambió y que vuelva a calcularla.
        //Es como decirle: "mirá otra vez, que cambié el dibujo".
        #endregion
        cam.OnTargetObjectWarped(player.transform, spawn.position - oldPosition);
        #region Avisar del teletransporte
        //Le avisa a la cámara que el jugador "se teletransportó", para que lo siga de golpe.
        //OnTargetObjectWarped: "el objeto que seguís se movió de golpe".
        //player.transform: a quién sigue. spawn.position - oldPosition: cuánto se movió (lugar nuevo menos lugar viejo).
        #endregion
        yield return null;
        #region Esperar un cuadro
        //Pausa la rutina un solo cuadro del juego (una fracción de segundo) y sigue.
        //yield return null: "esperá hasta el próximo cuadro".
        //Así la cámara tiene tiempo de acomodarse antes de devolverle la suavidad.
        #endregion
        confiner.Damping = dampingOriginal;
        #region Devolver la suavidad
        //Vuelve a poner la suavidad que se había anotado.
        #endregion
        confiner.SlowingDistance = slowingOriginal;
        #region Devolver el freno
        //Vuelve a poner la distancia de freno que se había anotado.
        #endregion
        player.enabled = true;
        #region Prender al jugador
        //Vuelve a prender el script del jugador, y puede moverse otra vez.
        #endregion
        menu.enabled = true;
        #region Prender el menú
        //Vuelve a prender el menú.
        #endregion
    }
}
