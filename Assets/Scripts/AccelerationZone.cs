using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    [SerializeField, Min(0f)]
    float acceleration = 10f, speed = 10f;

    void OnTriggerEnter (Collider other) {
        Rigidbody body = other.attachedRigidbody;
        if (body) {
            Accelerate(body);
        }
    }

    void Accelerate(Rigidbody body) {
        Vector3 velocity = transform.InverseTransformDirection(body.velocity);
        if (velocity.y >= speed) {
            return;
        }

        velocity.y = speed;
        body.velocity = transform.TransformDirection(velocity);

        if(body.TryGetComponent(out MovingSphere sphere)) {
            sphere.PreventSnapToGround();
        }

        if (acceleration > 0f) {
            velocity.y = Mathf.MoveTowards(velocity.y, speed, acceleration * Time.deltaTime);
        } else {
            velocity.y = speed;
        }
    }

    void OnTriggerStay (Collider other) {
        Rigidbody body = other.attachedRigidbody;
        if (body) {
            Accelerate(body);
        }
    }
}
