using UnityEngine;

public class CombatButton : MonoBehaviour
    //Guarda el nombre del boton de combate
{
    [SerializeField] public string button;
    [SerializeField] private SpriteRenderer visual;                 // NUEVO: el sprite que sí se ve (se arrastra en el Inspector)
    [SerializeField] private Color normalColor = Color.white;       // NUEVO: cómo se ve apagado (blanco = dibujo original)
    [SerializeField] private Color highlightColor = Color.yellow;

    private void Awake()                                            // NUEVO
    {                                                               // NUEVO
        SetHighlight(false);                                        // NUEVO: arranca apagado
    }                                                               // NUEVO

    public void SetHighlight(bool isLit)                            // NUEVO: public para que CombatMenu lo llame
    {                                                               // NUEVO
        if (visual == null) return;                                 // NUEVO: si todavía no le arrastraste el sprite, no hace nada ni da error
        visual.color = isLit ? highlightColor : normalColor;        // NUEVO: si isLit es true elige iluminado, si no, normal (como el ? : de Count en Inventory)
    }                                                               // NUEVO
}
