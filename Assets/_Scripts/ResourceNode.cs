using System.Buffers.Text;
using System.Collections;
using Unity.VisualScripting;
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

        StartCoroutine(hitTween(0.5f, 1.5f));
    }

    IEnumerator hitTween(float duration, float maxTweenScale)
    {
        float baseScale = transform.localScale.x;
        float elapsedGrowTime = 0f;
        while (elapsedGrowTime < duration/2)
        {
            elapsedGrowTime += Time.deltaTime;
            transform.localScale = Vector3.one * CubicOutLerp(elapsedGrowTime, 1f, maxTweenScale, duration/2);
            yield return null;
        }

        float elapsedShrinkTime = 0f;
        while(elapsedShrinkTime < duration/2)
        {
            elapsedShrinkTime += Time.deltaTime;
            transform.localScale = Vector3.one * (3 - CubicOutLerp(elapsedShrinkTime, 1f, maxTweenScale, duration/2));
            yield return null;
        }
    }

    float Lerp(float min, float max, float t)
    {
        float progress = (max - min) * t;
        float output = min + progress;
        return output;
    }

    float CubicOutLerp(float currentTime, float baseValue, float maxValue, float duration)
    {
        currentTime /= duration;
        currentTime--;
        float output = maxValue * (currentTime * currentTime * currentTime + 1) + baseValue;
        output -= 0.5f;
        return output;
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
