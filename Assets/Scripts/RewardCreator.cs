#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using System;
using System.Data;

public enum ServerRewardType { Cost, Consume, Monster }

[System.Serializable]
public class RewardServerData
{
    public ServerRewardType serverRewardType;

    [ConditionalHideOfEnum("serverRewardType", true, "Monster")]
    public MonsterData monsterData;

    [ConditionalHideOfEnum("serverRewardType", true, "Consume")]
    public ItemElementalData itemData;

    [ConditionalHideOfEnum("serverRewardType", true, "Cost")]
    public CostItemType costData;
    public int count;
}

public class RewardCreator : MonoBehaviour
{
   
}
#endif
