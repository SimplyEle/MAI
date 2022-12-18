using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModes : MonoBehaviour
{
    public UIManagerInGame uiManager;
    private DontDestroy dontDestroy;

    private int maxScore;
    public float maxTime;
    private int maxShots;

    void Start()
    {
        dontDestroy = GameObject.Find("ObjectDontDestroy").GetComponent<DontDestroy>();
        if (dontDestroy.gameMode == "numOfShots")
        {
            if (dontDestroy.gameModeValue == 0)
            {
                maxShots = 10;
            }
            else if (dontDestroy.gameModeValue == 1)
            {
                maxShots = 20;
            }
            else if (dontDestroy.gameModeValue == 2)
            {
                maxShots = 50;
            }
        }
        else if (dontDestroy.gameMode == "numOfPoints")
        {
            if (dontDestroy.gameModeValue == 0)
            {
                maxScore = 200;
            }
            else if (dontDestroy.gameModeValue == 1)
            {
                maxScore = 1000;
            }
            else if (dontDestroy.gameModeValue == 2)
            {
                maxScore = 2000;
            }
        }
        else if (dontDestroy.gameMode == "onTime")
        {
            if (dontDestroy.gameModeValue == 0)
            {
                maxTime = 30;
            }
            else if (dontDestroy.gameModeValue == 1)
            {
                maxTime = 60;
            }
            else if (dontDestroy.gameModeValue == 2)
            {
                maxTime = 300;
            }
        }
    }

    
    void Update()
    {
        if (dontDestroy.gameMode == "numOfShots" && GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().GetShots() == maxShots)
        {
            uiManager.GameOver();
        }
        else if (dontDestroy.gameMode == "numOfPoints" && GameObject.Find("GameManager").GetComponent<Score>().GetScore() >= maxScore)
        {
            uiManager.GameOver();
        }
        else if (dontDestroy.gameMode == "onTime" && GameObject.Find("GameManager").GetComponent<Timer>().GetTime() == 0f)
        {
            uiManager.GameOver();
        }
    }
}
