using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    bool isAttacking;
    [SerializeField] InputActionReference attack;
    public float attackDuration, damage;
    List<IDamageable> attackedDuringSwing = new List<IDamageable>();

    void OnEnable()
    {
        attack.action.started += StartAttack;
    }

    void OnDisable()
    {
        attack.action.started -= StartAttack;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (isAttacking)
        {
            Debug.Log("Weapon entered: " + collider.name);
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if(damageable == null || attackedDuringSwing.Contains(damageable)) return;
            damageable.DoDamage(damage);
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

            yield return null;
            attackTimer -= Time.deltaTime;
        }
        isAttacking = false;
        attackedDuringSwing.Clear();
    }
}

public interface IDamageable
{
    public void DoDamage(float damage);
}
