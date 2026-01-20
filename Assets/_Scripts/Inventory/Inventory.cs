using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{   
    public ItemSO woodItem, axeItem;
    [SerializeField] InputActionReference debugAddWood, debugAddAxe;
    public GameObject hotbarObj, inventorySlotParent;
    private List<ItemSlot> inventorySlots = new List<ItemSlot>();
    private List<ItemSlot> hotbarSlots = new List<ItemSlot>();
    private List<ItemSlot> allSlots = new List<ItemSlot>();
    private void Awake()
    {
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<ItemSlot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<ItemSlot>());
        
        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
    }
    // Update is called once per frame
    void OnEnable()
    {
        debugAddWood.action.started += AddWood;
        debugAddAxe.action.started += AddAxe;
    }

    void AddAxe(InputAction.CallbackContext callbackContext)
    {
        AddItem(axeItem, 1);
    }

    void AddWood(InputAction.CallbackContext callbackContext)
    {
        AddItem(woodItem, 3);
    }

    public void AddItem(ItemSO itemToAdd, int amount)
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
                    int amountToAdd = Math.Min(spaceLeft, remaining);
                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

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

                if(remaining <= 0)
                    return;
            }
        }


        if(remaining > 0)
        {
            Debug.LogWarning("Inventory is full, could not add " + remaining + " of " + itemToAdd.itemName);
        }
    }
}
