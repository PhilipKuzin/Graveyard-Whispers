using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IDamagable
{

    [SerializeField] private SkeletonVisual _visual;
    private EnemyAI _enemyAI;
    private Collider2D _collider;

    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 1f;
    private float _startTimeBtwAttack = 0.6f;
    private float _timeBtwAttack;
    private bool _isStunning = false;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _enemyAI = GetComponent<EnemyAI>();
    }
    private void Start()
    {
        _visual.OnEnemyWasBorn += () => _enemyAI.StateChangerChaising();
        _visual.OnEnemyAttacking += MainCharacter.Instance.ApplyDamage;
        _visual.OnEnemyAttacked += () => _enemyAI.StateChangerChaising();
        _visual.OnEnemyHurtFinished += () =>
        {
            _enemyAI.StateChangerChaising();
            _isStunning = false;
            EnableCollider();
        };
    }
    private void Update()
    {
        _timeBtwAttack -= Time.deltaTime;

        if (_timeBtwAttack <= 0)
            Attack();
    }
    public void ApplyFatalDamage()
    {
        Debug.Log("Fatal Damage applied");

        _visual.DestroyAnimation();
        _enemyAI.StateChangerDead();

        DisableCollider();
        Destroy(this);
    }
    public void ApplyDamage()
    {
        Debug.Log("Damage applied");

        DisableCollider();
        _isStunning = true;

        _visual.HurtAnimation();
        _enemyAI.StateChangerHurt();
    }
    public void Attack()
    {
        if (IsStunned())
            return;

        Collider2D[] hitedObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D mainCharacter in hitedObjects)
        {
            _enemyAI.StateChangerAttack();

            if (mainCharacter != null)
            {
                _timeBtwAttack = _startTimeBtwAttack;
                _visual.AttackAnimation();
            }
            else
            {
                Debug.Log("NullRefExep");
            }
        }
    }
    private void DisableCollider()
    {
        _collider.enabled = false;
    }
    private void EnableCollider()
    {
        _collider.enabled = true;
    }
    private bool IsStunned()
    {
        return _isStunning;
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        else
            return;
    }

}
