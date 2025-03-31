using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public TextMeshProUGUI timerText;
    private float timeElapsed;
    private bool timerStarted;

    void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        timerStarted = false;   
        timeElapsed = Time.time;
    }

    public void StartTimer() {
        timerStarted = true;
    }

    public void StopTimer() {
        timerStarted = false;
    }

    void Update() {
        if (!timerStarted) {
            timeElapsed = Time.time;
        } else {
            timerText.text = $"{string.Format("{0:N2}", Time.time - timeElapsed)} seconds";        
            SaveTime();
        }
    }

    public void SaveTime() {
        float maxTime = Math.Max(PlayerPrefs.GetFloat("Time", 0), (float)Math.Round(Time.time - timeElapsed, 2));
        PlayerPrefs.SetFloat("Time", maxTime);
    }
}
