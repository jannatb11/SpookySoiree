using UnityEngine;
using UnityEngine.UI;

public class DaisyPickup : MonoBehaviour
{
    private Button button;

    [Header("Inventory UI Reference")]
    public GameObject daisyInvUI; // drag daisyinv here in inspector

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

        
        if (daisyInvUI != null)
        {
            daisyInvUI.SetActive(true);
        }

        
        gameObject.SetActive(false);
    }
}