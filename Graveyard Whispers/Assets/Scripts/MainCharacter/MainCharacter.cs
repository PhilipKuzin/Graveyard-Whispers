using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MainCharacter : MonoBehaviour, IDamagable
{
    public event Action OnMainCharacterDead;
    public static MainCharacter Instance { get; private set; }
    public Transform MainCharacterTransform { get; private set; }
    public Transform attackPoint;


    [SerializeField] private MainCharacterVisual _visual;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    public LayerMask enemyLayers;
    private Vector2 _inputVectorPrevious;

    public float attackRange = 0.5f;
    private float _movingSpeed = 4f;
    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private void Awake()
    {
        Instance = this;

        MainCharacterTransform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

    }
    private void Start()
    {
        GameInput.Instance.OnMainCharacterAttack += GameInput_OnMainCharacterAttack;
    }
    private void GameInput_OnMainCharacterAttack(object sender, System.EventArgs e)
    {
        Attack();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        _rb.MovePosition(_rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed)
            _isRunning = true;
        else
            _isRunning = false;
    }
    public void AdjustFacing()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        _inputVectorPrevious = Vector2.zero;

        if ((inputVector.x <= 0 && inputVector.y >= 0) || (inputVector.x < 0 && inputVector.y < 0))   // влево вверх или влево вниз
        {
            if (_inputVectorPrevious.x != inputVector.x || _inputVectorPrevious.y != inputVector.y)
            {
                _inputVectorPrevious = inputVector;
                MainCharacterTransform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }
        else if ((inputVector.x > 0 && inputVector.y >= 0) || (inputVector.x > 0 && inputVector.y < 0)) // вправо вверх или вправо вниз 
        {
            if (_inputVectorPrevious.x != inputVector.x || _inputVectorPrevious.y != inputVector.y)
            {
                _inputVectorPrevious = inputVector;
                MainCharacterTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
    public bool IsRunning()
    {
        return _isRunning;
    }

    public void TakeDamage(int damage)
    {
        //_healthPoints--;
        //Debug.Log($"PLAYER'S LIFE {_healthPoints}!");

        //_visual.HurtAnimation();

        //if (_healthPoints == 0)
        //    ApplyFatalDamage();
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamagable enemyOnHit = enemy.GetComponent<IDamagable>();

            if (enemyOnHit != null)
                enemyOnHit.TakeDamage(1);
            else
                Debug.Log("NullRefExep");
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        else
            return;
    }

  
}
