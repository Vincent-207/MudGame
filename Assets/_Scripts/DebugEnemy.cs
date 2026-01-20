using TMPro;
using UnityEngine;

public class DebugEnemy : MonoBehaviour, IDamageable
{
    public TMP_Text healthText;
    public float health;

    void Start()
    {
        UpdateSigns();
    }
    void UpdateSigns()
    {
        healthText.text = health.ToString();
    }
    public void DoDamage(float damage, ToolType toolType, int toolLevel)
    {
        health -= damage;
        UpdateSigns();
        if(health <= 0)
        {
            Debug.Log("Dead");
        }
    }
}
