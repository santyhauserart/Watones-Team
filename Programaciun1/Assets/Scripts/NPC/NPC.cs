using UnityEngine;
using System.Collections.Generic;
public class NPC : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float speed = 5f;
    [SerializeField] float stopDistance = 0.8f;
    [SerializeField] float pointSpacing = 0.1f;
    [SerializeField] float pressTime = 1f;
    Queue<Vector2> trail = new Queue<Vector2>();
    Vector2 lastPoint;
    float stopTimer;
    void Start()
    {
        lastPoint = player.position;
    }
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= stopDistance)
        {
            trail.Clear();
            lastPoint = player.position;
            return;
        }
        if (Vector2.Distance(player.position, lastPoint) >= pointSpacing)
        {
            lastPoint = player.position;
            trail.Enqueue(lastPoint);
        }
        if (stopTimer > 0)
        {
            stopTimer -= Time.deltaTime;
            return;
        }
        if (trail.Count > 0)
        {
            Vector2 target = trail.Peek();

            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target) < 0.05f)
            {
                trail.Dequeue();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Button"))
        {
            GateButton button = other.GetComponent<GateButton>();
            if (button != null && button.GateIsOpen) return;
            stopTimer = pressTime;
        }
    }
}
