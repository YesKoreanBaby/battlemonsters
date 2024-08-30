using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum VictoryJudgmentType { Attacker, Physical, Tanker, Slerger, Random, Reverse, Many }
public class CombatManager : MonoBehaviour
{
    private static CombatManager instance = null;
    public static CombatManager Instance { get { return instance; } }

    public CombatUI combatUI;
    public FormationBuffUI formationBuffViewPrefab;
    public Vector2Int[] homegroundLocalPosition;
    public Vector2Int[] awayLocalPosition;
    public float spawnDistance = 0.25f;
    public float offsetDistance = 0f;
    public float heightDistance = 0.1f;
    public AI ai;

    public LinkedList<EntityMonster> battleQueue = new LinkedList<EntityMonster>();

    [System.NonSerialized]
    public List<EntityMonster> homegroundMonsters = new List<EntityMonster>();

    [System.NonSerialized]
    public List<EntityMonster> awayMonsters = new List<EntityMonster>();

    [System.NonSerialized]
    public bool isStart = false;

    [System.NonSerialized]
    public int monsterSpawnMaxCount = 3;

    [System.NonSerialized]
    public int currentLevel = 0;

    [System.NonSerialized]
    public int maxLevel = 0;

    [System.NonSerialized]
    public FieldType currentFieldType;

    [System.NonSerialized]
    public BattleType currentBattleType;

    [System.NonSerialized]
    public object battleDataObject;

    [System.NonSerialized]
    public int monsterSpawnCount = 0;

    [System.NonSerialized]
    public float timeScale = 1f;

    [System.NonSerialized]
    public float battleMaxTimer = 31f;

    [System.NonSerialized]
    public float battleTimer = 0f;

    [System.NonSerialized]
    public float rockBlasterTimer = 0f;

    [System.NonSerialized]
    public bool blockStory = false;

    [System.NonSerialized]
    public bool livingDeadAllDeadFlag = false;

    [System.NonSerialized]
    public List<Tuple<MonsterInstance, MonsterInstance>> playerInstances = new List<Tuple<MonsterInstance, MonsterInstance>>();

    [System.NonSerialized]
    public List<EntityMonster> allMonsters = new List<EntityMonster>();

    [System.NonSerialized]
    public Dictionary<float, Vector2> trapPositionDic = new Dictionary<float, Vector2>();

    [System.NonSerialized]
    public Dictionary<float, Wall> trapDic = new Dictionary<float, Wall>();

    [System.NonSerialized]
    public List<EntityMonster> dotDmgObjList = new List<EntityMonster>();

    [System.NonSerialized]
    public List<WorldItem> worldItemList = new List<WorldItem>();

    [System.NonSerialized]
    public List<Tuple<bool, EmptyPositionStructure, MonsterInstance>> deadMonsters = new List<Tuple<bool, EmptyPositionStructure, MonsterInstance>>();

    public Action startEvent;
    public Action<int> endEvent;
    public List<SelectBox> selectBoxies { get; private set; }

    [System.NonSerialized]
    public bool isEnd = false;

    private float dotDmgTime = 0f;
    private float shaodwCrewTime = 0f;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SettingSelectBox();

        trapPositionDic.Add(0, new Vector2(offsetDistance  + (heightDistance * 1), -1 * spawnDistance));
        trapPositionDic.Add(1, new Vector2(offsetDistance + (heightDistance * 2), 0f));
        trapPositionDic.Add(2, new Vector2(offsetDistance + (heightDistance * 3), spawnDistance));
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    ResetGame();
        if (isStart == false)
            return;
        if (isEnd == true)
            return;

        CheckWinAndLose();
        UpdateCombat();
    }

    public void CreateAwayMonsters()
    {
        var currentBattleData = battleDataObject as BattleData;
        if(currentBattleData != null)
        {
            //단일배틀
            var fieldDatas = MonsterDataBase.Instance.fieldTypes;
            foreach (var pair in fieldDatas)
            {
                pair.Value.SetActive(false);
            }
          

            Vector2 offset = new Vector2(offsetDistance, 0f);

            if(currentBattleData.battleType == BattleType.FriendShip)
            {
                MonsterClass monsterClass;
                AITranningLevel aiTranningLevel;
                TranningType tranningType;
                GetFriendShipValues(out monsterClass, out aiTranningLevel, out tranningType);
                var aiMonsters = GetFriendShipAIMonster(currentBattleData, monsterClass);

                foreach (var pair in aiMonsters)
                {
                    Vector2Int localPosition = pair.Key;
                    Vector2 fixedPosition = ((Vector2)localPosition * spawnDistance) + offset;

                    pair.Value.tranningType = tranningType;
                    pair.Value.tranningLevel = aiTranningLevel;
                    bool check = ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.piggyBank);
                    if (check == true)
                    {
                        pair.Value.tranningType = TranningType.Genius;

                    }
                   
                    var instance = pair.Value.Instance();
                    EntityMonster.CreateMonster(instance, fixedPosition, localPosition.x - 1, localPosition.y + 1, false);
                }
            }
            else if(currentBattleData.battleType == BattleType.Gamebling)
            {
                var aiMonsters = currentBattleData.monsterDataDic[Random.Range(0, currentBattleData.monsterDataDic.Count)];

                foreach (var pair in aiMonsters)
                {
                    pair.Value.tranningType = TranningType.Random;
                    pair.Value.tranningLevel = AITranningLevel.Easy;

                    Vector2Int localPosition = pair.Key;
                    Vector2 fixedPosition = ((Vector2)localPosition * spawnDistance) + offset;
                    var instance = pair.Value.Instance();
                    EntityMonster.CreateMonster(instance, fixedPosition, localPosition.x - 1, localPosition.y + 1, false);
                }
            }
            else
            {
                var aiMonsters = currentBattleData.monsterDataDic[Random.Range(0, currentBattleData.monsterDataDic.Count)];

                foreach (var pair in aiMonsters)
                {
                    Vector2Int localPosition = pair.Key;
                    Vector2 fixedPosition = ((Vector2)localPosition * spawnDistance) + offset;

                    var instance = pair.Value.Instance();
                    EntityMonster.CreateMonster(instance, fixedPosition, localPosition.x - 1, localPosition.y + 1, false);
                }
            }
            monsterSpawnMaxCount = currentBattleData.monsterCount;

            //필드타입변경
            if(currentBattleData.battleType == BattleType.FriendShip)
            {
                if(LeagueManager.Instance.friendshipLevel >= FriendShipLevel.Legendary)
                {
                    currentFieldType = FieldType.Shine;
                }
                else if((LeagueManager.Instance.friendshipLevel < FriendShipLevel.Legendary) && (LeagueManager.Instance.friendshipLevel >= FriendShipLevel.Gold))
                {
                    currentFieldType = MonsterDataBase.Instance.friendShipHardFields[Random.Range(0, MonsterDataBase.Instance.friendShipHardFields.Length)];
                }
                else
                {
                    currentFieldType = MonsterDataBase.Instance.friendShipNormalFIelds[Random.Range(0, MonsterDataBase.Instance.friendShipNormalFIelds.Length)];
                }
            }
            else
                currentFieldType = currentBattleData.fieldType;

            fieldDatas[currentFieldType].SetActive(true);

            currentBattleType = currentBattleData.battleType;
        }
        else
        {
            //스토리
            var currentBattleDatas = battleDataObject as List<BattleData>;
            if(currentBattleDatas != null)
            {
                currentBattleData = currentBattleDatas[currentLevel];

                var fieldDatas = MonsterDataBase.Instance.fieldTypes;
                foreach (var pair in fieldDatas)
                {
                    pair.Value.SetActive(false);
                }
                fieldDatas[currentBattleData.fieldType].SetActive(true);

                var aiMonsters = currentBattleData.monsterDataDic[Random.Range(0, currentBattleData.monsterDataDic.Count)];
                Vector2 offset = new Vector2(offsetDistance, 0f);
                foreach (var pair in aiMonsters)
                {
                    Vector2Int localPosition = pair.Key;
                    Vector2 fixedPosition = ((Vector2)localPosition * spawnDistance) + offset;

                    var instance = pair.Value.Instance();
                    EntityMonster.CreateMonster(instance, fixedPosition, localPosition.x - 1, localPosition.y + 1, false);
                }

                monsterSpawnMaxCount = currentBattleData.monsterCount;
                maxLevel = currentBattleDatas.Count;
                currentFieldType = currentBattleData.fieldType;
                currentBattleType = currentBattleData.battleType;

                var currentSlot = StoryUI.Instance.currentSlot;
                if (blockStory == false && currentSlot.isCleard == false)
                {
                    if (currentBattleData.conversationData != null && currentBattleData.conversationData.npcImage != null)
                    {
                        DialougeUI.Instance.PopUp(currentBattleData.conversationData, false, false);
                    }
                }
            }
           
        }

        SetBgm();
    }
    public void CreateHomegroundMonsters()
    {
        var currentBattleData = battleDataObject as BattleData;

        //단일배틀
        var fieldDatas = MonsterDataBase.Instance.fieldTypes;
        foreach (var pair in fieldDatas)
        {
            pair.Value.SetActive(false);
        }

        fieldDatas[currentBattleData.fieldType].SetActive(true);

        var aiMonsters = currentBattleData.monsterDataDic[Random.Range(0, currentBattleData.monsterDataDic.Count)];
        Vector2 offset = new Vector2(offsetDistance, 0f);

        foreach (var pair in aiMonsters)
        {
            pair.Value.tranningType = TranningType.Random;
            pair.Value.tranningLevel = AITranningLevel.Easy;

            Vector2Int localPosition = pair.Key;
            Vector2 fixedPosition = ((Vector2)localPosition * spawnDistance) + offset;
            fixedPosition = new Vector2(fixedPosition.x * -1f, fixedPosition.y);
            var instance = pair.Value.Instance();
            EntityMonster.CreateMonster(instance, fixedPosition, localPosition.x - 1, localPosition.y + 1, true);
        }
    }

    public void AddLevel()
    {
        var currentBattleDatas = battleDataObject as List<BattleData>;
        if (currentBattleDatas != null)
        {
            maxLevel = currentBattleDatas.Count - 1;
            currentLevel = Mathf.Min(currentLevel + 1, maxLevel);
        }
    }

    public bool CheckProgressingStory()
    {
        return (battleDataObject as List<BattleData> != null) && (currentLevel < (maxLevel - 1)); ;
    }

    public Vector2 GetWorldPosition(Vector3 localPosition)
    {
        Vector2 offset = new Vector2(offsetDistance, 0f);
        return ((Vector2)localPosition * spawnDistance) + offset;
    }

    public EntityMonster GetRandomTarget(EntityMonster player)
    {
        return GetRandomTarget(homegroundMonsters.Contains(player));
    }
    public EntityMonster GetRandomTarget(bool isHomeground)
    {
        if (isHomeground)
        {
            if (awayMonsters.Count > 0)
            {
                return awayMonsters[Random.Range(0, awayMonsters.Count)];
            }
            else
                return null;
        }
        else
        {
            if (homegroundMonsters.Count > 0)
            {
                return homegroundMonsters[Random.Range(0, homegroundMonsters.Count)];
            }
            else
                return null;
        }
    }
    public List<EmptyPositionStructure> GetEmptyPositions(bool isHomeground)
    {
        List<EmptyPositionStructure> positions = new List<EmptyPositionStructure>();
        if (isHomeground == true)
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    bool check = false;
                    for (int a = 0; a < homegroundMonsters.Count; ++a)
                    {
                        if ((homegroundMonsters[a].width == i) && (homegroundMonsters[a].height == j))
                        {
                            check = true;
                            goto jump;
                        }
                    }
                jump:
                    if (check == false)
                    {
                        EmptyPositionStructure empty = new EmptyPositionStructure();
                        empty.width = i;
                        empty.height = j;
                        empty.worldPosition = GetPosition(isHomeground, i, j);
                        positions.Add(empty);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    bool check = false;
                    for (int a = 0; a < awayMonsters.Count; ++a)
                    {
                        if ((awayMonsters[a].width == i) && (awayMonsters[a].height == j))
                        {
                            check = true;
                            goto jump;
                        }
                    }
jump:
                    if (check == false)
                    {
                        EmptyPositionStructure empty = new EmptyPositionStructure();
                        empty.width = i;
                        empty.height = j;
                        empty.worldPosition = GetPosition(isHomeground, i, j);
                        positions.Add(empty);
                    }
                }
            }
        }

        return positions;
    }
    public EmptyPositionStructure GetEmptyPosition(bool isHomeground, EmptyPositionStructure originPosition, out bool check)
    {
        var list = GetEmptyPositions(isHomeground);
        check = list.Count > 0;
        if (list.Count > 0)
        {
            int index = list.FindIndex(x => (x.width == originPosition.width) && (x.height == originPosition.height));
            if (index == -1)
            {
                return list[Random.Range(0, list.Count)];
            }
            else
            {
                return originPosition;
            }
        }

        return default;
    }
    public EmptyPositionStructure GetEmptyPosition(List<EmptyPositionStructure> emptyPositions, EmptyPositionStructure originPosition, out bool check)
    {
        var list = emptyPositions;
        check = list.Count > 0;
        if (list.Count > 0)
        {
            int index = list.FindIndex(x => (x.width == originPosition.width) && (x.height == originPosition.height));
            if (index == -1)
            {
                return list[Random.Range(0, list.Count)];
            }
            else
            {
                return originPosition;
            }
        }

        return default;
    }
    public void GetRandomDoubleTarget(EntityMonster player, SkillData skillData, out EntityMonster target_0, out EntityMonster target_1)
    {
        target_0 = null;
        target_1 = null;

        var targets = homegroundMonsters.Contains(player) ? awayMonsters : homegroundMonsters;
        targets = targets.ToList();

        if (targets.Count > 1)
        {
            target_0 = GetDetailTarget(player.battleInstance.currentSelectDetailTargetType, skillData, targets);
            target_1 = targets[Random.Range(0, targets.Count)];
            while (target_0 == target_1)
                target_1 = targets[Random.Range(0, targets.Count)];
        }
        else
        {
            target_0 = GetDetailTarget(player.battleInstance.currentSelectDetailTargetType, skillData, targets);
        }
    }
    public List<EntityMonster> GetSortTarget(bool isHomeground,TargetLayer layer)
    {
        if (isHomeground == true)
        {
            if (layer == TargetLayer.Short)
            {
                var targets = awayMonsters.FindAll(x => x != null && x.isDead == false && x.width == 0);
                if (targets.Count <= 0)
                    targets = awayMonsters.FindAll(x => x != null && x.isDead == false && x.width == 1);
                if (targets.Count <= 0)
                    targets = awayMonsters.FindAll(x => x != null && x.isDead == false && x.width == 2);

                return targets;

            }
            else if (layer == TargetLayer.Middle)
            {
                var targets = awayMonsters.FindAll(x => (x != null && x.isDead == false) && x.width == 1);
                targets.AddRange(awayMonsters.FindAll(x => (x != null && x.isDead == false) && x.width == 0));
                if (targets.Count <= 0)
                    targets = awayMonsters.FindAll(x => (x != null && x.isDead == false) && x.width == 2);

                return targets;
            }
            else
            {
                var targets = awayMonsters.FindAll(x => x != null && x.isDead == false && x.width == 2);
                targets.AddRange(awayMonsters.FindAll(x => x != null && x.isDead == false && x.width == 1));
                targets.AddRange(awayMonsters.FindAll(x => x != null && x.isDead == false && x.width == 0));
                return targets;
            }
        }
        else
        {
            if (layer == TargetLayer.Short)
            {
                var targets = homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 0);
                if (targets.Count <= 0)
                    targets = homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 1);
                if (targets.Count <= 0)
                    targets = homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 2);

                return targets;

            }
            else if (layer == TargetLayer.Middle)
            {
                var targets = homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 1);
                targets.AddRange(homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 0));
                if (targets.Count <= 0)
                    targets = homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 2);

                return targets;
            }
            else
            {
                var targets = homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 2);
                targets.AddRange(homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 1));
                targets.AddRange(homegroundMonsters.FindAll(x => x != null && x.isDead == false && x.width == 0));

                return targets;
            }
        }
    }
    public List<Vector3> GetWithinPosition(bool isHomeground, bool isCenter)
    {
        List<Vector3> positions = new List<Vector3>();

        var monster = (isHomeground) ? awayMonsters[0] : homegroundMonsters[0];
        var standardPosition = (isCenter) ? monster.center.position : monster.transform.position;
        positions.Add(standardPosition);
        if (monster.width == 0)
        {
            positions.Add(standardPosition + new Vector3(1.75f, 0f));
            positions.Add(standardPosition + new Vector3(1.75f * 2, 0f));
        }
        else if (monster.width == 1)
        {
            positions.Add(standardPosition + new Vector3(-1.75f, 0f));
            positions.Add(standardPosition + new Vector3(1.75f, 0f));
        }
        else if (monster.width == 2)
        {
            positions.Add(standardPosition + new Vector3(-1.75f, 0f));
            positions.Add(standardPosition + new Vector3(-1.75f * 2, 0f));
        }
        if (monster.height == 0)
        {
            positions.Add(positions[0] + new Vector3(0f, 1.75f));
            positions.Add(positions[0] + new Vector3(0f, 1.75f * 2));

            positions.Add(positions[1] + new Vector3(0f, 1.75f));
            positions.Add(positions[1] + new Vector3(0f, 1.75f * 2));

            positions.Add(positions[2] + new Vector3(0f, 1.75f));
            positions.Add(positions[2] + new Vector3(0f, 1.75f * 2));
        }
        else if (monster.height == 1)
        {
            positions.Add(positions[0] + new Vector3(0f, -1.75f));
            positions.Add(positions[0] + new Vector3(0f, 1.75f));

            positions.Add(positions[1] + new Vector3(0f, -1.75f));
            positions.Add(positions[1] + new Vector3(0f, 1.75f));

            positions.Add(positions[2] + new Vector3(0f, -1.75f));
            positions.Add(positions[2] + new Vector3(0f, 1.75f));
        }
        else if (monster.height == 2)
        {
            positions.Add(positions[0] + new Vector3(0f, -1.75f));
            positions.Add(positions[0] + new Vector3(0f, -1.75f * 2));

            positions.Add(positions[1] + new Vector3(0f, -1.75f));
            positions.Add(positions[1] + new Vector3(0f, -1.75f * 2));

            positions.Add(positions[2] + new Vector3(0f, -1.75f));
            positions.Add(positions[2] + new Vector3(0f, -1.75f * 2));
        }

        return positions;
    }
    public Vector3 GetPosition(bool isHomeground, int width, int height)
    {
        if(isHomeground)
        {
            Vector2 heightDisOffset = new Vector2(heightDistance * (height + 1), 0);
            Vector3 localPosition = new Vector3(-1 * width - 1, height - 1);
            Vector2 offset = new Vector2(offsetDistance, 0f);
            Vector2 fixedPosition = ((Vector2)localPosition * spawnDistance) + offset;
            return fixedPosition + heightDisOffset;
        }
        else
        {
            Vector2 heightDisOffset = new Vector2(heightDistance * (height + 1), 0);
            Vector3 localPosition = new Vector3(width + 1, height - 1);
            Vector2 offset = new Vector2(offsetDistance, 0f);
            Vector2 fixedPosition = ((Vector2)localPosition * spawnDistance) + offset;
            return fixedPosition + heightDisOffset;
        }
    }
    public List<EntityMonster> GetSortTarget(bool isHomeground, int width)
    {
        if(isHomeground)
        {
            return awayMonsters.FindAll((x => x != null && (x.isDead == false) && (x.width == width)));
        }
        else
        {
            return homegroundMonsters.FindAll((x => x != null && (x.isDead == false) && (x.width == width)));
        }
    }
    public EntityMonster[] GetNeighbourTarget(EntityMonster target, out int count)
    {
        count = 0;
        EntityMonster[] monsters = new EntityMonster[8];
        if(homegroundMonsters.Contains(target))
        {
            int currentHeight = target.height;
            int currentWidth = target.width;

            int upHeight = currentHeight + 1;
            int upWidth = currentWidth;

            int downHeight = currentHeight - 1;
            int downWidth = currentWidth;

            int leftWidth = currentWidth - 1;
            int leftHeight = currentHeight;

            int rightWidth = currentWidth + 1;
            int rightHeight = currentHeight;

            monsters[0] = homegroundMonsters.Find(x => (x.height == upHeight) && (x.width == upWidth));
            if (monsters[0] != null)
                count++;
            monsters[1] = homegroundMonsters.Find(x => (x.height == downHeight) && (x.width == downWidth));
            if (monsters[1] != null)
                count++;

            monsters[2] = homegroundMonsters.Find(x => (x.width == leftWidth) && (x.height == leftHeight));
            if (monsters[2] != null)
                count++;

            monsters[3] = homegroundMonsters.Find(x => (x.width == rightWidth) && (x.height == rightHeight));
            if (monsters[3] != null)
                count++;

            monsters[4] = homegroundMonsters.Find(x => (x.width == leftWidth) && (x.height == upHeight));
            if (monsters[4] != null)
                count++;

            monsters[5] = homegroundMonsters.Find(x => (x.width == rightWidth) && (x.height == upHeight));
            if (monsters[5] != null)
                count++;

            monsters[6] = homegroundMonsters.Find(x => (x.width == leftWidth) && (x.height == downHeight));
            if (monsters[6] != null)
                count++;

            monsters[7] = homegroundMonsters.Find(x => (x.width == rightWidth) && (x.height == downHeight));
            if (monsters[7] != null)
                count++;
        }
        else
        {
            int currentHeight = target.height;
            int currentWidth = target.width;

            int upHeight = currentHeight + 1;
            int upWidth = currentWidth;

            int downHeight = currentHeight - 1;
            int downWidth = currentWidth;

            int leftWidth = currentWidth - 1;
            int leftHeight = currentHeight;

            int rightWidth = currentWidth + 1;
            int rightHeight = currentHeight;

            monsters[0] = awayMonsters.Find(x => (x.height == upHeight) && (x.width == upWidth));
            if (monsters[0] != null)
                count++;
            monsters[1] = awayMonsters.Find(x => (x.height == downHeight) && (x.width == downWidth));
            if (monsters[1] != null)
                count++;

            monsters[2] = awayMonsters.Find(x => (x.width == leftWidth) && (x.height == leftHeight));
            if (monsters[2] != null)
                count++;

            monsters[3] = awayMonsters.Find(x => (x.width == rightWidth) && (x.height == rightHeight));
            if (monsters[3] != null)
                count++;

            monsters[4] = awayMonsters.Find(x => (x.width == leftWidth) && (x.height == upHeight));
            if (monsters[4] != null)
                count++;

            monsters[5] = awayMonsters.Find(x => (x.width == rightWidth) && (x.height == upHeight));
            if (monsters[5] != null)
                count++;

            monsters[6] = awayMonsters.Find(x => (x.width == leftWidth) && (x.height == downHeight));
            if (monsters[6] != null)
                count++;

            monsters[7] = awayMonsters.Find(x => (x.width == rightWidth) && (x.height == downHeight));
            if (monsters[7] != null)
                count++;
        }
        return monsters;
    }
    public EntityMonster GetDetailTarget(SelectDetailTargetType type, SkillData data, List<EntityMonster> targets)
    {
        if (targets.Count <= 0)
            return null;
        if (type == SelectDetailTargetType.Random)
            return targets[Random.Range(0, targets.Count)];
        else if(type == SelectDetailTargetType.High_Stat)
        {
            targets.Sort((a, b) =>
            {
                float sumValue = (a.battleInstance.maxHp * 0.1f) + (a.battleInstance.maxMp * 0.1f) + (a.battleInstance.atk) + (a.battleInstance.def) + (9 - a.battleInstance.maxDex);
                float sumValue2 = (b.battleInstance.maxHp * 0.1f) + (b.battleInstance.maxMp * 0.1f) + (b.battleInstance.atk) + (b.battleInstance.def) + (9 - b.battleInstance.maxDex);

                return sumValue2.CompareTo(sumValue);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.Low_Stat)
        {
            targets.Sort((a, b) =>
            {
                float sumValue = (a.battleInstance.maxHp * 0.1f) + (a.battleInstance.maxMp * 0.1f) + (a.battleInstance.atk) + (a.battleInstance.def) + (9 - a.battleInstance.maxDex);
                float sumValue2 = (b.battleInstance.maxHp * 0.1f) + (b.battleInstance.maxMp * 0.1f) + (b.battleInstance.atk) + (b.battleInstance.def) + (9 - b.battleInstance.maxDex);

                return sumValue.CompareTo(sumValue2);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.SpeedBreak)
        {
            targets.Sort((a, b) =>
            {
                float sumValue = 9 - a.battleInstance.maxDex;
                float sumValue2 = 9 - b.battleInstance.maxDex;

                return sumValue2.CompareTo(sumValue);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.High_Hp)
        {
            targets.Sort((a, b) =>
            {
                return b.battleInstance.hp.CompareTo(a.battleInstance.hp);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.Low_Hp)
        {
            targets.Sort((a, b) =>
            {
                return a.battleInstance.hp.CompareTo(b.battleInstance.hp);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.CenterBreak)
        {
            targets.Sort((a, b) =>
            {
                float sumValue = (a.height == 1) ? 1f : 0f;
                float sumValue2 = (b.height == 1) ? 1f : 0f;
                return sumValue2.CompareTo(sumValue);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.SideBreak)
        {
            targets.Sort((a, b) =>
            {
                float sumValue = (a.height != 1) ? 1f : 0f;
                float sumValue2 = (b.height != 1) ? 1f : 0f;
                return sumValue2.CompareTo(sumValue);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.Long_Dis)
        {
            targets.Sort((a, b) =>
            {
                return b.height.CompareTo(a.height);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.Short_Dis)
        {
            targets.Sort((a, b) =>
            {
                return a.height.CompareTo(b.height);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.Counter)
        {
            targets.Sort((a, b) =>
            {
                return b.StatusCheck(data).CompareTo(a.StatusCheck(data));
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.Low_Dodge)
        {
            targets.Sort((a, b) =>
            {
                return a.battleInstance.repeatRatio.CompareTo(b.battleInstance.repeatRatio);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.High_Dodge)
        {
            targets.Sort((a, b) =>
            {
                return b.battleInstance.repeatRatio.CompareTo(a.battleInstance.repeatRatio);
            });

            return targets[0];
        }
        else if(type == SelectDetailTargetType.InFighter)
        {
            var target = targets.Find(x => x.battleInstance.battleAIType == SelectSkillAIType.InFighter);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if (type == SelectDetailTargetType.OutFighter)
        {
            var target = targets.Find(x => x.battleInstance.battleAIType == SelectSkillAIType.OutFighter);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if (type == SelectDetailTargetType.Slerger)
        {
            var target = targets.Find(x => x.battleInstance.battleAIType == SelectSkillAIType.Slerger);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if (type == SelectDetailTargetType.Supporter)
        {
            var target = targets.Find(x => x.battleInstance.battleAIType == SelectSkillAIType.Supporter);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if(type == SelectDetailTargetType.Stern)
        {
            var target = targets.Find(x => x.sternStatus.Check() == true);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if (type == SelectDetailTargetType.Dot_Damage)
        {
            var target = targets.Find(x => x.dogDmgStatus.CheckBadStatus() == true);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else
            return targets[Random.Range(0, targets.Count)];
    }
    public Tuple<bool, EmptyPositionStructure, MonsterInstance> GetDetailTarget(SelectDetailTargetType type, List<Tuple<bool, EmptyPositionStructure, MonsterInstance>> targets)
    {
        if (type == SelectDetailTargetType.Random)
            return targets[Random.Range(0, targets.Count)];
        else if (type == SelectDetailTargetType.High_Stat)
        {
            targets.Sort((a, b) =>
            {
                var instanceA = a.Item3;
                var instanceB = b.Item3;
                float sumValue = (instanceA.maxHp * 0.1f) + (instanceA.maxMp * 0.1f) + (instanceA.atk) + (instanceA.def) + (9 - instanceA.maxDex);
                float sumValue2 = (instanceB.maxHp * 0.1f) + (instanceB.maxMp * 0.1f) + (instanceB.atk) + (instanceB.def) + (9 - instanceB.maxDex);

                return sumValue2.CompareTo(sumValue);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.Low_Stat)
        {
            targets.Sort((a, b) =>
            {
                var instanceA = a.Item3;
                var instanceB = b.Item3;
                float sumValue = (instanceA.maxHp * 0.1f) + (instanceA.maxMp * 0.1f) + (instanceA.atk) + (instanceA.def) + (9 - instanceA.maxDex);
                float sumValue2 = (instanceB.maxHp * 0.1f) + (instanceB.maxMp * 0.1f) + (instanceB.atk) + (instanceB.def) + (9 - instanceB.maxDex);

                return sumValue.CompareTo(sumValue2);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.SpeedBreak)
        {
            targets.Sort((a, b) =>
            {
                var instanceA = a.Item3;
                var instanceB = b.Item3;
                float sumValue = 9 - instanceA.maxDex;
                float sumValue2 = 9 - instanceB.maxDex;

                return sumValue2.CompareTo(sumValue);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.High_Hp)
        {
            targets.Sort((a, b) =>
            {
                var instanceA = a.Item3;
                var instanceB = b.Item3;
              
                return instanceB.hp.CompareTo(instanceA.hp);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.Low_Hp)
        {
            targets.Sort((a, b) =>
            {
                var instanceA = a.Item3;
                var instanceB = b.Item3;

                return instanceA.hp.CompareTo(instanceB.hp);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.Low_Dodge)
        {
            targets.Sort((a, b) =>
            {
                var instanceA = a.Item3;
                var instanceB = b.Item3;

                return instanceA.repeatRatio.CompareTo(instanceB.repeatRatio);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.High_Dodge)
        {
            targets.Sort((a, b) =>
            {
                var instanceA = a.Item3;
                var instanceB = b.Item3;

                return instanceB.repeatRatio.CompareTo(instanceA.repeatRatio);
            });

            return targets[0];
        }
        else if (type == SelectDetailTargetType.InFighter)
        {
            var target = targets.Find(x => x.Item3.battleAIType == SelectSkillAIType.InFighter);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if (type == SelectDetailTargetType.OutFighter)
        {
            var target = targets.Find(x => x.Item3.battleAIType == SelectSkillAIType.OutFighter);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if (type == SelectDetailTargetType.Slerger)
        {
            var target = targets.Find(x => x.Item3.battleAIType == SelectSkillAIType.Slerger);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else if (type == SelectDetailTargetType.Supporter)
        {
            var target = targets.Find(x => x.Item3.battleAIType == SelectSkillAIType.Supporter);
            if (target == null)
                return targets[Random.Range(0, targets.Count)];
            else
                return target;
        }
        else
            return targets[Random.Range(0, targets.Count)];
    }
    public int GetTrapTarget(EntityMonster player)
    {
        int index = 0;
        var type = player.battleInstance.currentSelectDetailTargetType;
        if (type == SelectDetailTargetType.Random)
            index = Random.Range(0, 3);
        else if (type == SelectDetailTargetType.CenterBreak)
            index = 1;
        else if(type == SelectDetailTargetType.SideBreak)
        {
            int isLeft = Random.Range(0, 2);
            if (isLeft == 1)
                index = 2;
            else
                index = 0;
        }    
        else
            index = Random.Range(0, 3);

        return index;
    }
    public SkillData CheckTrigger(EntityMonster player, List<Tuple<SkillTrigger, SkillData>> skillDatas, out SkillTrigger skillTrigger)
    {
        skillTrigger = SkillTrigger.Alone;
        for (int i = 0; i < skillDatas.Count; ++i)
        {
            var trigger = skillDatas[i].Item1;
            skillTrigger = trigger;
            if (trigger == SkillTrigger.CounterAttack)
            {
                if (player.passiveFlag == false)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.Alone)
            {
                var targets = homegroundMonsters.Contains(player) ? homegroundMonsters : awayMonsters;
                if(targets.Count == 1)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.OneEnemy)
            {
                var targets = homegroundMonsters.Contains(player) ? awayMonsters : homegroundMonsters;
                if (targets.Count == 1)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.OtherEnemy)
            {
                var targets = homegroundMonsters.Contains(player) ? awayMonsters : homegroundMonsters;
                if (targets.Count > 1)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.TotalHpLack)
            {
                var targets = homegroundMonsters.Contains(player) ? homegroundMonsters : awayMonsters;
                float sumValue = 0f;
                float sumValue2 = 0f;
                for (int j = 0; j < targets.Count; ++j)
                {
                    sumValue += targets[j].battleInstance.hp;
                    sumValue2 += targets[j].battleInstance.maxHp;
                }

                if (sumValue <= sumValue2 * 0.33f)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.HpLack)
            {
                if (player.battleInstance.hp <= player.battleInstance.maxHp * 0.33f)
                    return skillDatas[i].Item2;
            }
            else if (trigger == SkillTrigger.MpLack)
            {
                if (player.battleInstance.mp <= player.battleInstance.maxMp * 0.33f)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.DotDmgState)
            {
                var targets = homegroundMonsters.Contains(player) ? homegroundMonsters : awayMonsters;
                for(int j = 0; j < targets.Count; ++j)
                {
                    if(targets[j].dogDmgStatus.CheckBadStatus())
                        return skillDatas[i].Item2;
                }
            }
            else if (trigger == SkillTrigger.SternState)
            {
                var targets = homegroundMonsters.Contains(player) ? homegroundMonsters : awayMonsters;
                for (int j = 0; j < targets.Count; ++j)
                {
                    if (targets[j].sternStatus.Check())
                        return skillDatas[i].Item2;
                }
            }
            else if(trigger == SkillTrigger.NotDotDmgState)
            {
                var targets = homegroundMonsters.Contains(player) ? awayMonsters : homegroundMonsters;
                bool check = false;
                for (int j = 0; j < targets.Count; ++j)
                {
                    if (targets[j].dogDmgStatus.CheckBadStatus())
                        check = true;
                }

                if(check == false)
                    return skillDatas[i].Item2;
            }
            else if (trigger == SkillTrigger.NotSternState)
            {
                var targets = homegroundMonsters.Contains(player) ? awayMonsters : homegroundMonsters;
                bool check = false;
                for (int j = 0; j < targets.Count; ++j)
                {
                    if (targets[j].sternStatus.Check())
                        check = true;
                }

                if (check == false)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.FirstTurn)
            {
                if (player.checkFirstTurn == false)
                {
                    player.checkFirstTurn = true;
                    return skillDatas[i].Item2;
                }
            }
            else if(trigger == SkillTrigger.TeamDie)
            {
                if (deadMonsters.Count > 0)
                {
                    bool isHomeground = homegroundMonsters.Contains(player);

                    bool check = false;
                    for(int j = 0; j < deadMonsters.Count; ++j)
                    {
                        if(deadMonsters[j].Item1 == isHomeground)
                        {
                            check = true;
                            break;
                        }
                    }

                    if(check == true)
                        return skillDatas[i].Item2;
                }
            }
            else if (trigger == SkillTrigger.EnemyDie)
            {
                if (deadMonsters.Count > 0)
                {
                    bool isHomeground = homegroundMonsters.Contains(player);

                    bool check = false;
                    for (int j = 0; j < deadMonsters.Count; ++j)
                    {
                        if (deadMonsters[j].Item1 != isHomeground)
                        {
                            check = true;
                            break;
                        }
                    }

                    if (check == true)
                        return skillDatas[i].Item2;
                }
            }
            else if(trigger == SkillTrigger.SpeedLack)
            {
                bool isHomeground = homegroundMonsters.Contains(player);
                var otherTargets = isHomeground ? awayMonsters : homegroundMonsters;
                otherTargets = otherTargets.ToList();
                otherTargets.Sort((a, b) =>
                {
                    return a.battleInstance.maxDex.CompareTo(b.battleInstance.maxDex);
                });


                if (otherTargets[0].battleInstance.maxDex < player.battleInstance.maxDex)
                {
                    return skillDatas[i].Item2;
                }
            }
            else if(trigger == SkillTrigger.Disadvantage)
            {
                if (battleTimer < 8f)
                {
                    bool isHomeground = homegroundMonsters.Contains(player);
                    int value = VictoryJudgment(isHomeground);
                    if(value < 0)
                        return skillDatas[i].Item2;
                }
            }
            else if(trigger == SkillTrigger.HpLackAlways)
            {
                if (player.battleInstance.hp <= player.battleInstance.maxHp * 0.33f)
                    return skillDatas[i].Item2;
            }
            else if (trigger == SkillTrigger.MpLackAlways)
            {
                if (player.battleInstance.mp <= player.battleInstance.maxMp * 0.33f)
                    return skillDatas[i].Item2;
            }
            else if(trigger == SkillTrigger.Unbreakable)
            {
                if(player.unbreakableTarget != null)
                {
                    return skillDatas[i].Item2;
                }
            }
            else if(trigger == SkillTrigger.OtherTeam)
            {
                var targets = homegroundMonsters.Contains(player) ? homegroundMonsters : awayMonsters;
                if (targets.Count > 1)
                    return skillDatas[i].Item2;
            }
        }

        return null;
    }

    public bool CheckRemoveTrigger(SkillTrigger trigger)
    {
        if (trigger == SkillTrigger.CounterAttack)
        {
            return false;
        }
        else if (trigger == SkillTrigger.Alone)
        {
            return true;
        }
        else if (trigger == SkillTrigger.OneEnemy)
        {
            return false;
        }
        else if (trigger == SkillTrigger.OtherEnemy)
        {
            return false;
        }
        else if (trigger == SkillTrigger.TotalHpLack)
        {
            return false;
        }
        else if (trigger == SkillTrigger.HpLack)
        {
            return true;
        }
        else if (trigger == SkillTrigger.MpLack)
        {
            return true;
        }
        else if (trigger == SkillTrigger.DotDmgState)
        {
            return false;
        }
        else if (trigger == SkillTrigger.SternState)
        {
            return false;
        }
        else if (trigger == SkillTrigger.NotDotDmgState)
        {
            return false;
        }
        else if (trigger == SkillTrigger.NotSternState)
        {
            return false;
        }
        else if (trigger == SkillTrigger.FirstTurn)
        {
            return true;
        }
        else if (trigger == SkillTrigger.TeamDie)
        {
            return false;
        }
        else if (trigger == SkillTrigger.EnemyDie)
        {
            return true;
        }
        else if (trigger == SkillTrigger.SpeedLack)
        {
            return true;
        }
        else if (trigger == SkillTrigger.Disadvantage)
        {
            return true;
        }
        else if (trigger == SkillTrigger.HpLackAlways)
        {
            return false;
        }
        else if (trigger == SkillTrigger.MpLackAlways)
        {
            return false;
        }
        else if (trigger == SkillTrigger.Unbreakable)
        {
            return false;
        }
        else if (trigger == SkillTrigger.OtherTeam)
            return true;
        else
            return false;
    }
    public SkillData CheckPercent(List<Tuple<int, SkillData>> skillDatas)
    {
        for(int i = 0; i < skillDatas.Count; ++i)
        {
            float percent = skillDatas[i].Item1 * 0.01f;
            float value = Random.Range(0f, 1f);
            if(value <= percent)
                return skillDatas[i].Item2;
        }

        return null;
    }
    public SkillData CheckConfirm(EntityMonster player, ConfirmSkillPriority confirmSkillPriority, List<SkillData> datas, ref int currentIndex)
    {
        if (confirmSkillPriority == ConfirmSkillPriority.Random)
        {
            return datas[Random.Range(0, datas.Count)];
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Fit_Mp)
        {
            SortConfirm(player, ConfirmSkillPriority.Fit_Mp, datas);
            return datas[0];
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Performance)
        {
            int percent = 80;
            for (int i = 0; i < datas.Count; ++i)
            {
                int value = Random.Range(0, 100);
                if (value <= percent)
                {
                    return datas[i];
                }
                else
                    percent = Mathf.Max(10, percent - 20);
            }

            return datas[0];
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Persistence)
        {
            return datas[0];
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Surppoting)
        {
            var newDatas = datas.FindAll(x => (x.type == SkillType.Supporter) || (x.type == SkillType.Later));
            if (newDatas.Count > 0)
            {
                int value = Random.Range(0, 100);
                if (value <= 80)
                    return newDatas[Random.Range(0, newDatas.Count)];
                else
                {
                    newDatas = datas.FindAll(x => (x.type != SkillType.Supporter) && (x.type != SkillType.Later));
                    if (newDatas.Count > 0)
                        return newDatas[Random.Range(0, newDatas.Count)];
                    else
                        return datas[Random.Range(0, datas.Count)];
                }
            }
            else
                return datas[Random.Range(0, datas.Count)];
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.EverChanging)
        {
            currentIndex = Mathf.Min(datas.Count - 1, currentIndex);
            var data = datas[currentIndex];
            currentIndex++;
            if (currentIndex == datas.Count)
            {
                SortConfirm(player, ConfirmSkillPriority.EverChanging, datas);
                currentIndex = 0;
            }
            return data;
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.MainStatus)
        {
            var newDatas = datas.FindAll(x => x.status == player.battleInstance.monsterData.status);
            if (newDatas.Count > 0)
            {
                int value = Random.Range(0, 100);
                if (value <= 80)
                    return newDatas[Random.Range(0, newDatas.Count)];
                else
                {
                    newDatas = datas.FindAll(x => x.status != player.battleInstance.monsterData.status);
                    if (newDatas.Count > 0)
                        return newDatas[Random.Range(0, newDatas.Count)];
                    else
                        return datas[Random.Range(0, datas.Count)];
                }
            }
            else
                return datas[Random.Range(0, datas.Count)];
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.FocusStatus)
        {
            SortConfirm(player, ConfirmSkillPriority.FocusStatus, datas);
            float value = AttributeJudgment(player, datas[0]);

            if(value <= 1f)
            {
                var datas2 = datas.FindAll(x => x.type != SkillType.Supporter);
                if (datas2.Count > 0)
                    return datas2[Random.Range(0, datas2.Count)];
                else
                    return datas[Random.Range(0, datas.Count)];
            }
            else
                return datas[0];
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Ego)
        {
            SkillData data = null;
            if (player.battleInstance.mp <= player.battleInstance.maxMp * 0.33f)
                data = CheckConfirm(player, ConfirmSkillPriority.Fit_Mp, datas, ref currentIndex);
            else
            {
                int value = VictoryJudgment(homegroundMonsters.Contains(player));
                if (value >= 0f)
                    data = CheckConfirm(player, ConfirmSkillPriority.FocusStatus, datas, ref currentIndex);
                else
                    data = CheckConfirm(player, ConfirmSkillPriority.Surppoting, datas, ref currentIndex);
            }

            return data;
        }
        else
        {
            var data = datas[currentIndex];
            currentIndex++;
            if (currentIndex == datas.Count)
                currentIndex = 0;

            return data;
        }
    }
    public void SetFormationBuffDebuff(EntityMonster player)
    {
        //리셋
        if (!homegroundMonsters.Contains(player))
            return;
        if (currentBattleType == BattleType.Gamebling)
            return;
        if(isStart == false)
        {
            if (player.formationBuffView == null)
                player.formationBuffView = Instantiate(formationBuffViewPrefab, player.transform.position + formationBuffViewPrefab.transform.position, Quaternion.identity);
        }
        if (player == null)
            return;
        if (player.isDead == true)
            return;
        if (player.isShort == true)
        {
            float atk = Mathf.Ceil(player.battleInstance.atk * 0.33f);
            player.battleInstance.atk = Mathf.Max(0, player.battleInstance.atk - atk);

            player.battleInstance.repeatRatio += 0.1f;
        }
        if (player.isLong == true)
        {
            float atk = Mathf.Ceil(player.battleInstance.atk * 0.33f);
            player.battleInstance.atk += atk;

            player.battleInstance.repeatRatio -= 0.1f;

        }
        if (player.isSide == true)
        {
            player.battleInstance.creaticalRatio -= 0.1f;

            float def = Mathf.Ceil(player.battleInstance.def * 0.33f);
            player.battleInstance.def += def;
        }
        if (player.completeColum == true)
        {
            player.battleInstance.atk *= 0.5f;
        }
        if (player.completeRow == true)
        {
            float def = Mathf.Ceil(player.battleInstance.def * 0.33f);
            player.battleInstance.def = Mathf.Max(0, player.battleInstance.def - def);
        }

        player.isShort = false;
        player.isLong = false;
        player.isSide = false;
        player.completeColum = false;
        player.completeRow = false;

        //버프
        if (player.width == 0)
        {
            float atk = Mathf.Round(player.battleInstance.atk * 0.33f);
            player.battleInstance.atk += atk;

            player.battleInstance.repeatRatio -= 0.1f;
            player.isShort = true;

            if (player.formationBuffView != null)
            {
                player.formationBuffView.AddData("ATK<b><size=96>", true);
                player.formationBuffView.AddData("DDE<b><size=96>", false);
            }
        }
        else if (player.width == 2)
        {
            float atk = Mathf.Round(player.battleInstance.atk * 0.33f);
            player.battleInstance.atk = Mathf.Max(0, player.battleInstance.atk - atk);

            player.battleInstance.repeatRatio += 0.1f;
            player.isLong = true;

            if (player.formationBuffView != null)
            {
                player.formationBuffView.AddData("DDE<b><size=96>", true);
                player.formationBuffView.AddData("ATK<b><size=96>", false);
            }
        }

        if (player.height != 1)
        {
            player.battleInstance.creaticalRatio += 0.1f;

            float def = Mathf.Round(player.battleInstance.def * 0.33f);
            player.battleInstance.def = Mathf.Max(0, player.battleInstance.def - def);
            player.isSide = true;

            if (player.formationBuffView != null)
            {
                player.formationBuffView.AddData("CRI<b><size=96>", true);
                player.formationBuffView.AddData("DEF<b><size=96>", false);
            }
        }
        if (player.formationBuffView != null)
            player.formationBuffView.SettingView();
    }
    public void SetFormationBuffDebuffs()
    {
        if (currentBattleType == BattleType.Gamebling)
            return;
        for (int i = 0; i < homegroundMonsters.Count; ++i)
            SetFormationBuffDebuff(homegroundMonsters[i]);
    }
    public void SortConfirm(EntityMonster player, ConfirmSkillPriority confirmSkillPriority, List<SkillData> datas)
    {
        if (confirmSkillPriority == ConfirmSkillPriority.High_Mp)
        {
            datas.Sort((a, b) =>
            {
                return b.consumMpAmount.CompareTo(a.consumMpAmount);
            });
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Low_Mp)
        {
            datas.Sort((a, b) =>
            {
                return a.consumMpAmount.CompareTo(b.consumMpAmount);
            });
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.High_Atk)
        {
            datas.Sort((a, b) =>
            {
                return b.atk.CompareTo(a.atk);
            });
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Fit_Mp)
        {
            datas.Sort((a, b) =>
            {
                float value = Mathf.Abs(player.battleInstance.mp - a.consumMpAmount);
                float value2 = Mathf.Abs(player.battleInstance.mp - b.consumMpAmount);
                return value.CompareTo(value2);
            });
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Performance)
        {
            datas.Sort((a, b) =>
            {
                return b.atk.CompareTo(a.atk);
            });
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.Persistence)
        {
            if(datas.Count > 1)
            {
                for (int i = 0; i < 20; ++i)
                {
                    int a = Random.Range(0, datas.Count);
                    int b = Random.Range(0, datas.Count);
                    while (a == b)
                        b = Random.Range(0, datas.Count);

                    var tmp = datas[a];
                    datas[a] = datas[b];
                    datas[b] = tmp;
                }
            }
        }
        else if (confirmSkillPriority == ConfirmSkillPriority.EverChanging)
        {
            if (datas.Count > 1)
            {
                for (int i = 0; i < 20; ++i)
                {
                    int a = Random.Range(0, datas.Count);
                    int b = Random.Range(0, datas.Count);
                    while (a == b)
                        b = Random.Range(0, datas.Count);

                    var tmp = datas[a];
                    datas[a] = datas[b];
                    datas[b] = tmp;
                }
            }
        }
        else if(confirmSkillPriority == ConfirmSkillPriority.FocusStatus)
        {
            datas.Sort((a, b) =>
            {
                return AttributeJudgment(player, b).CompareTo(AttributeJudgment(player, a));
            });
        }
    }
    public float AttributeJudgment(EntityMonster player, SkillData skillData)
    {
        if (skillData.type != SkillType.Supporter)
        {
            float value = 1;
            var targets = homegroundMonsters.Contains(player) ? awayMonsters : homegroundMonsters;
            if (skillData.targetLayer == TargetLayer.Short)
            {
                bool check = false;
                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].width == 0)
                    {
                        value *= targets[i].StatusCheck(skillData);
                        check = true;
                    }
                }

                if (check == false)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        if (targets[i].width == 1)
                        {
                            value *= targets[i].StatusCheck(skillData);
                            check = true;
                        }
                    }
                }

                if (check == false)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        if (targets[i].width == 2)
                        {
                            value *= targets[i].StatusCheck(skillData);
                            check = true;
                        }
                    }
                }
            }
            else if (skillData.targetLayer == TargetLayer.Middle)
            {
                bool check = false;
                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].width == 1)
                    {
                        value *= targets[i].StatusCheck(skillData);
                        check = true;
                    }
                }
                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].width == 0)
                    {
                        value *= targets[i].StatusCheck(skillData);
                        check = true;
                    }
                }

                if (check == false)
                {
                    for (int i = 0; i < targets.Count; ++i)
                    {
                        if (targets[i].width == 2)
                        {
                            value *= targets[i].StatusCheck(skillData);
                            check = true;
                        }
                    }
                }
            }
            else if (skillData.targetLayer == TargetLayer.All)
            {
                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].width == 2)
                    {
                        value *= targets[i].StatusCheck(skillData);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].width == 1)
                    {
                        value *= targets[i].StatusCheck(skillData);
                    }
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    if (targets[i].width == 0)
                    {
                        value *= targets[i].StatusCheck(skillData);
                    }
                }
            }

            return value;
        }
        else
            return 0;
    }
    public float GetFieldStatus(SkillData skillData)
    {
        switch (currentFieldType)
        {
            case FieldType.Volcano:
                {
                    if (skillData.status == Status.Fire)
                        return 0.5f;
                    else if (skillData.status == Status.Ice)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Snow:
                {
                    if (skillData.status == Status.Ice)
                        return 0.5f;
                    else if (skillData.status == Status.Fire)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Snow2:
                {
                    if (skillData.status == Status.Ice)
                        return 0.5f;
                    else if (skillData.status == Status.Fire)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Desert:
                {
                    if (skillData.status == Status.Earth)
                        return 0.5f;
                    else if (skillData.status == Status.Elec)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Desert2:
                {
                    if (skillData.status == Status.Earth)
                        return 0.5f;
                    else if (skillData.status == Status.Elec)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Forest:
                {
                    if (skillData.status == Status.Wood)
                        return 0.5f;
                    else
                        return 0f;
                }
            case FieldType.Fall:
                {
                    if (skillData.status == Status.Wood)
                        return 0.5f;
                    else if (skillData.status == Status.Fire)
                        return 0.5f;
                    else
                        return 0f;
                }
            case FieldType.Cave:
                {
                    if (skillData.status == Status.Earth)
                        return 0.5f;
                    else if (skillData.status == Status.Dark)
                        return 0.5f;
                    else if (skillData.status == Status.Light)
                        return -0.5f;
                    else if (skillData.status == Status.Elec)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Plateau:
                {
                    if (skillData.status == Status.Wind)
                        return 0.5f;
                    else if (skillData.status == Status.Elec)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Shine:
                {
                    if (skillData.status == Status.Light)
                        return 0.5f;
                    else if (skillData.status == Status.Dark)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Spooky:
                {
                    if (skillData.status == Status.Dark)
                        return 0.5f;
                    else if (skillData.status == Status.Wood)
                        return 0.5f;
                    else if (skillData.status == Status.Light)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Spooky2:
                {
                    if (skillData.status == Status.Dark)
                        return 0.5f;
                    else if (skillData.status == Status.Wood)
                        return 0.5f;
                    else if (skillData.status == Status.Light)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Ship:
                {
                    if (skillData.status == Status.Elec)
                        return 0.5f;
                    else if (skillData.status == Status.Wind)
                        return -0.5f;
                    else if (skillData.status == Status.Earth)
                        return -0.5f;
                    else
                        return 0f;
                }
            case FieldType.Circuit:
                {
                    if (skillData.status == Status.Normal)
                        return 0.5f;
                    else
                        return 0f;
                }
            default:
                return 0f;
        }       
    }
    public float GetStoneStatus(SkillData skillData)
    {
        if (skillData.status == Status.Fire)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.fireStone))
                return 1f;
            else
                return 0f;
        }
        else if (skillData.status == Status.Ice)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.iceStone))
                return 1f;
            else
                return 0f;
        }
        else if (skillData.status == Status.Acid)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.poisonStone))
                return 1f;
            else
                return 0f;
        }
        else if (skillData.status == Status.Elec)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.elecStone))
                return 1f;
            else
                return 0f;
        }
        else if (skillData.status == Status.Normal)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.normalStone))
                return 1f;
            else
                return 0f;
        }
        else
            return 0f;
    }
    public SerializableDictionary<Vector2Int, AIMonsterData> GetFriendShipAIMonster(BattleData data, MonsterClass monsterClass)
    {
        var datas = data.monsterDataDic.FindAll(x => x.Values.First().monsterData.monsterClass == monsterClass);
        return datas[Random.Range(0, datas.Count)];
    }
    public FriendShipLevel GetFriendShipLevel()
    {
        int loseCount = ItemInventory.Instance.friendShipBattleCount - ItemInventory.Instance.friendShipBattleWin;
        int value = Mathf.Max(0, ItemInventory.Instance.friendShipBattleWin - loseCount);

        if (value < 3)
        {
            return FriendShipLevel.Begginer;
        }
        else if (value < 6 && value >= 3)
        {
            return FriendShipLevel.Bronz;
        }
        else if (value < 9 && value >= 6)
        {
            return FriendShipLevel.Silver;
        }
        else if (value < 12 && value >= 9)
        {
            return FriendShipLevel.Gold;
        }
        else if (value < 15 && value >= 12)
        {
            return FriendShipLevel.Platinum;
        }
        else if (value < 21 && value >= 15)
        {
            return FriendShipLevel.Diamond;
        }
        else if (value < 26 && value >= 21)
        {
            return FriendShipLevel.Legendary;
        }
        else if (value < 31 && value >= 26)
        {
            return FriendShipLevel.Ancient;
        }
        else if (value >= 31)
        {
            return FriendShipLevel.Myth;
        }
        else
            return 0;
    }
    public int GetFriendShipReward()
    {
        int defaultReward = 100;
        int multifly = (int)LeagueManager.Instance.friendshipLevel;

        return defaultReward + (25 * multifly * (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.piggyBank) ? 2 : 1));
    }
    public void GetFriendShipValues(out MonsterClass monsterClass, out AITranningLevel tranningLevel, out TranningType tranningType)
    {
        var battleClass = GetFriendShipLevel();
        if(battleClass == FriendShipLevel.Begginer)
        {
            monsterClass = MonsterClass.Low;
            tranningLevel = AITranningLevel.Default;
            tranningType = TranningType.Locked;
        }
        else if(battleClass == FriendShipLevel.Bronz)
        {
            int random = Random.Range(0, 2);
            monsterClass = (random == 0) ? MonsterClass.Low : MonsterClass.Middle;
            tranningLevel = (monsterClass == MonsterClass.Low) ? AITranningLevel.Easy : AITranningLevel.Default;
            tranningType = TranningType.Random;
        }
        else if (battleClass == FriendShipLevel.Silver)
        {
            monsterClass = MonsterClass.Middle;
            tranningLevel = AITranningLevel.Normal;
            tranningType = TranningType.Random;
        }
        else if(battleClass == FriendShipLevel.Gold)
        {
            int random = Random.Range(0, 2);
            monsterClass = (random == 0) ? MonsterClass.Middle : MonsterClass.High;
            tranningLevel = (monsterClass == MonsterClass.Middle) ? AITranningLevel.Normal : AITranningLevel.Easy;
            tranningType = TranningType.Random;
        }
        else if (battleClass == FriendShipLevel.Platinum)
        {
            monsterClass = MonsterClass.High;
            tranningLevel = (monsterClass == MonsterClass.Middle) ? AITranningLevel.Normal : AITranningLevel.Easy;
            tranningType = TranningType.Random;
        }
        else if (battleClass == FriendShipLevel.Diamond)
        {
            int random = Random.Range(0, 2);
            monsterClass = (random == 0) ? MonsterClass.High : MonsterClass.VeryHigh;
            tranningLevel = (random == 0) ? AITranningLevel.Normal : AITranningLevel.Hard;
            tranningType = TranningType.Genius;
        }
        else if (battleClass == FriendShipLevel.Legendary)
        {
            int random = Random.Range(0, 2);
            monsterClass = (random == 0) ? MonsterClass.VeryHigh : MonsterClass.Legend;
            tranningLevel = (monsterClass == MonsterClass.High) ? AITranningLevel.Hard : AITranningLevel.Normal;
            tranningType = TranningType.Genius;
        }
        else if (battleClass == FriendShipLevel.Ancient)
        {
            int random = Random.Range(0, 2);
            monsterClass = MonsterClass.Legend;
            tranningLevel = (random == 0) ? AITranningLevel.Hard : AITranningLevel.VeryHard;
            tranningType = TranningType.Genius;
        }
        else if (battleClass == FriendShipLevel.Myth)
        {
            monsterClass = MonsterClass.Myth;
            tranningLevel = AITranningLevel.VeryHard;
            tranningType = TranningType.Late_Bloomer;
        }
        else
        {
            monsterClass = MonsterClass.Low;
            tranningLevel = AITranningLevel.Default;
            tranningType = TranningType.Locked;
        }
    }
    public int VictoryJudgment(bool isHomeground)
    {
        float teamValue = 0;
        float otherValue = 0;

        var teams = isHomeground ? homegroundMonsters : awayMonsters;
        var ohers = isHomeground ? awayMonsters : homegroundMonsters;

        if (teams.Count > ohers.Count)
            teamValue++;
        if (ohers.Count > teams.Count)
            otherValue++;

        float teamSumHp = 0f;
        for (int j = 0; j < teams.Count; ++j)
        {
            teamSumHp += teams[j].battleInstance.hp;
        }

        float oherSumHp = 0f;
        for (int j = 0; j < ohers.Count; ++j)
        {
            oherSumHp += ohers[j].battleInstance.hp;
        }

        float teamSumMp = 0f;
        for (int j = 0; j < teams.Count; ++j)
        {
            teamSumMp += teams[j].battleInstance.mp;
        }

        float oherSumMp = 0f;
        for (int j = 0; j < ohers.Count; ++j)
        {
            oherSumMp += ohers[j].battleInstance.mp;
        }

        float teamSumDex = 0f;
        for (int j = 0; j < teams.Count; ++j)
        {
            teamSumDex += teams[j].battleInstance.dex;
        }

        float oherSumDex = 0f;
        for (int j = 0; j < ohers.Count; ++j)
        {
            oherSumDex += ohers[j].battleInstance.dex;
        }

        float teamSumStat = 0f;
        for (int j = 0; j < teams.Count; ++j)
        {
            teamSumStat = Mathf.Max(0, teams[j].battleInstance.atk + teams[j].battleInstance.def - teams[j].battleInstance.maxDex);
        }

        float oherSumStat = 0f;
        for (int j = 0; j < ohers.Count; ++j)
        {
            oherSumStat = Mathf.Max(0, ohers[j].battleInstance.atk + ohers[j].battleInstance.def - ohers[j].battleInstance.maxDex);
        }

        if (teamSumHp > oherSumHp)
            teamValue++;
        if (oherSumHp > teamSumHp)
            otherValue++;

        if (teamSumMp > oherSumMp)
            teamValue++;
        if (oherSumMp > teamSumMp)
            otherValue++;

        if (teamSumDex > oherSumDex)
            teamValue++;
        if (oherSumDex > teamSumDex)
            otherValue++;

        if (teamSumStat > oherSumStat)
        {
            float value = 1 - (1 / teamSumStat - oherSumStat);
            teamValue += value;
            otherValue -= value;
        }
        if (oherSumStat > teamSumStat)
        {
            float value = 1 - (1 / oherSumStat - teamSumStat);
            oherSumStat += value;
            teamSumStat -= value;
        }

        if (teamValue > otherValue)
            return 1;
        else if(teamValue < otherValue)
            return -1;
        else
            return 0;
    }
    public int VictoryJudgmentToGameble(int hightestIndex, int lowtestIndex, VictoryJudgmentType type, MonsterInstance homeground, MonsterInstance away)
    {
        if (type == VictoryJudgmentType.Random)
            return Random.Range(0, 3);
        else if(type == VictoryJudgmentType.Physical)
        {
            float homegroundValue = homeground.maxHp + homeground.atk + homeground.def;
            float awayValue = away.maxHp + away.atk + away.def;

            if (homegroundValue > awayValue)
                return 0;
            else if (homegroundValue < awayValue)
                return 1;
            else
                return 2;
        }
        else if(type == VictoryJudgmentType.Attacker)
        {
            float homegroundValue = 1f;
            float awayValue = 1f;

            if (homeground.atk > away.atk)
                homegroundValue += 2f;
            else if (homeground.atk < away.atk)
                awayValue += 2f;

            if (homeground.maxDex < away.maxDex)
                homegroundValue *= 2f;
            else if (homeground.maxDex > away.maxDex)
                awayValue *= 2f;

            if ((homeground.battleAIType == SelectSkillAIType.Supporter) || (homeground.battleAIType == SelectSkillAIType.Balance))
                homegroundValue /= 2f;
            if ((away.battleAIType == SelectSkillAIType.Supporter) || (away.battleAIType == SelectSkillAIType.Balance))
                awayValue /= 2f;

            if (homegroundValue > awayValue)
                return 0;
            else if (homegroundValue < awayValue)
                return 1;
            else
                return 2;
        }
        else if(type == VictoryJudgmentType.Slerger)
        {
            if ((homeground.battleAIType == SelectSkillAIType.Slerger) && (away.battleAIType != SelectSkillAIType.Slerger))
                return 0;
            if ((away.battleAIType == SelectSkillAIType.Slerger) && (homeground.battleAIType != SelectSkillAIType.Slerger))
                return 1;

            return 2;
        }
        else if(type == VictoryJudgmentType.Tanker)
        {
            if ((homeground.def >= away.atk) || (away.def >= homeground.atk))
                return 2;
            if ((homeground.maxHp >= away.atk * 8) || (away.maxHp >= homeground.atk * 8))
                return 2;

            return Random.Range(0, 2);
        }
        else if(type == VictoryJudgmentType.Many)
        {
            return hightestIndex;
        }
        else if(type == VictoryJudgmentType.Reverse)
        {
            return lowtestIndex;
        }

        return 0;
    }
    public void GetVictoryPercent(MonsterInstance homeground, MonsterInstance away, out float homegroundPercent, out float awayPercent, out float tiePercent)
    {
        List<int> indies = new List<int>();

        for(int i = 0; i < 60; ++i)
        {
            int index = VictoryJudgmentToGameble(2, 2, (VictoryJudgmentType)Random.Range(0, 4), homeground, away);
            indies.Add(index);
        }

        int index_0 = 0, index_1 = 0, index_2 = 0;  

        for(int i = 0; i < indies.Count; ++i)
        {
            if (indies[i] == 0)
                index_0++;
            else if (indies[i] == 1)
                index_1++;
            else if (indies[i] == 2)
                index_2++;
        }

        int hightestIndex = Mathf.Max(index_0, index_1);
        hightestIndex = Mathf.Max(hightestIndex, index_2);

        int lowtestIndex = Mathf.Min(index_0, index_1);
        lowtestIndex = Mathf.Min(lowtestIndex, index_2);

        int random = Random.Range(0, 3);
        if (random == 0)
        {
            for (int i = 0; i < 40; ++i)
            {
                int index = VictoryJudgmentToGameble(hightestIndex, lowtestIndex, VictoryJudgmentType.Random, homeground, away);
                indies.Add(index);
            }
        }
        else if(random == 1)
        {
            for (int i = 0; i < 40; ++i)
            {
                int index = VictoryJudgmentToGameble(hightestIndex, lowtestIndex, VictoryJudgmentType.Many, homeground, away);
                indies.Add(index);
            }
        }
        else if(random == 2)
        {
            for (int i = 0; i < 40; ++i)
            {
                int index = VictoryJudgmentToGameble(hightestIndex, lowtestIndex, VictoryJudgmentType.Reverse, homeground, away);
                indies.Add(index);
            }
        }

        index_0 = 0;
        index_1 = 0;
        index_2 = 0;
        for (int i = 0; i < indies.Count; ++i)
        {
            if (indies[i] == 0)
                index_0++;
            else if (indies[i] == 1)
                index_1++;
            else if (indies[i] == 2)
                index_2++;
        }

        homegroundPercent = 1 - Mathf.Clamp(index_0 / 100f, 0.01f, 0.99f);
        awayPercent = 1 - Mathf.Clamp(index_1 / 100f, 0.01f, 0.99f);
        tiePercent = 1 - Mathf.Clamp(index_2 / 100f, 0.01f, 0.99f);
    }
    public int CheckWinAndLoseBetting(int winAndLose)
    {
        int betIndex = BatUI.Instance.batIndex;
        if (betIndex == 0)
        {
            if (winAndLose == 1)
            {
                return 1;
            }
            else
                return -1;
        }
        else if (betIndex == 1)
        {
            if (winAndLose == 0)
            {
                return 1;
            }
            else
                return -1;
        }
        else if (betIndex == 2)
        {
            if (winAndLose == -1)
            {
                return 1;
            }
            else
                return -1;
        }
        else
            return 0;
    }
    public void EasterClear(MonsterInstance instance)
    {
        for (int i = 0; i < deadMonsters.Count; ++i)
        {
            if (deadMonsters[i].Item3 == instance)
            {
                deadMonsters.RemoveAt(i);
                break;
            }
        }

        var list = SkillManager.Instance.easterList;
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].obj == instance)
            {
                list.RemoveAt(i);
                break;
            }
        }
    }
    public void ReturnGame()
    {
        var rockClones = SkillManager.Instance.rockClones;
        for (int i = 0; i < rockClones.Count; ++i)
            Destroy(rockClones[i].gameObject);
        rockClones.Clear();

        for (int i = 0; i < allMonsters.Count; ++i)
        {
            allMonsters[i].sternStatus.Stop(allMonsters[i]);
            Destroy(allMonsters[i].gameObject);
        }

        foreach (var pair in trapDic)
        {
            Destroy(pair.Value.gameObject);
        }
        trapDic.Clear();

        Clear();
        isStart = false;
        isEnd = false;
        livingDeadAllDeadFlag = false;
        monsterSpawnCount = 0;
        rockBlasterTimer = 0f;
        combatUI.battleButton.interactable = false;
        battleTimer = battleMaxTimer;
        dotDmgTime = 0f;
        shaodwCrewTime = 0f;
        combatUI.PopUp();
        var slots = combatUI.slots;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UnBlock();
        }
        for(int i = 0; i < selectBoxies.Count; i++)
        {
            selectBoxies[i].UnBlock();
        }

        CreateAwayMonsters();

        VictoryUI.Instance.Closed();
    }
    public void ReturnPvpGame()
    {
        var rockClones = SkillManager.Instance.rockClones;
        for (int i = 0; i < rockClones.Count; ++i)
            Destroy(rockClones[i].gameObject);
        rockClones.Clear();

        for (int i = 0; i < allMonsters.Count; ++i)
        {
            allMonsters[i].sternStatus.Stop(allMonsters[i]);
            Destroy(allMonsters[i].gameObject);
        }

        foreach (var pair in trapDic)
        {
            Destroy(pair.Value.gameObject);
        }
        trapDic.Clear();

        Clear();
        isStart = false;
        isEnd = false;
        livingDeadAllDeadFlag = false;
        monsterSpawnCount = 0;
        rockBlasterTimer = 0f;
        combatUI.battleButton.interactable = false;
        battleTimer = battleMaxTimer;
        dotDmgTime = 0f;
        shaodwCrewTime = 0f;

        var slots = combatUI.slots;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UnBlock();
        }
        for (int i = 0; i < selectBoxies.Count; i++)
        {
            selectBoxies[i].UnBlock();
        }

        CreateAwayMonsters();

        VictoryUI.Instance.Closed();
    }
    public void ReturnGamebling()
    {
        var rockClones = SkillManager.Instance.rockClones;
        for (int i = 0; i < rockClones.Count; ++i)
            Destroy(rockClones[i].gameObject);
        rockClones.Clear();

        for (int i = 0; i < allMonsters.Count; ++i)
        {
            allMonsters[i].sternStatus.Stop(allMonsters[i]);
            Destroy(allMonsters[i].gameObject);
        }

        foreach (var pair in trapDic)
        {
            Destroy(pair.Value.gameObject);
        }
        trapDic.Clear();

        Clear();
        isStart = false;
        isEnd = false;
        livingDeadAllDeadFlag = false;
        monsterSpawnCount = 0;
        rockBlasterTimer = 0f;
        combatUI.battleButton.interactable = false;
        battleTimer = battleMaxTimer;
        dotDmgTime = 0f;
        shaodwCrewTime = 0f;
        combatUI.PopUp2();
        var slots = combatUI.slots;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].UnBlock();
        }
        for (int i = 0; i < selectBoxies.Count; i++)
        {
            selectBoxies[i].UnBlock();
        }

        CreateAwayMonsters();
        CreateHomegroundMonsters();
        VictoryUI.Instance.Closed();
    }
    public void ReturnEditing()
    {
        var rockClones = SkillManager.Instance.rockClones;
        for (int i = 0; i < rockClones.Count; ++i)
            Destroy(rockClones[i].gameObject);
        rockClones.Clear();

        for (int i = 0; i < allMonsters.Count; ++i)
        {
            allMonsters[i].sternStatus.Stop(allMonsters[i]);
            Destroy(allMonsters[i].gameObject);
        }

        foreach (var pair in trapDic)
        {
            Destroy(pair.Value.gameObject);
        }
        trapDic.Clear();

        Clear();
        isStart = false;
        isEnd = false;
        livingDeadAllDeadFlag = false;
        monsterSpawnCount = 0;
        rockBlasterTimer = 0f;
        combatUI.battleButton.interactable = false;
        battleTimer = battleMaxTimer;
        dotDmgTime = 0f;
        shaodwCrewTime = 0f;

        VictoryUI.Instance.Closed();
    }
    public void StartGame()
    {
        if (isStart == false)
        {
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlayEffect(158, 1f);
            SoundManager.Instance.PlayEffect(159, 1f);

            var soruce = SoundManager.Instance.FindEffect(159);
            SoundManager.Instance.Fade(soruce, 4f, 0f, 0.75f);

            combatUI.Closed();
            isStart = true;

            SetOriginSpeed();
            SettingSupportBuff();
            UpdateGardners();

            if (currentBattleType != BattleType.PvP)
            {
                for (int i = 0; i < homegroundMonsters.Count; ++i)
                {
                    playerInstances.Add(new Tuple<MonsterInstance, MonsterInstance>(homegroundMonsters[i].originInstance, homegroundMonsters[i].battleInstance));
                }
            }
           
            for(int i = 0; i < allMonsters.Count; ++i)
            {
                if (allMonsters[i].formationBuffView != null)
                {
                    Destroy(allMonsters[i].formationBuffView.gameObject);
                    allMonsters[i].formationBuffView = null;
                }
            }

            if(currentBattleType == BattleType.Official)
            {
                if (startEvent != null)
                    startEvent.Invoke();
            }

           
        }
    }
    public void StartGamebling()
    {
        if (isStart == false)
        {
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlayEffect(158, 1f);
            SoundManager.Instance.PlayEffect(159, 1f);

            var soruce = SoundManager.Instance.FindEffect(159);
            SoundManager.Instance.Fade(soruce, 4f, 0f, 0.75f);

            SetOriginSpeed();
            UpdateGardners();

            BatUI.Instance.Closed();
            combatUI.Closed2();

            LeagueManager.Instance.blockGamebling = true;
            isStart = true;
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        Time.timeScale = timeScale;
    }
    public void SetDoubleSpeed()
    {
        timeScale = 4f * SystemUI.Instance.addBattleSpeed;
        Time.timeScale = timeScale;
    }
    public void SetOriginSpeed()
    {
        timeScale = 2f * SystemUI.Instance.addBattleSpeed;
        Time.timeScale = timeScale;
    }
    public void SetResetSpeed()
    {
        timeScale = 1f;
        Time.timeScale = timeScale;
    }
    public void SettingSelectBox()
    {
        selectBoxies = new List<SelectBox>();
        Vector2 offset = new Vector2(offsetDistance, 0f);

        for(int i = 0; i < homegroundLocalPosition.Length; i++)
        {
            Vector2 fixedPosition = (((Vector2)homegroundLocalPosition[i] * spawnDistance) + offset);
            var effect = Instantiate(SkillManager.Instance.selectBox, fixedPosition, Quaternion.identity);

            effect.width = Mathf.Abs(homegroundLocalPosition[i].x) - 1;
            effect.height = homegroundLocalPosition[i].y + 1;
            effect.fixedPosition = fixedPosition;

            selectBoxies.Add(effect);
            effect.gameObject.SetActive(false);
        }
    }
    public void ActiveSelectBox(BoxCollider2D target)
    {
        for (int i = 0; i < selectBoxies.Count; ++i)
        {
            selectBoxies[i].gameObject.SetActive(true);
            selectBoxies[i].target = target;
   
        }
    }
    public void InActiveSelectBox()
    {
        for (int i = 0; i < selectBoxies.Count; ++i)
        {
            selectBoxies[i].gameObject.SetActive(false);
        }
    }
    public bool CheckMonsterCount()
    {
        return monsterSpawnCount >= monsterSpawnMaxCount;
    }
    public bool CheckPlayBattle()
    {
        return monsterSpawnCount >= 1;
    }
    public SelectBox CheckSelectBox()
    {
        for (int i = 0; i < selectBoxies.Count; ++i)
        {
            if (selectBoxies[i].isInit == true && selectBoxies[i].isBlock == false)
            {
                return selectBoxies[i];
            }
        }

        return null;
    }
    public void SetPvPBgm()
    {
        SoundManager.Instance.StopBgm();

        int index = Random.Range(35, 41);
        if (index == 39)
            index = 45;
        if (index == 40)
            index = 46;
        while (index == DialougeUI.Instance.previousBattleBgmIndex)
        {
            index = Random.Range(35, 41);
            if (index == 39)
                index = 45;
            if (index == 40)
                index = 46;
        }
        SoundManager.Instance.PlayBgm(index, 1f);
        DialougeUI.Instance.previousBattleBgmIndex = index;
    }
    private void SetBgm()
    {
        if (DialougeUI.Instance.isPopUp == true)
            return;

        var currentSlot = StoryUI.Instance.currentSlot;
        if (currentBattleType == BattleType.Story)
        {
            if (currentSlot.bgmCilps != null)
            {
                if (currentSlot.bgmCilps.ContainsKey(currentLevel))
                {
                    SoundManager.Instance.StopBgm();
                    SoundManager.Instance.PlayBgm(currentSlot.bgmCilps[currentLevel], 1f);
                    StoryUI.Instance.changeSoundIndex = currentSlot.bgmCilps[currentLevel];
                }
                else
                {
                    SoundManager.Instance.StopBgm();
                    SoundManager.Instance.PlayBgm(StoryUI.Instance.changeSoundIndex, 1f);
                }
            }
        }
        else if (currentBattleType == BattleType.FriendShip)
        {
            SoundManager.Instance.StopBgm();

            int index = Random.Range(35, 41);
            if (index == 39)
                index = 45;
            if (index == 40)
                index = 46;
            while (index == DialougeUI.Instance.previousBattleBgmIndex)
            {
                index = Random.Range(35, 41);
                if (index == 39)
                    index = 45;
                if (index == 40)
                    index = 46;
            }
            SoundManager.Instance.PlayBgm(index, 1f);
            DialougeUI.Instance.previousBattleBgmIndex = index;
        }
        else if(currentBattleType == BattleType.Gamebling)
        {
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlayBgm(42, 1f);
        }
        else if(currentBattleType == BattleType.Official)
        {
            SoundManager.Instance.StopBgm();

            if (LeagueManager.Instance.GetCurrentLeagueNumber() == 0)
            {
                SoundManager.Instance.PlayBgm(30, 1f);
            }
        }
    }
    private void CheckWinAndLose()
    {
        //누가 승리했는지결정하고 끝내기
        if (SkillManager.Instance.isBlock == false && SkillManager.Instance.willPowerStructure.isStart == false)
        {
            if (homegroundMonsters.Count <= 0 || awayMonsters.Count <= 0)
            {
                int value = 0;
                if (homegroundMonsters.Count > 0 && awayMonsters.Count <= 0)
                {
                    VictoryUI.Instance.PopUp(1);
                    value = 1;
                    if (currentBattleType == BattleType.Official)
                        LeagueManager.Instance.Report(1);
                }
                else if (homegroundMonsters.Count <= 0 && awayMonsters.Count > 0)
                {
                    VictoryUI.Instance.PopUp(-1);
                    value = -1;
                    if (currentBattleType == BattleType.Official)
                        LeagueManager.Instance.Report(-1);
                }
                else if (homegroundMonsters.Count <= 0 && awayMonsters.Count <= 0)
                {
                    VictoryUI.Instance.PopUp(0);
                    value = 0;
                    if (currentBattleType == BattleType.Official)
                        LeagueManager.Instance.Report(0);

                }

                if(currentBattleType != BattleType.PvP)
                    UpdateOriginInstances(value);
                if (currentBattleType == BattleType.Official)
                {
                    if (endEvent != null)
                        endEvent.Invoke(value);
                }
                isEnd = true;

                return;
            }
        }
    }
    private void SettingSupportBuff()
    {
        var supports = homegroundMonsters.FindAll(x => x.battleInstance.abilities.Contains(AbilityType.Support));
        for(int i = 0; i < supports.Count; ++i)
        {
            var mons = homegroundMonsters.ToList();
            mons.Remove(supports[i]);
            for(int j = 0;  j < mons.Count; ++j)
            {
                mons[j].battleInstance.atk += 3;
                mons[j].battleInstance.def += 3;
            }
        }

        supports = awayMonsters.FindAll(x => x.battleInstance.abilities.Contains(AbilityType.Support));
        for (int i = 0; i < supports.Count; ++i)
        {
            var mons = awayMonsters.ToList();
            mons.Remove(supports[i]);
            for (int j = 0; j < mons.Count; ++j)
            {
                mons[j].battleInstance.atk += 3;
                mons[j].battleInstance.def += 3;
            }
        }
    }
    private void Clear()
    {
        for (int i = 0; i < allMonsters.Count; ++i)
        {
            if (allMonsters[i].formationBuffView != null)
            {
                Destroy(allMonsters[i].formationBuffView.gameObject);
                allMonsters[i].formationBuffView = null;
            }
        }

        homegroundMonsters.Clear();
        awayMonsters.Clear();
        battleQueue.Clear();
        allMonsters.Clear();
        dotDmgObjList.Clear();
        deadMonsters.Clear();
        trapDic.Clear();
        worldItemList.Clear();
        

        SkillManager.Instance.supriseAttackList.Clear();
        SkillManager.Instance.shadowCrewList.Clear();
        SkillManager.Instance.bounceDestroyObjs.Clear();
        SkillManager.Instance.easterList.Clear();

        var darkHoleList = SkillManager.Instance.darkHoleList;
        for (int i = 0; i < darkHoleList.Count; ++i)
            Destroy(darkHoleList[i].obj.gameObject);
        darkHoleList.Clear();

        SkillManager.Instance.deadCount = 0;
        SkillManager.Instance.awayDawnCount = 0;
        SkillManager.Instance.homegroundDawnCount = 0;
        SkillManager.Instance.dawnTimer = 0f;
        SkillManager.Instance.checkSheercoldHomeground = false;
        SkillManager.Instance.sheerColdTime = 0f;
        SkillManager.Instance.sheerColdCount = 0;

    }

    private void UpdateGardners()
    {
        for(int i = 0; i < allMonsters.Count; ++i)
        {
            if (allMonsters[i].battleInstance.abilities.Contains(AbilityType.Gardner))
            {
                allMonsters[i].UpdateGardner();
            }
        }
    }
    private void UpdateCombat()
    {
        if (SkillManager.Instance.isBlock == false)
        {
            if(SkillManager.Instance.isBounceBlock == true)
            {
                SkillManager.Instance.PlayBounce();
            }
            else
            {
                //만약 부활이 있으면
                if (SkillManager.Instance.willPowerStructure.isStart == true)
                    SkillManager.Instance.PlayWillPower();
                else
                {
                    if (homegroundMonsters.Count <= 0 || awayMonsters.Count <= 0)
                    {
                        battleQueue.Clear();
                        return;
                    }

                    //같은 순서의 몬스터들끼리 정렬
                    if (battleQueue.Count > 1)
                    {
                        var list = battleQueue.ToList();

                        //운명의 동반자부터 정렬
                        var laterList = list.FindAll(x => x.passiveFlag == true);
                        var destinybondList = laterList.FindAll(x => (x.currentPassiveName == PassiveName.DestinyBond) && (x.battleInstance.hp <= 0));
                        laterList.RemoveAll(x => destinybondList.Contains(x));
                        list.RemoveAll(x => x.passiveFlag == true);

                        if (list.Count > 1)
                        {
                            list.Sort((a, b) =>
                            {
                                return a.battleInstance.originalPriority.CompareTo(b.battleInstance.originalPriority);
                            });

                            battleQueue.Clear();
                            battleQueue.AddRange(destinybondList);
                            battleQueue.AddRange(laterList);
                            battleQueue.AddRange(list);
                        }
                    }
                    if (battleQueue.Count > 0)
                    {
                        var entityObj = battleQueue.First.Value;
                        battleQueue.RemoveFirst();
                        if (entityObj == null)
                            return;
                        //반격기가 사용되면

                        bool check = false;
                        if (entityObj.passiveFlag == true)
                            check = entityObj.PlayPassiveSkill();

                        //리매이닝스턴 상태일때
                        bool check2 = false;
                        if(entityObj.sternStatus.CheckRemainningStern() == true)
                        {
                            float value = Random.Range(0f, 1f);
                            if (value <= entityObj.sternStatus.remaningPercent)
                            {
                                entityObj.StartCoroutine(entityObj.sternStatus.remaningRoutine);
                                check2 = true;
                            }
                            else
                            {
                                entityObj.sternStatus.Stop(entityObj);
                                check2 = false;
                            }
                        }

                        if (check == false && check2 == false)
                        {
                            //트리거 스킬이 한번 발생한다면 체크해준 후 삭제
                            SkillTrigger removeSkillTrigger;
                            var skillData = entityObj.GetSkillData(out removeSkillTrigger);
                            bool removeTriggerSkillCheck = CheckRemoveTrigger(removeSkillTrigger);
                            if (skillData != null && removeTriggerSkillCheck == true)
                            {
                                for(int i = 0; i < entityObj.battleInstance.triggerSkillDatas.Count; ++i)
                                {
                                    if((removeSkillTrigger == entityObj.battleInstance.triggerSkillDatas[i].Item1) && (skillData == entityObj.battleInstance.triggerSkillDatas[i].Item2))
                                    {
                                        entityObj.battleInstance.triggerSkillDatas.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                            SkillManager.Instance.Play(skillData.skillName, new BattleStructure(entityObj, skillData));
                        }
                    }
                    else
                    {
                        if (isEnd == false)
                        {
                            //경기시간
                            battleTimer -= Time.deltaTime;
                            if (battleTimer <= 0f)
                            {
                                isEnd = true;

                                UpdateOriginInstances(0);
                                VictoryUI.Instance.PopUp(0);
                                if (currentBattleType == BattleType.Official)
                                {
                                    LeagueManager.Instance.PlayerWinAndLose(0);
                                    LeagueManager.Instance.Report(0);
                                }
                                return;
                            }

                            //부활턴
                            var easterList = SkillManager.Instance.easterList;
                            bool check = false;
                            for(int i = 0; i < easterList.Count; ++i)
                            {
                                var easter = easterList[i];
                                if(easter.complete == false)
                                {
                                    if (easter.easterTimer >= 7f)
                                    {
                                        check = easter.complete = true;
                                    }
                                    else
                                        easter.easterTimer += Time.deltaTime;
                                }
                            }

                            if (check == true)
                                SkillManager.Instance.PlayEasterTurn(easterList);

                            //아이템 먹방턴
                            if(worldItemList.Count > 0)
                                SkillManager.Instance.PlayWorldItem();

                            //도트데미지턴
                            dotDmgObjList.RemoveAll(x => x == null || (x != null && x.isDead == true));
                            if (dotDmgObjList.Count > 0)
                            {
                                if (dotDmgTime > 3f)
                                {
                                    SkillManager.Instance.PlayDotDmgTurn();
                                    dotDmgTime = 0f;
                                }
                                else
                                    dotDmgTime += Time.deltaTime;
                            }
                            
                            //shaodwCrew턴
                            var shadowCrewList = SkillManager.Instance.shadowCrewList;
                            shadowCrewList.RemoveAll(x => x == null || (x != null && x.isDead == true));
                            if(shadowCrewList.Count > 0)
                            {
                                if (shaodwCrewTime > 5f)
                                {
                                    for(int i = 0; i < shadowCrewList.Count; ++i)
                                        shadowCrewList[i].CheckShaodwCrew(GetSortTarget(homegroundMonsters.Contains(shadowCrewList[i]), TargetLayer.Short)[0]);
                                    shaodwCrewTime = 0f;
                                }
                                else
                                    shaodwCrewTime += Time.deltaTime;
                            }

                            //rockblaster턴
                            var rocks = SkillManager.Instance.rockClones;
                            if(rocks.Count > 0)
                            {
                                if (rockBlasterTimer > 2f)
                                {
                                    var maxCount = Mathf.Min(7, rocks.Count);
                                    int random = Mathf.Min(maxCount, Random.Range(3, maxCount));
                                    for(int i = 0; i < random; ++i)
                                    {
                                        Destroy(rocks[i].gameObject);
                                    }
                                    rocks.RemoveRange(0, random);
                                    rockBlasterTimer = 0f;
                                }
                                else
                                    rockBlasterTimer += Time.deltaTime;
                            }

                            //리빙데드턴
                            check = true;
                            bool check2 = true;
                            for(int i = 0; i < homegroundMonsters.Count; ++i)
                            {
                                if (homegroundMonsters[i].battleInstance.hp > 0)
                                {
                                    check = false;
                                    break;
                                }
                            }
                            for (int i = 0; i < awayMonsters.Count; ++i)
                            {
                                if (awayMonsters[i].battleInstance.hp > 0)
                                {
                                    check2 = false;
                                    break;
                                }
                            }
                            if (check == true && check2 == true)
                                SkillManager.Instance.PlayLivingDeadTurn(allMonsters);
                            else if(check == true && check2 == false)
                                SkillManager.Instance.PlayLivingDeadTurn(homegroundMonsters);
                            else if(check == false && check2 == true)
                                SkillManager.Instance.PlayLivingDeadTurn(awayMonsters);

                            //Dawn턴
                            int homegroundDawnCount = SkillManager.Instance.homegroundDawnCount;
                            int awayDawnCount = SkillManager.Instance.awayDawnCount;
                            if(homegroundDawnCount > 0 || awayDawnCount > 0)
                            {
                                if (SkillManager.Instance.dawnTimer >= 2f)
                                {
                                    SkillManager.Instance.dawnTimer = 0f;
                                    SkillManager.Instance.PlayDawnTurn(homegroundDawnCount > 0);
                                }
                                else
                                    SkillManager.Instance.dawnTimer += Time.deltaTime;
                            }

                            //다크홀턴
                            var darkHoles = SkillManager.Instance.darkHoleList;
                            for(int i = 0; i < darkHoles.Count; ++i)
                            {
                                darkHoles[i].UpdateDarkHole();
                            }

                            //시어콜드턴
                            if(SkillManager.Instance.sheerColdCount > 0)
                            {
                                if(SkillManager.Instance.sheerColdTime <= 0f)
                                    SkillManager.Instance.SheerColdTurn();

                                SkillManager.Instance.sheerColdTime -= Time.deltaTime;
                            }
                        }
                    }
                }
            }
        }
    }
    private void UpdateOriginInstances(int winAndLose)
    {
        for(int i = 0; i < playerInstances.Count; ++i)
        {
            playerInstances[i].Item1.UpdateOrigin(playerInstances[i].Item2, winAndLose);
        }

        playerInstances.Clear();
    }
}
