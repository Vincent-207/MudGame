using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CookingStation : MonoBehaviour, IInteractable
{
    [SerializeField]
    List<ItemSlot> fuelSlots = new List<ItemSlot>(), ingrediantSlots = new List<ItemSlot>(),
    outputSlots = new List<ItemSlot>(), allSlots = new List<ItemSlot>();
    [SerializeField] Transform outputSlotHolder, fuelSlotHolder, ingrediantSlotHolder;
    public List<Recipe> allRecipes = new List<Recipe>();
    [SerializeField] float currentFuelTime = 0;
    bool isMenuOpen = false;
    [SerializeField] CanvasGroup MenuCanvasGroup;
    [SerializeField] Inventory inventory;
    void Awake()
    {
        fuelSlots.AddRange(fuelSlotHolder.GetComponentsInChildren<ItemSlot>());
        ingrediantSlots.AddRange(ingrediantSlotHolder.GetComponentsInChildren<ItemSlot>());
        outputSlots.AddRange(outputSlotHolder.GetComponentsInChildren<ItemSlot>());

        allSlots.AddRange(fuelSlots);
        allSlots.AddRange(ingrediantSlots);
        allSlots.AddRange(outputSlots);
    }

    void Update()
    {
        if(CanUseFuel())
        {
            UseFuel();
        }

    }


    void UseFuel()
    {
        // Remove a fuel from fuel slots
        foreach(ItemSlot itemSlot in fuelSlots)
        {
            if(itemSlot.HasItem())
            {
                itemSlot.RemoveAmount(1);
                break;
            }
        }
        StartCoroutine(ConsumeFuel());
    }

    IEnumerator ConsumeFuel()
    {
        currentFuelTime = 5f;
        Debug.LogWarning("Starting use!");
        yield return new WaitForSeconds(currentFuelTime);
        currentFuelTime = 0;
        // TODO
        Craft();
    }

    void Craft()
    {
        Recipe recipe = GetCraftableRecipe();
        if(recipe != null)
        {
            if(OutputCanFit(recipe))
            {
                Craft(recipe);
            }
            else
            {
                // Debug.Log("Output can't fit! not crafting.");
            }
        }
        // else Debug.Log("Can't find craftable recipe");

    }

    void Craft(Recipe recipe)
    {
        Debug.Log("Crafting: " + recipe.name);
        
        AddItem(recipe.result, recipe.resultAmount);
        ConsumeIngredients(recipe, ingrediantSlots);
        
    }

    void AddItem(ItemSO itemToAdd, int amount)
    {
        int remaining = amount;
        foreach(ItemSlot slot in allSlots)
        {
            if(slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStackSize = itemToAdd.maxStackSize;

                if(currentAmount < maxStackSize)
                {
                    int spaceLeft = maxStackSize - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);
                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    
                    remaining -= amountToAdd;
                    Debug.Log("remaining:  " + remaining);
                    if(remaining <= 0)
                    {

                        return;
                    }
                }
            }
        }


        foreach(ItemSlot slot in allSlots)
        {
            if(!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;
                if(remaining <= 0)
                {
                    return;
                }
            }
        }


        if(remaining > 0)
        {
            Debug.LogWarning("Inventory is full, could not add " + remaining + " of " + itemToAdd.itemName);
        }
    }

    bool CanUseFuel()
    {
        // Debug.LogWarning("Checking");
        if(currentFuelTime > 0) return false;
        // Debug.Log("No active fuel");
        if(IsEmptyCollection(fuelSlots)) return false;
        // Debug.Log("Is empty collection");
        Recipe recipeToCraft = GetCraftableRecipe();
        if(recipeToCraft == null) return false;
        // Debug.Log("Recipe found");
        if(OutputCanFit(recipeToCraft)) return true;
        // Debug.Log("Output can't fit");

        return true;

      // Fuel section should not let non fuel items be dragged in, so assume it has valid fuel if it contains any items.
    }

    bool OutputCanFit(Recipe recipe)
    {
        int remaining = recipe.resultAmount;
        ItemSO itemToAdd = recipe.result;
        // Add to any not fully filled tiles that have the same item
        foreach(ItemSlot slot in allSlots)
        {
            if(slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStackSize = itemToAdd.maxStackSize;

                if(currentAmount < maxStackSize)
                {
                    int spaceLeft = maxStackSize - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);
                    remaining -= amountToAdd;
                    Debug.Log("remaining:  " + remaining);
                    if(remaining <= 0)
                    {
                        return true;
                    }
                }
            }
        }
        // Then add to empty tiles. 
        foreach(ItemSlot slot in allSlots)
        {
            if(!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                
                remaining -= amountToPlace;
                if(remaining <= 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool HasEmptySlot(List<ItemSlot> collection)
    {
        foreach(ItemSlot slot in collection)
        {
            if(slot.HasItem() == false) return true;
        }
        return false;
    }
    bool IsEmptyCollection(List<ItemSlot> collection)
    {
        foreach(ItemSlot slot in collection)
        {
            if(slot.HasItem()) return false;
        }

        return true;
    }
    public Recipe GetCraftableRecipe()
    {
        foreach(Recipe recipe in allRecipes)
        {
            if(CanCraft(recipe)) return recipe;
        }

        return null;
    }
    public bool CanCraftAny()
    {
        foreach(Recipe recipe in allRecipes)
        {
            if(CanCraft(recipe)) return true;
        }

        return false;
    }
    public bool CanCraft(Recipe recipe)
    {
        foreach(Ingredient ingredient in recipe.ingredients)
        {
            int totalFound = 0;
            foreach(ItemSlot slot in ingrediantSlots)
            {
                if(slot.HasItem() && slot.GetItem() == ingredient.item)
                {
                    totalFound += slot.GetAmount();
                }
            }

            if(totalFound < ingredient.amount) return false;
        }

        return true;
    }

    private void ConsumeIngredients(Recipe recipe, List<ItemSlot> ingredientSlots)
    {
        foreach(Ingredient ingredient in recipe.ingredients)
        {
            RemoveItem(ingredient.item, ingredient.amount, ingredientSlots);
        }
    }

    void RemoveItem(ItemSO item, int amount, List<ItemSlot> itemSlots)
    {
        int remaining = amount;
            foreach(ItemSlot slot in itemSlots)
            {
                if(!slot.HasItem()) continue;
                if(slot.GetItem() != item) continue;

                int take = Mathf.Min(slot.GetAmount(), remaining);
                slot.SetItem(slot.GetItem(), slot.GetAmount() - take);
                if(slot.GetAmount() <= 0)
                {
                    slot.ClearSlot();
                }

                remaining -= take;
                if(remaining <= 0) break;
            }
    }

    public void Interact(float distance)
    {
        ToggleMenu();
    }

    void ToggleMenu()
    {
        if(isMenuOpen)
        {
            isMenuOpen = false;
            MenuCanvasGroup.alpha = 0f;
            MenuCanvasGroup.interactable = false;
            MenuCanvasGroup.blocksRaycasts = false;
            inventory.CloseInventory();
        }
        else
        {
            isMenuOpen = true;
            MenuCanvasGroup.alpha = 1f;
            MenuCanvasGroup.interactable = true;
            MenuCanvasGroup.blocksRaycasts = true;
            inventory.OpenInventory();
            
        }
    }
}
