using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour
{
    public Text fpsText; // Reference to a UI Text element for displaying FPS
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculate frame time in seconds
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        // Calculate frames per second
        float fps = 1.0f / deltaTime;

        // Display FPS in the UI Text element
        fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
    }
}
