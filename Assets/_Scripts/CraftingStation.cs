using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    [SerializeField] CanvasGroup stationMenu;
    [SerializeField] Inventory inventory;
    [SerializeField] float maxUseDistance = 7f;
    [SerializeField] Transform player;
    bool isOpen = false;
    void Start()
    {
        // Set default menu for when opened the first time, or else the inventory won't populate with recipes
        stationMenu.transform.GetChild(1).GetComponent<Craftingmenu>().OpenMenu();

        CloseStation();
        inventory.closeInventory.AddListener(CloseStation);
    }
    public void Interact(float distance)
    {
        Debug.Log("interacting!");
        stationMenu.alpha = 1f;
        stationMenu.interactable = true;
        stationMenu.blocksRaycasts = true;
        inventory.OpenInventory();
        if(distance > maxUseDistance)
        {
            CloseStation();
        }
    }

    void Update()
    {
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if(distanceToPlayer > maxUseDistance)
        {
            CloseStation();
        }
    }

    public void CloseStation()
    {
        stationMenu.alpha = 0f;
        stationMenu.interactable = false;
        stationMenu.blocksRaycasts = false;
    }
}
