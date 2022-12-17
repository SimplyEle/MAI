using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManagerInGame : MonoBehaviour
{
    public RectTransform InGameOverlay;
    public RectTransform GameOverPanel;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        InGameOverlay.gameObject.SetActive(false);
        GameOverPanel.gameObject.SetActive(true);
    }

}
