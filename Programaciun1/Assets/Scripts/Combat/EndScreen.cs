using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private string nextScene = "PrototypeScene"; // a dónde volvés (el mapa)
    [SerializeField] private float minWait = 1f;                  // segundos antes de poder continuar
    private float timer;                                          // cuenta el tiempo transcurrido

    private void Update()
    {
        timer += Time.deltaTime;                                  // suma el tiempo de cada frame

        // si pasó el tiempo mínimo y apretás Z (o Enter), continúa
        if (timer >= minWait && !SceneFader.IsBusy &&
            (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)))
        {
            SceneFader.Load(nextScene);                           // vuelve al mapa con el mismo fade que usás siempre
        }
    }
}
