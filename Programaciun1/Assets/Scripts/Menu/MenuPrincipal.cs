// Clases básicas de Unity
using UnityEngine;
// Permite cargar y cambiar de escena
using UnityEngine.SceneManagement;
// Permite usar la clase Button (los botones del Canvas)
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Nombres de escenas (exactos)")]
    // Nombre de tu escena de juego
    [SerializeField] private string escenaJuego = "PrototypeScene";
    // Nombre de tu escena de prueba
    [SerializeField] private string escenaPrueba = "TestScene";

    [Header("Botones del menú")]
    // Cada botón se arrastra desde la jerarquía en el Inspector
    [SerializeField] private Button botonJugar;
    [SerializeField] private Button botonPrueba;
    [SerializeField] private Button botonInstrucciones;
    [SerializeField] private Button botonSalir;

    [Header("Instrucciones")]
    // Panel con el texto de las instrucciones
    [SerializeField] private GameObject panelInstrucciones;
    // Botón "Volver" que está dentro del panel
    [SerializeField] private Button botonVolver;

    // Awake se ejecuta antes que Start, ideal para conectar los botones
    private void Awake()
    {
        // AddListener le dice al botón qué método ejecutar cuando se hace clic.
        // Reemplaza el trabajo de armar el evento On Click a mano en el Inspector.
        botonJugar.onClick.AddListener(Jugar);
        botonPrueba.onClick.AddListener(JugarEscenaPrueba);
        botonInstrucciones.onClick.AddListener(AbrirInstrucciones);
        botonSalir.onClick.AddListener(Salir);
        botonVolver.onClick.AddListener(CerrarInstrucciones);
    }

    private void Start()
    {
        // Oculta el panel de instrucciones al arrancar
        panelInstrucciones.SetActive(false);
        // Asegura que el tiempo corra normal (por si venís de una pausa)
        Time.timeScale = 1f;
    }

    // Carga la escena del juego
    private void Jugar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    // Carga la escena de prueba
    private void JugarEscenaPrueba()
    {
        SceneManager.LoadScene(escenaPrueba);
    }

    // Muestra el panel de instrucciones
    private void AbrirInstrucciones()
    {
        panelInstrucciones.SetActive(true);
    }

    // Oculta el panel de instrucciones
    private void CerrarInstrucciones()
    {
        panelInstrucciones.SetActive(false);
    }

    // Cierra el juego
    private void Salir()
    {
        Debug.Log("Saliendo del juego...");
        // Esta parte solo existe dentro del editor de Unity
#if UNITY_EDITOR
        // En el editor, detiene el modo Play
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // En el juego compilado, cierra la aplicación
        Application.Quit();
#endif
    }
}
