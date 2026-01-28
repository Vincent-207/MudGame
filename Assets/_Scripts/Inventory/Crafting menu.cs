using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Craftingmenu : MonoBehaviour
{
    public List<Recipe> menuRecipes = new List<Recipe>();
    public Transform craftingGrid;
    [SerializeField] Inventory inventory;
    bool menuIsOpen = false;
    public int index;
    public UnityEvent menuOpened;
    void Awake()
    {
        inventory = GameManager.Instance.inventory;
    }
    public void Interact()
    {
        Debug.Log("Interacting");
        OpenMenu();
    }
    public void OpenMenu()
    {
        inventory.CloseOtherMenus();
        inventory.craftingGrid = craftingGrid;
        craftingGrid.gameObject.SetActive(true);
        inventory.allRecipes = menuRecipes;
        inventory.PopulateCraftingGrid();
    }

    public void CloseMenu()
    {
        
    }
}


public interface IInteractable
{
    public void Interact(float distance);
}