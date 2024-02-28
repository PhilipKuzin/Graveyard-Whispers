using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }    
    private MainCharacterInputActions _mainCharacterInputActions;

    public event EventHandler OnMainCharacterAttack;

    private void Awake()
    {
        Instance = this;
        _mainCharacterInputActions = new MainCharacterInputActions();
        _mainCharacterInputActions.Enable();

        _mainCharacterInputActions.Combat.Attack.started += MainCharacterAttack_started;
    }
    private void MainCharacterAttack_started(InputAction.CallbackContext obj)
    {
        OnMainCharacterAttack?.Invoke(this,EventArgs.Empty);
    }
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _mainCharacterInputActions.MainCharacter.Move.ReadValue<Vector2>();
        return inputVector;
    }
    //public Vector3 GetMousePosition ()    // на будущее, возвращает позицию мыши в Vector3 формате
    //{
    //    Vector3 mousPos = Mouse.current.position.ReadValue();
    //    return mousPos;
    //}
}
