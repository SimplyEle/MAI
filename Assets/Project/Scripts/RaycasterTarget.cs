using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterTarget : MonoBehaviour
{
    private Score score;

    private void Start()
    {
        score = GameObject.Find("GameManager").GetComponent<Score>();
    }

    public void RayTargetHit()
    {
        if (gameObject.CompareTag("Target1line"))
        {
            score.AddScore(10);
        }
        else if (gameObject.CompareTag("Target2line")) 
        {
            score.AddScore(20);
        }
        else if (gameObject.CompareTag("Target3line"))
        {
            score.AddScore(30);
        }
    }
}