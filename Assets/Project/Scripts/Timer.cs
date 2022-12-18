using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI timerText;
    private GameModes gameModes;

    private float timeLeft = 0f;
    private bool timerOn = false;

    private void Start()
    {
        gameModes = GameObject.Find("GameManager").GetComponent<GameModes>();
        timeLeft = gameModes.maxTime;
        timerOn = true;
    }

    private void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimeText();
            }
            else
            {
                timeLeft = gameModes.maxTime;
                timerOn = false;
            }
        }
    }

    private void UpdateTimeText()
    {
        if (timeLeft < 0)
            timeLeft = 0;

        float minutes = Mathf.FloorToInt(timeLeft / 60);
        float seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void SetTime(float maxTime)
    {
        gameModes.maxTime = maxTime;
    }

    public float GetTime()
    {
        return timeLeft;
    }

    public void RestartTimer()
    {
        Start();
    }
}