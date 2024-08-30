using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct BettingData
{
    public MonsterInstance homeground;
    public MonsterInstance away;
    public float homegroundPercent;
    public float tiePercent;
    public float awayPercent;
}
public class BatUI : MonoBehaviour
{
    public Image[] icons;
    public Image[] itemSlots;
    public TextMeshProUGUI[] moneys;
    public Button nextButton;
    public Button prevButton;

    public RectTransform view;
    public RectTransform fixedView;
    public bool isPopUp { get; private set; }

    public float homegroundPercent { get; private set; }
    public float awayPercent { get; private set; }
    public float tiePercent { get; private set; }
    public int homegroundMoney { get; private set; }
    public int awayMoney { get; private set; }
    public int tieMoney { get; private set; }
    public int batIndex { get; private set; }
    public int currentItemIndex { get; private set; }
    public int maxItemIndex { get; private set; }
    public int indexCount { get; private set; }

    public List<object> objs { get; private set; }
    public MonsterInstance homegroundMonster { get; private set; }

    public MonsterInstance awayMonster { get; private set; }


    private static BatUI instance = null;
    public static BatUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        Init();
    }
    public void PopUp(BettingData bettingData)
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }

            view.gameObject.SetActive(true);
            fixedView.gameObject.SetActive(false);

            SettingDatas(bettingData);
        }
    }

    public void PopUp(List<object> objs)
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);
            isPopUp = true;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }

            view.gameObject.SetActive(false);
            fixedView.gameObject.SetActive(true);
            this.objs = objs;
            SettingDatas(objs);
        }
    }
    public void Closed()
    {
        if (isPopUp == true)
        {
            isPopUp = false;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(false);
            }
        }
    }

    public void ClearData()
    {
        awayMonster = homegroundMonster = null;
        homegroundPercent = awayPercent = tiePercent = 0;
        batIndex = -1;
    }

    public void BatHomeground()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        batIndex = 0;
        CombatManager.Instance.StartGamebling();
    }
    public void BatTie()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        batIndex = 1;
        CombatManager.Instance.StartGamebling();
    }

    public void BatAway()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        batIndex = 2;
        CombatManager.Instance.StartGamebling();
    }

    public int GetData()
    {
        if (batIndex == 0)
            return homegroundMoney;
        else if (batIndex == 1)
            return tieMoney;
        else if (batIndex == 2)
            return awayMoney;
        else
            return -1;
    }

    public void IncreaseItemSlot()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        currentItemIndex++;
        SettingFixedView(objs);
    }
    public void DecreaseItemSlot()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        currentItemIndex --;
        SettingFixedView(objs);
    }

    private void Init()
    {
        isPopUp = false;
        var childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(false);
        }
    }

    private void SettingDatas(BettingData bettingData)
    {
        for(int i = 0; i < icons.Length; ++i)
        {
            if(i == 0)
                icons[i].sprite = bettingData.homeground.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
            else if(i == 1)
                icons[i].sprite = bettingData.away.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

            bool check = (bettingData.homeground.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (bettingData.homeground.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (bettingData.homeground.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
            float value = (bettingData.homeground.monsterWeight != MonsterWeight.Big) ? 4f : 2f;
            if (check == true)
                value = 4f;
            icons[i].SetNativeSize();
            icons[i].rectTransform.sizeDelta = new Vector2(Mathf.Min(64f, icons[i].rectTransform.sizeDelta.x * value), Mathf.Min(64f, icons[i].rectTransform.sizeDelta.y * value));
        }

        moneys[0].color = (bettingData.homegroundPercent <= 0.25f) ? Color.red : (bettingData.homegroundPercent <= 0.75f) ? Color.black : Color.blue;
        moneys[1].color = (bettingData.awayPercent <= 0.25f) ? Color.red : (bettingData.awayPercent <= 0.75f) ? Color.black : Color.blue;
        moneys[2].color = (bettingData.tiePercent <= 0.25f) ? Color.red : (bettingData.tiePercent <= 0.75f) ? Color.black : Color.blue;

        homegroundMonster = bettingData.homeground;
        awayMonster = bettingData.away;
        this.homegroundPercent = bettingData.homegroundPercent;
        this.awayPercent = bettingData.awayPercent;
        this.tiePercent = bettingData.tiePercent;

        float homegroundCaculation = (homegroundPercent <= 0.25f) ? 0.25f : (homegroundPercent <= 0.75f) ? 0.5f : 1f;
        float awayCaculation = (awayPercent <= 0.25f) ? 0.25f : (awayPercent <= 0.75f) ? 0.5f : 1f;
        float tieCaculation = (tiePercent <= 0.25f) ? 0.25f : (tiePercent <= 0.75f) ? 0.5f : 1f;
        homegroundMoney = Mathf.FloorToInt((bettingData.homegroundPercent * CombatUI.Instance.batMoney) * homegroundCaculation);
        awayMoney = Mathf.FloorToInt((bettingData.awayPercent * CombatUI.Instance.batMoney) * awayCaculation);
        tieMoney = Mathf.FloorToInt((bettingData.tiePercent * CombatUI.Instance.batMoney) * tieCaculation);
       
        moneys[0].text = $"<b><size=18>$<b><size=24>{homegroundMoney}";
        moneys[1].text = $"<b><size=18>$<b><size=24>{awayMoney}";
        moneys[2].text = $"<b><size=18>$<b><size=24>{tieMoney}";
    }

    private void SettingDatas(List<object> objs)
    {
        currentItemIndex = 0;
        maxItemIndex = (objs.Count / 3);
        if (maxItemIndex % 3 > 0)
            maxItemIndex++;
        SettingFixedView(objs);
    }

    private void SettingFixedView(List<object> objs)
    {
        prevButton.interactable = currentItemIndex >= 1;
        nextButton.interactable = (maxItemIndex > 1 && ((currentItemIndex * 3) + 3) < objs.Count);

        for (int i = 0; i < itemSlots.Length; ++i)
        {
            int index = (currentItemIndex * 3) + i;
            if (index < objs.Count)
            {
                itemSlots[i].gameObject.SetActive(true);

                if (objs[index] is CostItemIndigrident)
                {
                    var data = objs[index] as CostItemIndigrident;
                    itemSlots[i].sprite = ItemInventory.Instance.GetSprite(data.itemType);
                    itemSlots[i].SetNativeSize();
                    itemSlots[i].rectTransform.sizeDelta *= 5.25f;
                }
                else if (objs[index] is ItemElementalData)
                {
                    var data = objs[index] as ItemElementalData;
                    if(data.effectType == ItemElementalEffectType.None)
                    {
                        itemSlots[i].sprite = data.itemImage;
                        itemSlots[i].SetNativeSize();
                        itemSlots[i].rectTransform.sizeDelta *= 2.25f;
                    }
                    else
                    {
                        itemSlots[i].sprite = data.itemImage;
                        itemSlots[i].SetNativeSize();
                        itemSlots[i].rectTransform.sizeDelta *= 3.5f;
                    }
                }
                else if (objs[index] is MonsterData)
                {
                    var data = objs[index] as MonsterData;
                    itemSlots[i].sprite = data.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

                    bool check = (data.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (data.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (data.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
                    float value = (data.monsterWeight != MonsterWeight.Big) ? 4f : 2f;
                    if (check == true)
                        value = 4f;
                    itemSlots[i].SetNativeSize();
                    itemSlots[i].rectTransform.sizeDelta = new Vector2(Mathf.Min(64f, itemSlots[i].rectTransform.sizeDelta.x * value), Mathf.Min(64f, itemSlots[i].rectTransform.sizeDelta.y * value));
                }
            }
            else
            {
                itemSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
