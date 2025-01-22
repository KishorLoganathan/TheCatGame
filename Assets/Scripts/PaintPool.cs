using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPool : MonoBehaviour
{
    public string paintColour;
    public Material beamMaterial;
    public Material paintbrushMaterial;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player stepped into " + paintColour + " pool.");

            PlayerPaintbrush playerPaintbrush = other.GetComponent<PlayerPaintbrush>();
            if (playerPaintbrush != null) {
                playerPaintbrush.ChangePaintColour(paintColour, paintbrushMaterial, beamMaterial);
            }
        }
    }
}
