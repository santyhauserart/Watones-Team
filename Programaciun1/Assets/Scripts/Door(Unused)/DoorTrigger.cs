using UnityEngine;
#region ¿Que hace este script?
// Destruye el collider de la puerta si eljugador posee la llave correspondiente
#endregion
public class DoorTrigger : MonoBehaviour
{
    #region Variables
    // keyIndexRequerido -> Revisa si tengo la llave que corresponde
    // keyValue -> El valor de la llave que pertenece a la puerta, para verificar que coinciden
    // door -> Permite colocar el collider de la puerta en el inspector
    #endregion

    public int keyIndexRequerido;
    public int keyValue;
    public GameObject door;
    public GameObject doorTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region ¿Que hace?
        // Cuando entro en el trigger, busca dentro de la clase Player -> Revisa el array de llaves y lo compara con el valor de llave de la puerta
        // Si coinciden ambos valores, el collider de la puerta se destruye
        #endregion
        if (collision.TryGetComponent<Player>(out Player player))
        {
            if (player.keys[keyIndexRequerido] == keyValue)
            {
                print("puerta abierta");
                Destroy(door.gameObject);
                Destroy(doorTrigger.gameObject);
            }
            else print($"Puerta cerrada, no tenes la llave {keyValue}");
        }
    }
}
