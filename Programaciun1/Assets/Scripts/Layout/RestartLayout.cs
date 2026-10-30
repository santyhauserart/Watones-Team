using UnityEngine;
using UnityEngine.SceneManagement;
// Script que guarda el metodor de Reiniciar el Layout
public class RestartLayout : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
    }
    // Fijate en la escena que estoy actualmente y reiniciala
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
