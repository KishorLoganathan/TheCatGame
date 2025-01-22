using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.CompareTag("Player")) {
            GameManager.Instance.TakeDamage(damage);
            Debug.Log("Player hit by enemy!");
        }
    }
}
