using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    private void Awake() {
        if (Instance == null) {

            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName) {
        //StartCoroutine(TransitionToScene(sceneName));
        Debug.Log($"Loading scene directly: {sceneName}");
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private IEnumerator TransitionToScene(string sceneName) {

        if (fadeCanvasGroup != null) {
            yield return StartCoroutine(Fade(1));
        }

        // Here, we are saving the players state
        GameManager.Instance.SaveGameState();

        SceneManager.LoadScene(sceneName);

        yield return null;

        if (fadeCanvasGroup != null) {
            yield return StartCoroutine(Fade(0));
        }

        GameManager.Instance.LoadGameState();

        Timer timer = FindObjectOfType<Timer>();
        if (timer != null) {
            timer.SetElapsedTime(GameManager.Instance.savedElapsedTime);
        }
    }

    private IEnumerator Fade(float targetAlpha) {
        float startAlpha = fadeCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;
    }
}
