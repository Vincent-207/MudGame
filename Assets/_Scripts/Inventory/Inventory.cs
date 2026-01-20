using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;
using System.ComponentModel;
using System.Buffers.Text;
using UnityEngine.AI;

public class Inventory : MonoBehaviour
{   
    
    [SerializeField] InputActionReference DragItem, mousePosition, toggleInventory, pickupItem, selectHotBarItem, dropItem;
    [Header("Item pickup")]
    public float itemPickupRange = 3f;
    [SerializeField] LayerMask itemLayerMask;
    private Item lookedAtItem = null;
    public Material highlightMaterial;
    private Material originialMaterial;
    private Renderer lookedAtRenderer;
    public GameObject hotbarObj, inventorySlotParent, equipmentSlotParent, container;
    private List<ItemSlot> inventorySlots = new List<ItemSlot>();
    private List<ItemSlot> hotbarSlots = new List<ItemSlot>();
    private List<ItemSlot> equipmentSlots = new List<ItemSlot>();
    private List<ItemSlot> allSlots = new List<ItemSlot>();
    // Dragging
    public Image dragIcon;
    private ItemSlot draggedSlot = null;
    private bool isDragging = false;

    // Equiping

    int equippedHotbarIndex = 0; // 0-5
    public float equippedOpacity = 0.9f, normalOpacity = 0.58f;
    public Transform hand;
    private GameObject currentHandItem;
    // Crafting
    public List<Recipe> allRecipes, defaultRecipes = new List<Recipe>();
    public Transform craftingGrid;
    public GameObject craftingButtonPrefab;

    private void Awake()
    {
        allRecipes = defaultRecipes;
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<ItemSlot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<ItemSlot>());
        equipmentSlots.AddRange(equipmentSlotParent.GetComponentsInChildren<ItemSlot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);
        allSlots.AddRange(equipmentSlots);

        PopulateCraftingGrid();
    }
    // Update is called once per frame
    void OnEnable()
    {
        // debugAddWood.action.started += AddWood;
        // debugAddAxe.action.started += AddAxe;
        
        DragItem.action.started += StartDrag;
        DragItem.action.canceled += EndDrag;

        toggleInventory.action.started += ToggleInventory;

        pickupItem.action.started += Pickup;
        
        selectHotBarItem.action.started +=  SelectHotBarItem;
        dropItem.action.started += DropItem;
    }

    void OnDisable()
    {
        DragItem.action.started -= StartDrag;
        DragItem.action.canceled -= EndDrag;

        toggleInventory.action.started -= ToggleInventory;

        pickupItem.action.started -= Pickup;
        
        selectHotBarItem.action.started -=  SelectHotBarItem;
        dropItem.action.started -= DropItem;
    }

    void Update()
    {
        UpdateDragItemPosition();
        DetectLookedAtItem();
    }
    private void ToggleInventory(InputAction.CallbackContext callbackContext)
    {
        bool isOpeningInventory = !container.activeInHierarchy;
        container.SetActive(isOpeningInventory);
        Cursor.lockState = isOpeningInventory ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpeningInventory ? true : false;
        PlayerMovement.doRotation = !isOpeningInventory;
        if(!isOpeningInventory)
        {
            
        }
        // TODO: disable player turning.
    }

    /* void AddAxe(InputAction.CallbackContext callbackContext)
    {
        AddItem(axeItem, 1);
    }

    void AddWood(InputAction.CallbackContext callbackContext)
    {
        AddItem(woodItem, 3);
    } */

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
                        PopulateCraftingGrid();
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
                    PopulateCraftingGrid();
                    return;
                }
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
        // WARNING:  Assumes from has a none null item when calling GetItem
        if(from == to) return;
        // Check swaping
        
        bool fromItemMatchesToSlot = from.GetItem().slotTag == to.slotTag || to.slotTag == SlotTag.None;
        bool canPlace = fromItemMatchesToSlot;

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
            bool toItemMatchesFromSlot = to.GetItem().slotTag == from.slotTag || from.slotTag == SlotTag.None;
            bool canSwap = (toItemMatchesFromSlot && fromItemMatchesToSlot) || from.slotTag == to.slotTag;
            if(canSwap)
            {
                SwapItems(from, to);
            }

            return;
        }

        // empty slot
        if(canPlace)
        {
            to.SetItem(from.GetItem(), from.GetAmount());
            from.ClearSlot();
            return;
        }
        else
        {
            Debug.Log("can't place");
        }

    }

    void SwapItems(ItemSlot from, ItemSlot to)
    {
        ItemSO tempItem = to.GetItem();
        int tempAmount = to.GetAmount();

        to.SetItem(from.GetItem(), from.GetAmount());
        from.SetItem(tempItem, tempAmount);
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

    private void Pickup(InputAction.CallbackContext callbackContext)
    {
        // Debug.Log("picking up");
        if(lookedAtItem != null)
        {
            // Debug.Log("not null");
            Item item = lookedAtRenderer.GetComponent<Item>();
            if(item != null)
            {
                // Debug.Log("item not null");
                AddItem(item.item, item.amount);
                Destroy(item.gameObject);
            }
        }

        EquipHandItem();
    }

    private void DetectLookedAtItem()
    {
        if(lookedAtRenderer != null)
        {
            lookedAtRenderer.material = originialMaterial;
            lookedAtRenderer = null;
            originialMaterial = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if(Physics.Raycast(ray, out RaycastHit hit, itemPickupRange, itemLayerMask))
        {
            Item item = hit.collider.GetComponent<Item>();
            if(item != null)
            {
                Renderer rend = item.GetComponent<Renderer>();
                if(rend != null)
                {
                    originialMaterial = rend.material;
                    rend.material = highlightMaterial;
                    lookedAtRenderer = rend;
                    lookedAtItem = item;
                }
            }
        }
    }

    private void UpdateHotbarOpacity()
    {
        for(int i = 0; i < hotbarSlots.Count; i++)
        {
            Image icon = hotbarSlots[i].GetComponent<Image>();
            if(icon != null)
            {
                icon.color = (i == equippedHotbarIndex) ? new Color(1, 1, 1, equippedOpacity) : new Color(1, 1, 1, normalOpacity);
            }
        }
    }

    private void SelectHotBarItem(InputAction.CallbackContext callbackContext)
    {
        
        equippedHotbarIndex = (int) callbackContext.action.ReadValue<float>() - 1;
        UpdateHotbarOpacity();
        EquipHandItem();

    }

    private void DropItem(InputAction.CallbackContext callbackContext)
    {
        
        ItemSlot equippedSlot = hotbarSlots[equippedHotbarIndex];

        if(!equippedSlot.HasItem()) return;

        ItemSO itemSO = equippedSlot.GetItem();
        GameObject prefab = itemSO.itemPrefab;

        if(prefab == null) return;

        GameObject dropped = Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);

        Item item = dropped.GetComponent<Item>();
        item.item = itemSO;
        item.amount = equippedSlot.GetAmount();

        equippedSlot.ClearSlot();
        EquipHandItem();
        PopulateCraftingGrid();
    }

    private void EquipHandItem()
    {
        if(currentHandItem != null)
        {
            Destroy(currentHandItem);
        }

        ItemSlot equippedSlot = hotbarSlots[equippedHotbarIndex];

        if(!equippedSlot.HasItem()) return;

        ItemSO item = equippedSlot.GetItem();
        if(item.handItemPrefab == null) return;

        currentHandItem = Instantiate(item.handItemPrefab, hand);
        currentHandItem.transform.localPosition = Vector3.zero;
        currentHandItem.transform.localRotation = Quaternion.identity;

    }

    public void PopulateCraftingGrid()
    {
        for(int i = craftingGrid.childCount - 1; i >= 0; i--)
        {
            Destroy(craftingGrid.GetChild(i).gameObject);
        }

        foreach(Recipe recipe in allRecipes)
        {
            GameObject btnObj = Instantiate(craftingButtonPrefab, craftingGrid);
            Image img = btnObj.transform.GetChild(0).GetComponent<Image>();
            img.sprite = recipe.result.Icon;

            Button btn = btnObj.GetComponent<Button>();

            btn.interactable = CanCraft(recipe);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => Craft(recipe));
        }
    }

    public void Craft(Recipe recipe)
    {
        if(!CanCraft(recipe))
        {
            return;
        }

        ConsumeIngredients(recipe);
        AddItem(recipe.result, recipe.resultAmount);

        PopulateCraftingGrid();
    }

    private void ConsumeIngredients(Recipe recipe)
    {
        foreach(Ingredient ingredient in recipe.ingredients)
        {
            int remaining = ingredient.amount;
            foreach(ItemSlot slot in allSlots)
            {
                if(!slot.HasItem()) continue;
                if(slot.GetItem() != ingredient.item) continue;

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
    }

    public bool CanCraft(Recipe recipe)
    {
        foreach(Ingredient ingredient in recipe.ingredients)
        {
            int totalFound = 0;
            foreach(ItemSlot slot in allSlots)
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
