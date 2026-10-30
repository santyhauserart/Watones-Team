using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] int buttonsNeeded = 3;
    [SerializeField] Collider2D gateCollider;
    int buttonsPressed;
    bool opened;
    // NUEVO: propiedad de solo lectura para que otros scripts puedan saber si la puerta ya se abrió
    public bool IsOpen => opened;
    public void ButtonPressed()
    {
        if (opened) return;
        buttonsPressed++;
        print($"Botones: {buttonsPressed}/{buttonsNeeded}");
        if (buttonsPressed >= buttonsNeeded)
        {
            opened = true;
            gateCollider.enabled = false;
        }
    }
    public void ButtonReleased()
    {
        if (opened) return;

        buttonsPressed--;
    }
}
