using UnityEngine;

[CreateAssetMenu(fileName = "Damageable", menuName = "Scriptable Objects/Damageable")]
public class Damageable : ScriptableObject
{
    public float health;
    public ToolType toolType;
    public int toolLevelRequired;
    public GameObject droppedItem;
}
