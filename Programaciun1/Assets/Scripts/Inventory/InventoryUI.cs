using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private string itemName = "Potion";
    [SerializeField] private TextMeshProUGUI countText;

    private void Update()
    {
        countText.text = "x" + Inventory.Count(itemName);
    }
}
