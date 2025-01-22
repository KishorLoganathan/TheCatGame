using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaintbrushAttack : MonoBehaviour
{
    public GameObject beamPrefab;
    public Transform shootPoint;
    public string brushColour = "Blue";

    public void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            ShootBeam();
        }
    }

    public void UpdateBrushColour(string newColour) {
        brushColour = newColour;
        Debug.Log("Paintbrush colour updates to: " + brushColour);
    }

    private void ShootBeam() {
        if (beamPrefab != null && shootPoint != null) {
            GameObject beam = Instantiate(beamPrefab, shootPoint.position, Quaternion.LookRotation(-shootPoint.forward));

            PaintbrushBeam beamScript = beam.GetComponent<PaintbrushBeam>();
            if (beamScript != null) {
                beamScript.correctColour = brushColour;
                Debug.Log("Beam shot with colour: " + beamScript.correctColour);

                
            }

            Debug.Log("Paintbrush beam shot!");
        }
    }
}
