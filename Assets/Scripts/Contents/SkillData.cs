using UnityEngine;

public enum TargetLayer { Short, All, Middle }
public enum SkillType { First, Later, Special, Supporter }

public enum SkillDmgType { Atk_SkillAtk, Atk_SkillAtk_Multiple, SkillAtk_Ignore, Atk_Ignore, Atk_SkillAtk_Ignore, enemyHalfHp, Ally_TatalAtk , None, SkillAtk_Ignore_Multiple }
public enum SkillEffectType { EffectNone, Poison, KnockBack, Unable, Burn, Paralysis, Holy, Coolling, Incarceration, DeadlyPoison, Seal, Stern, Freezing, ElectronicSeal, SealSkill, Unable_Burn, Burn_Poison, Poison_KnockBack }

[CreateAssetMenu(fileName = "SkillData", menuName = "Data/SkillData")]
public class SkillData : ScriptableObject
{
    public SkillName skillName;
    public PassiveName passiveName;
    public SkillType type;
    public Status status;
    public SkillDictionaryData skillDictionary;
    public MonsterData[] summonsMonsterDatas;
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    public int shieldCount;

    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    public TargetLayer targetLayer;
  
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    public float atk;
    public float consumMpAmount;
    public float buffValue;
    [Range(0f, 1f)]
    public float statusRatio = 0f;

    [Range(0f, 1f)]
    public float repeatRatio = 0f;

    [Range(0f, 1f)]
    public float creaticalRatio = 0f;
}
