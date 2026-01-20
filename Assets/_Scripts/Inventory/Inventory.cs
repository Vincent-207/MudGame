using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;

public class Inventory : MonoBehaviour
{   
    // DEBUG
    public ItemSO woodItem, axeItem;

    [SerializeField] InputActionReference debugAddWood, debugAddAxe, 
    // non debug
    pickupItem, mousePosition;
    public GameObject hotbarObj, inventorySlotParent;
    public Image dragIcon;
    private List<ItemSlot> inventorySlots = new List<ItemSlot>();
    private List<ItemSlot> hotbarSlots = new List<ItemSlot>();
    private List<ItemSlot> allSlots = new List<ItemSlot>();
    private ItemSlot draggedSlot = null;
    private bool isDragging = false;
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
        pickupItem.action.started += StartDrag;
        pickupItem.action.canceled += EndDrag;
    }

    void AddAxe(InputAction.CallbackContext callbackContext)
    {
        AddItem(axeItem, 1);
    }

    void AddWood(InputAction.CallbackContext callbackContext)
    {
        AddItem(woodItem, 3);
    }

    void Update()
    {
        UpdateDragItemPosition();
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
                    return;
            }
        }


        if(remaining > 0)
        {
            Debug.LogWarning("Inventory is full, could not add " + remaining + " of " + itemToAdd.itemName);
        }
    }

    private void StartDrag(InputAction.CallbackContext callbackContext)
    {
    
        ItemSlot hovered = GetHoveredSlot();

        if(hovered != null && hovered.HasItem())
        {
            draggedSlot = hovered;
            isDragging = true;
            dragIcon.sprite = draggedSlot.GetItem().Icon;
            dragIcon.color = new Color(1, 1, 1, 0.5f);
            dragIcon.enabled = true;
        }
    
    }
    private void EndDrag(InputAction.CallbackContext callbackContext)
    {
        if(isDragging)
        {
            ItemSlot hovered = GetHoveredSlot();
            if(hovered != null)
            {
                HandleDrop(draggedSlot, hovered);
                dragIcon.enabled = false;

                draggedSlot = null;
                isDragging = false;
            }
        }
    }

    private void HandleDrop(ItemSlot from, ItemSlot to)
    {
        if(from == to) return;

        // stacking
        if(to.HasItem() && to.GetItem() == from.GetItem())
        {
            int max = to.GetItem().maxStackSize;
            int space = max - to.GetAmount();

            if(space > 0)
            {
                int move = Mathf.Min(space, from.GetAmount());
                to.SetItem(to.GetItem(), to.GetAmount() + move);
                from.SetItem(from.GetItem(), from.GetAmount() - move);
                
                if(from.GetAmount() <= 0)
                {
                    from.ClearSlot();
                }

                return;
            }
        }

        // different items
        if(to.HasItem())
        {
            ItemSO tempItem = to.GetItem();
            int tempAmount = to.GetAmount();

            to.SetItem(from.GetItem(), from.GetAmount());
            from.SetItem(tempItem, tempAmount);
            return;
        }

        // empty slot

        to.SetItem(from.GetItem(), from.GetAmount());
        from.ClearSlot();
    }

    private void UpdateDragItemPosition()
    {
        if(isDragging)
        {
            dragIcon.transform.position = mousePosition.action.ReadValue<Vector2>();
        }
    }

    private ItemSlot GetHoveredSlot()
    {
        foreach(ItemSlot itemSlot in allSlots)
        {
            if(itemSlot.hovering)
            {
                return itemSlot;
            }
        }

        // Debug.LogWarning("Couldn't find hovered slot.");
        return null;
    }
}
