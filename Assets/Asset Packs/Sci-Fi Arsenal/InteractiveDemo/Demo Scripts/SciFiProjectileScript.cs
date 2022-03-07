using UnityEngine;
using System.Collections;

namespace SciFiArsenal
{
    public class SciFiProjectileScript : MonoBehaviour
    {
        public GameObject impactParticle;
        public GameObject projectileParticle;
        public GameObject muzzleParticle;
        public GameObject[] trailParticles;
        [HideInInspector]
        public Vector3 impactNormal; //Used to rotate impactparticle.

        private Transform parentOfMuzzleFlash;
        private float damage = 0;
        private bool hasCollided = false;

        public float Damage => damage;
        public Transform ParentOfMuzzleFlash => parentOfMuzzleFlash;

        public void SetDamage(float dmg) {
            damage = dmg;
        }

        public void SetParentOfMuzzleFlash(Transform parentTransform) {
            parentOfMuzzleFlash = parentTransform;
        }

        void Start() {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
            if (muzzleParticle) {
                muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation, parentOfMuzzleFlash) as GameObject;
                muzzleParticle.transform.rotation = transform.rotation * Quaternion.Euler(180, 0, 0);
                Destroy(muzzleParticle, .5f); // Lifetime of muzzle effect.
            }
        }

        void OnCollisionEnter(Collision hit) {
            if (!hasCollided) {
                hasCollided = true;
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

                if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
                {
                    Destroy(hit.gameObject);
                }
                EnemyHealth target = hit.gameObject.GetComponentInParent<EnemyHealth>();
                if (target != null) {
                    target.TakeDamage(damage);
                }

                foreach (GameObject trail in trailParticles) {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }
                Destroy(projectileParticle, 3f);
                Destroy(impactParticle, 5f);
                Destroy(gameObject);

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++) {

                    ParticleSystem trail = trails[i];

                    if (trail.gameObject.name.Contains("Trail")) {
                        trail.transform.SetParent(null);
                        Destroy(trail.gameObject, 2f);
                    }
                }
            }
        }
    }
}