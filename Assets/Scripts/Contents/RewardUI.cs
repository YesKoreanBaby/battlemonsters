using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct RewardData
{
    public ItemElementalData itemData;
    public MonsterInstance monsterInstance;
    public CostItemType itemType;
    public int count;
    public bool isTreagure;
}
public class RewardUI : MonoBehaviour
{
    public Animator rewardEffect;
    public Image itemImage;
    public TextMeshProUGUI itemCountText;
    public RectTransform skipButton;
    public Button infoButton;

    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public Action endEvt;

    [System.NonSerialized]
    public List<RewardData> rewardQueue = new List<RewardData>();

    private bool checkEventFlag = false;

    private static RewardUI instance = null;
    public static RewardUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        Init();
    }
    public void AddCostItem(CostItemType type, int count)
    {
        int findIndex = rewardQueue.FindIndex(x => (x.itemData == null) && (x.itemType == type));
        if(findIndex == -1)
        {
            var data = new RewardData();
            data.itemData = null;
            data.itemType = type;
            data.count = count;
            rewardQueue.Add(data);
        }
        else
        {
            var data = rewardQueue[findIndex];
            data.count += count;
            rewardQueue[findIndex] = data;
        }
    }
    public void AddConsumeItem(ItemElementalData itemData, int count)
    {
        int findIndex = rewardQueue.FindIndex(x => x.itemData == itemData);
        if (findIndex == -1)
        {
            var data = new RewardData();
            data.itemData = itemData;
            data.count = count;
            data.isTreagure = false;
            rewardQueue.Add(data);
        }
        else
        {
            var data = rewardQueue[findIndex];
            data.count += count;
            data.isTreagure = false;
            rewardQueue[findIndex] = data;
        }
    }
    public void AddTreagureItem(ItemElementalData itemData)
    {
        var data = new RewardData();
        data.itemData = itemData;
        data.count = 1;
        data.isTreagure = true;
        rewardQueue.Add(data);
    }
    public void AddMonster(MonsterInstance monsterInstance)
    {
        var data = new RewardData();
        data.monsterInstance = monsterInstance;
        data.count = 1;
        rewardQueue.Add(data);
    }
    public void PopUp()
    {
        if(rewardQueue.Count > 0)
        {
            CombatUI.Instance.animator.Play("dontactive");
           

            var data = rewardQueue[0];
            rewardQueue.RemoveAt(0);

            if (isPopUp == true)
                Closed();

            if(data.monsterInstance != null)
            {
                PopUpMonster(data.monsterInstance);
            }
            else
            {
                if (data.itemData != null)
                {
                    if (data.isTreagure == true)
                        PopUpTreagureItem(data.itemData);
                    else
                        PopUpItem(data.itemData, data.count);
                }
                else
                {
                    if (data.itemType == CostItemType.Money)
                        PopUpMoney(data.count);
                    else if (data.itemType == CostItemType.Diamond)
                        PopUpDiamond(data.count);
                    else if (data.itemType == CostItemType.Ranking)
                        PopUpRanking(data.count);
                    else if (data.itemType == CostItemType.Fame)
                        PopUpFame(data.count);
                }
            }
        }
        else
        {
            if (isPopUp == true)
            {
                if(endEvt == null)
                {
                    Closed();
                    MainSystemUI.Instance.PopUp();
                    CombatUI.Instance.animator.Play("closedidle");
                }
                else
                {
                    endEvt.Invoke();
                    endEvt = null;
                }
            }
        }
    }
    public void PopUpMoney(int count, Action evt = null)
    {
        if (isPopUp == false)
        {
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            infoButton.enabled = false;
            rewardEffect.Play("stay");
            itemImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Money);
            itemImage.rectTransform.sizeDelta = new Vector2(96f, 96f);
            itemCountText.text = $"<b><size=42>x<b><size=56>{count}";
            endEvt = evt;

            ItemInventory.Instance.money += count;

            CombatUI.Instance.animator.Play("dontactive");
        }
    }
    public void Skip()
    {
        rewardEffect.Play("end");
        skipButton.gameObject.SetActive(false);
        SoundManager.Instance.StopAllEffect(182);
        SoundManager.Instance.PlayEffect(184, 0.8f);
    }
    public void PopUpDiamond(int count, Action evt = null)
    {
        if (isPopUp == false)
        {
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            infoButton.enabled = false;
            rewardEffect.Play("stay");
            itemImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Diamond);
            itemImage.rectTransform.sizeDelta = new Vector2(96f, 96f);
            itemCountText.text = $"<b><size=42>x<b><size=56>{count}";
            endEvt = evt;

            ItemInventory.Instance.diamond += count;

            CombatUI.Instance.animator.Play("dontactive");
        }
    }
    public void PopUpFame(int count, Action evt = null)
    {
        if (isPopUp == false)
        {
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            infoButton.enabled = false;
            rewardEffect.Play("stay");
            itemImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Fame);
            itemImage.rectTransform.sizeDelta = new Vector2(96f, 96f);
            itemCountText.text = $"<b><size=42>x<b><size=56>{count}";
            endEvt = evt;

            ItemInventory.Instance.fame += count;

            CombatUI.Instance.animator.Play("dontactive");
        }
    }
    public void PopUpMonster(MonsterInstance monsterData, Action evt = null)
    {
        if (isPopUp == false)
        {
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            rewardEffect.Play("stay");
            itemImage.sprite = monsterData.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

            bool check = (monsterData.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (monsterData.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (monsterData.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
            float value = (monsterData.monsterWeight != MonsterWeight.Big) ? 75f : 50f;
            if (check == true)
                value = 75f;
            itemImage.SetNativeSize();
            itemImage.rectTransform.sizeDelta = new Vector2(Mathf.Min(100f, itemImage.rectTransform.sizeDelta.x * value), Mathf.Min(100f, itemImage.rectTransform.sizeDelta.y * value));

            itemCountText.text = $"<b><size=42>x<b><size=56>{1}";

            TranningUI.Instance.playerInventory.AddItem(monsterData);
          
            monsterData.currentConfirmSkillPriority = monsterData.confirmSkillPrioritys[0];
            monsterData.currentSelectDetailTargetType = monsterData.selectDetailTargetTypes[0];

            infoButton.enabled = true;
            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(() => { StatUI.Instance.PopUp(monsterData, true); });

            endEvt = evt;

            CombatUI.Instance.animator.Play("dontactive");
        }
    }
    private void PopUpTreagureItem(ItemElementalData itemData)
    {
        if (isPopUp == false)
        {
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            rewardEffect.Play("stay");
            itemImage.sprite = itemData.itemImage;
            itemImage.SetNativeSize();
            itemImage.rectTransform.sizeDelta *= 45f;

            itemCountText.text = $"<b><size=42>x<b><size=56>{1}";

            if (!ItemInventory.Instance.ContainsTreasureItem(itemData))
                ItemInventory.Instance.AddTreasureItem(itemData);

            infoButton.enabled = false;
        }
    }
    private void PopUpRanking(int count)
    {
        if (isPopUp == false)
        {
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            rewardEffect.Play("stay");
            itemImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Ranking);
            itemImage.rectTransform.sizeDelta = new Vector2(96f, 96f);
            itemCountText.text = $"<b><size=42>x<b><size=56>{count}";

            ItemInventory.Instance.ranking += count;

         

            infoButton.enabled = false;
        }
    }
    public void PopUpItem(ItemElementalData itemData, int count, Action evt = null)
    {
        if (isPopUp == false)
        {
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            rewardEffect.Play("stay");
            itemImage.sprite = itemData.itemImage;
            itemImage.SetNativeSize();
            itemImage.rectTransform.sizeDelta *= 50f;   
            itemCountText.text = $"<b><size=42>x<b><size=56>{count}";

            for(int i = 0; i < count; ++i)
                ItemInventory.Instance.AddConsumeItem(itemData);

            infoButton.enabled = true;
            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(() => { ItemInventoryUI.Instance.PopUp(itemData); });

            endEvt = evt;

            CombatUI.Instance.animator.Play("dontactive");
        }
    }
    public void Closed()
    {
        isPopUp = false;
        rewardEffect.Play("idle");
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (CombatUI.Instance.gameObject.activeInHierarchy == false)
            CombatUI.Instance.gameObject.SetActive(true);
    }
    private void Init()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
