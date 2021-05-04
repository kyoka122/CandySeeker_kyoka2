using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAttributeList : MonoBehaviour
{
    [System.NonSerialized] public string color1;
    [System.NonSerialized] public string color2;
    [System.NonSerialized] public string job;

    public void ColorSettings1(string localColor)
    {
        color1 = localColor;
        color2 = localColor;
    }

    public void ColorSettings2(string localColor)
    {
        color2 = localColor;
    }

    public void JobSettings(string localJob)
    {
        job = localJob;
    }
}
