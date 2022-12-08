using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    private int score;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        score = 0;
        AddScore(0);
    }
    
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
}
