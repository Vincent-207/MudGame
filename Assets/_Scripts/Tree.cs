using UnityEngine;

public class Tree : MonoBehaviour, IDamageable
{
    public void DoDamage(float damage, ToolType toolType)
    {
        
    }

    public void DoDamage(float damage, ToolType toolType, int toolLevel)
    {
        throw new System.NotImplementedException();
    }
}
