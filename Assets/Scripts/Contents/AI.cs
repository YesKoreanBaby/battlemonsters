using System.Collections.Generic;
using UnityEngine;

public enum AITranningLevel { Default, Easy, Normal, Hard, VeryHard }
[System.Serializable]
public class AIMonsterData
{
    public MonsterData monsterData;
    //   public int tranningLevel;
    public AITranningLevel tranningLevel;
    public TranningType tranningType;
    public SelectDetailTargetType selectDetailTargetType;
    public ConfirmSkillPriority confirmSkillPriority;
    public bool checkFullSkill;

    public MonsterInstance Instance()
    {
        if (tranningType == TranningType.Random)
            tranningType = (TranningType)(Random.Range(1, (int)TranningType.End));
        var instance = MonsterInstance.Instance(monsterData, true, tranningType);

        instance.tranningCicleInstance.TranningAI(tranningType, tranningLevel, checkFullSkill, confirmSkillPriority, selectDetailTargetType);
        return instance;
    }
}

[System.Serializable]
public class BattleData
{
    public List<SerializableDictionary<Vector2Int, AIMonsterData>> monsterDataDic;
    public int monsterCount;
    public FieldType fieldType;
    public BattleType battleType;
    public ConversationData conversationData;
}
public class AI : MonoBehaviour
{
    public BattleData friendshipData;
    public BattleData singleGameblingData;
    public BattleData tutorialData;
    public BattleData pvpData;
    //public BattleData testData;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            while(true)
            {

            }
        }
    }
}
