using UnityEngine;
using TMPro;

public class TimeAttack : MonoBehaviour
{
    #region 내부
    [SerializeField] private float LimitTime = 3.0f;
    [SerializeField] private float TimeRemaining;
    // bool
    private bool timerIsRunning = true;
    // UI 연결
    public TextMeshProUGUI TimeText;
    #endregion

    void Start()
    {
        TimeRemaining = LimitTime * 60;
        UpdateUI(TimeRemaining);
    }

    void Update()
    {
        if (GameManager.Instance.currentGameDirection != GameManager.GameDirection.Playing)
        {
            timerIsRunning = false;
            return;
        }

        timerIsRunning = true;

        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
        }
        else
        {
            TimeRemaining = 0;
            timerIsRunning = false;
            UpdateUI(TimeRemaining);
            GameManager.Instance.GameVictroy();
            return;
        }

        if (timerIsRunning)
        {
            UpdateUI(TimeRemaining);
        }
    }

    void UpdateUI(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60f); 
        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
    }
}
