using System;
using NUnit.Framework.Constraints;
using UnityEngine;

[Serializable]
public enum SlotTag 
{
    None,
    Head,
    Chest,
    Legs,
    Feet
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/ New Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public SlotTag slotTag;
    public Sprite Icon;
    public int maxStackSize;
    public GameObject itemPrefab, handItemPrefab;
}
