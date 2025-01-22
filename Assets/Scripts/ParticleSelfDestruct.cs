using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelfDestruct : MonoBehaviour
{
    private ParticleSystem ps;

    void Start() {
        ps = GetComponent<ParticleSystem>();
    }

    void Update() {
        if (ps != null && !ps.IsAlive()) {
            Destroy(gameObject);
        }
    }
}
