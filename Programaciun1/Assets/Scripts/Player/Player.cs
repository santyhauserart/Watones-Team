using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using static UnityEditor.ShaderData;
#region ¿Que hace este script?
// jugador
#endregion
public class Player : MonoBehaviour
{
    #region ¿Que hacen mis variables?
    // moveInput -> Me muestra en el inspector un selector de inputs preestablecidos, para colocar el input de movimiento en este caso
    // life -> vida (sin usar)
    // speed -> Para ajustar la velocidad de movimiento
    // position -> para modificar la posicion del player
    // input -> para ver en el inspector como cambian los valores segun las tecla presionada de movimiento
    // rb -> para utilizar el movimiento a partir del rigidbody
    #endregion
    #region Variables
    [SerializeField] private RestartLayout RestartLayout;
    [SerializeField] private Damage damage;
    public InputActionReference moveInput;
    public int life;
    public float speed;
    public Transform position;
    public Vector2 input;
    public Rigidbody2D rb;
    public int[] keys;
    #endregion
    #region ¿Que hace start?
    // le asigno un valor a la velocidad de movimiento
    #endregion
    void Start()
    {
        speed = 4; // le asigno un valor
    }
    #region ¿Que hace update?
    // Calculo frame a frame el valor actual (en x, y) de las teclas que presiona el jugador 
    // el resultado se lo paso a input para saber en que direccion se va a mover
    #endregion
    void Update()
    {
        input = moveInput.action.ReadValue<Vector2>();
        if (life == 0)
        {
            DeathPlayer();
        }
    }
    #region ¿Que hace FixedUpdate?
    // Calculo la posicion del rigidbody para mover a mi objeto a partir de su rigidbody, teniendo en cuenta las fisicas
    #endregion
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
    }
    #region ¿Que hace OnTriggerEnter2D?
    // Resumen -> Si el jugador colisiona con el trigger y ese trigger tiene el script Damage -> le resto 1 de vida (..)
    // (..) Mejor explicado -> Primero a creo una variable local "collision" de tipo Collider2D, luego en un if pregunto si "collision" tiene asignado el (..)
    // (..) Script de daño, entonces creo una variable local "damage" para usar el contenido de ese script. Si el if se cumple, entonces a mi variable (..)
    // (..) global "life" le resto el valor de la variable "damage" que se encuentra dentro del script "Damage".

    // Resumen -> Si el jugador colisiona con el trigger que tiene el script KeyDoor -> Creo un contador con mi array "keys" -> si el index actual de (..)
    // "keys" es igual a 0 -> lo cambio a 1
    // 
    // Destruyo la llave con la que colisiono
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Damage>(out Damage damage))
        {
            print($"perdiste -{damage.damage} puntos de vida");
            life -= damage.damage;
            print($"Ahora tienes {life} puntos de vida");
        }
        if (collision.TryGetComponent<KeyDoor>(out KeyDoor key))
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == 0)
                {
                    keys[i] = key.keyIndex; // o el valor que quieras guardar
                    //print($"llave {key.keyIndex} conseguida");
                    break;       // corta el loop apenas encuentra la primera vacía
                }
            }
            Destroy(collision.gameObject); // destruye el objeto de la llave
            print($"llave {key.keyIndex} conseguida");
        }
    }
    void DeathPlayer ()
    {
        print("El jugador murio");
        RestartLayout.RestartLevel();
    }
}
