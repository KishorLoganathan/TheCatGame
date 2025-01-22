using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string targetSceneName;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player entered the transition trigger. Loading scene: " + targetSceneName);
            SceneTransitionManager.Instance.LoadScene(targetSceneName);
        }
    }
}
