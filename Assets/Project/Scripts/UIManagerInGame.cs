using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using StarterAssets;

public class UIManagerInGame : MonoBehaviour
{
    public RectTransform InGameOverlay;
    public RectTransform GameOverPanel;
    public RectTransform EscPanel;
    public TextMeshProUGUI textScoreResult;
    private bool isPause;

    private DontDestroy dontDestroy;
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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void GameOver()
    {
        GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = false;
        textScoreResult.text = "Score: " + GameObject.Find("GameManager").GetComponent<Score>().GetScore();
        InGameOverlay.gameObject.SetActive(false);
        GameOverPanel.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetPause()
    {
        if (!isPause)
        {
            Time.timeScale = 0;
            EscPanel.gameObject.SetActive(true);
            GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {
            GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = true;
            EscPanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        isPause = !isPause;
    }
    public void ExitInMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void Restart()
    {
        if (EscPanel.gameObject.activeInHierarchy)
        {
            EscPanel.gameObject.SetActive(false);
            SetPause();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = true;
        }
        if (dontDestroy.gameMode == "onTime")
        {
            GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
            GameObject.Find("GameManager").GetComponent<Timer>().RestartTimer();
        }
        else if (dontDestroy.gameMode == "numOfPoints")
        {
            GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
            GameObject.Find("GameManager").GetComponent<Score>().SetZeroScore();
        }
        else if (dontDestroy.gameMode == "numOfShots")
        {
            GameObject.Find("GameManager").GetComponent<Score>().SetZeroScore();
            GameObject.Find("AssaultRIfle_02").GetComponent<Shooting>().SetZeroShots();
        }
        if (GameOverPanel.gameObject.activeInHierarchy)
        {
            GameOverPanel.gameObject.SetActive(false);
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
