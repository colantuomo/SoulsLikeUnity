using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combo
{
    public string name;
    public List<AnimationInfo> animations;
}

[CreateAssetMenu(fileName = "ComboCreator", menuName = "ScriptableObjects/ComboCreator")]
public class ComboCreator : ScriptableObject
{
    public List<Combo> comboList;
}
