using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{   
    [SerializeField]
    TMP_Text display, dayDisplay;
    float time;

    bool nightTriggered = false;
    public int day = 1;
    public
    int nightTimeStart, nightTimeEnd;
    void Start()
    {
        display = GetComponent<TMP_Text>();
        dayDisplay = transform.GetChild(0).GetComponent<TMP_Text>();

    }
    void Update()
    {
        time += Time.deltaTime;

        if(time > nightTimeStart && !nightTriggered)
        {
            OnNight();
        }

        if(time > nightTimeEnd)
        {
            nightTriggered = false;
            day++;
            time = 0;
        }

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        int minutes = (int) time / 60;
        int seconds = (int) time - minutes * 60;
        display.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        dayDisplay.text = "Day " + day;
    }
    
    void OnNight()
    {
        nightTriggered = true;
        FindFirstObjectByType<CreatureSpawning>().DoSpawn(day);
    }
}
