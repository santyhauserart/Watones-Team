using UnityEngine;
using System.Collections;

public class SoulTrail : MonoBehaviour
//Deja fantasmitas del corazón mientras se mueve; se van borrando solos y eso se ve como una estela.
{                                                                   // NUEVO
    [SerializeField] private SpriteRenderer spriteRenderer;         // NUEVO: el sprite del corazón (el que se ve)
    [SerializeField] private float spacing = 0.15f;                 // NUEVO: cada cuánto camino deja un fantasmita (en unidades del mundo)
    [SerializeField] private float fadeTime = 0.3f;                 // NUEVO: segundos que tarda cada fantasmita en desaparecer
    [SerializeField] private float startAlpha = 0.5f;               // NUEVO: qué tan visible nace (0 = invisible, 1 = igual al corazón)

    private Vector2 lastGhostPosition;                              // NUEVO: dónde dejó el último fantasmita

    private void Start()                                            // NUEVO
    {                                                               // NUEVO
        lastGhostPosition = spriteRenderer.transform.position;      // NUEVO: empieza contando desde donde está parado
    }                                                               // NUEVO

    private void Update()                                           // NUEVO
    {                                                               // NUEVO
        if (!spriteRenderer.enabled) return;                        // NUEVO: si el corazón está invisible (parpadeo), no dejes fantasmitas
        float walked = Vector2.Distance(spriteRenderer.transform.position, lastGhostPosition);   // NUEVO: cuánto caminó desde el último
        if (walked >= spacing)                                      // NUEVO: ¿ya caminó lo suficiente?
        {                                                           // NUEVO
            SpawnGhost();                                           // NUEVO: dejá uno
            lastGhostPosition = spriteRenderer.transform.position;  // NUEVO: anotá dónde lo dejaste
        }                                                           // NUEVO
    }                                                               // NUEVO

    private void SpawnGhost()                                       // NUEVO
    {                                                               // NUEVO
        GameObject ghost = new GameObject("SoulGhost");             // NUEVO: objeto vacío para el fantasmita
        ghost.transform.position = spriteRenderer.transform.position;     // NUEVO: ponelo donde está el corazón
        ghost.transform.rotation = spriteRenderer.transform.rotation;     // NUEVO
        ghost.transform.localScale = spriteRenderer.transform.lossyScale; // NUEVO: mismo tamaño que el corazón

        SpriteRenderer ghostRenderer = ghost.AddComponent<SpriteRenderer>();   // NUEVO: dale un renderer
        ghostRenderer.sprite = spriteRenderer.sprite;               // NUEVO: el mismo dibujo
        ghostRenderer.sharedMaterial = spriteRenderer.sharedMaterial;      // NUEVO: el mismo material
        ghostRenderer.sortingLayerID = spriteRenderer.sortingLayerID;      // NUEVO: la misma capa
        ghostRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;      // NUEVO: un escalón atrás del corazón

        Color ghostColor = spriteRenderer.color;                    // NUEVO: copia del color del corazón
        ghostColor.a = startAlpha;                                  // NUEVO: cambiale solo la transparencia
        ghostRenderer.color = ghostColor;                           // NUEVO

        Destroy(ghost, fadeTime);                                   // NUEVO: se borra solo al terminar (aunque algo falle)
        StartCoroutine(FadeGhost(ghostRenderer, ghostColor));       // NUEVO: arrancá el desvanecido
    }                                                               // NUEVO

    private IEnumerator FadeGhost(SpriteRenderer ghostRenderer, Color startColor)   // NUEVO
    {                                                               // NUEVO
        float t = 0f;                                               // NUEVO: segundos que pasaron
        while (t < fadeTime)                                        // NUEVO
        {                                                           // NUEVO
            t += Time.deltaTime;                                    // NUEVO
            if (ghostRenderer == null) yield break;                 // NUEVO: si ya lo borraron, cortá
            Color c = startColor;                                   // NUEVO
            c.a = Mathf.Lerp(startAlpha, 0f, t / fadeTime);         // NUEVO: baja la transparencia de startAlpha a 0
            ghostRenderer.color = c;                                // NUEVO
            yield return null;                                      // NUEVO: esperá un cuadro
        }                                                           // NUEVO
    }                                                               // NUEVO
}
