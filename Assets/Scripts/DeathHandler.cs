using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] BoolVariable isAlive;
    [SerializeField] Canvas reticleCanvas;
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInput;
    [SerializeField] UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController rbController;

    private void Awake() {
        gameOverCanvas.enabled = false;
        reticleCanvas.enabled = true;

        if (playerInput.currentActionMap.name != "Player") {
            Cursor.lockState = CursorLockMode.Locked;
            rbController.enabled = true;
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    public void HandleDeath() {
        if (TryGetComponent(out UnityEngine.AI.NavMeshAgent agent)) {
            agent.enabled = false;
        }
        reticleCanvas.enabled = false;
        gameOverCanvas.enabled = true;

        //Animation[] animations = gameOverCanvas.GetComponentsInChildren<Animation>();
        //foreach (Animation animation in animations) {
        //    animation.Play();
        //}

        //Animator[] animators = gameOverCanvas.GetComponentsInChildren<Animator>();
        //foreach (Animator animator in animators) {
        //    animator.enabled = true;
        //}

        Time.timeScale = .1f;
        DisableWeaponScript();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        rbController.enabled = false;
        playerInput.SwitchCurrentActionMap("UI");
        isAlive.Value = false;
        Debug.Log("Player has died.");
    }

    private void DisableWeaponScript() {
        Weapon[] weapons = GetComponentsInChildren<Weapon>();
        for (int i = 0; i < weapons.Length; i++) {
            weapons[i].enabled = false;
        }
    }
}
