using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using System;

[RequireComponent (typeof(Animator))]
public class SkeletonVisual : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _skeleton;
    
    private Animator _animator;

    private const string IS_DEAD = "IsDead";
    private const string ATTACK = "Attack";
    private const string HURT = "Hurt";
    private const string IS_IDLE = "IsIdle";
    private const string IS_RUNNING = "IsRun";
    private const string CHAISING_SPEED_MULTIPLIER = "ChaisingSpeedMultiplier";

    private void Awake()
    {
        _animator = GetComponent<Animator>(); 
    }
    private void Start()
    {
        _enemyAI.OnEnemyAttacking += OnAttackingAnimation;  
    }
    private void Update()
    {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(CHAISING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }
    private void OnDestroy()
    {
        _enemyAI.OnEnemyAttacking -= OnAttackingAnimation;
    }
    public void OnAttackingAnimation ()
    {
        _animator.SetTrigger(ATTACK);
    }
}
