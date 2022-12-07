using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Crosshair : MonoBehaviour
{
    public Texture2D crosshair;

    public void OnGUI() 
    { 
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 40, 40), crosshair); 
    }
}
