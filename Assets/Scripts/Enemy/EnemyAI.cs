using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] BoolVariable playerIsAlive;
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float assistRange = 5f;
    [SerializeField] RPG.Events.Vector3Event aggroEvent;

    Animator animator;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    float attackDistance;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackDistance = navMeshAgent.stoppingDistance + .35f;
        float randomIdle = UnityEngine.Random.Range(0, 5);
        float randomLocomotion = UnityEngine.Random.Range(0, 5);
        float randomAttack = UnityEngine.Random.Range(0, 5);
        animator.SetFloat("randomIdle", UnityEngine.Random.Range(0, 5));
        animator.SetFloat("randomLocomotion", UnityEngine.Random.Range(0, 5));
        animator.SetFloat("randomAttack", UnityEngine.Random.Range(0, 5));
        Debug.Log($"{name} has randomIdle value of " + animator.GetFloat("randomIdle"));
        Debug.Log($"{name} has randomLocomotion value of " + animator.GetFloat("randomLocomotion"));
        Debug.Log($"{name} has randomAttack value of " + animator.GetFloat("randomAttack"));
        //animator.SetFloat("randomIdle", UnityEngine.Random.Range(0, 5));
        animator.speed = UnityEngine.Random.Range(.9f, 1.2f);
        Debug.Log($"animator.speed: {animator.speed}");
    }

    void Update()
    {
        if (!playerIsAlive.Value) {
            isProvoked = false;
            return;
        }
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked) {
            CallForHelp();
            EngageTarget();
        }
        else if (distanceToTarget < chaseRange) {
            isProvoked = true;
        }
        //else {
        //    isProvoked = false;
        //}
    }

    private void CallForHelp() {
        aggroEvent.Raise(transform.position);
    }

    void EngageTarget() {
        FaceTarget();
        if (distanceToTarget > attackDistance) {
            animator.ResetTrigger("attacking");
            ChaseTarget();
        }
        if (distanceToTarget <= attackDistance) {
            animator.ResetTrigger("moving");
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Blend Tree")) {
                animator.SetTrigger("attacking");
            }
        }
    }

    void ChaseTarget() {
        navMeshAgent.SetDestination(target.position);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion Blend Tree")) {
            animator.SetTrigger("moving");
        }
    }

    void FaceTarget() {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, assistRange);
    }

    public void OnDamageTaken() {
        isProvoked = true;
        //if (collision.gameObject.TryGetComponent(out SciFiArsenal.SciFiProjectileScript projectile)) {
        //    isProvoked = true;
        //}
    }

    public void AggroCheck(Vector3 callingZombie) {
        if (!isProvoked && Vector3.Distance(transform.position, callingZombie) < assistRange) {
            isProvoked = true;
        }
    }
}
