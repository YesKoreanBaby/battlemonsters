using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status { Normal, Fire, Ice, Earth, Wind, Water, Wood, Elec, Acid, Dark, Light}
public enum SelectSkillAIType { Balance, InFighter, OutFighter, Slerger, InAndOutFighter, Supporter}
public enum AbilityType { WillPower, Recycle, Suction, Spurt, SpawnItem, UltimateHardness, Fighting, Miasma, Knowledge, Potential, PoisonReflexion, BurnReflexion, IceReflexion, Easter, Cell, Sturdy, LivingDead, Support, Gardner }
public enum ConfirmSkillPriority { Random, Custom, High_Mp, Low_Mp, Fit_Mp, High_Atk, Performance, Persistence, Surppoting, EverChanging, MainStatus, FocusStatus, Ego }
public enum SelectDetailTargetType { Random, Low_Hp, High_Hp, Low_Stat, High_Stat, CenterBreak, SideBreak, Long_Dis, Short_Dis, Counter, 
                                     SpeedBreak, Slerger, Supporter, OutFighter, InFighter, Low_Dodge, High_Dodge, Dot_Damage, Stern}

public enum MonsterClass { Low, Middle, High, VeryHigh, Legend, Myth}

public enum EvolutionRangeType { None, RandomBox, Bug, Dragon, RandomBox2}
public enum MonsterWeight { Normal, Small, Middle, Big}

public enum MonsterHeathState { None, Faint, Crippled, CrippedStrong, Blooding, FullCondition }

public enum DeadTranningType { Red, Green, Puple}

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class MonsterData : ScriptableObject
{
    public EntityMonster monsterPrefab;
    public SelectSkillAIType battleAIType;
    public EvolutionRangeType evolutionType;
    public Status status;
    public MonsterWeight monsterWeight;
    public MonsterClass monsterClass;
    public ItemData[] spawnItems;
    public TranningType tranningType;
    public MonsterDictionaryData dictionaryData;
    public float hp;
    public float mp;
    public float dex;
    public float atk;
    public float def;
    public float manaRecoveryRatio = 0.1f;
    public float hpRecoveryRatio = 0.1f;
    public float repeatRatio = 0.1f;
    public float creaticalRatio = 0.1f;
    public List<SkillData> datas;
    public List<PercentSkillData> percentSkillDatas;
    public List<TriggerSkillData> triggerSkillDatas;
    public List<AbilityType> abilities;
    public List<ConfirmSkillPriority> confirmSkillPrioritys;
    public List<SelectDetailTargetType> selectDetailTargetTypes;

    [Space]
    [Space]
    [Space]
    [Space]
    public List<SkillData> possibleSkillDatas;
    public List<PercentSkillData> possiblePercentSkillDatas;
    public List<TriggerSkillData> possibleTriggerSkillDatas;
    public List<AbilityType> possibleAbilitieDatas;
    public List<ConfirmSkillPriority> possibleConfirmSkillPriorityDatas;
    public List<SelectDetailTargetType> possibleSelectDetailTargetTypeDatas;
   
    [Space]
    [Space]
    [Space]
    [Space]
    public MonsterData evolutionMonsterData;
    public EvolutionRangeType evolutionRangeType;
    public bool isDeadLock;
    public bool isPocketLock;
}
