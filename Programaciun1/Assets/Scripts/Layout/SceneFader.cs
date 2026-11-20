using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
//Es el telón negro: se oscurece, cambia la escena y se aclara. Se usa con SceneFader.Load("NombreDeEscena").
{                                                               // NUEVO
    public static float fadeTime = 0.5f;                        // NUEVO: segundos que tarda en oscurecer (y lo mismo en aclarar)

    private static SceneFader instance;                         // NUEVO: el único telón que existe
    private static bool isLoading;                              // NUEVO: cartelito de sí/no: "¿ya estoy cambiando de escena?"
    public static bool IsBusy => isLoading;                     // NUEVO: ventanita para que otros scripts miren el cartelito

    private float alpha = 0f;                                   // NUEVO: qué tan negro está el telón (0 = nada, 1 = todo)

    public static void Load(string sceneName)                   // NUEVO: la orden que llaman los otros scripts
    {                                                           // NUEVO
        if (isLoading) return;                                  // NUEVO: si ya está cambiando, ignorá la orden
        if (instance == null)                                   // NUEVO: ¿todavía no existe el telón?
        {                                                       // NUEVO
            GameObject faderObject = new GameObject("SceneFader");               // NUEVO: creá un objeto vacío
            instance = faderObject.AddComponent<SceneFader>();                   // NUEVO: pegale este script
            DontDestroyOnLoad(faderObject);                                      // NUEVO: que sobreviva al cambio de escena
        }                                                       // NUEVO
        isLoading = true;                                       // NUEVO
        instance.StartCoroutine(instance.FadeAndLoad(sceneName));                // NUEVO: arrancá la rutina
    }                                                           // NUEVO

    private IEnumerator FadeAndLoad(string sceneName)           // NUEVO
    {                                                           // NUEVO
        yield return StartCoroutine(Fade(0f, 1f));              // NUEVO: oscurecer (de 0 a 1) y esperar a que termine
        SceneManager.LoadScene(sceneName);                      // NUEVO: cambiar de escena, con la pantalla negra
        yield return null;                                      // NUEVO: esperar un cuadro a que cargue la escena nueva
        yield return StartCoroutine(Fade(1f, 0f));              // NUEVO: aclarar (de 1 a 0)
        isLoading = false;                                      // NUEVO: listo, ya se pueden pedir otras transiciones
    }                                                           // NUEVO

    private IEnumerator Fade(float from, float to)              // NUEVO
    {                                                           // NUEVO
        float t = 0f;                                           // NUEVO: segundos que pasaron
        while (t < fadeTime)                                    // NUEVO: repetí hasta que pase el tiempo
        {                                                       // NUEVO
            t += Time.unscaledDeltaTime;                        // NUEVO
            alpha = Mathf.Lerp(from, to, t / fadeTime);         // NUEVO: ej. a mitad de tiempo, alpha vale 0,5
            yield return null;                                  // NUEVO: esperá un cuadro
        }                                                       // NUEVO
        alpha = to;                                             // NUEVO: asegurá el valor final exacto
    }                                                           // NUEVO

    private void OnGUI()                                        // NUEVO
    {                                                           // NUEVO
        if (alpha <= 0f) return;                                // NUEVO: si es invisible, no dibujes nada
        GUI.depth = -1000;                                      // NUEVO: dibujalo por encima de lo demás
        GUI.color = new Color(0f, 0f, 0f, alpha);               // NUEVO: negro con el alpha de ahora
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);   // NUEVO: rectángulo del tamaño de la pantalla
        GUI.color = Color.white;                                // NUEVO: devolvé el color normal
    }                                                           // NUEVO
}
