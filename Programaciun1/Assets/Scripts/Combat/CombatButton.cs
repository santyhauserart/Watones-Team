using UnityEngine;

public class CombatButton : MonoBehaviour
    //Guarda el nombre del boton de combate
{
    public string button;                          // lo que ya tenías ("Attack", "Flee", "Item")

    [SerializeField] private Sprite spriteNormal;  // versión apagada
    [SerializeField] private Sprite spriteBrillante; // versión brillante
    private SpriteRenderer sr;                     // el componente que dibuja el sprite

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();       // agarra el SpriteRenderer de este mismo objeto
    }

    public void SetHighlight(bool encendido)
    {
        // si está encendido pone el sprite brillante, si no el normal
        sr.sprite = encendido ? spriteBrillante : spriteNormal;
    }
    //[SerializeField] public string button;
    //[SerializeField] private SpriteRenderer visual;                 // NUEVO: el sprite que sí se ve (se arrastra en el Inspector)
    //[SerializeField] private Color normalColor = Color.white;       // NUEVO: cómo se ve apagado (blanco = dibujo original)
    //[SerializeField] private Color highlightColor = Color.yellow;

    //private void Awake()                                            // NUEVO
    //{                                                               // NUEVO
    //    SetHighlight(false);                                        // NUEVO: arranca apagado
    //}                                                               // NUEVO

    //public void SetHighlight(bool isLit)                            // NUEVO: public para que CombatMenu lo llame
    //{                                                               // NUEVO
    //    if (visual == null) return;                                 // NUEVO: si todavía no le arrastraste el sprite, no hace nada ni da error
    //    visual.color = isLit ? highlightColor : normalColor;        // NUEVO: si isLit es true elige iluminado, si no, normal (como el ? : de Count en Inventory)
    //}                                                               // NUEVO
}
