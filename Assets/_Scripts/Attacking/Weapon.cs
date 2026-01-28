using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public InputActionReference attack;
    String isAttackingParamName = "IsAttacking";
    public float attackTime = 0.5f;
    Animator animator;
    void OnEnable()
    {
        attack.action.started += DoAttack;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Weapon started");
    }



    void DoAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log("Starting attack");
        StartCoroutine(DoAttackAnim());
    }

    IEnumerator DoAttackAnim()
    {
        animator.SetBool(isAttackingParamName, true);
        yield return new WaitForSeconds(attackTime);
        animator.SetBool(isAttackingParamName, false);
    }
}
