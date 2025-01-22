using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;
    private float elapsedTime = 0f;
    private bool isTimerRunning = true;

    void Update() {
        if (isTimerRunning) {
            elapsedTime += Time.deltaTime;

            UpdateTimerDisplay();
        }

        GameManager.Instance.UpdateSavedTime(elapsedTime);
    }

    private void UpdateTimerDisplay() {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = string.Format("Timer: {0:00}:{1:00}", minutes, seconds);

    }

    public void StartTimer() {
        isTimerRunning = true;
    }

    public void StopTimer() {
        isTimerRunning = false;
    }

    public float GetElapsedTime() {
        return elapsedTime;
    }

    public void SetElapsedTime(float time) {
        elapsedTime = time;
        UpdateTimerDisplay();
    }
    public void ResetTimer() {
        elapsedTime = 0f;
        UpdateTimerDisplay();
    }
}
