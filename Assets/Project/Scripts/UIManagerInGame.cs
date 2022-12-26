using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using StarterAssets;

public class UIManagerInGame : MonoBehaviour
{
    private DontDestroy dontDestroy;
    private GameModes gameModes;
    private FirstPersonController firstPersonController;
    private Score score;

    private bool isPause;

    public RectTransform inGameOverlay;
    public RectTransform gameOverPanel;
    public RectTransform escPanel;
    public TextMeshProUGUI textScoreResult;

    private void Start()
    {
        isPause = false;
        dontDestroy = GameObject.Find("ObjectDontDestroy").GetComponent<DontDestroy>();
        gameModes = GameObject.Find("GameManager").GetComponent<GameModes>();
        firstPersonController = GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>();
        score = GameObject.Find("GameManager").GetComponent<Score>();

        if (firstPersonController.enabled == false)
        {
            firstPersonController.enabled = true;
        }

        if (gameModes.GetNumOfPointsList().Contains(dontDestroy.gameMode) || gameModes.GetNumOfShotsList().Contains(dontDestroy.gameMode))
        {
            GameObject.Find("TimerText").SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SetPause();
        }
    }

    public void GameOver()
    {
        firstPersonController.enabled = false;
        textScoreResult.text = "Score: " + score.GetScore();
        inGameOverlay.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetPause()
    {
        if (!isPause)
        {
            Time.timeScale = 0;
            escPanel.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            firstPersonController.enabled = false;
        }
        else
        {
            firstPersonController.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            escPanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        isPause = !isPause;
    }

    public void ExitInMainMenu()
    {
        if (isPause)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene("StartScene");
    }

    public void Restart()
    {
        if (escPanel.gameObject.activeInHierarchy)
        {
            escPanel.gameObject.SetActive(false);
            SetPause();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            firstPersonController.enabled = true;
        }

        SetZeroAll();

        if (gameOverPanel.gameObject.activeInHierarchy)
        {
            gameOverPanel.gameObject.SetActive(false);
        }

        inGameOverlay.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetZeroAll()
    {
        switch (dontDestroy.gameMode)
        {
            case GameMode.OnTimeFastest:
                SetZeroScoreShotsTime();
                break;
            case GameMode.OnTimeNormal:
                SetZeroScoreShotsTime();
                break;
            case GameMode.OnTimeSlowest:
                SetZeroScoreShotsTime();
                break;
            case GameMode.NumOfPointsFew:
                SetZeroScoreShots();
                break;
            case GameMode.NumOfPointsMedium:
                SetZeroScoreShots();
                break;
            case GameMode.NumOfPointsMany:
                SetZeroScoreShots();
                break;
            case GameMode.NumOfShotsFew:
                SetZeroScoreShots();
                break;
            case GameMode.NumOfShotsMedium:
                SetZeroScoreShots();
                break;
            case GameMode.NumOfShotsMany:
                SetZeroScoreShots();
                break;
        }
    }

    private void SetZeroScoreShots()
    {
        GameObject.Find("GameManager").GetComponent<Score>().SetZeroScore();
        GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
    }

    private void SetZeroScoreShotsTime()
    {
        GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
        GameObject.Find("GameManager").GetComponent<Score>().SetZeroScore();
        GameObject.Find("GameManager").GetComponent<Timer>().RestartTimer();
    }
}
