using System;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public Transform player;
    // Movement
    [SerializeField]
    float moveSpeed, maxSpeed, turnSpeed;
    Rigidbody rb;
    // Attacking
    bool isAttacking = false;
    float minimumAttackDistance, attacktime;
    [SerializeField] AttackCollisionHandler attackCollisionHandler;
    // Animations
    [SerializeField] Animator animator;
    String isAttackingParam;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackCollisionHandler.hit.AddListener(DoHit);
        
    }
    void FixedUpdate()
    {
        MoveAndTurnToPlayer();
        HandleAttacking();
    }

    void HandleAttacking()
    {
        if(CanAttack())
        {
            StartCoroutine(DoAttack());
            
        }
    }

    void DoHit()
    {
        Debug.Log("Applying damage to player - TODO");
    }

    bool CanAttack()
    {
        if(isAttacking) return false;
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if(distanceToPlayer > minimumAttackDistance) return false;
        return true;
    }

    IEnumerator DoAttack()
    {
        attackCollisionHandler.HandleAttack(attacktime);
        isAttacking = true;
        animator.SetBool(isAttackingParam, isAttacking);

        yield return new WaitForSeconds(attacktime);

        animator.SetBool(isAttackingParam, isAttacking);
        isAttacking = false;
    }
    void MoveAndTurnToPlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        // Prevent from providing upward force so enemies can't fly
        toPlayer.y = 0; 
        toPlayer.Normalize();

        // Move to player
        rb.AddForce(toPlayer * moveSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        Debug.DrawRay(transform.position, toPlayer * moveSpeed * Time.fixedDeltaTime);

        // Turn towards player
        float rot_z = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        // Quaternion toPlayerRot = Quaternion.Euler(0f, 0f, rot_z - 90);
        Quaternion lookAtRot = Quaternion.LookRotation(toPlayer, Vector3.up);
        Quaternion toPlayerRot =  Quaternion.Euler(0f, lookAtRot.y, 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toPlayerRot, turnSpeed * Time.fixedDeltaTime);
    }


}
