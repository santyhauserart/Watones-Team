using UnityEngine;

public class GateButton : MonoBehaviour
#region Qué hace el script
//Este script es un botón en el piso que se aprieta al pisarlo.
//Cuando alguien (el jugador o un NPC) lo pisa, le avisa a la reja (Gate), y cuando se baja, le avisa que se soltó.
//Si hay dos personas arriba del botón, cuenta como un solo botón apretado.
#endregion
{
    [SerializeField] Gate gate;
    #region La reja que abre
    //Es la reja a la que este botón le avisa. Se arrastra desde el Inspector.
    #endregion

    [SerializeField] SpriteRenderer visual;                     // NUEVO: el sprite que se ve (se arrastra en el Inspector)
    [SerializeField] Color releasedColor = Color.white;         // NUEVO: color con nadie encima
    [SerializeField] Color pressedColor = Color.yellow;         // NUEVO: color apretado

    int occupants;
    #region Cuántos están encima
    //Es cuántas personas están paradas arriba del botón ahora.
    #endregion
    public bool GateIsOpen => gate.IsOpen;
    #region Preguntar si la reja abrió
    //Es una ventanita para que otros scripts puedan mirar si la reja ya está abierta, a través de este botón.
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    #region Algo entró al botón
    //Unity llama a esta función cuando algo entra en la zona del botón.
    #endregion
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("NPC")) return;
        #region ¿Es jugador o NPC?
        //Pregunta: "¿lo que entró NO es el jugador y NO es un NPC?". Si es así, se termina la función acá y no hace nada.
        #endregion
        Debug.Log($"{name} pisado por {collision.name}");
        occupants++;
        #region Sumar una persona
        //Suma uno al contador de personas encima del botón.
        #endregion
        //if (occupants == 1) gate.ButtonPressed();
        #region ¿Es el primero? Avisar
        //Pregunta: "¿hay exactamente una persona encima?". Si sí, le avisa a la reja que el botón se apretó.
        #endregion
        if (occupants == 1)                                     // CAMBIADO: ahora con llaves, porque hace dos cosas
        {                                                       // NUEVO
            gate.ButtonPressed();                               // IGUAL (movida adentro de las llaves)
            SetPressed(true);                                   // NUEVO: pinta el botón de apretado
        }                                                       // NUEVO
    }
    void Awake()                                                // NUEVO (método entero)
    {                                                           // NUEVO
        SetPressed(false);                                      // NUEVO: arranca con el color de "suelto"
    }                                                           // NUEVO

    void SetPressed(bool isPressed)                             // NUEVO (método entero)
    {                                                           // NUEVO
        if (visual == null) return;                             // NUEVO: si falta arrastrar el sprite, no hace nada ni da error
        visual.color = isPressed ? pressedColor : releasedColor;// NUEVO: apretado = un color, suelto = el otro
    }                                                           // NUEVO
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("NPC")) return;
        #region ¿Es jugador o NPC?
        //Pregunta: "¿lo que salió NO es el jugador y NO es un NPC?". Si es así, se termina la función acá.
        #endregion
        occupants--;
        #region Restar una persona
        //Resta uno al contador de personas encima del botón.
        #endregion
        //if (occupants == 0) gate.ButtonReleased();
        #region ¿Quedó vacío? Avisar
        //Pregunta: "¿ya no queda nadie encima?". Si sí, le avisa a la reja que el botón se soltó.
        #endregion
        if (occupants == 0)                                     // CAMBIADO: ahora con llaves
        {                                                       // NUEVO
            gate.ButtonReleased();                              // IGUAL (movida adentro de las llaves)
            SetPressed(false);                                  // NUEVO: vuelve al color de suelto
        }
    }
}
