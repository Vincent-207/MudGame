using System;
using UnityEngine;

public class DebugAddItem : MonoBehaviour
{
    [SerializeField]
    ItemSet[] itemSets;
    public Inventory inventory;
    void Start()
    {
        foreach(ItemSet itemSet in itemSets)
        {
            inventory.AddItem(itemSet.item, itemSet.amount);
        }
    }
}

[Serializable]
public class ItemSet
{
    public ItemSO item;
    public int amount;
}