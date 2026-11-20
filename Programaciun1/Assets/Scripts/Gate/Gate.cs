using UnityEngine;

public class Gate : MonoBehaviour
#region Qué hace el script
//Este script es una reja que se abre sola cuando se aprietan suficientes botones.
//Cuenta cuántos botones están apretados y, cuando llega al número que necesita, apaga su collider y la reja deja pasar.
#endregion
{
    [SerializeField] private AudioClip openSound;
    //El sonido de cuando la reja se abre.
    [SerializeField] int buttonsNeeded = 2;
    #region Botones que hacen falta
    //Es cuántos botones hay que apretar para abrir la reja.
    #endregion
    [SerializeField] Collider2D gateCollider;
    #region El cuerpo de la reja
    //Es el collider de la reja, o sea, la parte que choca y bloquea el paso. Se arrastra desde el Inspector.
    #endregion
    int buttonsPressed;
    #region Botones apretados ahora
    //Es cuántos botones hay apretados en este momento. Es como contar con los dedos.
    #endregion
    bool opened;
    #region ¿Está abierta?
    //Es un cartelito de sí o no: "¿la reja ya se abrió?".
    #endregion
    public bool IsOpen => opened;
    #region Preguntar si está abierta
    //Es una ventanita para que otros scripts puedan mirar si la reja está abierta, pero no cambiarlo.
    #endregion
    public void ButtonPressed()
    #region Un botón se apretó
    //Es la función que un botón llama cuando lo pisan, para avisarle a la reja.
    #endregion
    {
        if (opened) return;
        #region Si ya está abierta, nada
        //Pregunta: "¿la reja ya está abierta?". Si sí, se termina la función acá y no hace nada más.
        #endregion
        buttonsPressed++;
        #region Sumar un botón
        //Suma uno al contador de botones apretados. Es como levantar un dedo más.
        #endregion
        Debug.Log($"Botones: {buttonsPressed}/{buttonsNeeded}");
        if (buttonsPressed >= buttonsNeeded)
        #region ¿Ya son suficientes?
        //Pregunta: "¿ya se apretaron todos los botones que hacían falta?".
        #endregion
        {
            opened = true;
            #region Marcar como abierta
            //Cambia el cartelito a sí: la reja ya se abrió.
            //Desde ahora, la línea de arriba (if (opened) return;) frena todo, así que la reja no se cierra más.
            #endregion
            gateCollider.enabled = false;
            #region Abrir el paso
            //Apaga el cuerpo de la reja. Sin collider, ya no choca con nada y se puede pasar. Así se "abre".
            //Ojo: la reja sigue estando a la vista. Solo deja de bloquear.
            #endregion
            SfxPlayer.Play(openSound);   // NUEVO
        }
    }
    public void ButtonReleased()
    #region Un botón se soltó
    //Es la función que un botón llama cuando lo sueltan, para avisarle a la reja.
    #endregion
    {
        if (opened) return;
        #region Si ya está abierta, nada
        //Pregunta: "¿la reja ya está abierta?". Si sí, se termina la función acá.
        //Así, una vez abierta, soltar un botón no la vuelve a cerrar.
        #endregion
        buttonsPressed--;
        #region Restar un botón
        //Resta uno al contador de botones apretados. Es como bajar un dedo.
        #endregion
    }
}
