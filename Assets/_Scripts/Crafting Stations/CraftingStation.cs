using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject stationMenuPrefab;
    [SerializeField] CanvasGroup stationMenu;
    [SerializeField] Inventory inventory;
    [SerializeField] float maxUseDistance = 7f;
    [SerializeField] Transform player;
    bool isOpen = false;
    void Start()
    {
        Debug.Log("I'm a thing: " + name);
        stationMenu = Instantiate(stationMenuPrefab, GameManager.Instance.canvas.transform).GetComponent<CanvasGroup>();
        if(stationMenu == null) Debug.LogWarning("I'm evil  " + name);

        // Set default menu for when opened the first time, or else the inventory won't populate with recipes 
        stationMenu.transform.GetChild(1).GetComponent<Craftingmenu>().OpenMenu();
        inventory = GameManager.Instance.inventory;
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
    }

    void Update()
    {
        
        
    }

    public void CloseStation()
    {
        stationMenu.alpha = 0f;
        stationMenu.interactable = false;
        stationMenu.blocksRaycasts = false;
    }
}
