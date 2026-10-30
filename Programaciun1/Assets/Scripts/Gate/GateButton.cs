using UnityEngine;

public class GateButton : MonoBehaviour
{
    [SerializeField] Gate gate;
    int occupants;
    // NUEVO: el NPC le pregunta esto al botón, y el botón se lo pregunta a su puerta
    public bool GateIsOpen => gate.IsOpen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("NPC")) return;
        print($"{name} pisado por {collision.name}");
        occupants++;
        if (occupants == 1) gate.ButtonPressed();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("NPC")) return;

        occupants--;
        if (occupants == 0) gate.ButtonReleased();
    }
}
