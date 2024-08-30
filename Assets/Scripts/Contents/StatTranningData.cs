using UnityEngine;

[System.Serializable]
public class StatTranningData
{
    public float buffValue;

    [Range(0f, 1f)]
    public float goalPercent;

    public float addGoalPercent;

    public int level;

    public CostItemType costItemType;
    public int cost;
    public int addCost;
    public float minGoalPercent;
}
