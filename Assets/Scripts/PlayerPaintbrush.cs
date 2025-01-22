using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaintbrush : MonoBehaviour
{
    public Renderer paintbrushRenderer;
    public GameObject beamPrefab;
    private string currentColour = "Red";

    public void ChangePaintColour(string newColour, Material newBrushMaterial, Material newBeamMaterial) {

        if (paintbrushRenderer != null) {
            paintbrushRenderer.material = newBrushMaterial;
        }

        if (beamPrefab != null) {
            Renderer beamRenderer = beamPrefab.GetComponent<Renderer>();
            if (beamRenderer != null) {
                beamRenderer.material = newBeamMaterial;
            }
        }

        currentColour = newColour;
        Debug.Log("Paintbrush colour changed to: " + currentColour);

        PlayerPaintbrushAttack attackScript = GetComponent<PlayerPaintbrushAttack>();
        if (attackScript != null) {
            attackScript.UpdateBrushColour(currentColour);
        }
    }

    public string GetCurrentColour() {
        return currentColour;
    }
}
