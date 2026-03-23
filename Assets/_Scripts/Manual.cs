using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

public class Manual : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public String[] texts =
    {
        "Stone is mineable from these boulder type nodes.",
        "Iron can be mined from these node with a red hue. It must be refined in a furnace into ingots to be useful.",
        "Mithril can be mined in these icey color nodes, it must be refined like iron.",
        "Wood is obtained by chopping trees like these down."

    };
    public Sprite[] images;
    int currentEntry = 0;
    TMP_Text textBox;
    Image image;
    void Start()
    {
        textBox = GetComponentInChildren<TMP_Text>();
        image = GetComponent<Image>();
    }
    public void LoadNext()
    {
        currentEntry++;
        if(currentEntry >= texts.Length)
        {
            currentEntry = texts.Length - 1;
        }

        UpdateDisplay();
    }

    public void LoadPrevious()
    {
        currentEntry--;
        if(currentEntry < 0)
        {
            currentEntry = 0;
        }

        UpdateDisplay();

    }

    public void UpdateDisplay()
    {
        image.sprite = images[currentEntry];
        textBox.text = texts[currentEntry];
    }
}

