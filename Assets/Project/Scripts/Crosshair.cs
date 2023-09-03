using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Crosshair : MonoBehaviour
{
    public Texture2D crosshair;
    public int crosshairSize = 40;

    public void OnGUI() 
    { 
        GUI.DrawTexture(new Rect((Screen.width - crosshairSize) / 2, (Screen.height - crosshairSize) / 2, crosshairSize, crosshairSize), crosshair);
    }
}
