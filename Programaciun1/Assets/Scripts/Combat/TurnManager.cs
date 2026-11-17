using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public CombatMenu playerTurn;
    public Player enemyTurn;
    public Transform combatBoxSpawner;
    [SerializeField] private float timeEnemyAttack = 10;
    [SerializeField] private float timeRemaining;
    public EnemyCombatAttack enemyAttack;
    private void Start()
    {
        StartPlayerTurn();
    }
    private void Update()
    {
        if (enemyTurn.enabled)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                StartPlayerTurn();
            }
        }
    }
    public void StartPlayerTurn()
    {
        enemyAttack.ClearBullets();
        enemyAttack.enabled = false;
        playerTurn.enabled = true;
        enemyTurn.enabled = false;
    }

    public void StartEnemyTurn()
    {
        enemyAttack.enabled = true;
        timeRemaining = timeEnemyAttack;
        playerTurn.enabled = false;
        enemyTurn.enabled = true;
        enemyTurn.transform.position = combatBoxSpawner.position;
    }
}
