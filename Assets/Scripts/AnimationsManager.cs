using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using PlayerManager;

public static class AttackAnimations
{
    public static string SWORD_SLASH = "Sword And Shield Slash";
    public static string SWORD_SLASH_ALTERNATE = "Sword And Shield Slash 1";
    public static string SWORD_360_SLASH = "Sword And Shield Attack";
    public static string SHIELD_BLOCK = "Sword And Shield Block Idle";
    public static string ROLL = "Sprinting Forward Roll";
}

public class AnimationsManager : MonoBehaviour
{
    public static AnimationsManager Instance;
    public AnimationsStorage combatAnimationsInfo;
    public ComboCreator combatCombos;
    private Animator _animator;
    private string _currentAnimationState;
    private string _defaultStrafeAnimation = "Strafe Movements";
    private string _defaultIdleAnimation = "Idle Walk Run Blend";
    private float _backToStrafeAnimationTime = 0.2f;
    private Buttons _lastButtonPressed = Buttons.None;
    private List<Combo> _currentAvailableCombos = new List<Combo>();
    private float _comboTimer;
    [SerializeField] private float _comboDelay = 1f;
    [SerializeField] private float _animationTransition = 0.1f;
    private StarterAssetsInputs _input;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _currentAvailableCombos.Clear();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_comboTimer > 0)
        {
            _comboTimer -= Time.deltaTime;
        }
        else
        {
            _comboTimer = 0;
            _currentAvailableCombos.Clear();
        }

    }

    public void ChangeAnimationState(string newState, float animationFade = 0.2f)
    {
        if (newState == _currentAnimationState) return;
        _currentAnimationState = newState;
        _animator.CrossFade(newState, animationFade, 0);
        var newStateAnimationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(BackToOldAnimation(_defaultIdleAnimation, newStateAnimationLength));
    }

    public void ChangeCombatAnimationState(Buttons buttonPressed, float animationFade = 0.1f)
    {
        bool comboEnded = _comboTimer <= 0;
        if (comboEnded)
        {
            SetAvailableCombosWhoStartWithPressedButton(buttonPressed);
        }
        ExecuteCombatAnim(buttonPressed);
    }

    private void ExecuteCombatAnim(Buttons buttonPressed)
    {
        int clipIndex = FindClipIndexByComboState(buttonPressed);
        if (clipIndex == -1) return;
        _comboTimer = _comboDelay;

        List<AnimationInfo> animations = _currentAvailableCombos[clipIndex].animations;
        AnimationInfo newAnimationInfo = animations.Find(x => x.button == buttonPressed);

        _lastButtonPressed = buttonPressed;
        _animator.applyRootMotion = true;
        PlayerStates.currentState = States.Attacking;

        StopAllCoroutines();
        string currentAnimation = "";
        foreach (var item in _animator.GetCurrentAnimatorClipInfo(0))
        {
            print(item.clip.name);
            if (item.clip.name == "Idle")
            {
                currentAnimation = item.clip.name;
            }
            break;
        }
        _animationTransition = currentAnimation == "Idle" ? 0f : _animationTransition;
        _animator.CrossFade(newAnimationInfo.clip.name, _animationTransition, 0);
        // _animator.Play(newAnimationInfo.clip.name);
        _currentAvailableCombos[clipIndex].animations.RemoveAt(0);
        StartCoroutine(BackToStrafeAnimation(newAnimationInfo.clip.length));
    }

    private int FindClipIndexByComboState(Buttons buttonPressed)
    {
        for (int i = 0; i < _currentAvailableCombos.Count; i++)
        {
            Combo combo = _currentAvailableCombos[i];
            foreach (AnimationInfo animInfo in combo.animations)
            {
                if (buttonPressed == animInfo.button)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private void SetAvailableCombosWhoStartWithPressedButton(Buttons buttonPressed)
    {
        var comboLocalList = new List<Combo>();
        foreach (Combo combo in combatCombos.comboList)
        {
            var newCombo = new Combo();
            newCombo.name = combo.name;
            newCombo.animations = new List<AnimationInfo>();
            foreach (AnimationInfo animInfo in combo.animations)
            {
                var firstComboAnimation = combo.animations[0];
                newCombo.animations.Add(animInfo);
                if (buttonPressed == firstComboAnimation.button)
                {
                    comboLocalList.Add(newCombo);
                }
            }
        }
        _currentAvailableCombos = new List<Combo>(comboLocalList);
    }

    private int FindClipIndexByName(string name)
    {
        for (int i = 0; i < combatAnimationsInfo.animations.Count; i++)
        {
            var item = combatAnimationsInfo.animations[i];
            if (item.clip.name == name)
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator BackToOldAnimation(string animation, float newStateAnimationLength)
    {
        yield return new WaitForSeconds(newStateAnimationLength);
        _currentAnimationState = "";
        _animator.CrossFade(animation, 0.3f, 0);
    }

    private IEnumerator BackToStrafeAnimation(float animationWaitFor)
    {
        yield return new WaitForSeconds(animationWaitFor);
        _animator.applyRootMotion = false;
        _currentAnimationState = _defaultIdleAnimation;
        _input.strafe = false;
        _animator.CrossFade(_defaultIdleAnimation, _backToStrafeAnimationTime, 0);
        PlayerStates.currentState = States.Idle;
    }
}
