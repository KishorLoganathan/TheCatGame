using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public Transform player;
    public float despawnDistance = 50f;
    public float spawnDistance = 30f;

    private GameObject[] coins;

    void Start() {
        coins = GameObject.FindGameObjectsWithTag("Coin");
    }
    void Update() {

        foreach (GameObject coin in coins) {
            if (coin == null) {
                continue;
            }

            float distanceToPlayer = Vector3.Distance(player.position, coin.transform.position);

            if (distanceToPlayer > despawnDistance && coin.activeSelf) {
                coin.SetActive(false);
            } else if (distanceToPlayer <= spawnDistance && !coin.activeSelf) {
                coin.SetActive(true);
            }

        }
        
    }
}
