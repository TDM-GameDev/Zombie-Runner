using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] FloatVariable hitPoints;
    [SerializeField] BoolVariable isAlive;
    [SerializeField] float startingHP = 100f;
    [SerializeField] DeathHandler deathHandler;

    private void Awake() {
        isAlive.Value = true;
        hitPoints.Value = startingHP;
    }

    void Update()
    {
        if (hitPoints.Value <= 0 && isAlive.Value) {
            //deathHandler.HandleDeath();
        }
    }

    public void TakeDamage(float dmgValue) {
        if (hitPoints.Value > 0) {
            hitPoints.Value -= dmgValue;
            Debug.Log($"Player Health: {hitPoints.Value}");
        }
    }
}
