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
