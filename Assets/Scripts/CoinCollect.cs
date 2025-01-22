using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    
    public int coinValue = 1; // This will be the value of our coin
    public GameObject particleEffectPrefab;

    void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player")) {
            GameManager.Instance.AddCoin(coinValue);
            Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
