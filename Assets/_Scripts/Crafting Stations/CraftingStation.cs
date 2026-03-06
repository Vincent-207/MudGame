using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject stationMenuPrefab;
    [SerializeField] CanvasGroup stationMenu;
    [SerializeField] Inventory inventory;
    [SerializeField] float maxUseDistance = 7f;
    [SerializeField] Transform player;
    int currentMenu = 0;
    bool isOpen = false;
    List<Craftingmenu> craftingMenus = new List<Craftingmenu>();
    void Start()
    {
        Debug.Log("I'm a thing: " + name);
        stationMenu = Instantiate(stationMenuPrefab, GameManager.Instance.canvas.transform).GetComponent<CanvasGroup>();
        if(stationMenu == null) Debug.LogWarning("I'm evil  " + name);
        craftingMenus.AddRange(stationMenu.GetComponentsInChildren<Craftingmenu>());

        // Set default menu for when opened the first time, or else the inventory won't populate with recipes 
        inventory = GameManager.Instance.inventory;
        AddListeners();
        OpenCraftingMenu(0);
        CloseStation();
        inventory.closeInventory.AddListener(CloseStation);
    }
    public void Interact(float distance)
    {
        Debug.Log("interacting!");
        stationMenu.alpha = 1f;
        stationMenu.interactable = true;
        stationMenu.blocksRaycasts = true;
        stationMenu.transform.GetChild(stationMenu.transform.childCount - 1).gameObject.SetActive(true);
        inventory.OpenInventory();
        OpenCraftingMenu(currentMenu);
    }

    void AddListeners()
    {
        for(int i = 0; i < craftingMenus.Count; i++)
        {
            Craftingmenu currentMenu = craftingMenus[i];
            currentMenu.menuOpened.AddListener(() => OpenCraftingMenu(i));
        }
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

    void OpenCraftingMenu(int index)
    {
        stationMenu.transform.GetChild(index).GetComponent<Craftingmenu>();
        currentMenu = index;
        craftingMenus[index].OpenMenu();
    }
}
