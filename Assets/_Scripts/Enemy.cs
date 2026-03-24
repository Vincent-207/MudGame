using System;
using System.Collections;
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
    [SerializeField]
    bool isAttacking = false;
    [SerializeField]
    float minimumAttackDistance, attacktime;
    [SerializeField] AttackCollisionHandler attackCollisionHandler;
    // Animations
    [SerializeField] Animator animator;
    String isAttackingParam = "IsAttacking";
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attackCollisionHandler.hit.AddListener(DoHit);
        player = FindAnyObjectByType<PlayerMovement>().transform;
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
        // Debug.Log("Applying damage to player - TODO");
    }

    bool CanAttack()
    {
        if(isAttacking) return false;
        float distanceToPlayer = (player.position - transform.position).magnitude;
        Debug.Log("distance to player: " + distanceToPlayer);
        if(distanceToPlayer > minimumAttackDistance) return false;
        return true;
    }

    IEnumerator DoAttack()
    {
        attackCollisionHandler.HandleAttack(attacktime);
        isAttacking = true;
        animator.SetBool(isAttackingParam, isAttacking);

        yield return new WaitForSeconds(attacktime);
        isAttacking = false;
        animator.SetBool(isAttackingParam, isAttacking);

        yield return new WaitForSeconds(attacktime); //cooldown
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

        TurnToPlayer();
        /* // Turn towards player
        float rot_z = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        // Quaternion toPlayerRot = Quaternion.Euler(0f, 0f, rot_z - 90);
        Quaternion lookAtRot = Quaternion.LookRotation(toPlayer, Vector3.up);
        Quaternion toPlayerRot =  Quaternion.Euler(0f, lookAtRot.y, 0f);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toPlayerRot, turnSpeed * Time.fixedDeltaTime); */
    }

    void TurnToPlayer()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0;
        toPlayer.Normalize();
        Quaternion lookAtPlayerRot = Quaternion.LookRotation(toPlayer, Vector3.up);
        transform.rotation = lookAtPlayerRot;
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtPlayerRot, turnSpeed * Time.fixedDeltaTime);
    }

}
