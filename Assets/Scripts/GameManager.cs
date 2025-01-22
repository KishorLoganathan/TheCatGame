using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int coinCount = 0;
    public int maxHealth = 10;
    private int currentHealth;

    public TMP_Text healthText;
    public GameObject healthIcon;

    public Color greenColor = Color.green;
    public Color orangeColor = new Color(1f, 0.65f, 0f);
    public Color redColor = Color.red;

    public Transform playerTransform;
    public Camera mainCamera;
    public Animator playerAnimator;

    public float savedElapsedTime;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount) {
        coinCount += amount;
        Debug.Log("Coins Collected: " + coinCount);
    }

    public void ResetCoins() {

        coinCount = 0;
    }
    
    private void Start() {
        currentHealth = maxHealth;
        Debug.Log("Player health initialized: " + currentHealth);
        UpdateHealthUI();
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            currentHealth = 0;
            Debug.Log("Player is dead!");
            StartCoroutine(HandlePlayerDeath());
        }

        Debug.Log("Player Health: " + currentHealth);
        UpdateHealthUI();
    }

    private IEnumerator HandlePlayerDeath() {

        // This will set the player's death animation
        if (playerAnimator != null) {
            playerAnimator.SetTrigger("Die");
        } else {
            Debug.LogWarning("PlayerAnimator is missing. Skipping Animation.");
        }

        if (mainCamera != null) {
            // This will zoom the camera towards the player during death
            Vector3 initialPosition = mainCamera.transform.position;
            Vector3 targetPosition = playerTransform.position + new Vector3(0, 2, -2);
            float zoomDuration = 1.5f;
            float elapsedTime = 0f;

            while (elapsedTime < zoomDuration) {
                elapsedTime += Time.deltaTime;
                mainCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / zoomDuration);
                yield return null;
            } 
        } else {
            Debug.LogWarning("MainCamera is missing. Skipping camera zoom.");
        }
        

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        yield return new WaitForSeconds(0.1f);

        ReinitializeSceneReferences();
        ResetPlayerState();
        ResetCoins();
    }

    public void Heal(int amount) {
        currentHealth += amount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }

        Debug.Log("Player Healed. Current Health: " + currentHealth);
        UpdateHealthUI();
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }

    private void UpdateHealthUI() {
        if (healthText != null) {
            healthText.text = currentHealth.ToString();

            if (currentHealth >= 8) {
                healthText.color = greenColor;
            } else if (currentHealth >= 4) {
                healthText.color = orangeColor;
            } else {
                healthText.color = redColor;
            }
        }
    }

    private void ResetPlayerState() {

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void ReinitializeSceneReferences() {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            playerTransform = player.transform;
            playerAnimator = player.GetComponent<Animator>();
        }

        GameObject healthUI = GameObject.Find("HealthAmount");
        if (healthUI != null) {
            healthText = healthUI.GetComponent<TMP_Text>();
        }

        UpdateHealthUI();

        Camera camera = Camera.main;
        if (camera != null) {
            mainCamera = camera;
        }

        Debug.Log("Scene references reinitialized.");
    }

    public void SaveGameState() {
        PlayerPrefs.SetInt("CoinCount", coinCount);
        PlayerPrefs.SetInt("Health", currentHealth);
        PlayerPrefs.SetFloat("ElapsedTime", savedElapsedTime);
        Debug.Log("Game State Saved.");
    }

    public void LoadGameState() {
        coinCount = PlayerPrefs.GetInt("CoinCount", 0);
        currentHealth = PlayerPrefs.GetInt("Health", maxHealth);
        savedElapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);
        Debug.Log("Game state loaded.");
    }

    public void UpdateSavedTime(float elapsedTime) {
        savedElapsedTime = elapsedTime;
    }


}
