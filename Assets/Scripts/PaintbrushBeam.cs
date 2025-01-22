using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintbrushBeam : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 1;

    public string correctColour = "Blue";

    private void Start() {
        Destroy(gameObject, lifetime);

    }

    private void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null) {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        /* else if (other.CompareTag("NoPaint")){
            NoPaintSpot spot = other.GetComponent<NoPaintSpot>();
            if (spot != null && spot.GetColour() == correctColour) {
                spot.Paint();
            }
            Destroy(gameObject);
        } else {
            Destroy(gameObject);
        }
        */
    }
}
