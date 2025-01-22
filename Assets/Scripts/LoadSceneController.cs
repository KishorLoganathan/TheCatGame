using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    
    public SceneFader sceneFader;
    public TextMeshProUGUI loadingText;
    public float loadDelay = 3f;

    // These functions act as a controller to load both the load scene as well as the scene afte it, working for any direction of scene travel
    void Start() {

        string nextScene = PlayerPrefs.GetString("NextScene", "Level 1");

        loadingText.text = "Loading " + nextScene;

        StartCoroutine(LoadNextScene(nextScene));
    }

    IEnumerator LoadNextScene(string sceneName) {

        yield return new WaitForSeconds(loadDelay);

        sceneFader.FadeToScene(sceneName);
        
    }
}
