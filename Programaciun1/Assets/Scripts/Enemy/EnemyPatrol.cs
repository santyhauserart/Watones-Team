using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private float waitRate = 1f;
    private float waitTime;
    private bool isChasing = false;
    [SerializeField] private float encounterDistance = 1f;
    private bool inEncounter = false;
    [SerializeField] private int guardId;
    [SerializeField] private string combatScene = "CombatScene";

    private void Start()
    {
        Debug.Log(guardId + " / " + GameState.defeatedGuards.Contains(guardId));
        if (GameState.defeatedGuards.Contains(guardId))
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (isChasing)
        {
            Vector2 playerDirection = (player.transform.position - transform.position).normalized;
            if (!inEncounter)
            {
                transform.Translate(playerDirection * speed * Time.deltaTime, Space.World);
            }
            float distancePlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distancePlayer < encounterDistance && !inEncounter)
            {
               inEncounter = true;
               player.SetCanMove(false);
                if (!GameState.encounterGuards.Contains(guardId))
                {
                    GameState.encounterGuards.Add(guardId);
                }
                GameState.playerPosition = player.transform.position;
                GameState.hasSavedPosition = true;
                SceneManager.LoadScene(combatScene);
            }
        }
        else
        {
            Transform target = waypoints[currentIndex];
            Vector2 waypointDirection = (target.position - transform.position).normalized;
            if (Time.time >= waitTime)
            {
                transform.Translate(waypointDirection * speed * Time.deltaTime, Space.World);
            }
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance < stopDistance)
            {
                waitTime = Time.time + waitRate;
                currentIndex = (currentIndex + 1) % waypoints.Length;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            isChasing = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            isChasing = false;
        }
    }
}
