using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyEntity : MonoBehaviour, IDamagable
{
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log("Damage has been applied");
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }

   public void Attack()
   {
   //    if (IsStunned())
   //        return;

   //    Collider2D[] hitedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

   //    foreach (Collider2D mainCharacter in hitedObjects)
   //    {
   //        _enemyAI.StateChangerAttack();

   //        if (mainCharacter != null)
   //        {
   //            _timeBtwAttack = _startTimeBtwAttack;
   //        }
   //        else
   //        {
   //            Debug.Log("NullRefExep");
   //        }
   //    }
   }


    //private void DisableCollider()
    //{
    //    _collider.enabled = false;
    //}
    //private void EnableCollider()
    //{
    //    _collider.enabled = true;
    //}

    //private bool IsStunned()
    //{
    //    return _isStunning;
    //}

}
