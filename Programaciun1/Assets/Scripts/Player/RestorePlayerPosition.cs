using UnityEngine;

public class RestorePlayerPosition : MonoBehaviour
{
    private void Start()
    {
        if (GameState.hasSavedPosition)
        {
            transform.position = GameState.playerPosition;
            GameState.hasSavedPosition = false;
        }
    }
}

