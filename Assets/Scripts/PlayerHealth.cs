using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] FloatVariable hitPoints;
    [SerializeField] BoolVariable isAlive;
    [SerializeField] float startingHP = 100f;
    //private float hitPoints = 100f;

    //public float HitPoints => hitPoints;

    private void Start() {
        isAlive.Value = true;
        hitPoints.Value = startingHP;
    }

    void Update()
    {
        if (hitPoints.Value <= 0 && isAlive.Value) {
            Die();
        }
    }

    private void Die() {
        isAlive.Value = false;
        Debug.Log("Player has died.");
    }

    public void TakeDamage(float dmgValue) {
        if (hitPoints.Value > 0) {
            hitPoints.Value -= dmgValue;
            Debug.Log($"Player Health: {hitPoints.Value}");
        }
    }
}
