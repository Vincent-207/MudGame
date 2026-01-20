using UnityEngine;

public class ResourceNode : MonoBehaviour, IDamageable
{
    public Damageable damageable;
    public float health;
    void Start()
    {
        health = damageable.health;
    }
    public void DoDamage(float damage, ToolType toolType, int toolLevel)
    {
        if(damageable.toolLevelRequired == 0 || damageable.toolType == ToolType.None)
        {
            ApplyDamage(damage);
        }
        else if(damageable.toolType == toolType && damageable.toolLevelRequired <= toolLevel)
        {
            ApplyDamage(damage);
            
        }
        else
        {
            Debug.Log("Tool doesn't match. Not doing damage");
        }

    }

    void ApplyDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(damageable.droppedItem, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
