using System;
using UnityEngine;

public class DebugAddItem : MonoBehaviour
{
    [SerializeField]
    DebugItemAddSet[] itemSets;
    public Inventory inventory;
    void Start()
    {
        foreach(DebugItemAddSet itemSet in itemSets)
        {
            inventory.AddItem(itemSet.item, itemSet.amount);
        }
    }
}

[Serializable]
public class DebugItemAddSet
{
    public ItemSO item;
    public int amount;
}