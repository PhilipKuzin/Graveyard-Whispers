using UnityEngine;
using UnityEngine.AI;
using GraveyardWhispers.Utils;
using System;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private States _startingState;

    [SerializeField] private float _roamingDistanceMin;
    [SerializeField] private float _roamingDistanceMax;
    [SerializeField] private float _roamingTimerMax;
    [SerializeField] private float _roamingTimer;

    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _nextAttackingTime;
    [SerializeField] private float _attackingRateTime;
    [SerializeField] private float _attackingDistance;

    [SerializeField] private bool _isChaisingEnemy = false;   
    [SerializeField] private float _chaisingDistance = 4f;
    [SerializeField] private float _chaisingSpeedMultiplier = 2f;

    [SerializeField] private float _nextCheckDirectionTime;
    [SerializeField] private float _checkDirectionDuration;

    private float _roamingSpeed;
    private float _chaisingSpeed;

    private States _currentState;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _startingPosition;
    private Vector3 _lastPosition;
    private Vector3 _roamPosition;
    
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
        Roaming,
        Chasing,
        Attacking,
        Idle,
        Death
    }
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;

        _roamingSpeed = _navMeshAgent.speed;
        _chaisingSpeed = _roamingSpeed * _chaisingSpeedMultiplier;
    }
    private void Start()
    {

    }
    private void Update()
    {
        StateHandler();
    }
    public float GetRoamingAnimationSpeed ()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }
    private void StateHandler()
    {
        switch (_currentState)
        {
            default:
            case States.Idle:
                break;
            case States.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case States.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case States.Chasing:
                CheckCurrentState();
                ChasingTarget();    
                break;
            case States.Death:
                break;

        }
    }
    private void Roaming()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamPosition();
        ChangeFaceDirection(_startingPosition, _roamPosition);
        _navMeshAgent.SetDestination(_roamPosition);
    }
    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(MainCharacter.Instance.MainCharacterTransform.position);
        ChangeFaceDirection(transform.position, MainCharacter.Instance.MainCharacterTransform.position);
    }

    private Vector3 GetRoamPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void AttackingTarget()
    {
        if (Time.time > _nextAttackingTime)
        {
            OnEnemyAttacking?.Invoke();
            _nextAttackingTime = Time.time + _attackingRateTime;
        }
    }
    private void CheckCurrentState()
    {
        States newState = States.Roaming;
        float distanceToMainCharacter = Vector3.Distance(transform.position, MainCharacter.Instance.MainCharacterTransform.position);

        if (_isChaisingEnemy)
        {
            if (distanceToMainCharacter <= _chaisingDistance)
                newState = States.Chasing;
        }
        if (_isAttackingEnemy)
        {
            if(distanceToMainCharacter <= _attackingDistance)
                newState = States.Attacking;
        }
        if (newState != _currentState)
        {
            if (newState == States.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chaisingSpeed;
            } 
            else if (newState == States.Roaming) 
            {
                _navMeshAgent.speed = _roamingSpeed;
                _roamingTimer = 0f;
            }
            else if (newState == States.Attacking)
            {
                _navMeshAgent.ResetPath();
            }
            _currentState = newState;
        }
    }
    private void ChangeFaceDirection(Vector3 selfPosition, Vector3 targetPosition)
    {
        if (selfPosition.x > targetPosition.x)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void StateChangerChaising()
    {
        _currentState = States.Chasing;
    }


}
