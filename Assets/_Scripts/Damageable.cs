using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Damageable", menuName = "Scriptable Objects/Damageable")]
public class Damageable : ScriptableObject
{
    public float health;
    public ToolType toolType;
    public int toolLevelRequired;
    public ItemDrop itemDrop;
}

[Serializable]
public class ItemDrop
{
    public GameObject droppedItem;
    public int minDrop, maxDrop;
}
