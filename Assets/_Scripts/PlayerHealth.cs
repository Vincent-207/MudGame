using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    
    public float health;
    public float armour;
    public void DoDamage(float damage, ToolType toolType, int toolLevel)
    {
        health += Mathf.Min(armour - damage, 0);
    }


    
}
