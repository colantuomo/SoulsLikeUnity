using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buttons
{
    LightAttack,
    HeavyAttack,
    Block,
    Roll,
    Jump,
    Dodge,
    Special,
    Interact,
    Pause,
    Cancel,
    Confirm,
    Up,
    Down,
    Left,
    Right,
    Select,
    Back,
    None
}

[System.Serializable]
public class AnimationInfo
{
    public AnimationClip clip;
    public Buttons button;
}

[CreateAssetMenu(fileName = "AnimationsStorage", menuName = "ScriptableObjects/AnimationsStorage")]
public class AnimationsStorage : ScriptableObject
{
    public List<AnimationInfo> animations = new List<AnimationInfo>();
}
