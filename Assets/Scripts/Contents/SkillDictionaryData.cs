using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDictionaryData", menuName = "Data/SkillDictionaryData")]
public class SkillDictionaryData : ScriptableObject
{
    public SkillDmgType skillDmgType;
    public SkillEffectType skillEffectType;
    public string targetRange;
    public Sprite skillIcon;
 
}
