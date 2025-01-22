using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;

    public ParticleSystem deathParticlesPrefab;
    public float launchForce = 5f;
    public float deathDelay = 1f;

    private Rigidbody rb;

    private void Start() {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody>();
        if (rb == null) {
            Debug.LogWarning("Rigidbody is missing! Adding one for physics-based death effect.");
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
    }

    // This function will be for when the enemy takes damage
    public void TakeDamage(int damage) {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage! Current health: " + currentHealth);

        if (currentHealth <= 0) {
            EnemyDeath();
        }
    }

    private void EnemyDeath() {

        Debug.Log(gameObject.name + " had died!");

        DisableEnemy();

        rb.isKinematic = false;

        Vector3 launchDirection = -transform.forward + Vector3.up;
        rb.isKinematic = false;
        rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);

        StartCoroutine(DeathSequence());
        
    }

    private void DisableEnemy() {

        // Disable colliders, movement scripts, and AI
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders) {
            col.enabled = false;
        }
    }

    private IEnumerator DeathSequence() {
        if (deathParticlesPrefab != null) {
            ParticleSystem deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            deathParticles.Play();
            Destroy(deathParticles.gameObject, deathParticles.main.duration + deathParticles.main.startLifetime.constant);

        }

        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }
}
