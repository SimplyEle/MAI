using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public RectTransform StartPanel;
    public RectTransform GameModesPanel;
    public RectTransform TimePanel;
    public RectTransform PointsPanel;
    public RectTransform ShootsPanel;

    private GameModes gameModes;

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
        StartPanel.gameObject.SetActive(false);
        GameModesPanel.gameObject.SetActive(true);
    }

    public void SetGameModeOnTime()
    {
        gameModes.gameMode = "onTime";
    }
    public void SetGameModeNumOfShots()
    {
        gameModes.gameMode = "numOfShots";
    }
    public void SetGameModeNumOfPoints()
    {
        gameModes.gameMode = "numOfPoints";
    }

    public void SetGameModeValue(int value)
    {
        gameModes.gameModeValue = value;
    }
}
