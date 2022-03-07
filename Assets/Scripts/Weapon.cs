using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField] float range = 1000f;
    [SerializeField] float rateOfFire = .5f; // Rounds per second
    [SerializeField] float damage = 15f;
    [SerializeField] float speed = 2000f;

    [SerializeField] bool isProjectile = false;
    [SerializeField] bool isBeam = false;

    [SerializeField] GameObject projectileObj;

    [SerializeField] Transform projectileSpawnPosition;

    Camera cam;

    //public InputAction weaponControls;

    public PlayerInputActions controls;
    private InputAction fire;

    private float timeSinceLastFired = Mathf.Infinity;

    private void Awake() {
        controls = new PlayerInputActions();
    }
    private void Start() {
    }

    void Update() {
        cam = Camera.main;
        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.yellow);
        HandleWeaponInput();
    }

    private void HandleWeaponInput() {
        if (fire.IsPressed() && timeSinceLastFired >= rateOfFire) {

            //Debug.Log("Firing!");
            timeSinceLastFired = 0;

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, range)) {
                FireWeapon(hit);
                //if (hit.transform.TryGetComponent(out EnemyHealth target)) {
                //    target.TakeDamage(damage);
                //}
            }
        }
        else {
            timeSinceLastFired += Time.deltaTime;
        }
    }


    private void FireWeapon(RaycastHit hit) {
        GameObject projectile = Instantiate(projectileObj, projectileSpawnPosition.position, Quaternion.identity) as GameObject; //Spawns the projectile
        if (projectile.TryGetComponent(out SciFiArsenal.SciFiProjectileScript projScript)) {
            projScript.SetDamage(damage);
            projScript.SetParentOfMuzzleFlash(projectileSpawnPosition);
        }
        projectile.transform.LookAt(hit.point); //Sets the projectile's rotation to look at the target
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * speed); //Set the speed of the projectile by applying force to the rigidbody
    }

    public void OnJump(InputValue value) {
        Debug.Log(value);
    }


    #region Enable/Disable
    private void OnEnable() {
        fire = controls.Player.Fire;
        fire.Enable();
    }

    private void OnDisable() {
        fire.Disable();
    }
    #endregion
}