using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    private DontDestroy gameModes;

    public RectTransform startPanel;
    public RectTransform gameModesPanel;

    private void Start()
    {
        gameModes = GameObject.Find("ObjectDontDestroy").GetComponent<DontDestroy>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SceneTargets");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChooseGameMode()
    {
        startPanel.gameObject.SetActive(false);
        gameModesPanel.gameObject.SetActive(true);
    }

    public void SetGameModeOnTimeFastest()
    {
        gameModes.gameMode = GameMode.OnTimeFastest;
    }
    public void SetGameModeOnTimeNormal()
    {
        gameModes.gameMode = GameMode.OnTimeNormal;
    }
    public void SetGameModeOnTimeSlowest()
    {
        gameModes.gameMode = GameMode.OnTimeSlowest;
    }
    public void SetGameModeNumOfShotsFew()
    {
        gameModes.gameMode = GameMode.NumOfShotsFew;
    }
    public void SetGameModeNumOfShotsMedium()
    {
        gameModes.gameMode = GameMode.NumOfShotsMedium;
    }
    public void SetGameModeNumOfShotsMany()
    {
        gameModes.gameMode = GameMode.NumOfShotsMany;
    }
    public void SetGameModeNumOfPointsFew()
    {
        gameModes.gameMode = GameMode.NumOfPointsFew;
    }
    public void SetGameModeNumOfPointsMedium()
    {
        gameModes.gameMode = GameMode.NumOfPointsMedium;
    }
    public void SetGameModeNumOfPointsMany()
    {
        gameModes.gameMode = GameMode.NumOfPointsMany;
    }
}
