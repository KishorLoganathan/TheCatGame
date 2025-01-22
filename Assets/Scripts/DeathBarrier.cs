using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player fell off the level!");
            GameManager.Instance.TakeDamage(GameManager.Instance.maxHealth);
        }
    }
}
