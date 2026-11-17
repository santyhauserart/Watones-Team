using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GameState.savePointPosition = transform.position;
            GameState.hasSavePoint = true;
            GameState.savedDefeatedGuards = new List<int>(GameState.defeatedGuards);
        }
    }
}
