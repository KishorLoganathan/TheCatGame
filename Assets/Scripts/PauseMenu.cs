using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void PauseGame() {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame() {
        isPaused = false;
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenuScene");
    }
}
