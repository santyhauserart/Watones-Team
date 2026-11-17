using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Dictionary<string, int> items = new Dictionary<string, int>();

    public static void Add(string itemName, int amount = 1)
    {
        if (items.ContainsKey(itemName))
        {
            items[itemName] += amount;
        }
        else
        {
            items[itemName] = amount;
        }
        UnityEngine.Debug.Log("Agarraste: " + itemName + ". Ahora tenés " + Count(itemName));
    }

    public static int Count(string itemName)
    {
        return items.ContainsKey(itemName) ? items[itemName] : 0;
    }

    public static bool Remove(string itemName)
    {
        if (Count(itemName) <= 0) return false;
        items[itemName] -= 1;
        UnityEngine.Debug.Log("Usaste: " + itemName + ". Te quedan " + Count(itemName));
        return true;
    }
}
