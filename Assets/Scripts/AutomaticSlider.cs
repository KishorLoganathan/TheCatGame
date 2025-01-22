using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Events;

public class AutomaticSlider : MonoBehaviour
{
    
    [SerializeField, Min(0f)]
    float duration = 1f;

    float value;

    [System.Serializable]
    public class OnValueChangedEvent : UnityEvent<float> {  }

    [SerializeField]
    OnValueChangedEvent onValueChanged = default;

    [SerializeField]
    bool autoReverse = false, smoothstep = false;

    bool reversed;

    float SmoothedValue => 3f * value * value - 2f * value * value * value;

    void FixedUpdate() {
        float delta = Time.deltaTime / duration;
        if (reversed) {
            value -= delta;
            if (value <= 0f) {
                if (autoReverse) {
                    value = Mathf.Min(1f, -value);
                    reversed = false;
                } else {
                    value = 0f;
                    enabled = false;
                }
            }
        } else {
            value += delta;
            if (value >= 1f) {
                if (autoReverse) {
                    value = Mathf.Max(0f, 2f - value);
                    reversed = true;
                } else {
                    value = 1f;
                    enabled = false;
                }
            }
        }
        onValueChanged.Invoke(smoothstep ? SmoothedValue : value);
    }
}
