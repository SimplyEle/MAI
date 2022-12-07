using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int score;

    void Start()
    {
        score = 0;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 100), "Score: "+ score);
    }
    
    public void AddScore(int points)
    {
        score += points;
    }
}
