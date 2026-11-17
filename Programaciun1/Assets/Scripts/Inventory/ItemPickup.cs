using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string itemName = "Potion";
    [SerializeField] private int pickupId;

    private void Start()
    {
        if (GameState.collectedPickups.Contains(pickupId))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Me tocó: " + collision.name);
            Inventory.Add(itemName);
            GameState.collectedPickups.Add(pickupId);
            Destroy(gameObject);
        }
    }
}
