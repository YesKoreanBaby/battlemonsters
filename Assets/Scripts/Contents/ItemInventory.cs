using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CostItemType { Money, Diamond, Fame, Ranking}  
public class ItemInventory : MonoBehaviour
{
    private static ItemInventory instance = null;
    public static ItemInventory Instance { get { return instance; } }
    public Sprite moneySprite;
    public Sprite diamondSprite;
    public Sprite fameSprite;
    public Sprite rankingSprite;

    public ItemElementalData[] consumeItemDatas { get; private set; }
    public ItemElementalData[] treasureDatas { get; private set; }

    public List<object> rulletItemDatas { get; private set; }

    public int money
    {
        get { return moneyValue; } set { moneyValue = Mathf.Clamp(value, 0, 99999); }
    }

    private int moneyValue;

    [System.NonSerialized]
    public int saveMoney;

    [System.NonSerialized]
    public int friendShipBattleCount;

    [System.NonSerialized]
    public int friendShipBattleWin;

    public int diamond
    {
        get { return diamondValue; }
        set { diamondValue = Mathf.Clamp(value, 0, 99999); }
    }

    private int diamondValue;

    public int fame
    {
        get { return fameValue; }
        set { fameValue = Mathf.Clamp(value, 0, 99999); }
    }

    private int fameValue;

    public int ranking
    {
        get { return rankingValue; }
        set { rankingValue = Mathf.Clamp(value, 0, 99999); }
    }

    private int rankingValue;

    public int startValue { get; private set; }
    public int endValue { get; private set; }

    public CostItemType currentCostItemType { get; private set; }

    private void Awake()
    {
        instance = this;
        consumeItemDatas = new ItemElementalData[30];
        treasureDatas = new ItemElementalData[9];
        rulletItemDatas = new List<object>();
    }
    public void SetCount(int money, int diamond)
    {
        this.money = money;
        startValue = endValue = money;
        this.diamond = diamond;
    }
    public int FindConsumeItemEmptyIndex()
    {
        for(int i = 0; i < consumeItemDatas.Length; ++i)
        {
            if (consumeItemDatas[i] == null)
                return i;
        }

        return -1;
    }
    public void AddConsumeItem(ItemElementalData data)
    {
        int emptyIndex = FindConsumeItemEmptyIndex();
        if(emptyIndex != -1)
        {
            consumeItemDatas[emptyIndex] = data;
        }
    }
    public void RemoveConsumItem(int index)
    {
        consumeItemDatas[index] = null;
    }
    public int FindTreasureItemEmptyIndex()
    {
        for (int i = 0; i < treasureDatas.Length; ++i)
        {
            if (treasureDatas[i] == null)
                return i;
        }

        return -1;
    }
    public void AddTreasureItem(ItemElementalData data)
    {
        int emptyIndex = FindTreasureItemEmptyIndex();
        if (emptyIndex != -1)
        {
            treasureDatas[emptyIndex] = data;
        }
    }
    public bool ContainsTreasureItem(ItemElementalData data)
    {
        var item = Array.Find(treasureDatas, x => x == data);
        return item != null;
    }
    public bool CheckLimit(int value, CostItemType costType)
    {
        if (costType == CostItemType.Money)
        {
            if (money > 0 && value <= money)
            {
                startValue = money;
                money -= value;
                endValue = money;
                return true;
            }
            else
                return false;
        }
        else if (costType == CostItemType.Diamond)
        {
            if (diamond > 0 && value <= diamond)
            {
                startValue = money;
                diamond -= value;
                endValue = money;
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
    public string GetSplitMoneyText()
    {
        string value = "ABCDE";
        int a = money;

        int b = a / 10000;
        a = Mathf.Max(0, a - (b * 10000));
        value = value.Replace("A", b.ToString());

        b = a / 1000;
        a = Mathf.Max(0, a - (b * 1000));
        value = value.Replace("B", b.ToString());

        b = a / 100;
        a = Mathf.Max(0, a - (b * 100));
        value = value.Replace("C", b.ToString());

        b = a / 10;
        a = Mathf.Max(0, a - (b * 10));
        value = value.Replace("D", b.ToString());

        b = a / 1;
        value = value.Replace("E", b.ToString());

        return value;
    }

    public Sprite GetSprite(CostItemType costType)
    {
        if (costType == CostItemType.Money)
        {
            return moneySprite;
        }
        else if (costType == CostItemType.Diamond)
        {
            return diamondSprite;
        }
        else if(costType == CostItemType.Fame)
        {
            return fameSprite;
        }
        else if(costType == CostItemType.Ranking)
        {
            return rankingSprite;
        }
        else
            return null;
    }

    public int GetData(CostItemType costType)
    {
        if (costType == CostItemType.Money)
        {
            return money;
        }
        else if (costType == CostItemType.Diamond)
        {
            return diamond;
        }
        else if(costType == CostItemType.Fame)
        {
            return fame;
        }
        else if(costType == CostItemType.Ranking)
        {
            return ranking;
        }
        else
            return -1;
    }

    public void PlayDecrease(TextMeshProUGUI text, CostItemType costType)
    {
        if (costType != currentCostItemType)
        {
            currentCostItemType = costType;
            int value = GetData(costType);
            startValue = endValue = value;
            Decrease(text, value);
        }
        else
        {
            int value = GetData(costType);
            if (endValue != value)
            {
                startValue = endValue = value;
                Decrease(text, value);
            }
            else
                StartCoroutine(DecreaseRoutine(text));
        }
    }

    private void Decrease(TextMeshProUGUI text, int value)
    {
        text.text = $"x{value}";
    }
    private IEnumerator DecreaseRoutine(TextMeshProUGUI text)
    {
        int startValue = this.startValue;
        int endValue = this.endValue;
        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            int value = Mathf.FloorToInt(Mathf.Lerp(startValue, endValue, currentSpeed));
            text.text = $"x{value}";
            yield return null;
        }
    }
}
