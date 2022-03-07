using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage) {
        hitPoints -= damage;
        if (hitPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        Debug.Log($"{transform.name} has died!");
        Destroy(gameObject);
    }
}
