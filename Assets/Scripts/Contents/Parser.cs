using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class TranningCicleInstanceParser
{
    public MonsterInstanceParser monsterInstance;
    public string tranningData;

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

    public int skillLevel;
    public int skillMaxLevel;
    public float skillPercent;
    public float skillAddPercent;
    public int skillCost = 1;

    public int abilityLevel;
    public int abilityMaxLevel;
    public float abilityPercent;
    public float abilityAddPercent;
    public int abilityCost = 1;

    public int iqLevel;
    public int iqMaxLevel;
    public float iqPercent;
    public int iqCost = 5;
}
[System.Serializable]
public class MonsterInstanceParser
{
    public float maxHp;
    public float hp;

    public float maxMp;
    public float mp;

    public float atk;
    public float def;

    public float maxDex;

    public float manaRecoveryRatio;
    public float hpRecoveryRatio;

    public float dodge;
    public float critial;

    public SelectSkillAIType battleAIType;
    public MonsterWeight monsterWeight;
    public ConfirmSkillPriority currentConfirmSkillPriority;
    public SelectDetailTargetType currentSelectDetailTargetType;
    public MonsterHeathState healthState;
    public TranningCicleInstanceParser tranningCicleInstance;

    public string monsterData;
    public List<string> skillDatas;
    public List<Tuple<int, string>> percentSkillDatas;
    public List<Tuple<SkillTrigger, string>> triggerSkillDatas;
    public List<AbilityType> abilityTypes;
    public List<ConfirmSkillPriority> confirmSkillPriorities;
    public List<SelectDetailTargetType> selectDetailTargetTypes;

    public List<string> possibleSkillDatas;
    public List<string> possiblePercentSkillDatas;
    public List<string> possibleTriggerSkillDatas;
    public List<AbilityType> possibleAbilityTypes;
    public List<ConfirmSkillPriority> possibleConfirmSkillPrioritys;
    public List<SelectDetailTargetType> possibleSelectDetailTargetTypes;
    public List<DeadTranningType> deadTranningTypes;
    public BattleTranningType[] battleTranningTypes;

    public MonsterInstanceParser previousMonsterInstance;

    public int originalPriority;
    public BattleRecode battleRecode;
}

public class MonsterInstanceParserInDataBase
{
    public float maxHp;
    public float hp;

    public float maxMp;
    public float mp;

    public float atk;
    public float def;

    public float maxDex;

    public float manaRecoveryRatio;
    public float hpRecoveryRatio;

    public float dodge;
    public float critial;

    public SelectSkillAIType battleAIType;
    public MonsterWeight monsterWeight;
    public ConfirmSkillPriority currentConfirmSkillPriority;
    public SelectDetailTargetType currentSelectDetailTargetType;
    public MonsterHeathState healthState;

    public string monsterData;

    public List<string> skillDatas;
    public List<Tuple<int, string>> percentSkillDatas;
    public List<Tuple<SkillTrigger, string>> triggerSkillDatas;
    public List<AbilityType> abilityTypes;
    public List<ConfirmSkillPriority> confirmSkillPriorities;
    public List<SelectDetailTargetType> selectDetailTargetTypes;

    public int originalPriority;
}

[System.Serializable]
public class LeagueDataParser
{
    public int currentLeagueNumber;
    public List<TeamDataParser> league_3TeamDatas;
    public List<TeamDataParser> league_2TeamDatas;
    public List<TeamDataParser> league_1TeamDatas;
    public List<TeamDataParser> league_0TeamDatas;
    public TeamDataParser defeindingChampionTeamData;
    public List<Tuple<TeamDataParser, TeamDataParser>> finalMatch;

    public Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>> league_3;
    public Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>> league_2;
    public Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>> league_1;
    public Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>> league_0;
    public List<string> battleRecode;
    public int xIndex;
    public int yIndex;
}

public class StoryDataParser
{
    public StageSelectParser level_1;
    public StageSelectParser level_1_hidden;
    public StageSelectParser level_1_hidden_2;

    public StageSelectParser level_2;
    public StageSelectParser level_3;

    public StageSelectParser level_4;
    public StageSelectParser level_4_hidden;
    public StageSelectParser level_4_hidden_2;

    public StageSelectParser level_5;

    public StageSelectParser level_6;
    public StageSelectParser level_6_hidden;
    public StageSelectParser level_6_hidden2;
    public StageSelectParser level_6_hidden2_1;

    public StageSelectParser special_1;
    public StageSelectParser special_2;
    public StageSelectParser special_3;
}

[System.Serializable]
public class BoxInfoParser
{
    public bool isSell;
    public string resource;
    public CostItemType costType;
    public int count;
    public bool isHidden;
}

[System.Serializable]
public class StageSelectParser
{
    public StoryTransitionType storyTransitionType;
    public bool isTransition;
    public bool isCleared;
    public bool isNext;
    public bool clearBlock;
}

[System.Serializable]
public class TeamDataParser
{
    public string teamName;
    public int win;
    public int lose;
    public int draw;
    public int score;
}

[System.Serializable]
public class RulletDataParser
{
    public int count = 0;
    public int objectType = -1;
    public CostItemType costType = CostItemType.Money;
    public string objectName = "";

    public bool isDevil = false;
    public bool isBlock = false;
}
[System.Serializable]
public class GameData
{
    public bool isTutorial;
    public int monsterHashKey;
    public int money;
    public int diamond;
    public int fame;
    public int saveMoney;
    public int friendShipBattleCount;
    public int friendShipBattleWin;
    public int timeCount;
    public int currrentTimeCount;
    public bool blockGamebling;
    public bool blockResetItemForAds;
    public bool blockResetMonsterForAds;
    public int diamondToGold;
    public FriendShipLevel friendShipLevel;
    public float startBgmValue;
    public float startEffectValue;
    public float startBattleSpeedValue;
    public float bgmValue;
    public float effectValue;
    public float battleSpeedValue;
    public float addBattleSpeed;
    public SystemLanguage language;

    public TeamDataParser currentTeamData;
    public LeagueDataParser leagueData;

    public string[] consumeItemDatas;
    public string[] treagureDatas;
    public List<CostItemIndigrident> rulletCostItemDatas;
    public List<string> rulletMonsterDatas;
    public List<string> rulletItemDatas;
    public BoxInfoParser[] itemBoxInfos;
    public BoxInfoParser[] monsterBoxInfos;

    public StoryDataParser storyDataParser;

    public MonsterInstanceParser[] monsterInstances;

    public List<RewardData> rewardDatas;

    public RulletDataParser[] rulletDatas;
}

public class Parser : MonoBehaviour
{
    private static Parser instance = null;
    public static Parser Instance { get { return instance; } }
   
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameData gameData;
        bool check = LoadData(out gameData);
     

       

        if (check == false)
        {
          
            TextManager.Instance.language = (Application.systemLanguage == SystemLanguage.Korean) ? SystemLanguage.Korean : SystemLanguage.English;
         
            MainSystemUI.Instance.SetTextFormats();
            SystemUI.Instance.SetTextFormats();
        
            SelectTeamUI.Instance.PopUp();
          
        }
        else
        {
         
            StoryUI.Instance.SetStageSelectSlotsForLoading();
            CombatUI.Instance.UpdateMonsterImages();
            CalendarUI.Instance.UpdateCalander();
            MainSystemUI.Instance.PopUp();
          
        }

    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
    private void OnApplicationPause(bool pause)
    {
        SaveData();
    }
    public void SaveData()
    {
        string path;
        CreateFolder(out path);
        ConvertData(path);
    }

    public void RemoveData()
    {
        string path = $"{DataManager.Instance.GetAbsolutePath()}/_0";
        DataManager.Instance.Remove(path, "_0");
    }
    public bool LoadData(out GameData gameData)
    {
        string path = $"{DataManager.Instance.GetAbsolutePath()}/_0";
        var check = DataManager.Instance.Load(path, "_0", out gameData);
        if (check == false)
            return false;

        check = DeconvertData(gameData);
        if (check == false)
            return false;
        return true;

    }

    public void CreateFolder(out string path)
    {
        path = $"{DataManager.Instance.GetAbsolutePath()}/_0";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public MonsterInstanceParserInDataBase ConvertMonsterInstanceParserInDataBase(MonsterInstance monsterInstance)
    {
        if (monsterInstance != null)
        {
            MonsterData data = monsterInstance.monsterData;
            MonsterInstanceParserInDataBase parser = new MonsterInstanceParserInDataBase();
           
            parser.maxHp = monsterInstance.maxHp;
            parser.hp = monsterInstance.hp;

            parser.maxMp = monsterInstance.maxMp;
            parser.mp = monsterInstance.mp;

            parser.atk = monsterInstance.atk;
            parser.def = monsterInstance.def;

            parser.maxDex = monsterInstance.maxDex;

            parser.manaRecoveryRatio = monsterInstance.manaRecoveryRatio;
            parser.hpRecoveryRatio = monsterInstance.hpRecoveryRatio;

            parser.dodge = monsterInstance.repeatRatio;
            parser.critial = monsterInstance.creaticalRatio;

            parser.battleAIType = monsterInstance.battleAIType;
            parser.monsterWeight = monsterInstance.monsterWeight;
            parser.currentConfirmSkillPriority = monsterInstance.currentConfirmSkillPriority;
            parser.currentSelectDetailTargetType = monsterInstance.currentSelectDetailTargetType;
            parser.healthState = monsterInstance.heathState;

            parser.monsterData = monsterInstance.monsterData.name;

            parser.skillDatas = new List<string>();
            for (int i = 0; i < monsterInstance.skillDatas.Count; ++i)
            {
                parser.skillDatas.Add(monsterInstance.skillDatas[i].name);
            }
            parser.percentSkillDatas = new List<Tuple<int, string>>();
            for (int i = 0; i < monsterInstance.percentSkillDatas.Count; ++i)
            {
                parser.percentSkillDatas.Add(new Tuple<int, string>(monsterInstance.percentSkillDatas[i].Item1, (monsterInstance.percentSkillDatas[i].Item2.name)));
            }
            parser.triggerSkillDatas = new List<Tuple<SkillTrigger, string>>();
            for (int i = 0; i < monsterInstance.triggerSkillDatas.Count; ++i)
            {
                parser.triggerSkillDatas.Add(new Tuple<SkillTrigger, string>(monsterInstance.triggerSkillDatas[i].Item1, (monsterInstance.triggerSkillDatas[i].Item2.name)));
            }

            parser.abilityTypes = new List<AbilityType>();
            for (int i = 0; i < monsterInstance.abilities.Count; ++i)
            {
                parser.abilityTypes.Add(monsterInstance.abilities[i]);
            }

            parser.confirmSkillPriorities = new List<ConfirmSkillPriority>();
            for (int i = 0; i < monsterInstance.confirmSkillPrioritys.Count; ++i)
            {
                parser.confirmSkillPriorities.Add(monsterInstance.confirmSkillPrioritys[i]);
            }
            parser.selectDetailTargetTypes = new List<SelectDetailTargetType>();
            for (int i = 0; i < monsterInstance.selectDetailTargetTypes.Count; ++i)
            {
                parser.selectDetailTargetTypes.Add(monsterInstance.selectDetailTargetTypes[i]);
            }

            parser.originalPriority = monsterInstance.originalPriority;
           
            return parser;
        }
        else
            return null;
    }
    public MonsterInstance DeConvertMonsterInstanceParserInDatraBase(MonsterInstanceParserInDataBase parser)
    {
        if (parser != null)
        {
            MonsterInstance instance = new MonsterInstance();
            instance.tranningCicleInstance = null;
            instance.previousMonsterData = null;

            instance.maxHp = parser.maxHp;
            instance.hp = parser.hp;
            if (parser.healthState != MonsterHeathState.Faint && instance.hp <= 0)
                instance.hp = 1;

            instance.maxMp = parser.maxMp;
            instance.mp = parser.mp;

            instance.atk = parser.atk;
            instance.def = parser.def;

            instance.maxDex = parser.maxDex;
            instance.dex = 0f;

            instance.manaRecoveryRatio = parser.manaRecoveryRatio;
            instance.hpRecoveryRatio = parser.hpRecoveryRatio;

            instance.repeatRatio = parser.dodge;
            instance.creaticalRatio = parser.critial;

            instance.battleAIType = parser.battleAIType;
            instance.monsterWeight = parser.monsterWeight;
            instance.currentConfirmSkillPriority = parser.currentConfirmSkillPriority;
            instance.currentSelectDetailTargetType = parser.currentSelectDetailTargetType;
            instance.heathState = parser.healthState;

            instance.monsterData = Resources.Load<MonsterData>($"MonsterData/{parser.monsterData}");
            if (instance.monsterData == null)
                throw new NotImplementedException();

            instance.skillDatas = new List<SkillData>();
            for (int i = 0; i < parser.skillDatas.Count; ++i)
            {
                SkillData skillData = Resources.Load<SkillData>($"SkillData/{parser.skillDatas[i]}");
                if (skillData == null)
                    throw new NotImplementedException();

                instance.skillDatas.Add(skillData);
            }

            instance.percentSkillDatas = new List<Tuple<int, SkillData>>();
            if(parser.percentSkillDatas != null)
            {
                for (int i = 0; i < parser.percentSkillDatas.Count; ++i)
                {
                    SkillData skillData = Resources.Load<SkillData>($"SkillData/{parser.percentSkillDatas[i].Item2}");
                    if (skillData == null)
                        throw new NotImplementedException();

                    instance.percentSkillDatas.Add(new Tuple<int, SkillData>(parser.percentSkillDatas[i].Item1, skillData));
                }
            }
            

            instance.triggerSkillDatas = new List<Tuple<SkillTrigger, SkillData>>();

            if(parser.triggerSkillDatas != null)
            {
                for (int i = 0; i < parser.triggerSkillDatas.Count; ++i)
                {
                    SkillData skillData = Resources.Load<SkillData>($"SkillData/{parser.triggerSkillDatas[i].Item2}");
                    if (skillData == null)
                        throw new NotImplementedException();

                    instance.triggerSkillDatas.Add(new Tuple<SkillTrigger, SkillData>(parser.triggerSkillDatas[i].Item1, skillData));
                }
            }
            

            instance.abilities = new List<AbilityType>();

            if(parser.abilityTypes != null)
            {
                for (int i = 0; i < parser.abilityTypes.Count; ++i)
                {
                    instance.abilities.Add(parser.abilityTypes[i]);
                }
            }
            

            instance.confirmSkillPrioritys = new List<ConfirmSkillPriority>();
            if(parser.confirmSkillPriorities != null)
            {
                for (int i = 0; i < parser.confirmSkillPriorities.Count; ++i)
                {
                    instance.confirmSkillPrioritys.Add(parser.confirmSkillPriorities[i]);
                }

                if (!instance.confirmSkillPrioritys.Contains(parser.currentConfirmSkillPriority))
                    instance.confirmSkillPrioritys.Insert(0, instance.currentConfirmSkillPriority);
            }
            

            instance.selectDetailTargetTypes = new List<SelectDetailTargetType>();
            if(parser.selectDetailTargetTypes != null)
            {
                for (int i = 0; i < parser.selectDetailTargetTypes.Count; ++i)
                {
                    instance.selectDetailTargetTypes.Add(parser.selectDetailTargetTypes[i]);
                }

                if (!instance.selectDetailTargetTypes.Contains(parser.currentSelectDetailTargetType))
                    instance.selectDetailTargetTypes.Insert(0, instance.currentSelectDetailTargetType);
            }
            

            instance.possibleSkillDatas = null;


            instance.possiblePercentSkillDatas = null;


            instance.possibleTriggerSkillDatas = null;


            instance.possibleAbilitieDatas = null;


            instance.possibleConfirmSkillPriorityDatas = null;


            instance.possibleSelectDetailTargetTypeDatas = null;
            instance.deadTrannings = null;
            instance.battleTrannings = null;
            instance.originalPriority = parser.originalPriority;
            instance.battleRecode = null;
            return instance;
        }
        else
        {
            return null;
        }
    }

    private void ConvertData(string path)
    {
        GameData gameData = new GameData();
        gameData.monsterHashKey = MonsterInstance.hashKey;
        gameData.money = ItemInventory.Instance.money;
        gameData.diamond = ItemInventory.Instance.diamond;
        gameData.fame = ItemInventory.Instance.fame;
        gameData.saveMoney = ItemInventory.Instance.saveMoney;
        gameData.friendShipBattleCount = ItemInventory.Instance.friendShipBattleCount;
        gameData.friendShipBattleWin = ItemInventory.Instance.friendShipBattleWin;
        gameData.timeCount = LeagueManager.Instance.timecount;
        gameData.currrentTimeCount = LeagueManager.Instance.currentTimeCount;
        gameData.blockGamebling = LeagueManager.Instance.blockGamebling;
        gameData.blockResetItemForAds = LeagueManager.Instance.blockResetItemForAds;
        gameData.blockResetMonsterForAds = LeagueManager.Instance.blockResetMonsterForAds;
        gameData.diamondToGold = LeagueManager.Instance.diamondToGold;
        gameData.friendShipLevel = LeagueManager.Instance.friendshipLevel;
        gameData.startBgmValue = SystemUI.Instance.startBgmValue;
        gameData.startEffectValue = SystemUI.Instance.startEffectValue;
        gameData.startBattleSpeedValue = SystemUI.Instance.startBattleSpeedValue;
        gameData.bgmValue = SystemUI.Instance.bgmBar.value;
        gameData.effectValue = SystemUI.Instance.effectBar.value;
        gameData.battleSpeedValue = SystemUI.Instance.battleSpeedBar.value;
        gameData.addBattleSpeed = SystemUI.Instance.addBattleSpeed;
        gameData.language = TextManager.Instance.language;

        var currentTeam = SelectTeamUI.Instance.selectTeam;
        if(currentTeam != null)
            gameData.currentTeamData = ConvertTeamDataParser(currentTeam);

        var defendingChampion = LeagueManager.Instance.defendingChampionData;
        if(defendingChampion != null)
        {
            gameData.leagueData = new LeagueDataParser();

            gameData.leagueData.xIndex = CalendarUI.Instance.xIndex;
            gameData.leagueData.yIndex = CalendarUI.Instance.yIndex;

            int leagueNumber = LeagueManager.Instance.GetCurrentLeagueNumber();
            if (leagueNumber != 0)
                gameData.leagueData.currentLeagueNumber = leagueNumber;
            else
                gameData.leagueData.currentLeagueNumber = 1;

            gameData.leagueData.defeindingChampionTeamData = ConvertTeamDataParser(defendingChampion);
            gameData.leagueData.league_1TeamDatas = ConvertTeamDataParserList(LeagueManager.Instance.league1TeamDatas);
            gameData.leagueData.league_2TeamDatas = ConvertTeamDataParserList(LeagueManager.Instance.league2TeamDatas);
            gameData.leagueData.league_3TeamDatas = ConvertTeamDataParserList(LeagueManager.Instance.league3TeamDatas);
            gameData.leagueData.league_0TeamDatas = ConvertTeamDataParserList(LeagueManager.Instance.league0TeamDatas);
            gameData.leagueData.league_1 = ConvertTeamDataParserDic(LeagueManager.Instance.league_1);
            gameData.leagueData.league_2 = ConvertTeamDataParserDic(LeagueManager.Instance.league_2);
            gameData.leagueData.league_3 = ConvertTeamDataParserDic(LeagueManager.Instance.league_3);
            gameData.leagueData.league_0 = ConvertTeamDataParserDic(LeagueManager.Instance.league_0);
            gameData.leagueData.battleRecode = LeagueManager.Instance.battleReports;
        }

        gameData.monsterBoxInfos = ConvertBoxInfoParsers(DialougeUI.Instance.monsterBoxInfos);
        gameData.itemBoxInfos = ConvertBoxInfoParsers(DialougeUI.Instance.itemBoxInfos);

        gameData.storyDataParser = new StoryDataParser();
        gameData.storyDataParser.level_1 = ConvertStageSelectParser(StoryUI.Instance.level_1);
        gameData.storyDataParser.level_2 = ConvertStageSelectParser(StoryUI.Instance.level_2);
        gameData.storyDataParser.level_3 = ConvertStageSelectParser(StoryUI.Instance.level_3);
        gameData.storyDataParser.level_4 = ConvertStageSelectParser(StoryUI.Instance.level_4);
        gameData.storyDataParser.level_5 = ConvertStageSelectParser(StoryUI.Instance.level_5);
        gameData.storyDataParser.level_6 = ConvertStageSelectParser(StoryUI.Instance.level_6);
        gameData.storyDataParser.level_1_hidden = ConvertStageSelectParser(StoryUI.Instance.level_1_hidden);
        gameData.storyDataParser.level_1_hidden_2 = ConvertStageSelectParser(StoryUI.Instance.level_1_hidden_2);
        gameData.storyDataParser.level_4_hidden = ConvertStageSelectParser(StoryUI.Instance.level_4_hidden);
        gameData.storyDataParser.level_4_hidden_2 = ConvertStageSelectParser(StoryUI.Instance.level_4_hidden_2);
        gameData.storyDataParser.level_6_hidden = ConvertStageSelectParser(StoryUI.Instance.level_6_hidden);
        gameData.storyDataParser.level_6_hidden2 = ConvertStageSelectParser(StoryUI.Instance.level_6_hidden2);
        gameData.storyDataParser.level_6_hidden2_1 = ConvertStageSelectParser(StoryUI.Instance.level_6_hidden2_1);
        gameData.storyDataParser.special_1 = ConvertStageSelectParser(StoryUI.Instance.special_1);
        gameData.storyDataParser.special_2 = ConvertStageSelectParser(StoryUI.Instance.special_2);
        gameData.storyDataParser.special_3 = ConvertStageSelectParser(StoryUI.Instance.special_3);


        gameData.consumeItemDatas = new string[ItemInventory.Instance.consumeItemDatas.Length];
        for(int i = 0; i < gameData.consumeItemDatas.Length; ++i)
        {
            if (ItemInventory.Instance.consumeItemDatas[i] != null)
                gameData.consumeItemDatas[i] = ItemInventory.Instance.consumeItemDatas[i].name;
            else
                gameData.consumeItemDatas[i] = "";
        }
        gameData.treagureDatas = new string[ItemInventory.Instance.treasureDatas.Length];
        for (int i = 0; i < gameData.treagureDatas.Length; ++i)
        {
            if (ItemInventory.Instance.treasureDatas[i] != null)
                gameData.treagureDatas[i] = ItemInventory.Instance.treasureDatas[i].name;
            else
                gameData.treagureDatas[i] = "";
        }

        if(ItemInventory.Instance.rulletItemDatas != null)
        {
            var rulletIndigridentDatas = ItemInventory.Instance.rulletItemDatas.FindAll(x => x is CostItemIndigrident);
            gameData.rulletCostItemDatas = new List<CostItemIndigrident>();
            for(int i = 0; i < rulletIndigridentDatas.Count; ++i)
            {
                gameData.rulletCostItemDatas.Add(rulletIndigridentDatas[i] as CostItemIndigrident);
            }

            var rulletMonsterDatas = ItemInventory.Instance.rulletItemDatas.FindAll(x => x is MonsterData);
            gameData.rulletMonsterDatas = new List<string>();
            for (int i = 0; i < rulletMonsterDatas.Count; ++i)
            {
                var data = rulletMonsterDatas[i] as MonsterData;
                gameData.rulletMonsterDatas.Add(data.name);
            }

            var rulletItemDatas = ItemInventory.Instance.rulletItemDatas.FindAll(x => x is ItemElementalData);
            gameData.rulletItemDatas = new List<string>();
            for (int i = 0; i < rulletItemDatas.Count; ++i)
            {
                var data = rulletItemDatas[i] as ItemElementalData;
                gameData.rulletItemDatas.Add(data.name);
            }
        }

        var monsterInstances = (TranningUI.Instance.playerInventory == null) ? null : TranningUI.Instance.playerInventory.monsterDatas;
        gameData.monsterInstances = ConvertMonsterInstanceParser(monsterInstances);

        gameData.rewardDatas = RewardUI.Instance.rewardQueue;

        var rulletObjects = RulletUI.Instance.rulletObjs;

        gameData.rulletDatas = new RulletDataParser[] { ConvertRulletData(rulletObjects[0].rewardData, rulletObjects[0].isDevil, rulletObjects[0].isBlock),
            ConvertRulletData(rulletObjects[1].rewardData, rulletObjects[1].isDevil, rulletObjects[1].isBlock),
            ConvertRulletData(rulletObjects[2].rewardData, rulletObjects[2].isDevil, rulletObjects[2].isBlock),
            ConvertRulletData(rulletObjects[3].rewardData, rulletObjects[3].isDevil, rulletObjects[3].isBlock) };

        DataManager.Instance.Save(path, "_0", gameData);
    }

    private bool DeconvertData(GameData gameData)
    {
       

        MonsterInstance.hashKey = gameData.monsterHashKey;

        //따로 호출하는 곳이있음
        ItemInventory.Instance.money = gameData.money;
        ItemInventory.Instance.diamond = gameData.diamond;
        ItemInventory.Instance.fame = gameData.fame;
        ItemInventory.Instance.saveMoney = gameData.saveMoney;
        ItemInventory.Instance.friendShipBattleCount = gameData.friendShipBattleCount;
        ItemInventory.Instance.friendShipBattleWin = gameData.friendShipBattleWin;

        var consumDatas = ItemInventory.Instance.consumeItemDatas;
        for (int i = 0; i < gameData.consumeItemDatas.Length; ++i)
        {
            if (gameData.consumeItemDatas[i] != "")
            {
                ItemElementalData itemData = Resources.Load<ItemElementalData>($"ItemElementalData/{gameData.consumeItemDatas[i]}");
                if (itemData == null)
                    return false;

                consumDatas[i] = itemData;
            }
        }

        var treasureDatas = ItemInventory.Instance.treasureDatas;
        for (int i = 0; i < gameData.treagureDatas.Length; ++i)
        {
            if (gameData.treagureDatas[i] != "")
            {
                ItemElementalData itemData = Resources.Load<ItemElementalData>($"ItemElementalData/{gameData.treagureDatas[i]}");
                if (itemData == null)
                    return false;

                treasureDatas[i] = itemData;
            }
        }

        if (gameData.rulletCostItemDatas != null)
        {
            for (int i = 0; i < gameData.rulletCostItemDatas.Count; ++i)
            {
                RewardUI.Instance.AddCostItem(gameData.rulletCostItemDatas[i].itemType, gameData.rulletCostItemDatas[i].count);
            }
        }

        if (gameData.rulletMonsterDatas != null)
        {
            for (int i = 0; i < gameData.rulletMonsterDatas.Count; ++i)
            {
                var monsterData = Resources.Load<MonsterData>($"MonsterData/{gameData.rulletMonsterDatas[i]}");
                if (monsterData == null)
                    return false;
                MonsterInstance instance = MonsterInstance.Instance(monsterData);
                RewardUI.Instance.AddMonster(instance);
            }
        }

        if (gameData.rulletItemDatas != null)
        {
            for (int i = 0; i < gameData.rulletItemDatas.Count; ++i)
            {
                var itemData = Resources.Load<ItemElementalData>($"ItemElementalData/{gameData.rulletItemDatas[i]}");
                if (itemData == null)
                    return false;
                RewardUI.Instance.AddConsumeItem(itemData, 1);
            }
        }

        LeagueManager.Instance.timecount = gameData.timeCount;
        LeagueManager.Instance.currentTimeCount = gameData.currrentTimeCount;
        LeagueManager.Instance.blockGamebling = gameData.blockGamebling;
        LeagueManager.Instance.blockResetItemForAds = gameData.blockResetItemForAds;
        LeagueManager.Instance.blockResetMonsterForAds = gameData.blockResetMonsterForAds;
        LeagueManager.Instance.diamondToGold = gameData.diamondToGold;
        LeagueManager.Instance.friendshipLevel = gameData.friendShipLevel;

        //따로 호출하는 곳이 있음
        SystemUI.Instance.startBgmValue = gameData.startBgmValue;
        SystemUI.Instance.startEffectValue = gameData.startEffectValue;
        SystemUI.Instance.startBattleSpeedValue = gameData.startBattleSpeedValue;
        SystemUI.Instance.bgmBar.value = gameData.bgmValue;
        SystemUI.Instance.effectBar.value = gameData.effectValue;
        SystemUI.Instance.battleSpeedBar.value = gameData.battleSpeedValue;
        SystemUI.Instance.addBattleSpeed = gameData.addBattleSpeed;

        TextManager.Instance.language = gameData.language;
      
        MainSystemUI.Instance.SetTextFormats();
        SystemUI.Instance.SetTextFormats();
      

        //따로 호출하는 곳이 있음
        SelectTeamUI.Instance.selectTeam = DeconvertTeamData(gameData.currentTeamData);
        if (SelectTeamUI.Instance.selectTeam == null)
            return false;

        if (gameData.leagueData != null)
        {
            LeagueManager.Instance.defendingChampionData = DeconvertTeamData(gameData.leagueData.defeindingChampionTeamData);
            if (LeagueManager.Instance.defendingChampionData == null)
                return false;

            LeagueManager.Instance.league1TeamDatas = DeconvertTeamDataList(gameData.leagueData.league_1TeamDatas);
            if (LeagueManager.Instance.league1TeamDatas == null)
                return false;

            LeagueManager.Instance.league2TeamDatas = DeconvertTeamDataList(gameData.leagueData.league_2TeamDatas);
            if (LeagueManager.Instance.league2TeamDatas == null)
                return false;

            LeagueManager.Instance.league3TeamDatas = DeconvertTeamDataList(gameData.leagueData.league_3TeamDatas);
            if (LeagueManager.Instance.league3TeamDatas == null)
                return false;

            if (gameData.leagueData.league_0TeamDatas != null)
                LeagueManager.Instance.league0TeamDatas = DeconvertTeamDataList(gameData.leagueData.league_0TeamDatas);


            LeagueManager.Instance.league_1 = DeConvertTeamDataDic(gameData.leagueData.league_1);
            if (LeagueManager.Instance.league_1 == null)
                return false;

            LeagueManager.Instance.league_2 = DeConvertTeamDataDic(gameData.leagueData.league_2);
            if (LeagueManager.Instance.league_2 == null)
                return false;

            LeagueManager.Instance.league_3 = DeConvertTeamDataDic(gameData.leagueData.league_3);
            if (LeagueManager.Instance.league_3 == null)
                return false;

            if (gameData.leagueData.league_0 != null)
                LeagueManager.Instance.league_0 = DeConvertTeamDataDic(gameData.leagueData.league_0);

            LeagueManager.Instance.currentLeagTeam = LeagueManager.Instance.GetLeagueTeams(gameData.leagueData.currentLeagueNumber);
            LeagueManager.Instance.currentLeague = LeagueManager.Instance.GetLeague(gameData.leagueData.currentLeagueNumber);
            LeagueManager.Instance.SetFinalMatch();

            LeagueManager.Instance.battleReports = gameData.leagueData.battleRecode;

            CalendarUI.Instance.xIndex = gameData.leagueData.xIndex;
            CalendarUI.Instance.yIndex = gameData.leagueData.yIndex;
        }
        else
            return false;

        bool check = DeConvertBoxInfos(gameData.itemBoxInfos, DialougeUI.Instance.itemBoxInfos);
        if (check == false)
            return false;

        check = DeConvertBoxInfos(gameData.monsterBoxInfos, DialougeUI.Instance.monsterBoxInfos);
        if (check == false)
            return false;

        //따로 호출하는 곳이 있음
        DeConvertStageSelectSlot(gameData.storyDataParser.level_1, StoryUI.Instance.level_1);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_2, StoryUI.Instance.level_2);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_3, StoryUI.Instance.level_3);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_4, StoryUI.Instance.level_4);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_5, StoryUI.Instance.level_5);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_6, StoryUI.Instance.level_6);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_1_hidden, StoryUI.Instance.level_1_hidden);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_1_hidden_2, StoryUI.Instance.level_1_hidden_2);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_4_hidden, StoryUI.Instance.level_4_hidden);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_4_hidden_2, StoryUI.Instance.level_4_hidden_2);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_6_hidden, StoryUI.Instance.level_6_hidden);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_6_hidden2, StoryUI.Instance.level_6_hidden2);
        DeConvertStageSelectSlot(gameData.storyDataParser.level_6_hidden2_1, StoryUI.Instance.level_6_hidden2_1);
        DeConvertStageSelectSlot(gameData.storyDataParser.special_1, StoryUI.Instance.special_1);
        DeConvertStageSelectSlot(gameData.storyDataParser.special_2, StoryUI.Instance.special_2);
        DeConvertStageSelectSlot(gameData.storyDataParser.special_3, StoryUI.Instance.special_3);

        if (gameData.rulletDatas != null)
        {
            var rulletObjs = RulletUI.Instance.rulletObjs;
            for (int i = 0; i < gameData.rulletDatas.Length; ++i)
            {
                bool isDevil, isBlock;
                var obj = DeconvertRulletData(gameData.rulletDatas[i], out isDevil, out isBlock);
                rulletObjs[i].rewardData = obj;
                rulletObjs[i].isDevil = isDevil;
                rulletObjs[i].isBlock = isBlock;

                rulletObjs[i].Init();
                rulletObjs[i].ReClear();
            }
        }

        var monsterInstances = DeConvertMonsterInstances(gameData.monsterInstances);
        if (monsterInstances == null)
            return false;
        TranningUI.Instance.playerInventory.monsterDatas = monsterInstances;

        RewardUI.Instance.rewardQueue.AddRange(gameData.rewardDatas);
        return true;

    }

    

    

    private TeamDataParser ConvertTeamDataParser(TeamData teamData)
    {
        if(teamData != null)
        {
            var teamDataParser = new TeamDataParser();
            teamDataParser.teamName = teamData.name;
            teamDataParser.win = teamData.win;
            teamDataParser.draw = teamData.draw;
            teamDataParser.lose = teamData.lose;
            teamDataParser.score = teamData.score;
            return teamDataParser;
        }
        else
        {
            return null;
        }
    }

    private TeamData DeconvertTeamData(TeamDataParser teamDataParser)
    {
        if (teamDataParser != null)
        {
            TeamData teamData = Resources.Load<TeamData>($"TeamData/{teamDataParser.teamName}");
            if (teamData == null)
                throw new NotImplementedException();
            else
            {
                teamData.win = teamDataParser.win;
                teamData.draw = teamDataParser.draw;
                teamData.lose = teamDataParser.lose;
                teamData.score = teamDataParser.score;
                return teamData;
            }
        }
        else
            return null;
    }

    private List<TeamDataParser> ConvertTeamDataParserList(List<TeamData> teamDataList)
    {
        if (teamDataList != null && teamDataList.Count > 0)
        {
            List<TeamDataParser> parserList = new List<TeamDataParser>();

            for(int i = 0; i < teamDataList.Count; ++i)
            {
                var parser = ConvertTeamDataParser(teamDataList[i]);
                parserList.Add(parser);
            }

            return parserList;
        }
        else
            return null;
    }

    private List<TeamData> DeconvertTeamDataList(List<TeamDataParser> teamDataParserList)
    {
        if (teamDataParserList != null && teamDataParserList.Count > 0)
        {
            List<TeamData> teamDataList = new List<TeamData>();
            for (int i = 0; i < teamDataParserList.Count; ++i)
            {
                var teamData = DeconvertTeamData(teamDataParserList[i]);
                teamDataList.Add(teamData);
            }

            return teamDataList;
        }
        else
            return null;
    }

    private RulletDataParser ConvertRulletData(object rulletData, bool isDevil, bool isDead)
    {
        RulletDataParser parser = new RulletDataParser();

        if(isDevil == true || isDead == true)
        {
            parser.isDevil = isDevil;
            parser.isBlock = isDead;
            return parser;
        }
        else
        {
            if (rulletData is CostItemIndigrident)
            {
                var data = rulletData as CostItemIndigrident;

                parser.costType = data.itemType;
                parser.count = data.count;
                parser.objectType = 0;

                return parser;
            }
            else if (rulletData is ItemElementalData)
            {
                var data = rulletData as ItemElementalData;

                parser.objectName = data.name;
                parser.objectType = 1;

                return parser;
            }
            else if (rulletData is MonsterData)
            {
                var data = rulletData as MonsterData;
                parser.objectName = data.name;
                parser.objectType = 2;

                return parser;
            }
            else
                return null;
        }
    }
    private object DeconvertRulletData(RulletDataParser parser, out bool isDevil, out bool isDead)
    {
        isDevil = isDead = false;

        if (parser == null)
            return null;
        else
        {
            if(parser.isBlock == true || parser.isDevil == true)
            {
                isDead = parser.isBlock;
                isDevil = parser.isDevil;
                return null;
            }
            else
            {
                if (parser.objectType == 0)
                {
                    CostItemIndigrident data = new CostItemIndigrident();
                    data.count = parser.count;
                    data.itemType = parser.costType;
                    return data;
                }
                else if (parser.objectType == 1)
                {
                    var data = Resources.Load<ItemElementalData>($"ItemElementalData/{parser.objectName}");
                    return data;
                }
                else if (parser.objectType == 2)
                {
                    var data = Resources.Load<MonsterData>($"MonsterData/{parser.objectName}");
                    return data;
                }
                else
                    return null;
            }
        }
    }
    private List<Tuple<TeamDataParser, TeamDataParser>> ConvertTeamDataParserListTuple(List<Tuple<TeamData, TeamData>> teamDataTuples)
    {
        if (teamDataTuples != null && teamDataTuples.Count > 0)
        {
            List<Tuple<TeamDataParser, TeamDataParser>> parserTuples = new List<Tuple<TeamDataParser, TeamDataParser>>();

            for(int i = 0; i < teamDataTuples.Count; ++i)
            {
                var tuple = teamDataTuples[i];
                TeamDataParser a = ConvertTeamDataParser(tuple.Item1);
                TeamDataParser b = ConvertTeamDataParser(tuple.Item2);

                parserTuples.Add(new Tuple<TeamDataParser, TeamDataParser>(a, b));
            }
            return parserTuples;
        }
        else
            return null;
    }

    private List<Tuple<TeamData, TeamData>> DeConvertTeamDataListTuple(List<Tuple<TeamDataParser, TeamDataParser>> teamDataParserTuples)
    {
        if (teamDataParserTuples != null && teamDataParserTuples.Count > 0)
        {
            List<Tuple<TeamData, TeamData>> teamDataTuples = new List<Tuple<TeamData, TeamData>>();
            for (int i = 0; i < teamDataParserTuples.Count; ++i)
            {
                var tuple = teamDataParserTuples[i];
                TeamData a = DeconvertTeamData(tuple.Item1);
                TeamData b = DeconvertTeamData(tuple.Item2);

                teamDataTuples.Add(new Tuple<TeamData, TeamData>(a, b));
            }

            return teamDataTuples;
        }
        else
            return null;
    }

    private Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>> ConvertTeamDataParserDic(Dictionary<int, List<Tuple<TeamData, TeamData>>> teamDataDic)
    {
        if (teamDataDic != null && teamDataDic.Count > 0)
        {
            Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>> parserDic = new Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>>();

            foreach (var teamData in teamDataDic)
            {
                var parserKey = teamData.Key;
                var parserValue = ConvertTeamDataParserListTuple(teamData.Value);

                parserDic.Add(parserKey, parserValue);
            }
            return parserDic;
        }
        else
            return null;
    }

    private Dictionary<int, List<Tuple<TeamData, TeamData>>> DeConvertTeamDataDic(Dictionary<int, List<Tuple<TeamDataParser, TeamDataParser>>> teamDataParserDic)
    {
        if (teamDataParserDic != null && teamDataParserDic.Count > 0)
        {
            Dictionary<int, List<Tuple<TeamData, TeamData>>> parserDic = new Dictionary<int, List<Tuple<TeamData, TeamData>>>();

            foreach (var teamDataParser in teamDataParserDic)
            {
                var key = teamDataParser.Key;
                var value = DeConvertTeamDataListTuple(teamDataParser.Value);

                parserDic.Add(key, value);
            }
            return parserDic;
        }
        else
            return null;
    }

    private BoxInfoParser ConvertBoxInfoParser(BoxInfoSlot boxInfoSlot)
    {
        BoxInfoParser boxInfoParser = new BoxInfoParser();

        if(boxInfoSlot.monsterShopData != null)
        {
            boxInfoParser.resource = boxInfoSlot.monsterShopData.monster.name;
            boxInfoParser.costType = boxInfoSlot.monsterShopData.costType;
            boxInfoParser.count = boxInfoSlot.monsterShopData.count;
        }
        if (boxInfoSlot.itemShopData != null)
        {
            boxInfoParser.resource = boxInfoSlot.itemShopData.item.name;
            boxInfoParser.costType = boxInfoSlot.itemShopData.costType;
            boxInfoParser.count = boxInfoSlot.itemShopData.count;
        }
      
        boxInfoParser.isSell = boxInfoSlot.isSell;
        boxInfoParser.isHidden = boxInfoSlot.isHidden;

        return boxInfoParser;
    }

    private bool DeConvertBoxInfo(BoxInfoParser parser, BoxInfoSlot boxInfoSlot)
    {
        if (parser != null)
        {
            ItemElementalData itemData = Resources.Load<ItemElementalData>($"ItemElementalData/{parser.resource}");
            if (itemData == null)
            {
                MonsterData monsterData = Resources.Load<MonsterData>($"MonsterData/{parser.resource}");
                if (monsterData == null)
                    throw new NotImplementedException();
                else
                {
                    MonsterShopData shopData = new MonsterShopData();
                    shopData.monster = monsterData;
                    shopData.count = parser.count;
                    shopData.costType = parser.costType;
                    boxInfoSlot.monsterShopData = shopData;
                }
            }
            else
            {
                ItemShopData shopData = new ItemShopData();
                shopData.item = itemData;
                shopData.count = parser.count;
                shopData.costType = parser.costType;
                boxInfoSlot.itemShopData = shopData;
            }

            boxInfoSlot.isSell = parser.isSell;
            boxInfoSlot.isHidden = parser.isHidden;

            boxInfoSlot.SetUp();
            return true;
        }
        else
            return false;
     
    }

    private BoxInfoParser[] ConvertBoxInfoParsers(BoxInfoSlot[] boxInfos)
    {
        var boxInfoParsers = new BoxInfoParser[boxInfos.Length];

        for(int i = 0; i < boxInfos.Length; i++)
        {
            boxInfoParsers[i] = ConvertBoxInfoParser(boxInfos[i]);
        }

        return boxInfoParsers;
    }

    private bool DeConvertBoxInfos(BoxInfoParser[] boxInfoParsers , BoxInfoSlot[] boxInfos)
    {
        if (boxInfoParsers != null && boxInfoParsers.Length > 0)
        {
            for (int i = 0; i < boxInfoParsers.Length; i++)
            {
                DeConvertBoxInfo(boxInfoParsers[i], boxInfos[i]);
            }

            return true;
        }
        else
            return false;
    }

    private StageSelectParser ConvertStageSelectParser(StageSelectSlot stageSelectSlot)
    {
        StageSelectParser stageSelectParser = new StageSelectParser();
        stageSelectParser.isCleared = stageSelectSlot.isCleard;
        stageSelectParser.isTransition = stageSelectSlot.isTransition;
        stageSelectParser.isNext = stageSelectSlot.isNext;
        stageSelectParser.storyTransitionType = stageSelectSlot.transitionType;
        stageSelectParser.clearBlock = stageSelectSlot.clearBlock;

        return stageSelectParser;
    }

    private bool DeConvertStageSelectSlot(StageSelectParser parser , StageSelectSlot stageSelectSlot)
    {
        if (parser != null)
        {
            stageSelectSlot.isCleard = parser.isCleared;
            stageSelectSlot.isTransition = parser.isTransition;
            stageSelectSlot.isNext = parser.isNext;
            stageSelectSlot.transitionType = parser.storyTransitionType;
            if (stageSelectSlot.transitionType == StoryTransitionType.Transition || stageSelectSlot.transitionType == StoryTransitionType.HiddenTransition)
                stageSelectSlot.transitionType = StoryTransitionType.Open;
            stageSelectSlot.clearBlock = parser.clearBlock;
            return true;
        }
        else
            return false;
    }

    private MonsterInstanceParser[] ConvertMonsterInstanceParser(MonsterInstance[] monsterInstance)
    {
        if (monsterInstance != null)
        {
            MonsterInstanceParser[] monsterInstanceParsers = new MonsterInstanceParser[monsterInstance.Length];
            for(int i = 0; i < monsterInstanceParsers.Length; i++)
            {
                monsterInstanceParsers[i] = ConvertMonsterInstanceParser(monsterInstance[i]);
            }

            return monsterInstanceParsers;
        }
        else
            return null;
    }
    private MonsterInstance[] DeConvertMonsterInstances(MonsterInstanceParser[] parsers)
    {
        if (parsers != null)
        {
            MonsterInstance[] monsterInstances = new MonsterInstance[parsers.Length];
            for (int i = 0; i < monsterInstances.Length; i++)
            {
                monsterInstances[i] = DeConvertMonsterInstance(parsers[i]);
            }

            return monsterInstances;
        }
        else
            return null;
    }
    private MonsterInstanceParser ConvertMonsterInstanceParser(MonsterInstance monsterInstance)
    {
        if (monsterInstance != null)
        {
            MonsterData data = monsterInstance.monsterData;
            MonsterInstanceParser parser = new MonsterInstanceParser();
            parser.tranningCicleInstance = ConvertTranningCicleInstanceParser(monsterInstance.tranningCicleInstance, parser);
            parser.previousMonsterInstance = ConvertMonsterInstanceParser(monsterInstance.previousMonsterData);

            parser.maxHp = monsterInstance.maxHp;
            parser.hp = monsterInstance.hp;

            parser.maxMp = monsterInstance.maxMp;
            parser.mp = monsterInstance.mp;

            parser.atk = monsterInstance.atk;
            parser.def = monsterInstance.def;

            parser.maxDex = monsterInstance.maxDex;

            parser.manaRecoveryRatio = monsterInstance.manaRecoveryRatio;
            parser.hpRecoveryRatio = monsterInstance.hpRecoveryRatio;

            parser.dodge = monsterInstance.repeatRatio;
            parser.critial = monsterInstance.creaticalRatio;

            parser.battleAIType = monsterInstance.battleAIType;
            parser.monsterWeight = monsterInstance.monsterWeight;
            parser.currentConfirmSkillPriority = monsterInstance.currentConfirmSkillPriority;
            parser.currentSelectDetailTargetType = monsterInstance.currentSelectDetailTargetType;
            parser.healthState = monsterInstance.heathState;

            parser.monsterData = monsterInstance.monsterData.name;

            parser.skillDatas = new List<string>();
            for(int i = 0; i < monsterInstance.skillDatas.Count; ++i)
            {
                parser.skillDatas.Add(monsterInstance.skillDatas[i].name);
            }
            parser.percentSkillDatas = new List<Tuple<int, string>>();
            for(int i = 0; i < monsterInstance.percentSkillDatas.Count; ++i)
            {
                parser.percentSkillDatas.Add(new Tuple<int, string>(monsterInstance.percentSkillDatas[i].Item1, (monsterInstance.percentSkillDatas[i].Item2.name)));
            }
            parser.triggerSkillDatas = new List<Tuple<SkillTrigger, string>>();
            for (int i = 0; i < monsterInstance.triggerSkillDatas.Count; ++i)
            {
                parser.triggerSkillDatas.Add(new Tuple<SkillTrigger, string>(monsterInstance.triggerSkillDatas[i].Item1, (monsterInstance.triggerSkillDatas[i].Item2.name)));
            }

            parser.abilityTypes = new List<AbilityType>();
            for (int i = 0; i < monsterInstance.abilities.Count; ++i)
            {
                parser.abilityTypes.Add(monsterInstance.abilities[i]);
            }
            parser.confirmSkillPriorities = new List<ConfirmSkillPriority>();
            for (int i = 0; i < monsterInstance.confirmSkillPrioritys.Count; ++i)
            {
                parser.confirmSkillPriorities.Add(monsterInstance.confirmSkillPrioritys[i]);
            }
            parser.selectDetailTargetTypes = new List<SelectDetailTargetType>();
            for (int i = 0; i < monsterInstance.selectDetailTargetTypes.Count; ++i)
            {
                parser.selectDetailTargetTypes.Add(monsterInstance.selectDetailTargetTypes[i]);
            }

            parser.possibleSkillDatas = new List<string>();
            for (int i = 0; i < monsterInstance.possibleSkillDatas.Count; ++i)
            {
                parser.possibleSkillDatas.Add(monsterInstance.possibleSkillDatas[i].name);
            }

            parser.possiblePercentSkillDatas = new List<string>();
            for (int i = 0; i < monsterInstance.possiblePercentSkillDatas.Count; ++i)
            {
                parser.possiblePercentSkillDatas.Add(monsterInstance.possiblePercentSkillDatas[i].name);
            }

            parser.possibleTriggerSkillDatas = new List<string>();
            for (int i = 0; i < monsterInstance.possibleTriggerSkillDatas.Count; ++i)
            {
                parser.possibleTriggerSkillDatas.Add(monsterInstance.possibleTriggerSkillDatas[i].name);
            }

            parser.possibleAbilityTypes = new List<AbilityType>();
            for (int i = 0; i < monsterInstance.possibleAbilitieDatas.Count; ++i)
            {
                parser.possibleAbilityTypes.Add(monsterInstance.possibleAbilitieDatas[i]);
            }

            parser.possibleConfirmSkillPrioritys = new List<ConfirmSkillPriority>();
            for (int i = 0; i < monsterInstance.possibleConfirmSkillPriorityDatas.Count; ++i)
            {
                parser.possibleConfirmSkillPrioritys.Add(monsterInstance.possibleConfirmSkillPriorityDatas[i]);
            }

            parser.possibleSelectDetailTargetTypes = new List<SelectDetailTargetType>();
            for (int i = 0; i < monsterInstance.possibleSelectDetailTargetTypeDatas.Count; ++i)
            {
                parser.possibleSelectDetailTargetTypes.Add(monsterInstance.possibleSelectDetailTargetTypeDatas[i]);
            }

            if (monsterInstance.deadTrannings != null)
            {
                parser.deadTranningTypes = new List<DeadTranningType>();
                for (int i = 0; i < monsterInstance.deadTrannings.Count; ++i)
                {
                    parser.deadTranningTypes.Add(monsterInstance.deadTrannings[i]);
                }
            }

            if (monsterInstance.battleTrannings != null)
            {
                parser.battleTranningTypes = new BattleTranningType[monsterInstance.battleTrannings.Length];
                for (int i = 0; i < monsterInstance.battleTrannings.Length; ++i)
                {
                    parser.battleTranningTypes[i] = monsterInstance.battleTrannings[i];
                }
            }
          
            parser.originalPriority = monsterInstance.originalPriority;
            parser.battleRecode = monsterInstance.battleRecode;

            return parser;
        }
        else
            return null;
    }
    private MonsterInstance DeConvertMonsterInstance(MonsterInstanceParser parser)
    {
        if(parser != null)
        {
            MonsterInstance instance = new MonsterInstance();
            instance.tranningCicleInstance = DeConvertTranningCicleInstance(parser.tranningCicleInstance, instance);
            instance.previousMonsterData = DeConvertMonsterInstance(parser.previousMonsterInstance);

            instance.maxHp = parser.maxHp;
            instance.hp = parser.hp;
            if (parser.healthState != MonsterHeathState.Faint && instance.hp <= 0)
                instance.hp = 1;

            instance.maxMp = parser.maxMp;
            instance.mp = parser.mp;

            instance.atk = parser.atk;
            instance.def = parser.def;

            instance.maxDex = parser.maxDex;
            instance.dex = 0f;

            instance.manaRecoveryRatio = parser.manaRecoveryRatio;
            instance.hpRecoveryRatio = parser.hpRecoveryRatio;

            instance.repeatRatio = parser.dodge;
            instance.creaticalRatio = parser.critial;

            instance.battleAIType = parser.battleAIType;
            instance.monsterWeight = parser.monsterWeight;
            instance.currentConfirmSkillPriority = parser.currentConfirmSkillPriority;
            instance.currentSelectDetailTargetType = parser.currentSelectDetailTargetType;
            instance.heathState = parser.healthState;

            instance.monsterData = Resources.Load<MonsterData>($"MonsterData/{parser.monsterData}");
            if (instance.monsterData == null)
                throw new NotImplementedException();

            instance.skillDatas = new List<SkillData>();
            for (int i = 0; i < parser.skillDatas.Count; ++i)
            {
                SkillData skillData = Resources.Load<SkillData>($"SkillData/{parser.skillDatas[i]}");
                if(skillData == null)
                    throw new NotImplementedException();

                instance.skillDatas.Add(skillData);
            }

            instance.percentSkillDatas = new List<Tuple<int, SkillData>>();
            for (int i = 0; i < parser.percentSkillDatas.Count; ++i)
            {
                SkillData skillData = Resources.Load<SkillData>($"SkillData/{parser.percentSkillDatas[i].Item2}");
                if (skillData == null)
                    throw new NotImplementedException();

                instance.percentSkillDatas.Add(new Tuple<int, SkillData>(parser.percentSkillDatas[i].Item1, skillData));
            }

            instance.triggerSkillDatas = new List<Tuple<SkillTrigger, SkillData>>();
            for (int i = 0; i < parser.triggerSkillDatas.Count; ++i)
            {
                SkillData skillData = Resources.Load<SkillData>($"SkillData/{parser.triggerSkillDatas[i].Item2}");
                if (skillData == null)
                    throw new NotImplementedException();

                instance.triggerSkillDatas.Add(new Tuple<SkillTrigger, SkillData>(parser.triggerSkillDatas[i].Item1, skillData));
            }

            instance.abilities = new List<AbilityType>();
            for(int i = 0; i < parser.abilityTypes.Count; ++i)
            {
                instance.abilities.Add(parser.abilityTypes[i]);
            }

            instance.confirmSkillPrioritys = new List<ConfirmSkillPriority>();
            for (int i = 0; i < parser.confirmSkillPriorities.Count; ++i)
            {
                instance.confirmSkillPrioritys.Add(parser.confirmSkillPriorities[i]);
            }

            instance.selectDetailTargetTypes = new List<SelectDetailTargetType>();
            for (int i = 0; i < parser.selectDetailTargetTypes.Count; ++i)
            {
                instance.selectDetailTargetTypes.Add(parser.selectDetailTargetTypes[i]);
            }

            instance.possibleSkillDatas = new List<SkillData>();
            for (int i = 0; i < parser.possibleSkillDatas.Count; ++i)
            {
                SkillData skillData = Resources.Load<SkillData>($"SkillData/{parser.possibleSkillDatas[i]}");
                if (skillData == null)
                    throw new NotImplementedException();

                instance.possibleSkillDatas.Add(skillData);
            }

            instance.possiblePercentSkillDatas = new List<PercentSkillData>();
            for (int i = 0; i < parser.possiblePercentSkillDatas.Count; ++i)
            {
                PercentSkillData skillData = Resources.Load<PercentSkillData>($"SkillData/{parser.possiblePercentSkillDatas[i]}");
                if (skillData == null)
                    throw new NotImplementedException();
                instance.possiblePercentSkillDatas.Add(skillData);
            }

            instance.possibleTriggerSkillDatas = new List<TriggerSkillData>();
            for (int i = 0; i < parser.possibleTriggerSkillDatas.Count; ++i)
            {
                TriggerSkillData skillData = Resources.Load<TriggerSkillData>($"SkillData/{parser.possibleTriggerSkillDatas[i]}");
                if (skillData == null)
                    throw new NotImplementedException();
                instance.possibleTriggerSkillDatas.Add(skillData);
            }

            instance.possibleAbilitieDatas = new List<AbilityType>();
            for (int i = 0; i < parser.possibleAbilityTypes.Count; ++i)
            {
                instance.possibleAbilitieDatas.Add(parser.possibleAbilityTypes[i]);
            }

            instance.possibleConfirmSkillPriorityDatas = new List<ConfirmSkillPriority>();
            for (int i = 0; i < parser.possibleConfirmSkillPrioritys.Count; ++i)
            {
                instance.possibleConfirmSkillPriorityDatas.Add(parser.possibleConfirmSkillPrioritys[i]);
            }

            instance.possibleSelectDetailTargetTypeDatas = new List<SelectDetailTargetType>();
            for (int i = 0; i < parser.possibleSelectDetailTargetTypes.Count; ++i)
            {
                instance.possibleSelectDetailTargetTypeDatas.Add(parser.possibleSelectDetailTargetTypes[i]);
            }

            if (parser.deadTranningTypes != null)
            {
                instance.deadTrannings = new List<DeadTranningType>();
                for (int i = 0; i < parser.deadTranningTypes.Count; ++i)
                {
                    instance.deadTrannings.Add(parser.deadTranningTypes[i]);
                }
            }

            if (parser.battleTranningTypes != null)
            {
                instance.battleTrannings = new BattleTranningType[parser.battleTranningTypes.Length];
                for (int i = 0; i < parser.battleTranningTypes.Length; ++i)
                {
                    instance.battleTrannings[i] = parser.battleTranningTypes[i];
                }
            }

            instance.originalPriority = parser.originalPriority;
            instance.battleRecode = parser.battleRecode;
            return instance;
        }
        else
        {
            return null;
        }
    }
    private TranningCicleInstanceParser ConvertTranningCicleInstanceParser(TranningCicleInstance tranningCicleInstance, MonsterInstanceParser monsterInstanceParser)
    {
        TranningCicleInstanceParser parser = new TranningCicleInstanceParser();
        parser.monsterInstance = monsterInstanceParser;
        parser.tranningData = tranningCicleInstance.tranningData.name;

        parser.hpGoalPercent = tranningCicleInstance.hpGoalPercent;
        parser.hpLevel = tranningCicleInstance.hpLevel;
        parser.hpCost = tranningCicleInstance.hpCost;

        parser.mpGoalPercent = tranningCicleInstance.mpGoalPercent;
        parser.mpLevel = tranningCicleInstance.mpLevel;
        parser.mpCost = tranningCicleInstance.mpCost;

        parser.atkGoalPercent = tranningCicleInstance.atkGoalPercent;
        parser.atkLevel = tranningCicleInstance.atkLevel;
        parser.atkCost = tranningCicleInstance.atkCost;

        parser.defGoalPercent = tranningCicleInstance.defGoalPercent;
        parser.defLevel = tranningCicleInstance.defLevel;
        parser.defCost = tranningCicleInstance.defCost;

        parser.dexGoalPercent = tranningCicleInstance.dexGoalPercent;
        parser.dexLevel = tranningCicleInstance.dexLevel;
        parser.dexCost = tranningCicleInstance.dexCost;

        parser.hpRecoveryGoalPercent = tranningCicleInstance.hpRecoveryGoalPercent;
        parser.hpRecoveryLevel = tranningCicleInstance.hpRecoveryLevel;
        parser.hpRecoveryCost = tranningCicleInstance.hpRecoveryCost;

        parser.mpRecoveryGoalPercent = tranningCicleInstance.mpRecoveryGoalPercent;
        parser.mpRecoveryLevel = tranningCicleInstance.mpRecoveryLevel;
        parser.mpRecoveryCost = tranningCicleInstance.mpRecoveryCost;

        parser.dodgeGoalPercent = tranningCicleInstance.defGoalPercent;
        parser.dodgeLevel = tranningCicleInstance.dodgeLevel;
        parser.dodgeCost = tranningCicleInstance.dodgeCost;

        parser.criticalGoalPercent = tranningCicleInstance.criticalGoalPercent;
        parser.criticalLevel = tranningCicleInstance.criticalLevel;
        parser.criticalCost = tranningCicleInstance.criticalCost;

        parser.skillLevel = tranningCicleInstance.skillLevel;
        parser.skillMaxLevel = tranningCicleInstance.skillMaxLevel;
        parser.skillPercent = tranningCicleInstance.skillPercent;
        parser.skillAddPercent = tranningCicleInstance.skillAddPercent;
        parser.skillCost = tranningCicleInstance.skillCost;

        parser.abilityLevel = tranningCicleInstance.abilityLevel;
        parser.abilityMaxLevel = tranningCicleInstance.abilityMaxLevel;
        parser.abilityPercent = tranningCicleInstance.abilityPercent;
        parser.abilityAddPercent = tranningCicleInstance.abilityAddPercent;
        parser.abilityCost = tranningCicleInstance.abilityCost;

        parser.iqLevel = tranningCicleInstance.iqLevel;
        parser.iqMaxLevel = tranningCicleInstance.iqMaxLevel;
        parser.iqPercent = tranningCicleInstance.iqPercent;
        parser.iqCost = tranningCicleInstance.iqCost;

        return parser;
    }
    private TranningCicleInstance DeConvertTranningCicleInstance(TranningCicleInstanceParser tranningCicleInstanceParser, MonsterInstance monsterInstance)
    {
        TranningCicleInstance tranningCicleInstance = new TranningCicleInstance();
        tranningCicleInstance.monsterInstance = monsterInstance;
        tranningCicleInstance.tranningData = Resources.Load<TranningCicleData>($"TranningData/{tranningCicleInstanceParser.tranningData}");
        if (tranningCicleInstance.tranningData == null)
            throw new NotImplementedException();

        tranningCicleInstance.hpGoalPercent = tranningCicleInstanceParser.hpGoalPercent;
        tranningCicleInstance.hpLevel = tranningCicleInstanceParser.hpLevel;
        tranningCicleInstance.hpCost = tranningCicleInstanceParser.hpCost;

        tranningCicleInstance.mpGoalPercent = tranningCicleInstanceParser.mpGoalPercent;
        tranningCicleInstance.mpLevel = tranningCicleInstanceParser.mpLevel;
        tranningCicleInstance.mpCost = tranningCicleInstanceParser.mpCost;

        tranningCicleInstance.atkGoalPercent = tranningCicleInstanceParser.atkGoalPercent;
        tranningCicleInstance.atkLevel = tranningCicleInstanceParser.atkLevel;
        tranningCicleInstance.atkCost = tranningCicleInstanceParser.atkCost;

        tranningCicleInstance.defGoalPercent = tranningCicleInstanceParser.defGoalPercent;
        tranningCicleInstance.defLevel = tranningCicleInstanceParser.defLevel;
        tranningCicleInstance.defCost = tranningCicleInstanceParser.defCost;

        tranningCicleInstance.dexGoalPercent = tranningCicleInstanceParser.dexGoalPercent;
        tranningCicleInstance.dexLevel = tranningCicleInstanceParser.dexLevel;
        tranningCicleInstance.dexCost = tranningCicleInstanceParser.dexCost;

        tranningCicleInstance.hpRecoveryGoalPercent = tranningCicleInstanceParser.hpRecoveryGoalPercent;
        tranningCicleInstance.hpRecoveryLevel = tranningCicleInstanceParser.hpRecoveryLevel;
        tranningCicleInstance.hpRecoveryCost = tranningCicleInstanceParser.hpRecoveryCost;

        tranningCicleInstance.mpRecoveryGoalPercent = tranningCicleInstanceParser.mpRecoveryGoalPercent;
        tranningCicleInstance.mpRecoveryLevel = tranningCicleInstanceParser.mpRecoveryLevel;
        tranningCicleInstance.mpRecoveryCost = tranningCicleInstanceParser.mpRecoveryCost;

        tranningCicleInstance.dodgeGoalPercent = tranningCicleInstanceParser.defGoalPercent;
        tranningCicleInstance.dodgeLevel = tranningCicleInstanceParser.dodgeLevel;
        tranningCicleInstance.dodgeCost = tranningCicleInstanceParser.dodgeCost;

        tranningCicleInstance.criticalGoalPercent = tranningCicleInstanceParser.criticalGoalPercent;
        tranningCicleInstance.criticalLevel = tranningCicleInstanceParser.criticalLevel;
        tranningCicleInstance.criticalCost = tranningCicleInstanceParser.criticalCost;

        tranningCicleInstance.skillLevel = tranningCicleInstanceParser.skillLevel;
        tranningCicleInstance.skillMaxLevel = tranningCicleInstanceParser.skillMaxLevel;
        tranningCicleInstance.skillPercent = tranningCicleInstanceParser.skillPercent;
        tranningCicleInstance.skillAddPercent = tranningCicleInstanceParser.skillAddPercent;
        tranningCicleInstance.skillCost = tranningCicleInstanceParser.skillCost;

        tranningCicleInstance.abilityLevel = tranningCicleInstanceParser.abilityLevel;
        tranningCicleInstance.abilityMaxLevel = tranningCicleInstanceParser.abilityMaxLevel;
        tranningCicleInstance.abilityPercent = tranningCicleInstanceParser.abilityPercent;
        tranningCicleInstance.abilityAddPercent = tranningCicleInstanceParser.abilityAddPercent;
        tranningCicleInstance.abilityCost = tranningCicleInstanceParser.abilityCost;

        tranningCicleInstance.iqLevel = tranningCicleInstanceParser.iqLevel;
        tranningCicleInstance.iqMaxLevel = tranningCicleInstanceParser.iqMaxLevel;
        tranningCicleInstance.iqPercent = tranningCicleInstanceParser.iqPercent;
        tranningCicleInstance.iqCost = tranningCicleInstanceParser.iqCost;
        return tranningCicleInstance;
    }


}
