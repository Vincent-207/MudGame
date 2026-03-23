using System;
using UnityEngine;

public class DebugPrint : MonoBehaviour
{
    public String toPrint;
    public void Print()
    {
        Debug.Log(toPrint);
    }
}
