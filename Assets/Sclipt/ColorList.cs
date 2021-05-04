using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorList : MonoBehaviour
{
    /*public Color gNameListColor(string color)
    {
        switch (color)
        {
            case "red":  
                return new Color(255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f);
            case "blue":
                return new Color(0.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
            case "yellow":
                return new Color(255.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
            case "green":
                return new Color(0.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
            case "brown":
                return new Color(222.0f / 255.0f, 156.0f / 255.0f, 52.0f / 255.0f);
            case "purple":
                return new Color(144.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f);
            case "lightblue":
                return new Color(0.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
            case "black":
                return new Color(0.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f);
            case "pink":
                return new Color(255.0f / 255.0f, 126.0f / 255.0f, 255.0f / 255.0f);
            case "lightgreen":
                return new Color(209.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f);
            case "bluegreen":
                return new Color(0.0f / 255.0f, 255.0f / 255.0f, 194.0f / 255.0f);
            default:
                Debug.Log("Color NotFound");
                return new Color(1.0f,1.0f,1.0f);
        }
    }*/

    public Color NameListColor(string color)
    {
        switch (color)
        {
            case "red":
                return new Color(255.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f, 0.25f);
            case "blue":
                return new Color(0.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f, 0.25f);
            case "yellow":
                return new Color(255.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f, 0.25f);
            case "green":
                return new Color(0.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f, 0.5f);
            case "brown":
                return new Color(222.0f / 255.0f, 156.0f / 255.0f, 52.0f / 255.0f, 0.25f);
            case "purple":
                return new Color(144.0f / 255.0f, 0.0f / 255.0f, 255.0f / 255.0f, 0.25f);
            case "lightblue":
                return new Color(0.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f, 0.25f);
            case "black":
                return new Color(0.0f / 255.0f, 0.0f / 255.0f, 0.0f / 255.0f, 0.25f);
            case "pink":
                return new Color(255.0f / 255.0f, 126.0f / 255.0f, 255.0f / 255.0f, 0.25f);
            case "lightgreen":
                return new Color(209.0f / 255.0f, 255.0f / 255.0f, 0.0f / 255.0f, 0.25f);
            case "bluegreen":
                return new Color(0.0f / 255.0f, 255.0f / 255.0f, 145.0f / 255.0f, 0.25f);
            default:
                Debug.Log("Color NotFound");
                return new Color(1.0f, 1.0f, 1.0f, 0.25f);
        }
    }
}   
