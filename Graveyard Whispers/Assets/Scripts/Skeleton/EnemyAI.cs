using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Graveyard.Utils;
using System;
using System.Data;


public class EnemyAI : MonoBehaviour
{

    [SerializeField] private States _startingState;

    private States _currentState;
    private NavMeshAgent _navMeshAgent;

    private float _attackingDistance = 2f;
    private float _nextAttackTime = 0f;
    private float _attackRateTime = 0f;

    private float _nextCheckDirectionTime = 0f;
    private float _checkDirectionDuration = 0.1f;
    private Vector3 _lastPosition; 

    public event Action OnEnemyAttacking;
    public bool IsRunning
    {
        get
        {
            if (_navMeshAgent.velocity == Vector3.zero)
                return false;
            else
                return true;
        }
    }

    private enum States
    {
        Chasing,
        Hurt,
        Attack,
        Dead,
        Idle,
        Born
    }
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = States.Born;
    }
    private void Start()
    {
        MainCharacter.Instance.OnMainCharacterDead += MainCharacter_OnMainCharacterDead;
    }
    private void Update()
    {
        StateHandler();
        MovementDirectionHandler();
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            default:
            case States.Born:
                _navMeshAgent.velocity = Vector3.zero;
                break;
            case States.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case States.Hurt:
                _navMeshAgent.velocity = Vector3.zero;
                break;
            case States.Attack:
                AttackingTarget();
                CheckCurrentState();
                break;
            case States.Dead:
                Destroy(_navMeshAgent);
                Destroy(this);
                break;
            case States.Idle:
                _navMeshAgent.velocity = Vector3.zero;
                Destroy(_navMeshAgent);
                Destroy(this);
            break;
        }
    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            OnEnemyAttacking?.Invoke();
            _nextAttackTime = Time.time + _attackRateTime;
        }
        
    }

    private void CheckCurrentState()
    {
        States newState = States.Chasing;
        float distanceToMainCharacter = Vector3.Distance (transform.position, MainCharacter.Instance.MainCharacterTransform.position);

        if (distanceToMainCharacter < _attackingDistance)
        {
            newState = States.Attack;
        } 

        if (newState != _currentState)
        {
            if (newState == States.Attack)
            {
                _navMeshAgent.ResetPath();
            }
            _currentState = newState;
        }
        
    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(MainCharacter.Instance.MainCharacterTransform.position);
    }

    private void MovementDirectionHandler()
    {
        if (Time.time > _nextCheckDirectionTime)
        {
            if (IsRunning)
            {
                ChangeFaceDirection(_lastPosition, transform.position);
            } 
            else if (_currentState == States.Attack)
            {
                ChangeFaceDirection(transform.position, MainCharacter.Instance.MainCharacterTransform.position);
            }
            _lastPosition = transform.position;
            _nextCheckDirectionTime = Time.time + _checkDirectionDuration;
        }
    }
    private void ChangeFaceDirection(Vector3 selfPosition, Vector3 targetPosition)
    {
        if (selfPosition.x > targetPosition.x)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void MainCharacter_OnMainCharacterDead()
    {
        _currentState = States.Idle;
    }
    public void StateChangerChaising()
    {
        _currentState = States.Chasing;
    }
    public void StateChangerAttack()
    {
        _currentState = States.Attack;
    }
    public void StateChangerDead()
    {
        _currentState = States.Dead;
    }
    public void StateChangerHurt()
    {
        _currentState = States.Hurt;
    }
    
}
