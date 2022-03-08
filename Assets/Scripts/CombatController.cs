using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerManager;

enum Attacks
{
    Light,
    Heavy,
}

public class CombatController : StarterAssetsInputs
{
    private Animator _animator;

    private bool _isBlocking;
    private InputAction _blockingAction;
    private PlayerInput _playerInput;
    private List<Attacks> _comboList = new List<Attacks>();
    public float comboTiming = 1f;
    private float _timeRemaining;
    private bool _isCombo;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _timeRemaining = 0f;
        // _playerInput.onActionTriggered += ReadInputAction;
        // _blockingAction = _playerInput.currentActionMap.FindAction("Blocking");
    }

    private void Update()
    {
        if (_timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
        }
        if (_timeRemaining <= 0)
        {
            Debug.Log("Times up!");
            var lastCombo = "";
            _comboList.ForEach((combo) =>
            {
                Debug.Log(combo);
                lastCombo += combo + " + ";
            });
            Debug.Log(lastCombo);
            _comboList.Clear();
            _isCombo = false;
            _timeRemaining = 0f;
            _animator.SetInteger("ComboSequence", 0);
        }
        _animator.SetBool("InCombo", _isCombo);
    }

    public void OnLightAttack(InputValue value)
    {
        if (!strafe) return;
        _animator.applyRootMotion = true;
        PlayerStates.currentState = States.Attacking;
        _timeRemaining = comboTiming;
        _comboList.Add(Attacks.Light);
        // _animator.ResetTrigger("LightAttack");
        // _animator.SetTrigger("LightAttack");
        _isCombo = true;
        _animator.SetInteger("ComboSequence", _comboList.Count);
        _animator.Play("Sword And Shield Slash");
    }

    public void OnHeavyAttack(InputValue value)
    {
        if (!strafe) return;
        _animator.applyRootMotion = true;
        PlayerStates.currentState = States.Attacking;
        _timeRemaining = comboTiming;
        _comboList.Add(Attacks.Heavy);
        // _animator.ResetTrigger("HeavyAttack");
        // _animator.SetTrigger("HeavyAttack");
        _isCombo = true;
        _animator.CrossFade("Sword And Shield Attack", .1f, 0);
        // _animator.Play("Sword And Shield Attack");
    }

    public void OnBlocking(InputValue value)
    {
        _animator.SetBool("Blocking", value.isPressed);
    }

    // private void ReadInputAction(InputAction.CallbackContext ctx)
    // {
    //     // Debug.Log("Read action!");
    //     if (ctx.action == _blockingAction)
    //     {
    //         _isBlocking = true;
    //         if (ctx.phase == InputActionPhase.Canceled)
    //         {
    //             _isBlocking = false;
    //         }
    //         _animator.SetBool("Blocking", _isBlocking);
    //     }
    // }

    public void FinishAttacks()
    {
        _animator.applyRootMotion = false;
        PlayerStates.currentState = States.Idle;
    }

}

