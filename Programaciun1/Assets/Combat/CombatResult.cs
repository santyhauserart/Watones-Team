using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatResult : MonoBehaviour
{
    [SerializeField] private string prototypeScene = "PrototypeScene";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lose();
        }
    }
    public void Win()
    {
        foreach (int id in GameState.encounterGuards)
        {
            if (!GameState.defeatedGuards.Contains(id))
            {
                GameState.defeatedGuards.Add(id);
            }
        }
        GameState.encounterGuards.Clear();
        SceneManager.LoadScene(prototypeScene);
    }
    public void Lose()
    {
        GameState.encounterGuards.Clear();

        if (GameState.hasSavePoint)
        {
            GameState.playerPosition = GameState.savePointPosition;
            GameState.hasSavedPosition = true;
            GameState.defeatedGuards = new List<int>(GameState.savedDefeatedGuards);
        }

        SceneManager.LoadScene(prototypeScene);
    }
}

