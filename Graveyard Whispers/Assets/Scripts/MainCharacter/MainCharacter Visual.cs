using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterVisual : MonoBehaviour
{
    private Animator _animator;

    private const string IS_RUN = "IsRun";
    private const string IS_DEAD = "IsDead";
    private const string ATTACK = "Attack";
    private const string HURT = "Hurt";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameInput.Instance.OnMainCharacterAttack += GameInput_OnMainCharacterAttack;
    }

    private void GameInput_OnMainCharacterAttack(object sender, System.EventArgs e)
    {  
        _animator.SetTrigger(ATTACK);
    }

    private void Update()
    {
        _animator.SetBool(IS_RUN, MainCharacter.Instance.IsRunning());

        MainCharacter.Instance.AdjustFacing();     
    } 
    public void DestroyAnimation()
    {
        _animator.SetBool(IS_DEAD, true);
    }
    public void HurtAnimation ()
    {
        _animator.SetTrigger(HURT);
    }
}

