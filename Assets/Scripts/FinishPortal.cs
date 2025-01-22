using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        Debug.Log($"Something entered the portal: {other.name}");
        if (other.CompareTag("Player")) {
            Debug.Log("Player entered the portal. Finishing run...");
            GameManager.Instance.SaveGameState();
            SceneTransitionManager.Instance.LoadScene("LeaderboardScene");
            if (SceneTransitionManager.Instance == null) {
                Debug.LogError("SceneTransitionManager.Instance is null!");
            } else {
                SceneTransitionManager.Instance.LoadScene("LeaderboardScene");
            }

        }
    }
}
