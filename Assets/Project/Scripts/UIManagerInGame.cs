using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using StarterAssets;

public class UIManagerInGame : MonoBehaviour
{
    private DontDestroy dontDestroy;

    private bool isPause;

    public RectTransform inGameOverlay;
    public RectTransform gameOverPanel;
    public RectTransform escPanel;
    public TextMeshProUGUI textScoreResult;

    private void Start()
    {
        isPause = false;
        dontDestroy = GameObject.Find("ObjectDontDestroy").GetComponent<DontDestroy>();

        if (GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled == false)
        {
            GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = true;
        }

        if (dontDestroy.gameMode == "numOfPoints" || dontDestroy.gameMode == "numOfShots")
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
        GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = false;
        textScoreResult.text = "Score: " + GameObject.Find("GameManager").GetComponent<Score>().GetScore();
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
            GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = true;
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
            GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = true;
        }

        switch (dontDestroy.gameMode)
        {
            case "onTime":
                GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
                GameObject.Find("GameManager").GetComponent<Timer>().RestartTimer();
                break;
            case "numOfPoints":
                GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
                GameObject.Find("GameManager").GetComponent<Score>().SetZeroScore();
                break;
            case "numOfShots":
                GameObject.Find("GameManager").GetComponent<Score>().SetZeroScore();
                GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
                break;
        }

        if (gameOverPanel.gameObject.activeInHierarchy)
        {
            gameOverPanel.gameObject.SetActive(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
