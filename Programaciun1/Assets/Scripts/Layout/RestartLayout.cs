using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartLayout : MonoBehaviour
#region Qué hace el script
//Este script es el botón de "empezar de nuevo" del nivel: vuelve a cargar la escena en la que estás, y todo arranca de cero.
//Es como dar vuelta la caja de juguetes y volver a armarla.
//No hace nada por sí solo: otro script le pide que reinicie. Player lo usa cuando el jugador se queda sin vida.
//public: otros scripts lo pueden ver. class: es un molde de script.
//RestartLayout: es su nombre ("reiniciar el nivel"). : MonoBehaviour: permite pegarlo a un objeto del juego.
#endregion
{
    public void RestartLevel()
    #region La función de reiniciar
    //Es la función que reinicia el nivel.
    //public: otros scripts la pueden llamar (Player la llama en DeathPlayer).
    //void: no devuelve nada, solo hace su trabajo.
    //RestartLevel: es el nombre ("reiniciar nivel"). (): no necesita que le des nada.
    #endregion
    {
        SceneFader.Load(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        #region Volver a cargar la escena
        //Carga otra vez la escena en la que estás ahora, así que todo se arma de nuevo desde el principio.
        //SceneManager.LoadScene(...): "cargá esta escena". Se le puede dar el nombre de la escena o su número.
        //SceneManager.GetActiveScene(): "dame la escena en la que estoy ahora".
        //.buildIndex: el número de esa escena en la lista de Build Profiles (Build Settings). La primera es la 0, la segunda es la 1, etc.
        //Es como decir "volvé a la página en la que estoy", sin tener que escribir su nombre.
        #endregion
    }
}
