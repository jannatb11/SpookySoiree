using UnityEngine;
using UnityEngine.UI;

public class DaisyPickup : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Pickup);
        }
    }

    void Pickup()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.CollectDaisy();
        }
        else
        {
            Debug.LogError("InventoryManager.Instance is NULL!");
        }

        gameObject.SetActive(false);
    }
}