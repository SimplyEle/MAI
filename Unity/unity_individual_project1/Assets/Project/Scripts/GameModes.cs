using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    NumOfShotsFew = 10,
    NumOfShotsMedium = 20,
    NumOfShotsMany = 50,
    NumOfPointsFew = 200,
    NumOfPointsMedium = 1000,
    NumOfPointsMany = 2000,
    OnTimeFastest = 30,
    OnTimeNormal = 60,
    OnTimeSlowest = 300
}

public class GameModes : MonoBehaviour
{
    private DontDestroy dontDestroy;
    private Shooting getShots;
    private Score getScore;
    private Timer getTimer;

    private int maxScore;
    private int maxShots;
    private List<GameMode> numOfShotsList = new List<GameMode>
        {
            GameMode.NumOfShotsFew,
            GameMode.NumOfShotsMedium,
            GameMode.NumOfShotsMany
        };
    private List<GameMode> numOfPointsList = new List<GameMode>
        {
            GameMode.NumOfPointsFew,
            GameMode.NumOfPointsMedium,
            GameMode.NumOfPointsMany
        };
    private List<GameMode> onTimeList = new List<GameMode>
        {
            GameMode.OnTimeFastest,
            GameMode.OnTimeNormal,
            GameMode.OnTimeSlowest
        };

    public float maxTime;
    public UIManagerInGame uiManager;

    private void Start()
    {
        dontDestroy = GameObject.Find("ObjectDontDestroy").GetComponent<DontDestroy>();
        getShots = GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>();
        getScore = GameObject.Find("GameManager").GetComponent<Score>();
        getTimer = GameObject.Find("GameManager").GetComponent<Timer>();        

        ChoosingGameMode();
    }

    private void Update()
    {
        ShowGameOverPanel();
    }

    private void ChoosingGameMode()
    {
        switch (dontDestroy.gameMode)
        {
            case GameMode.NumOfShotsFew:
                maxShots = (int)GameMode.NumOfShotsFew;
                break;
            case GameMode.NumOfShotsMedium:
                maxShots = (int)GameMode.NumOfShotsMedium;
                break;
            case GameMode.NumOfShotsMany:
                maxShots = (int)GameMode.NumOfShotsMany;
                break;
            case GameMode.NumOfPointsFew:
                maxScore = (int)GameMode.NumOfPointsFew;
                break;
            case GameMode.NumOfPointsMedium:
                maxScore = (int)GameMode.NumOfPointsMedium;
                break;
            case GameMode.NumOfPointsMany:
                maxScore = (int)GameMode.NumOfPointsMany;
                break;
            case GameMode.OnTimeFastest:
                maxTime = (int)GameMode.OnTimeFastest;
                break;
            case GameMode.OnTimeNormal:
                maxTime = (int)GameMode.OnTimeNormal;
                break;
            case GameMode.OnTimeSlowest:
                maxTime = (int)GameMode.OnTimeSlowest;
                break;
        }
    }

    private void ShowGameOverPanel()
    {

        if (numOfShotsList.Contains(dontDestroy.gameMode) && getShots.GetShots() == maxShots)
        {
            uiManager.GameOver();
        }
        else if (numOfPointsList.Contains(dontDestroy.gameMode) && getScore.GetScore() >= maxScore)
        {
            uiManager.GameOver();
        }
        else if (onTimeList.Contains(dontDestroy.gameMode) && getTimer.GetTime() == 0f)
        {
            uiManager.GameOver();
        }
    }
    
    public List<GameMode> GetNumOfShotsList()
    {
        return numOfShotsList;
    }
    public List<GameMode> GetNumOfPointsList()
    {
        return numOfPointsList;
    }
    public List<GameMode> GetOnTimeList()
    {
        return onTimeList;
    }

}
