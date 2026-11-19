using UnityEngine;

public class RestorePlayerPosition : MonoBehaviour
#region Qué hace el script
//Este script le devuelve al jugador el lugar donde estaba parado cuando vuelve de un combate.
//Es como un cartelito que dice "vos estabas acá", y al empezar la escena te lleva de vuelta a ese punto.
#endregion
{
    private void Start()
    {
        if (GameState.hasSavedPosition)
        #region ¿Hay posición anotada?
        //Pregunta: "¿en el pizarrón (GameState) hay una posición anotada para usar?".
        #endregion
        {
            transform.position = GameState.playerPosition;
            #region Mover al lugar anotado
            //Lleva a este objeto al lugar que estaba anotado.
            #endregion
            GameState.hasSavedPosition = false;
            #region Borrar el aviso
            //Cambia el cartelito a "no", porque la posición ya se usó.
            //Sirve para que, si abrís otra escena después, no te vuelva a llevar a ese lugar viejo.
            #endregion
        }
    }
}

