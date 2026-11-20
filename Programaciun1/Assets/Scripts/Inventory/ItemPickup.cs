using UnityEngine;

public class ItemPickup : MonoBehaviour
// Es para una pocion en el suelo
// Cuando el jugador la toca se guarda en su inventario y desaparece del suelo
// No reaparece despues de un cambio de escena si la agarraste antes
{
    [SerializeField] private string itemName = "Potion";
    [SerializeField] private int pickupId; // identificador de cada pocion
    [SerializeField] private AudioClip pickupSound; // sonido al agarrar la poción

    private void Start()
    {
        if (GameState.collectedPickups.Contains(pickupId)) // ¿ya agarraron esta poción?
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Me tocó: {collision.name}");
        if (collision.GetComponent<Player>() != null) // ¿lo que entró es el jugador?
        {
            Inventory.Add(itemName); // Mete la poción en la "mochila".
            GameState.collectedPickups.Add(pickupId); // Anota en la "libretita" que esta poción ya fue agarrada.
            SfxPlayer.Play(pickupSound); // suena aunque el objeto desaparezca enseguida
            Destroy(gameObject);
        }
    }
}
