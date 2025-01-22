using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusAI : MonoBehaviour
{
    // This will be the AI script for the octopus enemy.

    public float detectionRadius = 10f;
    public float moveSpeed = 2f;
    public float retreatDistance = 3f;
    public float attackCooldown = 2f;


    private Transform player;
    private bool isAttacking = false;
    private bool isRetreating = false;
    private Vector3 retreatTarget;
    private float nextAttackTime = 0f;


    private void Start() {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform;
        }
    }

    private void Update() {
        if (player == null) {
            return;
        }

        float distanceToPlayer = Vector3.Distance (transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && !isAttacking && !isRetreating) {
            MoveTowardsPlayer();
        } else if (isRetreating) {
            RetreatFromPlayer();
        }
    }

    private void MoveTowardsPlayer() {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.position) < 1f) {
            StartCoroutine(AttackPlayer());
        }
    }

    private IEnumerator AttackPlayer() {

        isAttacking = true;

        GameManager.Instance.TakeDamage(1);

        // Here, we will retreat
        retreatTarget = transform.position - (player.position - transform.position).normalized * retreatDistance;
        isRetreating = true;

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void RetreatFromPlayer() {
        Vector3 direction = (retreatTarget - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, retreatTarget) < 0.1f) {
            isRetreating = false;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
