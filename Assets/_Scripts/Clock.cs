using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{   
    [SerializeField]
    TMP_Text display, dayDisplay;
    float time;

    bool nightTriggered = false;
    int day = 1;
    void Start()
    {
        display = GetComponent<TMP_Text>();
        dayDisplay = GetComponentInChildren<TMP_Text>();

    }
    void Update()
    {
        time += Time.deltaTime;

        if(time > 600 && !nightTriggered)
        {
            OnNight();
        }

        if(time > 725)
        {
            nightTriggered = false;
            time = 0;
        }

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        int minutes = (int) time / 60;
        int seconds = (int) time - minutes * 60;
        display.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");

    }
    
    void OnNight()
    {
        nightTriggered = true;
        FindFirstObjectByType<CreatureSpawning>().DoSpawn(day);
        day++;
    }
}
