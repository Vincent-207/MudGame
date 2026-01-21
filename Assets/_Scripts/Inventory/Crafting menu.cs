using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craftingmenu : MonoBehaviour, IInteractable
{
    public List<Recipe> menuRecipes = new List<Recipe>();
    public Transform craftingGrid;
    [SerializeField] Inventory inventory;
    bool menuIsOpen = false;
    public void Interact()
    {
        Debug.Log("Interacting");
        OpenMenu();
    }
    public void OpenMenu()
    {
        inventory.craftingGrid = craftingGrid;
        inventory.allRecipes = menuRecipes;
        inventory.PopulateCraftingGrid();
    }

    public void CloseMenu()
    {
        
    }
}


public interface IInteractable
{
    public void Interact();
}