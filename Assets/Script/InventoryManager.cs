using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Transform itemParent;
    public GameObject itemIconPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(string itemName)
    {
        GameObject icon = Instantiate(itemIconPrefab, itemParent);
        icon.GetComponentInChildren<Text>().text = itemName;
    }
}
