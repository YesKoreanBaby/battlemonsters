using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionSuccessType { Enemy_5, Enemy_15, Enemy_30, Enemy_60, Enemy_100, Win_3, Win_5, Win_10, Win_25, Win_100, Reversal, OldOver, Boss1vs1, Evolution, FullHp, NoMana, OneTurn}
public enum BattleBuffType { HP, MP, DEX, DEF, ATK, DDE, CRI, HRC, MRC, SKILL, ABILITY}

[CreateAssetMenu(fileName = "MissionData", menuName = "Data/MissionData")]
public class MissionData : ScriptableObject
{
    public MissionSuccessType missionSuccessType;
    public BattleBuffType battleBuffType;
    public AbilityType abilityType;
    public ScriptableObject obj;
    public float buffValue;

    public void Buff(MonsterInstance originInstance)
    {
        if(battleBuffType == BattleBuffType.HP)
        {
            originInstance.maxHp += buffValue;
            originInstance.hp = Mathf.Min(originInstance.maxHp, originInstance.hp + buffValue);
        }
        else if(battleBuffType == BattleBuffType.MP)
        {
            originInstance.maxMp += buffValue;
            originInstance.mp = Mathf.Min(originInstance.maxMp, originInstance.mp + buffValue);
        }
        else if (battleBuffType == BattleBuffType.DEX)
        {
            originInstance.maxDex = Mathf.Max(0.1f, originInstance.maxDex - buffValue);
        }
        else if(battleBuffType == BattleBuffType.ATK)
        {
            originInstance.atk += buffValue;
        }
        else if (battleBuffType == BattleBuffType.DEF)
        {
            originInstance.def += buffValue;
        }
        else if (battleBuffType == BattleBuffType.CRI)
        {
            originInstance.creaticalRatio = Mathf.Min(1f, originInstance.creaticalRatio + buffValue);
        }
        else if (battleBuffType == BattleBuffType.DDE)
        {
            originInstance.repeatRatio = Mathf.Min(1f, originInstance.repeatRatio + buffValue);
        }
        else if (battleBuffType == BattleBuffType.HRC)
        {
            originInstance.hpRecoveryRatio = Mathf.Min(1f, originInstance.hpRecoveryRatio + buffValue);
        }
        else if (battleBuffType == BattleBuffType.MRC)
        {
            originInstance.manaRecoveryRatio = Mathf.Min(1f, originInstance.manaRecoveryRatio + buffValue);
        }
        else if(battleBuffType == BattleBuffType.SKILL)
        {
            SkillData skillData = obj as SkillData;
            if(skillData != null)
            {
                originInstance.skillDatas.Add(skillData);
            }
            else
            {
                TriggerSkillData triggerSkillData = obj as TriggerSkillData;
                if(triggerSkillData != null)
                {
                    originInstance.triggerSkillDatas.Add(new System.Tuple<SkillTrigger, SkillData>(triggerSkillData.skillTrigger, triggerSkillData.skillData));
                }
                else
                {
                    PercentSkillData percentSkillData = obj as PercentSkillData;
                    if(percentSkillData != null)
                    {
                        originInstance.percentSkillDatas.Add(new System.Tuple<int, SkillData>(percentSkillData.percent, percentSkillData.skillData));
                    }
                }
            }
        }
        else if(battleBuffType == BattleBuffType.ABILITY)
        {
            originInstance.abilities.Add(abilityType);
        }
    }
}
public class MissionObject
{
    public MissionData missionData;
    public bool success = false;
}

public class MissionInstance
{
    public int enemyDeadCount = 0;
    public int winCount = 0;
    public bool checkReversal;
    public bool checkOldOver;
    public bool checkBoss1vs1;
    public bool checkEvolution;
    public bool checkFullHp;
    public bool checkNoMana;
    public bool checkOneTurn;
    public List<MissionObject> missionDatas = new List<MissionObject>();

    public void Init(MissionData[] missionDatas)
    {
        for(int i = 0; i < missionDatas.Length; i++)
        {
            MissionObject data = new MissionObject();
            data.missionData = missionDatas[i];
            data.success = false;
        
            this.missionDatas.Add(data);
        }

        enemyDeadCount = 0;
        winCount = 0;
    }

    public void SetBuff(MonsterInstance originInstance)
    {
        for(int i = 0; i < missionDatas.Count; ++i)
        {
            if (missionDatas[i].success == true)
                continue;
            if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Enemy_5)
            {
                if(enemyDeadCount > 4)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Enemy_15)
            {
                if (enemyDeadCount > 14)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Enemy_30)
            {
                if (enemyDeadCount > 14)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Enemy_60)
            {
                if (enemyDeadCount > 59)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Enemy_100)
            {
                if (enemyDeadCount > 99)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Win_3)
            {
                if (winCount > 2)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Win_5)
            {
                if (winCount > 4)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Win_10)
            {
                if (winCount > 9)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Win_25)
            {
                if (winCount > 24)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Win_100)
            {
                if (winCount > 99)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Reversal)
            {
                if (checkReversal == true)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.OldOver)
            {
                if (checkOldOver == true)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Boss1vs1)
            {
                if (checkBoss1vs1 == true)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.Evolution)
            {
                if (checkEvolution == true)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.FullHp)
            {
                if (checkFullHp == true)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.NoMana)
            {
                if (checkNoMana == true)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
            else if (missionDatas[i].missionData.missionSuccessType == MissionSuccessType.OneTurn)
            {
                if (checkOneTurn == true)
                {
                    missionDatas[i].missionData.Buff(originInstance);
                    missionDatas[i].success = true;
                }
            }
        }
    }
}
