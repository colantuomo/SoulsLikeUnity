using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerManager;

enum Attacks
{
    Light = 1,
    Heavy = 2,
}

public class CombatController : MonoBehaviour
{
    [SerializeField]
    private Transform _hitBox;
    private Animator _animator;
    private bool _isBlocking;
    private InputAction _blockingAction;
    private PlayerInput _playerInput;
    private List<Attacks> _comboList = new List<Attacks>();
    public float comboTiming = 1f;
    private float _timeRemaining;
    private bool _inCombo;
    private int _comboSequence = 0;
    private StarterAssetsInputs _input;

    private Attacks[] combo1 = { Attacks.Light, Attacks.Light, Attacks.Heavy };
    private Attacks[] combo2 = { Attacks.Light, Attacks.Heavy };

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<StarterAssetsInputs>();
        _timeRemaining = 0f;
        // _playerInput.onActionTriggered += ReadInputAction;
        // _blockingAction = _playerInput.currentActionMap.FindAction("Blocking");
    }

    private void Update()
    {
        _animator.SetBool("InCombo", _inCombo);
    }

    public void OnLightAttack(InputValue value)
    {
        if (!_input.strafe) return;
        _comboList.Add(Attacks.Light);
        // _input.strafe = true;
        // StartCoroutine(ExecuteLightAttack());
        _animator.applyRootMotion = true;
        PlayerStates.currentState = States.Attacking;
        if (_inCombo) return;
        _inCombo = true;
        _animator.Play("Sword And Shield Slash");
    }

    public void OnHeavyAttack(InputValue value)
    {
        _input.strafe = true;
        _comboList.Add(Attacks.Heavy);
        StartCoroutine(ExecuteHeavyAttack());
        // StopAllCoroutines();
        // _animator.applyRootMotion = true;
        // PlayerStates.currentState = States.Attacking;
        // if (_inCombo) return;
        // _inCombo = true;
        // _animator.CrossFade("Sword And Shield Attack", .1f, 0);
    }

    private IEnumerator ExecuteLightAttack()
    {
        yield return new WaitForSeconds(.1f);
        _animator.applyRootMotion = true;
        PlayerStates.currentState = States.Attacking;
        if (!_inCombo)
        {
            _inCombo = true;
            _animator.Play("Sword And Shield Slash");
        }
    }

    private IEnumerator ExecuteHeavyAttack()
    {
        yield return new WaitForSeconds(.1f);
        _animator.applyRootMotion = true;
        PlayerStates.currentState = States.Attacking;
        if (!_inCombo)
        {
            _inCombo = true;
            _animator.CrossFade("Sword And Shield Attack", .1f, 0);
        }
    }

    public void OnBlocking(InputValue value)
    {
        if (!_input.strafe) return;
        // _input.strafe = value.isPressed;
        PlayerStates.currentState = value.isPressed ? States.Attacking : States.Idle;
        _animator.SetBool("Blocking", value.isPressed);
    }

    private IEnumerator ComboHandler()
    {
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitForSeconds(comboTiming);
        Debug.Log("Combo finished!");
        _animator.applyRootMotion = false;
        // _inCombo = false;
        // _comboSequence = 0;
        // var lastCombo = "";
        // _comboList.ForEach((combo) =>
        // {
        //     Debug.Log(combo);
        //     lastCombo += combo + " + ";
        // });
        // Debug.Log(lastCombo);
        // _comboList.Clear();
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
        if (!IsACombo(_comboList))
        {
            ResetCombo();
            return;
        }
        _comboList.RemoveAt(0);
        _animator.SetInteger("ComboSequence", (int)_comboList[0]);
    }

    private bool IsACombo(List<Attacks> playerCombo)
    {
        if (playerCombo.Count <= 1) return false;
        bool[] combos = { ComboExists(playerCombo, combo1), ComboExists(playerCombo, combo2) };
        List<bool> validCombos = new List<bool>(combos);
        return validCombos.Contains(true);
    }

    private bool ComboExists(List<Attacks> playerCombo, Attacks[] combo)
    {
        // Debug.Log(playerCombo.Count + " / " + combo.Length);
        bool playerComboIsBiggerThanCurrentCombo = playerCombo.Count > combo.Length;
        if (playerComboIsBiggerThanCurrentCombo) return false;
        bool isACombo = true;
        for (int i = 0; i < playerCombo.Count; i++)
        {
            // Debug.Log("Index: " + i + " / Player combo: " + playerCombo[i] + " / Game Combo: " + combo[i]);
            if (combo[i] != playerCombo[i])
            {
                isACombo = false;
            }
        }
        return isACombo;
    }

    private void HitAnEnemy(float damage)
    {
        Debug.Log("Searching for enemies...");
        Collider[] enemies = Physics.OverlapSphere(_hitBox.position, .4f, LayerMask.GetMask("Enemy"));
        if (enemies == null) return;
        foreach (Collider enemy in enemies)
        {
            Debug.Log("Enemy found! " + enemy.transform.name);
            enemy.GetComponent<EnemyManager>().TakeDamage(damage, transform.position);
        }
    }

    private void ResetCombo()
    {
        _inCombo = false;
        _comboSequence = 0;
        _comboList.Clear();
        _animator.applyRootMotion = false;
        _animator.SetInteger("ComboSequence", 0);
        PlayerStates.currentState = States.Idle;
    }

}

