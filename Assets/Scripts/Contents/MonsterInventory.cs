using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInventory : MonoBehaviour
{
    public MonsterInstance[] monsterDatas = new MonsterInstance[12];
    public void AddItem(MonsterData data)
    {
        int index = FindEmptyIndex();
        if(index != -1)
            monsterDatas[index] = MonsterInstance.Instance(data);
    }

    public void AddItem(MonsterInstance data)
    {
        int index = FindEmptyIndex();
        if (index != -1)
            monsterDatas[index] = data;
    }

    public void AddItems(List<MonsterData> datas)
    {
        for (int i = 0; i < datas.Count; ++i)
        {
            monsterDatas[i] = MonsterInstance.Instance(datas[i]);
        }
    }
    public int FindEmptyIndex()
    {
        int index = Array.FindIndex(monsterDatas, x => (x == null));
        return index;
    }
    public int FindIndex(MonsterInstance data)
    {
        int index = Array.FindIndex(monsterDatas, x => (x == data));
        return index;
    }

    public MonsterInstance FindItem(int index)
    {
        var data = Array.Find(monsterDatas, x => (x == monsterDatas[index]));
        return data;
    }

    public void RemoveItem(int index)
    {
        var data = FindItem(index);
        if(data != null)
        {
            if(index != monsterDatas.Length - 1)
            {
                monsterDatas[index] = null;
                for (int i = index + 1; i < monsterDatas.Length; ++i)
                {
                    monsterDatas[i - 1] = monsterDatas[i];
                    monsterDatas[i] = null;
                }
            }
            else
                monsterDatas[index] = null;
        }
    }

    public void Clear()
    {
        for(int i = 0; i < monsterDatas.Length; ++i)
            monsterDatas[i] = null;
    }
}
