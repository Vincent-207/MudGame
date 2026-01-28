using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum ToolType
{
    None,
    Ore,
    Wood
}
public class Tool : MonoBehaviour
{
    
    // Tool checking
    public ToolType toolType;
    public int toolLevel;

    // Damaging
    List<IDamageable> attackedDuringSwing = new List<IDamageable>();
    public float attackDuration, damage;

    // attack animation
    [SerializeField] InputActionReference attack;
    string isAttackingParam = "IsAttacking";
    public bool isAttacking;
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    void OnEnable()
    {
        attack.action.started += StartAttack;
    }

    void OnDisable()
    {
        attack.action.started -= StartAttack;
    }
    void OnTriggerStay(Collider collider)
    {
        if (isAttacking)
        {
            // Debug.Log("Weapon entered: " + collider.name);
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if(damageable == null || attackedDuringSwing.Contains(damageable)) return;
            damageable.DoDamage(damage, toolType, toolLevel);
            attackedDuringSwing.Add(damageable);
        }
    }

    void StartAttack(InputAction.CallbackContext callbackContext)
    {
        if(isAttacking) return;
        
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        float attackTimer = attackDuration;
        isAttacking = true;
        while(attackTimer > 0)
        {
            animator.SetBool(isAttackingParam, isAttacking);
            yield return null;
            attackTimer -= Time.deltaTime;
        }
        isAttacking = false;
        animator.SetBool(isAttackingParam, isAttacking);
        attackedDuringSwing.Clear();
    }
}

public interface IDamageable
{
    public void DoDamage(float damage, ToolType toolType, int toolLevel);
}
