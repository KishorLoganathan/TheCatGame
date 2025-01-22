using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPaintSpot : MonoBehaviour
{
    private string colour;
    private bool isPainted = false;

    public void SetColour(string newColour) {
        colour = newColour;
    }

    public string GetColour() {
        return colour;
    }

    public void Paint() {
        if (!isPainted) {
            Debug.Log("Spot painted with " + colour);
            isPainted = true;

            GetComponent<Renderer>().material.color = Color.blue;
        }
    }
}
