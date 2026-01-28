using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AttackCollisionHandler : MonoBehaviour
{

    public UnityEvent hit;
    bool isAttacking = false;
    String playerTag = "Player";
    public void HandleAttack(float attackLength)
    {
        StartCoroutine(DoAttack(attackLength));
    }

    IEnumerator DoAttack(float length)
    {
        isAttacking = true;
        yield return new WaitForSeconds(length);
        isAttacking = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag(playerTag) && isAttacking)
        {
            hit.Invoke();
            // Prevent enemy from hitting the player multiple times in the same attack. 
            isAttacking = false;

            PlayerHealth playerHealth = collider.GetComponentInChildren<PlayerHealth>();
            if(playerHealth == null) return;
            
            playerHealth.DoDamage(10, ToolType.None, 0);
            
        }
    }


    
}
