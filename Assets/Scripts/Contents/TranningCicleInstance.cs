using System.Linq;
using UnityEngine;

public class TranningCicleInstance
{
    public MonsterInstance monsterInstance;
    public TranningCicleData tranningData;

    public float hpGoalPercent;
    public int hpLevel;
    public int hpCost;
   
    public float mpGoalPercent;
    public int mpLevel;
    public int mpCost;

    public float atkGoalPercent;
    public int atkLevel;
    public int atkCost;

    public float defGoalPercent;
    public int defLevel;
    public int defCost;

    public float dexGoalPercent;
    public int dexLevel;
    public int dexCost;

    public float hpRecoveryGoalPercent;
    public int hpRecoveryLevel;
    public int hpRecoveryCost;

    public float mpRecoveryGoalPercent;
    public int mpRecoveryLevel;
    public int mpRecoveryCost;

    public float dodgeGoalPercent;
    public int dodgeLevel;
    public int dodgeCost;

    public float criticalGoalPercent;
    public int criticalLevel;
    public int criticalCost;

    //skill
    public int skillLevel;
    public int skillMaxLevel;
    public float skillPercent;
    public float skillAddPercent;
    public int skillCost = 1;
  
    //능력
    public int abilityLevel;
    public int abilityMaxLevel;
    public float abilityPercent;
    public float abilityAddPercent;
    public int abilityCost = 1;
  
    //지능
    public int iqLevel;
    public int iqMaxLevel;
    public float iqPercent;
    public int iqCost = 5;
 
    public static TranningCicleInstance Instance(MonsterInstance monsterInstance, TranningCicleData tranningData)
    {
        TranningCicleInstance instance = new TranningCicleInstance();
        instance.monsterInstance = monsterInstance;
        instance.tranningData = tranningData;

        instance.hpGoalPercent = tranningData.hpData.goalPercent;
        instance.hpCost = tranningData.hpData.cost;

        instance.mpGoalPercent = tranningData.mpData.goalPercent;
        instance.mpCost = tranningData.mpData.cost;

        instance.atkGoalPercent = tranningData.atkData.goalPercent;
        instance.atkCost = tranningData.atkData.cost;

        instance.defGoalPercent = tranningData.defData.goalPercent;
        instance.defCost = tranningData.defData.cost;

        instance.dexGoalPercent = tranningData.dexData.goalPercent;
        instance.dexCost = tranningData.dexData.cost;

        instance.hpRecoveryGoalPercent = tranningData.hpRecoveryData.goalPercent;
        instance.hpRecoveryCost = tranningData.hpRecoveryData.cost;

        instance.mpRecoveryGoalPercent = tranningData.mpRecoveryData.goalPercent;
        instance.mpRecoveryCost = tranningData.mpRecoveryData.cost;

        instance.dodgeGoalPercent = tranningData.dodgeData.goalPercent;
        instance.dodgeCost = tranningData.dodgeData.cost;

        instance.criticalGoalPercent = tranningData.criticalData.goalPercent;
        instance.criticalCost = tranningData.criticalData.cost;

        instance.skillAddPercent = 0.05f;
        instance.skillPercent = 0.25f;
        instance.skillMaxLevel = monsterInstance.possiblePercentSkillDatas.Count + monsterInstance.possibleTriggerSkillDatas.Count + monsterInstance.possibleSkillDatas.Count;
        instance.skillLevel = 0;
       
        instance.abilityAddPercent = 0.05f;
        instance.abilityPercent = 0.25f;
        instance.abilityMaxLevel = monsterInstance.possibleAbilitieDatas.Count;
        instance.abilityLevel = 0;
        
        instance.iqPercent = 0.75f;
        instance.iqMaxLevel = monsterInstance.possibleConfirmSkillPriorityDatas.Count + monsterInstance.possibleSelectDetailTargetTypeDatas.Count;
        instance.iqLevel = 0;
        return instance;
    }
    public void TranningAI(TranningType tranningType, AITranningLevel tranningLevel, bool checkFullSkill, ConfirmSkillPriority currentConfirmSkill, SelectDetailTargetType currrentSelectDetailTarget)
    {
        float hpLevel, mpLevel, atkLevel, defLevel, dexLevel, mrcLevel, hrcLevel, criLevel, ddgLevel;

        GetAITranningValue(tranningType, tranningLevel, out hpLevel, out mpLevel, out atkLevel, out defLevel, out dexLevel, out mrcLevel, out hrcLevel, out criLevel, out ddgLevel);

        monsterInstance.maxHp += Mathf.RoundToInt(hpLevel);
        monsterInstance.hp = Mathf.Min(monsterInstance.maxHp, monsterInstance.hp + Mathf.RoundToInt(hpLevel));

        monsterInstance.maxMp += Mathf.RoundToInt(mpLevel);
        monsterInstance.mp = Mathf.Min(monsterInstance.maxMp, monsterInstance.mp + Mathf.RoundToInt(mpLevel));

        monsterInstance.atk += Mathf.RoundToInt(atkLevel);

        monsterInstance.def += Mathf.RoundToInt(defLevel);

        monsterInstance.maxDex = Mathf.Max(0.1f, monsterInstance.maxDex - dexLevel);

        monsterInstance.manaRecoveryRatio = Mathf.Min(1f, monsterInstance.manaRecoveryRatio + mrcLevel);

        monsterInstance.hpRecoveryRatio = Mathf.Min(1f, monsterInstance.hpRecoveryRatio + hrcLevel);
        monsterInstance.repeatRatio = Mathf.Min(1f, monsterInstance.repeatRatio + ddgLevel);
        monsterInstance.creaticalRatio = Mathf.Min(1f, monsterInstance.creaticalRatio + criLevel);

        if(checkFullSkill)
        {
            monsterInstance.abilities.AddRange(monsterInstance.possibleAbilitieDatas);
            monsterInstance.skillDatas.AddRange(monsterInstance.possibleSkillDatas);
            monsterInstance.possiblePercentSkillDatas.AddRange(monsterInstance.possiblePercentSkillDatas);
            for (int i = 0; i < monsterInstance.possibleTriggerSkillDatas.Count; ++i)
            {
                monsterInstance.triggerSkillDatas.Add(new System.Tuple<SkillTrigger, SkillData>(monsterInstance.possibleTriggerSkillDatas[i].skillTrigger, monsterInstance.possibleTriggerSkillDatas[i].skillData));
            }
        }

        monsterInstance.currentSelectDetailTargetType = currrentSelectDetailTarget;
        monsterInstance.currentConfirmSkillPriority = currentConfirmSkill;
    }

    public void GetAITranningValue(TranningType tranningType, AITranningLevel level, out float hp, out float mp, out float atk, out float def, out float dex, out float mrc, out float hrc, out float cri, out float dde)
    {
        hp = mp = atk = def = dex = mrc = hrc = cri = dde = 0;

        if (tranningType == TranningType.Laziness)
        {
            if (level == AITranningLevel.Easy)
            {
                hp = 1;
                mp = 1;
                atk = 1;
                def = 1;
                dex = 0.25f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.Normal)
            {
                hp = 5;
                mp = 5;
                atk = 2;
                def = 1;
                dex = 0.25f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.Hard)
            {
                hp = 15;
                mp = 15;
                atk = 5;
                def = 1;
                dex = 0.25f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                hp = 25;
                mp = 20;
                atk = 7;
                def = 1;
                dex = 0.25f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
        }
        else if (tranningType == TranningType.Balance)
        {
            if (level == AITranningLevel.Easy)
            {
                hp = 10;
                mp = 10;
                atk = 5;
                def = 1;
            }
            else if (level == AITranningLevel.Normal)
            {
                hp = 25;
                mp = 10;
                atk = 5;
                def = 5;
            }
            else if (level == AITranningLevel.Hard)
            {
                hp = 30;
                mp = 30;
                atk = 15;
                def = 5;
                dex = 0.5f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                hp = 50;
                mp = 50;
                atk = 25;
                def = 15;
                dex = 1f;
            }
        }
        else if (tranningType == TranningType.Late_Bloomer)
        {
            if (level == AITranningLevel.Easy)
            {
                hp = 5;
                mp = 5;
                atk = 2;
                def = 1;
            }
            else if (level == AITranningLevel.Normal)
            {
                hp = 7;
                mp = 7;
                atk = 2;
                def = 1;
            }
            else if (level == AITranningLevel.Hard)
            {
                hp = 25;
                mp = 20;
                atk = 7;
                def = 7;
                dex = 0.25f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                hp = 50;
                mp = 50;
                atk = 15;
                def = 15;
                dex = 1f;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.25f;
                dde = 0.1f;
            }
        }
        else if (tranningType == TranningType.Genius)
        {
            if (level == AITranningLevel.Easy)
            {
                hp = 25;
                mp = 20;
                atk = 15;
                def = 5;
                dex = 0.25f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.Normal)
            {
                hp = 30;
                mp = 30;
                atk = 15;
                def = 5;
                dex = 0.5f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.Hard)
            {
                hp = 35;
                mp = 35;
                atk = 15;
                def = 5;
                dex = 1f;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.1f;
                dde = 0.1f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                hp = 50;
                mp = 50;
                atk = 25;
                def = 5;
                dex = 0.25f;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.25f;
                dde = 0.25f;
            }
        }
        else if (tranningType == TranningType.Hp)
        {
            if (level == AITranningLevel.Easy)
            {
                hp = 20;
            }
            else if (level == AITranningLevel.Normal)
            {
                hp = 50;
            }
            else if (level == AITranningLevel.Hard)
            {
                hp = 50;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                hp = 75;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.1f;
                dde = 0.1f;
            }
        }
        else if (tranningType == TranningType.Atk)
        {
            if (level == AITranningLevel.Easy)
            {
                atk = 5;
            }
            else if (level == AITranningLevel.Normal)
            {
                atk = 15;
            }
            else if (level == AITranningLevel.Hard)
            {
                atk = 15;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                atk = 38;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.1f;
                dde = 0.1f;
            }
        }
        else if (tranningType == TranningType.Def)
        {
            if (level == AITranningLevel.Easy)
            {
                def = 5;
            }
            else if (level == AITranningLevel.Normal)
            {
                def = 10;
            }
            else if (level == AITranningLevel.Hard)
            {
                def = 10;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                def = 25;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.1f;
                dde = 0.1f;
            }
        }
        else if (tranningType == TranningType.Mp)
        {
            if (level == AITranningLevel.Easy)
            {
                mp = 20;
            }
            else if (level == AITranningLevel.Normal)
            {
                mp = 50;
            }
            else if (level == AITranningLevel.Hard)
            {
                mp = 50;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                mp = 75;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.1f;
                dde = 0.1f;
            }
        }
        else if (tranningType == TranningType.Speed)
        {
            if (level == AITranningLevel.Easy)
            {
                dex = 0.5f;
            }
            else if (level == AITranningLevel.Normal)
            {
                dex = 1f;
            }
            else if (level == AITranningLevel.Hard)
            {
                dex = 1f;
                mrc = 0.1f;
                hrc = 0.1f;
                cri = 0.05f;
                dde = 0.05f;
            }
            else if (level == AITranningLevel.VeryHard)
            {
                dex = 2f;
                mrc = 0.25f;
                hrc = 0.25f;
                cri = 0.1f;
                dde = 0.1f;
            }
        }
    }
    public TranningCicleInstance Evolution(MonsterInstance evolutionMonster)
    {
        TranningCicleInstance instance = Instance(evolutionMonster, tranningData);

        for (int i = 0; i < hpLevel; ++i)
            instance.TranningHpComplete();

        for (int i = 0; i < mpLevel; ++i)
            instance.TranningMpComplete();

        for (int i = 0; i < atkLevel; ++i)
            instance.TranningAtkComplete();

        for (int i = 0; i < defLevel; ++i)
            instance.TranningDefComplete();

        for (int i = 0; i < dexLevel; ++i)
            instance.TranningDexComplete();

        for (int i = 0; i < hpRecoveryLevel; ++i)
            instance.TranningHpRecoveryComplete();

        for (int i = 0; i < mpRecoveryLevel; ++i)
            instance.TranningMpRecoveryComplete();

        for (int i = 0; i < dodgeLevel; ++i)
            instance.TranningDodgeComplete();

        for (int i = 0; i < criticalLevel; ++i)
            instance.TranningCriticalComplete();

        return instance;
    }
    public void EvolutionCopy(MonsterInstance evolutionMonster)
    {
        int hpLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.hpLevel - this.hpLevel);
        for (int i = 0; i < hpLevel; ++i)
            this.TranningHpComplete();

        int mpLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.hpLevel - this.mpLevel);
        for (int i = 0; i < mpLevel; ++i)
            this.TranningMpComplete();

        int atkLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.atkLevel - this.atkLevel);
        for (int i = 0; i < atkLevel; ++i)
            this.TranningAtkComplete();

        int defLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.defLevel - this.defLevel);
        for (int i = 0; i < defLevel; ++i)
            this.TranningDefComplete();

        int dexLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.dexLevel - this.dexLevel);
        for (int i = 0; i < dexLevel; ++i)
            this.TranningDexComplete();

        int hpRecoveryLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.hpRecoveryLevel - this.hpRecoveryLevel);
        for (int i = 0; i < hpRecoveryLevel; ++i)
            this.TranningHpRecoveryComplete();

        int mpRecoveryLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.mpRecoveryLevel - this.mpRecoveryLevel);
        for (int i = 0; i < mpRecoveryLevel; ++i)
            this.TranningMpRecoveryComplete();

        int dodgeLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.dodgeLevel - this.dodgeLevel);
        for (int i = 0; i < dodgeLevel; ++i)
            this.TranningDodgeComplete();

        int criticalLevel = Mathf.Max(0, evolutionMonster.tranningCicleInstance.criticalLevel - this.criticalLevel);
        for (int i = 0; i < criticalLevel; ++i)
            this.TranningCriticalComplete();
    }
    public float TranningHp()
    {
        if (hpLevel < tranningData.hpData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(hpCost, tranningData.hpData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= hpGoalPercent)
            {
                float value = TranningHpComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }
    public float TranningHpComplete()
    {
        float value = tranningData.hpData.buffValue;
        monsterInstance.maxHp += value;

        if(monsterInstance.heathState != MonsterHeathState.Faint)
            monsterInstance.hp = Mathf.Min(monsterInstance.maxHp, monsterInstance.hp + value);

        hpGoalPercent = Mathf.Max(tranningData.hpData.minGoalPercent, hpGoalPercent - tranningData.hpData.addGoalPercent);
        hpLevel = Mathf.Min(tranningData.hpData.level, hpLevel + 1);
        hpCost += tranningData.hpData.addCost;

        return value;
    }
    public float TranningMp()
    {
        if (mpLevel < tranningData.mpData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(mpCost, tranningData.mpData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= mpGoalPercent)
            {
                float value = TranningMpComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }
    public float TranningMpComplete()
    {
        float value = tranningData.mpData.buffValue;
        monsterInstance.maxMp += value;
        monsterInstance.mp = Mathf.Min(monsterInstance.maxMp, monsterInstance.mp + value);

        mpGoalPercent = Mathf.Max(tranningData.mpData.minGoalPercent, mpGoalPercent - tranningData.mpData.addGoalPercent);
        mpLevel = Mathf.Min(tranningData.mpData.level, mpLevel + 1);
        mpCost += tranningData.mpData.addCost;
        return value;
    }

    public float TranningAtk()
    {
        if (atkLevel < tranningData.atkData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(atkCost, tranningData.atkData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= atkGoalPercent)
            {
                float value = TranningAtkComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }

    public float TranningAtkComplete()
    {
        float value = tranningData.atkData.buffValue;
        monsterInstance.atk += value;

        atkGoalPercent = Mathf.Max(tranningData.atkData.minGoalPercent, atkGoalPercent - tranningData.atkData.addGoalPercent);
        atkLevel = Mathf.Min(tranningData.atkData.level, atkLevel + 1);
        atkCost += tranningData.atkData.addCost;
        return value;
    }

    public float TranningDef()
    {
        if (defLevel < tranningData.defData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(defCost, tranningData.defData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= defGoalPercent)
            {
                float value = TranningDefComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }

    public float TranningDefComplete()
    {
        float value = tranningData.defData.buffValue;
        monsterInstance.def += value;

        defLevel = Mathf.Min(tranningData.defData.level, defLevel + 1);
        defGoalPercent = Mathf.Max(tranningData.defData.minGoalPercent, defGoalPercent - tranningData.defData.addGoalPercent);
        defCost += tranningData.defData.addCost;
        return value;
    }

    public float TranningDex()
    {
        if (dexLevel < tranningData.dexData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(dexCost, tranningData.dexData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= dexGoalPercent)
            {
                float value = TranningDexComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }

    public float TranningDexComplete()
    {
        float value = tranningData.dexData.buffValue;
        monsterInstance.maxDex = Mathf.Max(0.1f, monsterInstance.maxDex - value);
        var dex = System.Math.Round(monsterInstance.maxDex, 3);
        monsterInstance.maxDex = (float)dex;

        dexLevel++;
        dexGoalPercent = Mathf.Max(tranningData.dexData.minGoalPercent, dexGoalPercent - tranningData.dexData.addGoalPercent);
        dexCost += tranningData.dexData.addCost;
        return value;
    }

    public float TranningHpRecovery()
    {
        if (hpRecoveryLevel < tranningData.hpRecoveryData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(hpRecoveryCost, tranningData.hpRecoveryData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= hpRecoveryGoalPercent)
            {
                float value = TranningHpRecoveryComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }

    public float TranningHpRecoveryComplete()
    {
        float value = tranningData.hpRecoveryData.buffValue;
        monsterInstance.hpRecoveryRatio = Mathf.Min(1f, monsterInstance.hpRecoveryRatio + value);

        double hpRatio = System.Math.Round(monsterInstance.hpRecoveryRatio, 3);
        monsterInstance.hpRecoveryRatio = (float)hpRatio;

        hpRecoveryLevel = Mathf.Min(tranningData.hpRecoveryData.level, hpRecoveryLevel + 1);
        hpRecoveryGoalPercent = Mathf.Max(tranningData.hpRecoveryData.minGoalPercent, hpRecoveryGoalPercent - tranningData.hpRecoveryData.addGoalPercent);
        hpRecoveryCost += tranningData.hpRecoveryData.addCost;
        return value;
    }

    public float TranningMpRecovery()
    {
        if (mpRecoveryLevel < tranningData.mpRecoveryData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(mpRecoveryCost, tranningData.mpRecoveryData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= mpGoalPercent)
            {
                float value = TranningMpRecoveryComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }
    public float TranningMpRecoveryComplete()
    {
        float value = tranningData.mpRecoveryData.buffValue;
        monsterInstance.manaRecoveryRatio = Mathf.Min(1f, monsterInstance.manaRecoveryRatio + value);

        var manaRatio = System.Math.Round(monsterInstance.manaRecoveryRatio, 3);
        monsterInstance.manaRecoveryRatio = (float)manaRatio;

        mpRecoveryLevel = Mathf.Min(tranningData.mpRecoveryData.level, mpRecoveryLevel + 1);
        mpRecoveryGoalPercent = Mathf.Max(tranningData.mpRecoveryData.minGoalPercent, mpRecoveryGoalPercent - tranningData.mpRecoveryData.addGoalPercent);
        mpRecoveryCost += tranningData.mpRecoveryData.addCost;
        return value;
    }

    public float TranningDodge()
    {
        if (dodgeLevel < tranningData.dodgeData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(dodgeCost, tranningData.dodgeData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= dodgeGoalPercent)
            {
                float value = TranningDodgeComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }
    public float TranningDodgeComplete()
    {
        float value = tranningData.dodgeData.buffValue;
        monsterInstance.repeatRatio = Mathf.Min(1f, monsterInstance.repeatRatio + value);

        var dde = System.Math.Round(monsterInstance.repeatRatio, 3);
        monsterInstance.repeatRatio = (float)dde;

        dodgeLevel = Mathf.Min(tranningData.dodgeData.level, dodgeLevel + 1);
        dodgeGoalPercent = Mathf.Max(tranningData.dodgeData.minGoalPercent, dodgeGoalPercent - tranningData.dodgeData.addGoalPercent);
        dodgeCost += tranningData.dodgeData.addCost;
        return value;
    }

    public float TranningCritical()
    {
        if (criticalLevel < tranningData.criticalData.level)
        {
            bool check = ItemInventory.Instance.CheckLimit(criticalCost, tranningData.criticalData.costItemType);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= criticalGoalPercent)
            {
                float value = TranningCriticalComplete();
                return value;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }

    public float TranningCriticalComplete()
    {
        float value = tranningData.criticalData.buffValue;
        monsterInstance.creaticalRatio = Mathf.Min(1f, monsterInstance.creaticalRatio + value);

        var cri = System.Math.Round(monsterInstance.creaticalRatio, 3);
        monsterInstance.creaticalRatio = (float)cri;

        criticalLevel = Mathf.Min(tranningData.criticalData.level, criticalLevel + 1);
        criticalGoalPercent = Mathf.Max(tranningData.criticalData.minGoalPercent, criticalGoalPercent - tranningData.criticalData.addGoalPercent);
        criticalCost += tranningData.criticalData.addCost;
        return value;
    }

    public float TranningSkill(out Sprite skillValue)
    {
        skillValue = null;
        if (skillLevel < skillMaxLevel)
        {
            bool check = ItemInventory.Instance.CheckLimit(skillCost, CostItemType.Diamond);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= skillPercent)
            {
                if (monsterInstance.possibleSkillDatas.Count > 0)
                {
                    var data = monsterInstance.possibleSkillDatas.First();
                    monsterInstance.possibleSkillDatas.Remove(data);
                    monsterInstance.skillDatas.Add(data);

                    skillLevel = Mathf.Min(skillMaxLevel, skillLevel + 1);
                    skillPercent = Mathf.Max(0.01f, skillPercent - skillAddPercent);
                    skillValue = data.skillDictionary.skillIcon;
                    return 1f;
                }
                else
                {
                    if (monsterInstance.possiblePercentSkillDatas.Count > 0)
                    {
                        var data = monsterInstance.possiblePercentSkillDatas.First();
                        monsterInstance.possiblePercentSkillDatas.Remove(data);
                        monsterInstance.percentSkillDatas.Add(new System.Tuple<int, SkillData>(data.percent, data.skillData));

                        skillLevel++;
                        skillPercent = Mathf.Max(0.01f, skillPercent - skillAddPercent);
                        skillValue = data.skillData.skillDictionary.skillIcon;
                        return 1f;
                    }
                    else
                    {
                        var data = monsterInstance.possibleTriggerSkillDatas.First();
                        monsterInstance.possibleTriggerSkillDatas.Remove(data);
                        monsterInstance.triggerSkillDatas.Add(new System.Tuple<SkillTrigger, SkillData>(data.skillTrigger, data.skillData));

                        skillLevel++;
                        skillPercent = Mathf.Max(0.01f, skillPercent - skillAddPercent);
                        skillValue = data.skillData.skillDictionary.skillIcon;
                        return 1f;
                    }
                }
            }
            else
                return -1;
        }
        else
            return 0f;
    }
    public float TranningSkill()
    {
        if (skillLevel < skillMaxLevel)
        {
            bool check = ItemInventory.Instance.CheckLimit(skillCost, CostItemType.Diamond);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= skillPercent)
            {
                if (monsterInstance.possibleSkillDatas.Count > 0)
                {
                    var data = monsterInstance.possibleSkillDatas.First();
                    monsterInstance.possibleSkillDatas.Remove(data);
                    monsterInstance.skillDatas.Add(data);

                    skillLevel = Mathf.Min(skillMaxLevel, skillLevel + 1);
                    skillPercent = Mathf.Max(0.01f, skillPercent - skillAddPercent);
                    return 1f;
                }
                else
                {
                    if (monsterInstance.percentSkillDatas.Count > 0)
                    {
                        var data = monsterInstance.possiblePercentSkillDatas.First();
                        monsterInstance.possiblePercentSkillDatas.Remove(data);
                        monsterInstance.possiblePercentSkillDatas.Add(data);

                        skillLevel = Mathf.Min(skillMaxLevel, skillLevel + 1);
                        skillPercent = Mathf.Max(0.01f, skillPercent - skillAddPercent);
                        return 1f;
                    }
                    else
                    {
                        var data = monsterInstance.possibleTriggerSkillDatas.First();
                        monsterInstance.possibleTriggerSkillDatas.Remove(data);
                        monsterInstance.possibleTriggerSkillDatas.Add(data);

                        skillLevel = Mathf.Min(skillMaxLevel, skillLevel + 1);
                        skillPercent = Mathf.Max(0.01f, skillPercent - skillAddPercent);
                        return 1f;
                    }
                }
            }
            else
                return -1;
        }
        else
            return 0f;
    }
    public float TranningAbility(out Sprite skillValue)
    {
        skillValue = null;
        if (abilityLevel < abilityMaxLevel)
        {
            bool check = ItemInventory.Instance.CheckLimit(abilityCost, CostItemType.Diamond);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= abilityPercent)
            {
                var data = monsterInstance.possibleAbilitieDatas.First();
                monsterInstance.possibleAbilitieDatas.Remove(data);
                monsterInstance.abilities.Add(data);

                abilityLevel = Mathf.Min(abilityMaxLevel, abilityLevel + 1);
                abilityPercent = Mathf.Max(0.01f, abilityPercent - abilityAddPercent);
                skillValue = MonsterDataBase.Instance.abilityDatas[data].icon;
                return 1f;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }
    public float TranningAbility()
    {
        if (abilityLevel < abilityMaxLevel)
        {
            bool check = ItemInventory.Instance.CheckLimit(abilityCost, CostItemType.Diamond);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= abilityPercent)
            {
                var data = monsterInstance.possibleAbilitieDatas.First();
                monsterInstance.possibleAbilitieDatas.Remove(data);
                monsterInstance.abilities.Add(data);

                abilityLevel = Mathf.Min(abilityMaxLevel, abilityLevel + 1);
                abilityPercent = Mathf.Max(0.01f, abilityPercent - abilityAddPercent);
                return 1f;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }
    public float TranningIq(out string skillValue)
    {
        skillValue = "";
        if (iqLevel < iqMaxLevel)
        {
            bool check = ItemInventory.Instance.CheckLimit(iqCost, CostItemType.Money);
            if (check == false)
                return -1f;

            float random = Random.Range(0f, 1f);

            if (random <= iqPercent)
            {
                if(monsterInstance.possibleConfirmSkillPriorityDatas.Count > 0 && monsterInstance.possibleSelectDetailTargetTypeDatas.Count > 0)
                {
                    int random2 = Random.Range(0, 2);
                    if(random2 == 0)
                    {
                        var data = monsterInstance.possibleConfirmSkillPriorityDatas[Random.Range(0, monsterInstance.possibleConfirmSkillPriorityDatas.Count)];
                        monsterInstance.possibleConfirmSkillPriorityDatas.Remove(data);
                        monsterInstance.confirmSkillPrioritys.Add(data);
                        skillValue = "complete";
                    }
                    else
                    {
                        var data = monsterInstance.possibleSelectDetailTargetTypeDatas[Random.Range(0, monsterInstance.possibleSelectDetailTargetTypeDatas.Count)];
                        monsterInstance.possibleSelectDetailTargetTypeDatas.Remove(data);
                        monsterInstance.selectDetailTargetTypes.Add(data);
                        skillValue = "complete";
                    }
                }
                else
                {
                    if(monsterInstance.possibleConfirmSkillPriorityDatas.Count > 0)
                    {
                        var data = monsterInstance.possibleConfirmSkillPriorityDatas[Random.Range(0, monsterInstance.possibleConfirmSkillPriorityDatas.Count)];
                        monsterInstance.possibleConfirmSkillPriorityDatas.Remove(data);
                        monsterInstance.confirmSkillPrioritys.Add(data);
                        skillValue = "complete";
                    }
                    else
                    {
                        var data = monsterInstance.possibleSelectDetailTargetTypeDatas[Random.Range(0, monsterInstance.possibleSelectDetailTargetTypeDatas.Count)];
                        monsterInstance.possibleSelectDetailTargetTypeDatas.Remove(data);
                        monsterInstance.selectDetailTargetTypes.Add(data);
                        skillValue = "complete";
                    }
                }

                iqLevel = Mathf.Min(iqMaxLevel, iqLevel + 1);
                return 1f;
            }
            else
                return -1f;
        }
        else
            return 0f;
    }
}
