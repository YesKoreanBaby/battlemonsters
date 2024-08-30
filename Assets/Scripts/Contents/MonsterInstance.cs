using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleRecode
{
    public int winCount;
    public int battleCount;
    public int price;
}
public class MonsterInstance
{
    public float maxHp;
    public float hp;

    public float maxMp;
    public float mp;
    public float manaRecoveryRatio;
    public float hpRecoveryRatio;

    public float dex;
    public float maxDex;
    public float repeatRatio;
    public float creaticalRatio;

    public float atk;
    public float def;

    public SelectSkillAIType battleAIType;
    public MonsterWeight monsterWeight;
    public ConfirmSkillPriority currentConfirmSkillPriority;
    public SelectDetailTargetType currentSelectDetailTargetType;
    public Status status;

    public MonsterHeathState heathState;
    public TranningCicleInstance tranningCicleInstance;

    public MonsterData monsterData;
    public List<SkillData> skillDatas = new List<SkillData>();
    public List<Tuple<int, SkillData>> percentSkillDatas = new List<Tuple<int, SkillData>>();
    public List<Tuple<SkillTrigger, SkillData>> triggerSkillDatas = new List<Tuple<SkillTrigger, SkillData>>();
    public List<AbilityType> abilities = new List<AbilityType>();
    public List<ConfirmSkillPriority> confirmSkillPrioritys = new List<ConfirmSkillPriority>();
    public List<SelectDetailTargetType> selectDetailTargetTypes = new List<SelectDetailTargetType>();

    public List<SkillData> possibleSkillDatas = new List<SkillData>();
    public List<PercentSkillData> possiblePercentSkillDatas = new List<PercentSkillData>();
    public List<TriggerSkillData> possibleTriggerSkillDatas = new List<TriggerSkillData>();
    public List<AbilityType> possibleAbilitieDatas = new List<AbilityType>();
    public List<ConfirmSkillPriority> possibleConfirmSkillPriorityDatas = new List<ConfirmSkillPriority>();
    public List<SelectDetailTargetType> possibleSelectDetailTargetTypeDatas = new List<SelectDetailTargetType>();
    public List<DeadTranningType> deadTrannings;
    public BattleTranningType[] battleTrannings;
    public MonsterInstance previousMonsterData;
    public int originalPriority = 0;
    public BattleRecode battleRecode;

    public static int hashKey = -2147483648;
    public bool DexUpdate()
    {
        if (dex >= maxDex)
            return true;
        else
        {
            dex += Time.deltaTime;
            return false;
        }
    }

    public static MonsterInstance Instance(MonsterData data, bool isAI = false, TranningType aiTranningType = TranningType.Random)
    {
        MonsterInstance instance = new MonsterInstance();
        instance.monsterData = data;

        instance.skillDatas.AddRange(data.datas);
        instance.abilities.AddRange(data.abilities);
        instance.confirmSkillPrioritys.AddRange(data.confirmSkillPrioritys);
        instance.selectDetailTargetTypes.AddRange(data.selectDetailTargetTypes);

        for(int i = 0; i < data.percentSkillDatas.Count; ++i)
        {
            Tuple<int, SkillData> item = new Tuple<int, SkillData>(data.percentSkillDatas[i].percent, data.percentSkillDatas[i].skillData);
            instance.percentSkillDatas.Add(item);
        }

        for (int i = 0; i < data.triggerSkillDatas.Count; ++i)
        {
            Tuple<SkillTrigger, SkillData> item = new Tuple<SkillTrigger, SkillData>(data.triggerSkillDatas[i].skillTrigger, data.triggerSkillDatas[i].skillData);
            instance.triggerSkillDatas.Add(item);
        }
        instance.currentConfirmSkillPriority = data.confirmSkillPrioritys[0];
        instance.currentSelectDetailTargetType = data.selectDetailTargetTypes[0];

        instance.hp = instance.maxHp = data.hp;
        instance.mp = instance.maxMp = data.mp;
        instance.manaRecoveryRatio = data.manaRecoveryRatio;
        instance.hpRecoveryRatio = data.hpRecoveryRatio;
        instance.dex = 0f;
        instance.maxDex = data.dex;
        instance.repeatRatio = data.repeatRatio;
        instance.creaticalRatio = data.creaticalRatio;
        instance.atk = data.atk;
        instance.def = data.def;
        instance.battleAIType = data.battleAIType;
        instance.monsterWeight = data.monsterWeight;
        instance.status = data.status;
        instance.heathState = MonsterHeathState.None;

        if(isAI == false)
        {
            instance.originalPriority = hashKey;
            hashKey++;
        }
        else
            instance.originalPriority = Random.Range(int.MinValue, int.MaxValue);

        instance.possibleSkillDatas.AddRange(data.possibleSkillDatas);
        instance.possibleTriggerSkillDatas.AddRange(data.possibleTriggerSkillDatas);
        instance.possiblePercentSkillDatas.AddRange(data.possiblePercentSkillDatas);
        instance.possibleSelectDetailTargetTypeDatas.AddRange(data.possibleSelectDetailTargetTypeDatas);
        instance.possibleConfirmSkillPriorityDatas.AddRange(data.possibleConfirmSkillPriorityDatas);
        instance.possibleAbilitieDatas.AddRange(data.possibleAbilitieDatas);

        TranningCicleData tranningData = null;

        if(MonsterDataBase.Instance != null)
        {
            if (isAI == false)
            {
                if (instance.monsterData.tranningType == TranningType.Random)
                {
                    var random = (TranningType)Random.Range(1, (int)TranningType.End);
                    tranningData = MonsterDataBase.Instance.tranningDatas[random];
                }
                else
                    tranningData = MonsterDataBase.Instance.tranningDatas[instance.monsterData.tranningType];
            }
            else
                tranningData = MonsterDataBase.Instance.tranningDatas[aiTranningType];

            instance.tranningCicleInstance = TranningCicleInstance.Instance(instance, tranningData);
        }
     
        instance.battleTrannings = new BattleTranningType[5];
        return instance;
    }

    public static MonsterInstance Instance(MonsterInstance data)
    {
        bool editCheck = CombatUI.Instance.activeEdit;

        MonsterInstance instance = new MonsterInstance();
        instance.monsterData = data.monsterData;
        instance.skillDatas.AddRange(data.skillDatas);
        instance.percentSkillDatas.AddRange(data.percentSkillDatas);
        instance.triggerSkillDatas.AddRange(data.triggerSkillDatas);
        instance.abilities.AddRange(data.abilities);
        instance.currentConfirmSkillPriority = data.currentConfirmSkillPriority;
        instance.currentSelectDetailTargetType = data.currentSelectDetailTargetType;

        float value = 1f;
        if(editCheck == false)
        {
            if (data.heathState == MonsterHeathState.Crippled)
                value = 0.5f;
            else if (data.heathState == MonsterHeathState.CrippedStrong)
                value = 0.25f;
        }
        

        float value2 = 1f;
        if(editCheck == false)
        {
            if (data.heathState == MonsterHeathState.FullCondition)
                value2 = 2f;
        }

        float swordMasterAtk = ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.symbolofSwordMaster) ? Mathf.RoundToInt(instance.atk * 0.25f) : 0f;
        float gunslingerDef = ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.symbolofGunslinger) ? Mathf.RoundToInt(instance.def * 0.25f) : 0f;

        instance.hp = (editCheck == false) ? data.hp * value : data.maxHp;
        instance.maxHp = data.maxHp;

        instance.mp = instance.maxMp = data.maxMp;
        instance.mp *= value;

        instance.manaRecoveryRatio = data.manaRecoveryRatio;
        instance.hpRecoveryRatio = data.hpRecoveryRatio;

        instance.maxDex = data.maxDex * (1 / value2);
        instance.dex = instance.abilities.Contains(AbilityType.Spurt) ? (instance.maxDex * 0.33f) * value : 0f;

        instance.repeatRatio = data.repeatRatio;
        instance.creaticalRatio = data.creaticalRatio;
        instance.atk = (swordMasterAtk + data.atk) * value2;
        instance.def = (gunslingerDef + data.def) * value2;
        instance.battleAIType = data.battleAIType;
        instance.monsterWeight = data.monsterWeight;
        instance.status = data.status;
        instance.originalPriority = data.originalPriority;

        if(editCheck == true)
        {
            instance.confirmSkillPrioritys.AddRange(data.confirmSkillPrioritys);
            instance.selectDetailTargetTypes.AddRange(data.selectDetailTargetTypes);
        }
        return instance;
    }
    public void UpdateOrigin(MonsterInstance battleInstance, int winAndLose)
    {
        hp = battleInstance.hp;
        if (hp > 0 && hp <= 1)
            hp = 1;

        hp = Mathf.Ceil(hp);
        BattleStateUpdate();

        UpdateBattleRecode(winAndLose);
    }
    public void UpdateBattleRecode(int windAndLose)
    {
        if (battleRecode == null)
            battleRecode = new BattleRecode();

        battleRecode.battleCount++;
        if (windAndLose == 1)
            battleRecode.winCount++;

        int minPrice = Mathf.RoundToInt((100 * battleRecode.battleCount) * ((float)battleRecode.winCount / (float)battleRecode.battleCount));
        int maxPrice = this.abilities.Contains(AbilityType.Potential) ? minPrice * 4 : minPrice * 2;
        int avil = Random.Range(minPrice, maxPrice + 1);

        int random = Random.Range(0, 5);
        int multifly = (int)CombatManager.Instance.GetFriendShipLevel() + 1;
        if (random == 0)
            battleRecode.price = Mathf.Min(99999, Mathf.RoundToInt(minPrice * multifly * 0.75f));
        else if (random == 4)
            battleRecode.price = Mathf.Min(Mathf.RoundToInt(maxPrice * multifly * 0.75f));
        else
            battleRecode.price = Mathf.Min(Mathf.RoundToInt(avil * multifly * 0.75f));
    }
    public void ClearBattleRecode()
    {
        if(battleRecode != null)
        {
            battleRecode.battleCount = battleRecode.winCount = 0;
        }
    }
    public void Copy(MonsterInstance data)
    {
        for(int i = 0; i < data.skillDatas.Count; ++i)
        {
            if (!skillDatas.Contains(data.skillDatas[i]))
                skillDatas.Add(data.skillDatas[i]);  
        }
        for (int i = 0; i < data.percentSkillDatas.Count; ++i)
        {
            if (!percentSkillDatas.Contains(data.percentSkillDatas[i]))
                percentSkillDatas.Add(data.percentSkillDatas[i]);
        }
        for (int i = 0; i < data.triggerSkillDatas.Count; ++i)
        {
            if (!triggerSkillDatas.Contains(data.triggerSkillDatas[i]))
                triggerSkillDatas.Add(data.triggerSkillDatas[i]);
        }
        for (int i = 0; i < data.abilities.Count; ++i)
        {
            if (!abilities.Contains(data.abilities[i]))
                abilities.Add(data.abilities[i]);
        }

        currentConfirmSkillPriority = data.currentConfirmSkillPriority;
        currentSelectDetailTargetType = data.currentSelectDetailTargetType;

        maxHp = data.hp;
        maxMp = data.mp;
        manaRecoveryRatio = data.manaRecoveryRatio;
        hpRecoveryRatio = data.hpRecoveryRatio;

        if(maxDex != data.maxDex)
            dex = 0;

        maxDex = data.maxDex;

        repeatRatio = data.repeatRatio;
        creaticalRatio = data.creaticalRatio;
        atk = data.atk;
        def = data.def;
        battleAIType = data.battleAIType;
        monsterWeight = data.monsterWeight;
        status = data.status;
        originalPriority = data.originalPriority;
    }

    public void BattleStateUpdate()
    {
        if(hp <= 0)
        {
            if(monsterData.isDeadLock == false)
            {
                float random = Random.Range(0f, 1f);

                if(random <= 0.01f)
                {
                    var nextMonsterInstance = Instance(MonsterDataBase.Instance.deadTable[Random.Range(0, MonsterDataBase.Instance.deadTable.Length)]);
                    nextMonsterInstance.previousMonsterData = this;
                    int index = TranningUI.Instance.playerInventory.FindIndex(this);
                    TranningUI.Instance.playerInventory.monsterDatas[index] = nextMonsterInstance;
                    CombatUI.Instance.UpdateMonsterImages();
                    ClearBattleRecode();
                    return;
                }
            }
        }
        else
        {
            var index = Array.FindIndex(battleTrannings, x => x == BattleTranningType.None);
            if(index != -1)
            {
                battleTrannings[index] = (BattleTranningType)Random.Range((int)BattleTranningType.None + 1, (int)BattleTranningType.End);
                if (battleTrannings[index] == BattleTranningType.Atk)
                    atk += 2;
                else if (battleTrannings[index] == BattleTranningType.Def)
                    def += 2;
                else if (battleTrannings[index] == BattleTranningType.Mp)
                {
                    maxMp += 5;
                    mp = Mathf.Min(maxMp, mp + 5);
                }
                else if (battleTrannings[index] == BattleTranningType.Hp)
                {
                    maxHp += 5;
                    if (heathState != MonsterHeathState.Faint)
                        hp = Mathf.Min(maxHp, hp + 5);
                }
                else if (battleTrannings[index] == BattleTranningType.Balance)
                {
                    maxHp += 1;
                    if (heathState != MonsterHeathState.Faint)
                        hp = Mathf.Min(maxHp, hp + 1);

                    maxMp += 5;
                    mp = Mathf.Min(maxMp, mp + 5);

                    atk += 1;
                    def += 1;

                }
            }
        }
        //상태업데이트
        if (heathState == MonsterHeathState.None)
        {
            if (hp <= 0f)
            {
                float random = Random.Range(0f, 1f);
                if (random <= 0.95f)
                    heathState = MonsterHeathState.Faint;
                else
                {
                    random = Random.Range(0f, 1f);

                    if (random <= 0.75f)
                        heathState = MonsterHeathState.Crippled;
                    else
                        heathState = MonsterHeathState.CrippedStrong;
                    hp = maxHp * hpRecoveryRatio;
                }
            }
            else
            {
                if (hp <= (maxHp * 0.33f))
                {
                    float random = Random.Range(0f, 1f);
                    if (random <= 0.25f)
                        heathState = MonsterHeathState.Blooding;
                }

                float random2 = Random.Range(0f, 1f);
                if (random2 <= 0.1f)
                {
                    heathState = MonsterHeathState.FullCondition;
                }
            }
        }
        else if (heathState == MonsterHeathState.Blooding)
        {
            if (hp <= 0f)
            {
                float random = Random.Range(0f, 1f);
                if (random <= 0.95f)
                    heathState = MonsterHeathState.Faint;
                else
                {
                    random = Random.Range(0f, 1f);

                    if (random <= 0.75f)
                        heathState = MonsterHeathState.Crippled;
                    else
                        heathState = MonsterHeathState.CrippedStrong;
                    hp = maxHp * hpRecoveryRatio;
                }
            }
            else
            {
                float random = Random.Range(0f, 1f);
                if (random <= 0.25f)
                    heathState = MonsterHeathState.None;
            }
        }
        else if (heathState == MonsterHeathState.FullCondition)
        {
            if (hp <= 0f)
                heathState = MonsterHeathState.Faint;
            else
                heathState = MonsterHeathState.None;
        }
        else if (heathState == MonsterHeathState.CrippedStrong)
        {
            if (hp <= 0f)
                heathState = MonsterHeathState.Faint;
        }
        else if (heathState == MonsterHeathState.Crippled)
        {
            if (hp <= 0f)
            {
                float random = Random.Range(0f, 1f);
                if (random <= 0.75f)
                    heathState = MonsterHeathState.Faint;
                else
                    heathState = MonsterHeathState.CrippedStrong;
            }
        }
    }

    public void RestStateUpdate()
    {
        if (heathState == MonsterHeathState.None)
        {
            float random = Random.Range(0f, 1f);
            if (random <= 0.1f)
                heathState = MonsterHeathState.FullCondition;
        }
        else if(heathState == MonsterHeathState.Blooding)
        {
            float random = Random.Range(0f, 1f);
            if (random <= 0.25f)
                heathState = MonsterHeathState.None;
        }
        else if (heathState == MonsterHeathState.Faint)
        {
            float random = Random.Range(0f, 1f);
            if (random <= 0.25f)
            {
                heathState = MonsterHeathState.None;
                hp = 1;
            }
        }
    }

    public MonsterInstance Evolution()
    {
        if (this.monsterData.evolutionMonsterData == null && this.monsterData.evolutionRangeType == EvolutionRangeType.None)
            return null;

        MonsterData data = null;
        if(this.monsterData.evolutionMonsterData != null)
            data = this.monsterData.evolutionMonsterData;
        if(this.monsterData.evolutionRangeType != EvolutionRangeType.None)
        {
            var type = this.monsterData.evolutionRangeType;
            if (type == EvolutionRangeType.RandomBox)
                data = MonsterDataBase.Instance.randomBoxies[Random.Range(0, MonsterDataBase.Instance.randomBoxies.Length)];
            else if (type == EvolutionRangeType.Bug)
                data = MonsterDataBase.Instance.bugs[Random.Range(0, MonsterDataBase.Instance.bugs.Length)];
            else if (type == EvolutionRangeType.Dragon)
                data = MonsterDataBase.Instance.dragons[Random.Range(0, MonsterDataBase.Instance.dragons.Length)];
            else if(type == EvolutionRangeType.RandomBox2)
                data = MonsterDataBase.Instance.randomBoxies2[Random.Range(0, MonsterDataBase.Instance.randomBoxies2.Length)];
        }

        MonsterInstance instance = new MonsterInstance();
        instance.monsterData = data;
        instance.previousMonsterData = this;

        instance.skillDatas.AddRange(data.datas);
        instance.abilities.AddRange(data.abilities);
        instance.confirmSkillPrioritys.AddRange(data.confirmSkillPrioritys);
        instance.selectDetailTargetTypes.AddRange(data.selectDetailTargetTypes);

        for (int i = 0; i < data.percentSkillDatas.Count; ++i)
        {
            Tuple<int, SkillData> item = new Tuple<int, SkillData>(data.percentSkillDatas[i].percent, data.percentSkillDatas[i].skillData);
            instance.percentSkillDatas.Add(item);
        }

        for (int i = 0; i < data.triggerSkillDatas.Count; ++i)
        {
            Tuple<SkillTrigger, SkillData> item = new Tuple<SkillTrigger, SkillData>(data.triggerSkillDatas[i].skillTrigger, data.triggerSkillDatas[i].skillData);
            instance.triggerSkillDatas.Add(item);
        }
        instance.currentConfirmSkillPriority = data.confirmSkillPrioritys[0];
        instance.currentSelectDetailTargetType = data.selectDetailTargetTypes[0];

        instance.hp = instance.maxHp = data.hp;
        instance.mp = instance.maxMp = data.mp;
        instance.manaRecoveryRatio = data.manaRecoveryRatio;
        instance.hpRecoveryRatio = data.hpRecoveryRatio;
        instance.dex = 0f;
        instance.maxDex = data.dex;
        instance.repeatRatio = data.repeatRatio;
        instance.creaticalRatio = data.creaticalRatio;
        instance.atk = data.atk;
        instance.def = data.def;
        instance.battleAIType = data.battleAIType;
        instance.monsterWeight = data.monsterWeight;
        instance.status = data.status;
        instance.heathState = MonsterHeathState.None;

        instance.originalPriority = hashKey;
        hashKey++;

        instance.possibleSkillDatas.AddRange(data.possibleSkillDatas);
        instance.possibleTriggerSkillDatas.AddRange(data.possibleTriggerSkillDatas);
        instance.possiblePercentSkillDatas.AddRange(data.possiblePercentSkillDatas);
        instance.possibleSelectDetailTargetTypeDatas.AddRange(data.possibleSelectDetailTargetTypeDatas);
        instance.possibleConfirmSkillPriorityDatas.AddRange(data.possibleConfirmSkillPriorityDatas);
        instance.possibleAbilitieDatas.AddRange(data.possibleAbilitieDatas);

        if(data.tranningType != TranningType.Locked)
        {
            instance.tranningCicleInstance = this.tranningCicleInstance.Evolution(instance);
        }
        else
        {
            var tranningData = MonsterDataBase.Instance.tranningDatas[TranningType.Locked];
            instance.tranningCicleInstance = TranningCicleInstance.Instance(instance, tranningData);
        }

        instance.battleTrannings = new BattleTranningType[5];
        return instance;
    }
    public MonsterInstance Karma()
    {
        if(previousMonsterData == null)
        {
            if (monsterData == MonsterDataBase.Instance.darkKnightWisp)
                previousMonsterData = Instance(MonsterDataBase.Instance.darkKnight);
            else if (monsterData == MonsterDataBase.Instance.undeadKingWisp)
                previousMonsterData = Instance(MonsterDataBase.Instance.undeadKing);
        }
        if(previousMonsterData != null)
        {
            if (previousMonsterData.deadTrannings == null)
                previousMonsterData.deadTrannings = new List<DeadTranningType>();

            if (this.monsterData.tranningType != TranningType.Locked)
                previousMonsterData.tranningCicleInstance.EvolutionCopy(this);

            if(this.monsterData == MonsterDataBase.Instance.redGhost)
            {
                previousMonsterData.atk += 5;
                previousMonsterData.def += 5;
                previousMonsterData.deadTrannings.Add(DeadTranningType.Red);
            }
            else if(this.monsterData == MonsterDataBase.Instance.greenGhost)
            {
                previousMonsterData.maxDex = Mathf.Max(0.1f, previousMonsterData.maxDex - 0.1f);
                previousMonsterData.deadTrannings.Add(DeadTranningType.Green);
            }
            else if (this.monsterData == MonsterDataBase.Instance.pupleGhost)
            {
                previousMonsterData.maxHp += 10;
                if (heathState != MonsterHeathState.Faint)
                    previousMonsterData.hp = Mathf.Min(previousMonsterData.maxHp, previousMonsterData.hp + 10);

                previousMonsterData.maxMp += 10;
                previousMonsterData.mp = Mathf.Min(previousMonsterData.maxMp, previousMonsterData.mp + 10);

                previousMonsterData.deadTrannings.Add(DeadTranningType.Puple);
            }

        }

        if(previousMonsterData != null)
        {
            previousMonsterData.hp = Mathf.CeilToInt(previousMonsterData.maxHp * 0.1f);
            previousMonsterData.ClearBattleRecode();
        }
        return previousMonsterData;
    }
    public void SplitDeadTranning(out float hp, out float mp, out float atk, out float def, out float dex)
    {
        hp = mp = atk = def = dex = 0;
        if (deadTrannings != null && deadTrannings.Count > 0)
        {
            for (int i = 0; i < deadTrannings.Count; ++i)
            {
                if (deadTrannings[i] == DeadTranningType.Puple)
                {
                    mp += 10;
                    hp += 10;
                }
                else if (deadTrannings[i] == DeadTranningType.Red)
                {
                    atk += 5;
                    def += 5;
                }
                else if (deadTrannings[i] == DeadTranningType.Green)
                {
                    dex += 0.1f;
                }
            }
        }
    }

    public void SplitBattleTranning(out float hp, out float mp, out float atk, out float def)
    {
        hp = mp = atk = def = dex = 0;
        for(int i = 0; i < battleTrannings.Length; ++i)
        {
            if (battleTrannings[i] == BattleTranningType.Atk)
                atk += 2;
            else if (battleTrannings[i] == BattleTranningType.Def)
                def += 2;
            else if (battleTrannings[i] == BattleTranningType.Mp)
                mp += 5;
            else if (battleTrannings[i] == BattleTranningType.Hp)
                hp += 5;
            else if (battleTrannings[i] == BattleTranningType.Balance)
            {
                hp += 1;
                mp += 1;
                atk += 1;
                def += 1;

            }
        }
    }
}
