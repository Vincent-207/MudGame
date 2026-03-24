using System;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    PlayerHealth playerHealth;
    TMP_Text textBox;
    void Start()
    {
        textBox = GetComponent<TMP_Text>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        String healthValue = ((int) playerHealth.health).ToString();
        textBox.text = "Health: " + healthValue;
    }
}
