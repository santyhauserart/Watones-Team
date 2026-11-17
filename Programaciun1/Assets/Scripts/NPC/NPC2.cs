using UnityEngine;

public class NPC2 : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 0.1f;
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, player.targetPoint);
        Vector2 playerDirection = ((Vector3)player.targetPoint - transform.position).normalized;
        if (distance <= stopDistance)
        {
            return;
        }
        transform.Translate(playerDirection * speed * Time.deltaTime, Space.World);
    }
}
