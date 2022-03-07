using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float stoppingDistance = .5f;
    [SerializeField] float turnSpeed = 5f;

    Animator animator;
    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    float velocity = 0f;
    Vector3 previousPosition;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked) {
            EngageTarget();
        }
        else if (distanceToTarget < chaseRange) {
            isProvoked = true;
        }
        //else {
        //    isProvoked = false;
        //}
    }

    void EngageTarget() {
        FaceTarget();
        if (distanceToTarget >= navMeshAgent.stoppingDistance) {
            animator.ResetTrigger("attacking");
            ChaseTarget();
        }
        if (distanceToTarget <= navMeshAgent.stoppingDistance) {
            animator.ResetTrigger("moving");
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking")) {
                animator.SetTrigger("attacking");
            }
        }
    }

    void ChaseTarget() {
        navMeshAgent.SetDestination(target.position);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ForwardMovement")) {
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
    }
}
