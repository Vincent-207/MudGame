using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    
    public float health;
    public float armour;
    float maxHealth;
    [SerializeField]
    float regenRate;
    public void DoDamage(float damage, ToolType toolType, int toolLevel)
    {
        health += Mathf.Min(armour - damage, 0);

        if(health <= 0)
        {
            Die();   
        }
    }

    void Die()
    {
        Debug.Log("I'm a little dead guy!");
        // Debug.Break();
        Clock clock = FindAnyObjectByType<Clock>();
        int daysSurvived = clock.day;
        PlayerPrefs.SetInt("DaysSurvived", daysSurvived);
        PlayerPrefs.Save();
        SceneManager.LoadScene(1);
    }
    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        health += Time.deltaTime * regenRate;
        if(health > maxHealth) health = maxHealth;
    }


    
}
