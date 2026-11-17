using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatResult : MonoBehaviour
{
    [SerializeField] private string prototypeScene = "PrototypeScene";
    [SerializeField] private float fleeImmunitySeconds = 3f;
    [SerializeField] private Player player;
    [SerializeField] private int fleeDamage = 5;

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
        GameState.guardPositions.Clear();
        GameState.guardWaypoints.Clear();

        if (GameState.hasSavePoint)
        {
            GameState.playerPosition = GameState.savePointPosition;
            GameState.hasSavedPosition = true;
            GameState.defeatedGuards = new List<int>(GameState.savedDefeatedGuards);
        }

        SceneManager.LoadScene(prototypeScene);
    }
    public void Flee()
    {
        player.life -= fleeDamage;
        GameState.playerLife = player.life;

        if (player.life <= 0)
        {
            Lose();
            return;
        }
        GameState.encounterGuards.Clear();
        GameState.encounterImmuneUntil = Time.time + fleeImmunitySeconds;
        SceneManager.LoadScene(prototypeScene);
    }
}

