using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStation : MonoBehaviour
{
    [SerializeField]
    List<ItemSlot> fuelSlots = new List<ItemSlot>(), ingrediantSlots = new List<ItemSlot>(),
    outputSlots = new List<ItemSlot>(), allSlots = new List<ItemSlot>();
    Transform outputSlotHolder, fuelSlotHolder, ingrediantSlotHolder;
    List<Recipe> allRecipes = new List<Recipe>();
    float currentFuelTime = 0;
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
        StartCoroutine(ConsumeFuel());
    }

    IEnumerator ConsumeFuel()
    {
        float time = 5f;

        yield return new WaitForSeconds(time);
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
                Debug.Log("Output can't fit! not crafting.");
            }
        }
        else Debug.Log("Can't find craftable recipe");

    }

    void Craft(Recipe recipe)
    {
        Debug.Log("Crafting: " + recipe.name);
    }

    bool CanUseFuel()
    {
        if(currentFuelTime > 0) return false;
        if(IsEmptyCollection(fuelSlots)) return false;
        Recipe recipeToCraft = GetCraftableRecipe();
        if(recipeToCraft == null) return false;
        if(OutputCanFit(recipeToCraft)) return true;

        return true;

      // Fuel section should not let non fuel items be dragged in, so assume it has valid fuel if it contains any items.
    }

    bool OutputCanFit(Recipe recipe)
    {
        int remaining = recipe.resultAmount;
        ItemSO itemToAdd = recipe.result;
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
                        return true;
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
}
