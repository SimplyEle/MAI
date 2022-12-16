using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycasterTarget : MonoBehaviour
{
    private Score score;

    void Start()
    {
        score = GameObject.Find("GameManager").GetComponent<Score>();
        
    }

    public void RayTargetHit()
    {
        if (gameObject.CompareTag("Target1line"))
        {            
            Debug.Log("First");
            score.AddScore(10);
        }
        else if (gameObject.CompareTag("Target2line")) 
        {   
            Debug.Log("Second");
            score.AddScore(20);
        }
        else if (gameObject.CompareTag("Target3line"))
        {
            Debug.Log("Third");
            score.AddScore(30);
        }
    }
}