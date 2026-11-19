using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
//guarda la posicion del jugador en el overworld y los enemigos que fueron derrotados
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        //Si colisiona con el player
        {
            GameState.savePointPosition = transform.position;
            //se guarda la posicion del save point en GameState (codigo que no se reinicia durante cambios de escena)
            GameState.hasSavePoint = true;
            //afirma que hay un savepoint activo, se lo pasa a GameState
            GameState.savedDefeatedGuards = new List<int>(GameState.defeatedGuards);
            //guarda los enemigos derrotados creando una lista nueva con la cantidad de guardias
            //derrotados(sin guardar aun) que tambien se encuentra en GameState

            //nuevo
            GameState.savedRoom = GameState.currentRoom; //sacar la foto
        }
    }
}
