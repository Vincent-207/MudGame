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
        Debug.Log("applying damage!");
        health -= damage;
        if(health <= 0)
        {
            Debug.Log("applying damage 2!");  
            Die();
        }
    }

    void Die()
    {
        DropItems();
        Destroy(gameObject);
    }

    void DropItems()
    {
        Debug.Log("Starting drop");
        int dropAmount = Random.Range(damageable.itemDrop.minDrop, damageable.itemDrop.maxDrop);
        if(damageable == null) Debug.Log("null damageable");
        if(damageable.itemDrop == null) Debug.Log("null itemdrop");
        if(damageable.itemDrop.droppedItem == null) Debug.Log("null dropped item");
        GameObject drop = Instantiate(damageable.itemDrop.droppedItem, transform.position, Quaternion.identity);
        Debug.Log("made it");
        drop.GetComponent<Item>().amount = dropAmount;
        Debug.Log("after");
        
    }
}
