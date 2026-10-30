using UnityEngine;
using System.Collections.Generic;
public class NPC : MonoBehaviour
{
    [SerializeField] Transform player;          // a quién sigue
    [SerializeField] float speed = 5f;          // qué tan rápido camina
    [SerializeField] float stopDistance = 0.8f; // a qué distancia se frena
    [SerializeField] float pointSpacing = 0.1f; // cada cuanta distancia el jugador deja una miga
    [SerializeField] float pressTime = 1f; // NUEVO: segundos que se queda parado "presionando" el botón
    Queue<Vector2> trail = new Queue<Vector2>(); // la fila de migas (la más vieja sale primero)
    Vector2 lastPoint; // dónde dejó la última miga
    float stopTimer; // NUEVO: cuenta regresiva de cuánto le falta para volver a caminar
    void Start()
    {
        lastPoint = player.position; // la primera miga arranca donde está el jugador
    }
    void Update()
    {
        // CAMBIO: esta línea estaba más abajo, la subí al principio
        float distance = Vector2.Distance(transform.position, player.position);

        // NUEVO: si el jugador está dentro del radio de frenado...
        if (distance <= stopDistance)
        {
            trail.Clear();                // NUEVO: vacía toda la fila de migas
            lastPoint = player.position;  // NUEVO: la "última miga" pasa a ser donde estás ahora
            return;                       // NUEVO: corta el Update acá, este frame no se hace nada más
        }

        // si el jugador se alejó lo suficiente de la última miga, deja una nueva
        if (Vector2.Distance(player.position, lastPoint) >= pointSpacing)
        {
            lastPoint = player.position;
            trail.Enqueue(lastPoint);
        }
        // NUEVO: si está "presionando" un botón, descuenta tiempo y no se mueve
        if (stopTimer > 0)
        {
            stopTimer -= Time.deltaTime;  // NUEVO: resta el tiempo que pasó desde el frame anterior
            return;                       // NUEVO: corta acá, no camina este frame
        }
        // CAMBIO: antes era "distance > stopDistance && trail.Count > 0".
        // Ya no hace falta chequear la distancia porque el return de arriba ya la descartó.
        if (trail.Count > 0)
        {
            Vector2 target = trail.Peek(); // mira la miga más vieja sin sacarla

            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime); // antes iba hacia player.position, ahora hacia la miga

            if (Vector2.Distance(transform.position, target) < 0.05f) // si ya llegó a la miga...
            {
                trail.Dequeue(); // se la come y pasa a la siguiente
            }
        }
    }
    // NUEVO: Unity llama a este método solo, una vez, cuando este objeto entra en un trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Button"))   // NUEVO: ¿el objeto que toqué tiene el tag "button"?
        {
            // NUEVO: busca el script GateButton en el objeto que tocó
            GateButton button = other.GetComponent<GateButton>();

            // NUEVO: si ese botón existe y su puerta ya está abierta, no se frena
            if (button != null && button.GateIsOpen) return;
            stopTimer = pressTime;        // NUEVO: arranca la cuenta regresiva
        }
    }
}
