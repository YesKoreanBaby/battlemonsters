using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterIndigrident
{
    public MonsterData monsterData;
    public int count = 1;
}

[Serializable]
public class CostItemIndigrident
{
    public CostItemType itemType;
    public int count = 1;
}

[Serializable]
public class ConsumeItemIndigrident
{
    public  ItemElementalData consumeItem;
    public int count = 1;
}

[CreateAssetMenu(fileName = "CombineData", menuName = "Data/CombineData")]
public class CombineData : ScriptableObject
{
    public MonsterData combineMonster;
    public ItemElementalData combineItem;
    public List<MonsterIndigrident> monsterDatas;
    public List<CostItemIndigrident> costItemDatas;
    public List<ConsumeItemIndigrident> consumeItemDatas;
    public bool onlyOne = false;


    public bool CheckExistMonster()
    {
        if(onlyOne == false)
            return true;
        else
        {
            var data = Array.Find(TranningUI.Instance.playerInventory.monsterDatas, x => x != null &&  (x.monsterData == combineMonster || (x.previousMonsterData != null && x.previousMonsterData.monsterData == combineMonster)));
            if (data != null)
                return false;
            else
                return true;
        }
    }

    public bool CheckExistItem()
    {
        if (onlyOne == false)
            return true;
        else
        {
            var data = Array.Find(ItemInventory.Instance.consumeItemDatas, x => x != null && x == combineItem);
            if (data != null)
                return false;
            else
                return true;
        }
    }
    public bool CheckActive()
    {
        bool check = false;

        if (monsterDatas != null)
        {
            for (int i = 0; i < monsterDatas.Count; ++i)
            {
                check = CheckActiveMonster(monsterDatas[i]);
                if (check == false)
                    goto jump;
            }
        }
        else
            check = true;

        if (costItemDatas != null)
        {
            for (int i = 0; i < costItemDatas.Count; ++i)
            {
                check = CheckActiveCostItem(costItemDatas[i]);
                if (check == false)
                    goto jump;
            }
        }
        else
            check = true;

        if (consumeItemDatas != null)
        {
            for (int i = 0; i < consumeItemDatas.Count; ++i)
            {
                check = CheckActiveConsumeItem(consumeItemDatas[i]);
                if (check == false)
                    goto jump;
            }
        }
        else
            check = true;
jump:
        return check;
    }
    public List<object> GetDatas()
    {
        List<object> objs = new List<object>();
        objs.AddRange(monsterDatas);
        objs.AddRange(costItemDatas);
        objs.AddRange(consumeItemDatas);

        return objs;
    }
    public bool CheckActiveMonster(MonsterIndigrident indigridentData)
    {
        var inven = TranningUI.Instance.playerInventory.monsterDatas;
        bool check = true;
        var monsterDatas = Array.FindAll(inven, x => (x != null) && x.monsterData.monsterPrefab == indigridentData.monsterData.monsterPrefab);
        check = (monsterDatas != null) && (monsterDatas.Length >= indigridentData.count);

        return check;
    }
    public bool CheckActiveCostItem(CostItemIndigrident indigridentData)
    {
        int count = ItemInventory.Instance.GetData(indigridentData.itemType);
        bool check = count >= indigridentData.count;

        return check;
    }
    public bool CheckActiveConsumeItem(ConsumeItemIndigrident indigridentData)
    {
        if (consumeItemDatas != null)
        {
            var datas = ItemInventory.Instance.consumeItemDatas;
            var itemDatas = Array.FindAll(datas, x => x == indigridentData.consumeItem);
            bool check = (itemDatas != null) && (itemDatas.Length >= indigridentData.count);

            return check;
        }
        else
            return true;
    }
}
