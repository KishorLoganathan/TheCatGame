using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    public GameObject mainMenuUI;
    public GameObject leaderboardUI;
    public TMP_Text leaderboardText;

    private void Start() {
        ShowMainMenu();
    }

    public void PlayGame() {
        SceneManager.LoadScene("LevelScene");
    }

    public void ShowLeaderboard() {
        mainMenuUI.SetActive(false);
        leaderboardUI.SetActive(true);

        UpdateLeaderboardDisplay();
    }

    public void HideLeaderboard() {
        leaderboardUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void UpdateLeaderboardDisplay() {
        int count = PlayerPrefs.GetInt("LeaderboardCount", 0);
        if (count == 0) {
            leaderboardText.text = "No leaderboard entries";
        } else {
            string displayText = "";
            for (int i = 0; i < count; i++) {
                string entry = PlayerPrefs.GetString($"LeaderboardEntry_{i}", "");
                displayText += entry = "\n";
            }
            leaderboardText.text = displayText;
        }
    }

    private void ShowMainMenu() {
        mainMenuUI.SetActive(true);
        leaderboardUI.SetActive(false);
    }
}
