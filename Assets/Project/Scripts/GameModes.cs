using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModes : MonoBehaviour
{
    private DontDestroy dontDestroy;

    private int maxScore;
    private int maxShots;

    public float maxTime;
    public UIManagerInGame uiManager;

    void Start()
    {
        dontDestroy = GameObject.Find("ObjectDontDestroy").GetComponent<DontDestroy>();

        switch (dontDestroy.gameMode)
        {
            case "numOfShots":
                switch (dontDestroy.gameModeValue)
                {
                    case 0:
                        maxShots = 10;
                        break;
                    case 1:
                        maxShots = 20;
                        break;
                    case 2:
                        maxShots = 50;
                        break;
                }
                break;
            case "numOfPoints":
                switch (dontDestroy.gameModeValue)
                {
                    case 0:
                        maxScore = 200;
                        break;
                    case 1:
                        maxScore = 1000;
                        break;
                    case 2:
                        maxScore = 2000;
                        break;
                }
                break;
            case "onTime":
                switch (dontDestroy.gameModeValue)
                {
                    case 0:
                        maxTime = 30;
                        break;
                    case 1:
                        maxTime = 60;
                        break;
                    case 2:
                        maxTime = 300;
                        break;
                }
                break;
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
