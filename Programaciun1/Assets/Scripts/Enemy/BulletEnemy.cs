using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private Vector2 direction;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 1;
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
