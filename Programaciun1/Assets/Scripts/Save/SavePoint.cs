using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
//guarda la posicion del jugador y los enemigos derrotados cuando apretas Z cerca.
//El Dialogue muestra "guardaste la partida" y el juego queda en pausa hasta que cierres el texto.
{
    [SerializeField] private Dialogue dialogue;        // el dialogo que dice "guardaste la partida"
    [SerializeField] private AudioClip saveSound;      // un sonido opcional al guardar

    private void Start()
    {
        if (dialogue != null)
        {
            dialogue.onStarted += Save;                // anotate para escuchar el timbre
        }
    }

    private void Save()
    {
        GameState.savePointPosition = transform.position;
        GameState.hasSavePoint = true;
        GameState.savedDefeatedGuards = new List<int>(GameState.defeatedGuards);
        GameState.savedRoom = GameState.currentRoom;
        SfxPlayer.Play(saveSound);
        Debug.Log("partida guardada");
    }
}
