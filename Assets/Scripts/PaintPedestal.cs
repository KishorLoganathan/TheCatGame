using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPedestal : MonoBehaviour
{
    
    public string requiredColour;
    public GameObject[] platforms;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Beam")) {
            PaintbrushBeam beam = other.GetComponent<PaintbrushBeam>();
            if (beam != null) {
                Debug.Log($"Pedestal hit by {beam.correctColour} beam.");
                if (beam.correctColour == requiredColour) {
                    Debug.Log("Correct colour beam received! Enabling platform.");

                    foreach (GameObject platform in platforms) {
                        platform.SetActive(true);
                    }

                    Destroy(other.gameObject);
                } else {
                    Debug.Log("Incorrect colour beam.");
                    Destroy(other.gameObject);
                }
            }
        }
    }

}
