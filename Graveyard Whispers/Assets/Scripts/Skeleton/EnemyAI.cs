using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Graveyard.Utils;


public class EnemyAI : MonoBehaviour
{

    [SerializeField] private States _startingState;

    private States _currentState;
    private NavMeshAgent _navMeshAgent;

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
        switch (_currentState)
        {
            default:
            case States.Born:
                _navMeshAgent.velocity = Vector3.zero;
                break;
            case States.Chasing:
                _navMeshAgent.SetDestination(MainCharacter.Instance.MainCharacterTransform.position);
                ChangeFaceDirection(transform.position, MainCharacter.Instance.MainCharacterTransform.position);
                break;
            case States.Hurt:
                _navMeshAgent.velocity = Vector3.zero;
                break;
            case States.Attack:
                _navMeshAgent.velocity = Vector3.zero;
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
