using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using System;

public class SkeletonVisual : MonoBehaviour
{
    public event Action OnEnemyAttacking;
    public event Action OnEnemyAttacked;
    public event Action OnEnemyHurtFinished;
    public event Action OnEnemyWasBorn;
    
    private Animator _animator;

    private const string IS_DEAD = "IsDead";
    private const string ATTACK = "Attack";
    private const string HURT = "Hurt";
    private const string IS_IDLE = "IsIdle";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
    }
    private void Start()
    {
        MainCharacter.Instance.OnMainCharacterDead += MainCharacter_OnMainCharacterDead;
    }
    
    private void EnemyAttacked ()
    {
        OnEnemyAttacked?.Invoke();
    }
    private void HurtFinished ()
    {
        OnEnemyHurtFinished?.Invoke();
    }
    public void DestroyAnimation ()
    {
        _animator.SetBool(IS_DEAD, true); 
    }
    public void AttackAnimation ()
    {
        _animator.SetTrigger(ATTACK);
    }
    public void HurtAnimation ()
    {
        _animator.SetTrigger(HURT);
    }
    private void MainCharacter_OnMainCharacterDead()
    {
        _animator.SetBool(IS_IDLE, true);
    }
    public void EnemyAttack()
    {
        OnEnemyAttacking?.Invoke();  
    }
    public void EnemyWasBorn ()
    {
        OnEnemyWasBorn?.Invoke();
    }
}
