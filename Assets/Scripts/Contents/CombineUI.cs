using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct CombineSuccess
{
    public MonsterInstance monsterInstance;
    public ItemElementalData itemData;
    public CostItemType constType;
    public int type;
    public int count;
    public int index;
}
public class CombineUI : MonoBehaviour
{
    public bool isPopUp { get; private set; }

    public RectTransform[] combineViews;
    public Button[] descButtons;

    private static CombineUI instance = null;
    public static CombineUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        Init();
    }
    public void PopUp()
    {
        if(isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }

            SettingData();

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false)
            //    AdmobManager.Instance.DestroyBannerAds();
        }
    }

    public void DescButtonUpdate(List<CombineData> combineDatas)
    {
        for(int i = 0; i < descButtons.Length; ++i)
        {
            descButtons[i].interactable = false;
        }
        for(int i = 0; i < combineDatas.Count; ++i)
        {
            bool check = combineDatas[i].combineMonster != null ? combineDatas[i].CheckExistMonster() : combineDatas[i].CheckExistItem();
            if (check == true)
            {
                descButtons[i].interactable = true;

                if (combineDatas[i].combineMonster != null)
                {
                    int index = i;
                    descButtons[index].onClick.RemoveAllListeners();
                    descButtons[index].onClick.AddListener(() => { SkillUI.Instance.PopUp(combineDatas[index].combineMonster); });
                }
                else
                {
                    if (combineDatas[i].combineItem != null)
                    {
                        int index = i;
                        descButtons[index].onClick.RemoveAllListeners();
                        descButtons[index].onClick.AddListener(() => { ItemInventoryUI.Instance.PopUp(combineDatas[index].combineItem); });
                    }
                }
            }
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

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
            //    AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);
        }
    }
    public void MakeMonster(int index, Animator effect)
    {
        List<CombineSuccess> successList = new List<CombineSuccess>();

        CombineData data = SelectTeamUI.Instance.selectTeam.startCombineDatas[index];
        var monsterInstances = TranningUI.Instance.playerInventory;
        var itemInventorys = ItemInventory.Instance.consumeItemDatas;
        var objs = data.GetDatas();
        for(int i = 0; i < objs.Count; ++i)
        {
            MonsterIndigrident monsterData = objs[i] as MonsterIndigrident;
            CostItemIndigrident costData = objs[i] as CostItemIndigrident;
            ConsumeItemIndigrident consumeData = objs[i] as ConsumeItemIndigrident;

            if(monsterData != null)
            {
                int count = monsterData.count;

                for(int j = 0; j < count; ++j)
                {
                    int findIndex = Array.FindIndex(monsterInstances.monsterDatas, x => (x != null) && (x.monsterData.monsterPrefab == monsterData.monsterData.monsterPrefab));
                   
                    CombineSuccess success = new CombineSuccess();
                    success.type = 0;
                    success.monsterInstance = monsterInstances.monsterDatas[findIndex];
                    successList.Add(success);

                    monsterInstances.monsterDatas[findIndex] = null;
                }
            }
            else
            {
                if(costData != null)
                {
                    if(costData.itemType == CostItemType.Diamond || costData.itemType == CostItemType.Money)
                    {
                        CombineSuccess success = new CombineSuccess();
                        success.type = 1;
                        success.constType = costData.itemType;
                        success.count = costData.count;
                        successList.Add(success);

                        if (costData.itemType == CostItemType.Diamond)
                            ItemInventory.Instance.diamond -= costData.count;
                        else if(costData.itemType == CostItemType.Money)
                            ItemInventory.Instance.money -= costData.count;
                    }
                }
                else
                {
                    if(consumeData != null)
                    {
                        int count = consumeData.count;

                        for (int j = 0; j < count; ++j)
                        {
                            int findIndex = Array.FindIndex(itemInventorys, x => (x != null) && (x == consumeData.consumeItem));

                            CombineSuccess success = new CombineSuccess();
                            success.type = 2;
                            success.itemData = itemInventorys[findIndex];
                            successList.Add(success);

                            itemInventorys[findIndex] = null;
                        }
                    }
                }
            }
        }

        int monsterIndex = monsterInstances.FindEmptyIndex();
        if(monsterIndex == -1)
        {
            AlarmUI.Instance.PopUpForKey("fullmonster", null);

            for (int j = 0; j < successList.Count; ++j)
            {
                if (successList[j].type == 0)
                {
                    int emptyIndex = monsterInstances.FindEmptyIndex();
                    monsterInstances.monsterDatas[emptyIndex] = successList[j].monsterInstance;
                }
                else
                {
                    if (successList[j].type == 1)
                    {
                        if (successList[j].constType == CostItemType.Money)
                            ItemInventory.Instance.money += successList[j].count;
                        else if (successList[j].constType == CostItemType.Diamond)
                            ItemInventory.Instance.diamond += successList[j].count;
                    }
                    else
                    {
                        if (successList[j].type == 2)
                        {
                            if (successList[j].constType == CostItemType.Money)
                                ItemInventory.Instance.money += successList[j].count;
                            else if (successList[j].constType == CostItemType.Diamond)
                                ItemInventory.Instance.diamond += successList[j].count;
                        }
                    }
                }
            }

            CombatUI.Instance.UpdateMonsterImages();
            return;
        }

        int combineIndex = monsterInstances.FindEmptyIndex();
        monsterInstances.monsterDatas[combineIndex] = MonsterInstance.Instance(data.combineMonster);
        CombatUI.Instance.UpdateMonsterImages();
        DialougeUI.Instance.SettingFixedView(true);
        StartCoroutine(EffectRoutine(effect));

        SoundManager.Instance.PlayEffect(186, 1f);
    }
    public void MakeItem(int index, Animator effect)
    {
        List<CombineSuccess> successList = new List<CombineSuccess>();

        CombineData data = SelectTeamUI.Instance.selectTeam.startCombineDatas[index];
        var monsterInstances = TranningUI.Instance.playerInventory;
        var itemInventorys = ItemInventory.Instance.consumeItemDatas;
        var objs = data.GetDatas();

        for (int i = 0; i < objs.Count; ++i)
        {
            MonsterIndigrident monsterData = objs[i] as MonsterIndigrident;
            CostItemIndigrident costData = objs[i] as CostItemIndigrident;
            ConsumeItemIndigrident consumeData = objs[i] as ConsumeItemIndigrident;

            if (monsterData != null)
            {
                int count = monsterData.count;

                for (int j = 0; j < count; ++j)
                {
                    int findIndex = Array.FindIndex(monsterInstances.monsterDatas, x => (x != null) && (x.monsterData.monsterPrefab == monsterData.monsterData.monsterPrefab));

                    CombineSuccess success = new CombineSuccess();
                    success.type = 0;
                    success.monsterInstance = monsterInstances.monsterDatas[findIndex];
                    successList.Add(success);

                    monsterInstances.monsterDatas[findIndex] = null;
                }
            }
            else
            {
                if (costData != null)
                {
                    if (costData.itemType == CostItemType.Diamond || costData.itemType == CostItemType.Money)
                    {
                        CombineSuccess success = new CombineSuccess();
                        success.type = 1;
                        success.constType = costData.itemType;
                        success.count = costData.count;
                        successList.Add(success);

                        if (costData.itemType == CostItemType.Diamond)
                            ItemInventory.Instance.diamond -= costData.count;
                        else if (costData.itemType == CostItemType.Money)
                            ItemInventory.Instance.money -= costData.count;
                    }
                }
                else
                {
                    if (consumeData != null)
                    {
                        int count = consumeData.count;

                        for (int j = 0; j < count; ++j)
                        {
                            int findIndex = Array.FindIndex(itemInventorys, x => (x != null) && (x == consumeData.consumeItem));

                            CombineSuccess success = new CombineSuccess();
                            success.type = 2;
                            success.itemData = itemInventorys[findIndex];
                            successList.Add(success);

                            itemInventorys[findIndex] = null;
                        }
                    }
                }
            }
        }

        int itemIndex = ItemInventory.Instance.FindConsumeItemEmptyIndex();
        if (itemIndex == -1)
        {
            AlarmUI.Instance.PopUpForKey("fullitem", null);

            for (int j = 0; j < successList.Count; ++j)
            {
                if (successList[j].type == 0)
                {
                    int emptyIndex = monsterInstances.FindEmptyIndex();
                    monsterInstances.monsterDatas[emptyIndex] = successList[j].monsterInstance;
                }
                else
                {
                    if (successList[j].type == 1)
                    {
                        if (successList[j].constType == CostItemType.Money)
                            ItemInventory.Instance.money += successList[j].count;
                        else if (successList[j].constType == CostItemType.Diamond)
                            ItemInventory.Instance.diamond += successList[j].count;
                    }
                    else
                    {
                        if (successList[j].type == 2)
                        {
                            if (successList[j].constType == CostItemType.Money)
                                ItemInventory.Instance.money += successList[j].count;
                            else if (successList[j].constType == CostItemType.Diamond)
                                ItemInventory.Instance.diamond += successList[j].count;
                        }
                    }
                }
            }

            CombatUI.Instance.UpdateMonsterImages();
            return;
        }

        ItemInventory.Instance.AddConsumeItem(data.combineItem);

        CombatUI.Instance.UpdateMonsterImages();
        DialougeUI.Instance.SettingFixedView(true);
        StartCoroutine(EffectRoutine(effect));
    }
    private void SettingData()
    {
        var datas = SelectTeamUI.Instance.selectTeam.startCombineDatas;
        DescButtonUpdate(datas);
        for (int i = 0; i < combineViews.Length; ++i)
        {
            ActiveCombineView(i, false);
        }
        for(int i = 0; i < datas.Count; ++i)
        {
            ActiveCombineView(i, true);
            SettingCombineView(i, datas[i]);
        }
    }
    private void SettingCombineView(int index, CombineData combineData)
    {
        bool checkActive = combineData.combineMonster != null ? combineData.CheckExistMonster() : combineData.CheckExistItem();
        if (combineData.combineMonster != null)
        {
            var mainframeIcon = combineViews[index].GetChild(0).GetChild(1).GetComponent<Image>();
            mainframeIcon.color = checkActive ? Color.white : Color.black;
            mainframeIcon.sprite = combineData.combineMonster.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

            bool check = (combineData.combineMonster.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (combineData.combineMonster.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (combineData.combineMonster.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
            float value = (combineData.combineMonster.monsterWeight != MonsterWeight.Big) ? 1.25f : 1 / 1.5f;
            if (check == true)
                value = 1.25f;
            mainframeIcon.SetNativeSize();
            mainframeIcon.rectTransform.sizeDelta = new Vector2(Mathf.Min(120.8334f, mainframeIcon.rectTransform.sizeDelta.x * value), Mathf.Min(120.8334f, mainframeIcon.rectTransform.sizeDelta.y * value));

            var effect = combineViews[index].GetChild(0).GetChild(3).GetComponent<Animator>();

            var mainframeButton = combineViews[index].GetChild(0).GetChild(2).GetComponent<Button>();
            mainframeButton.onClick.RemoveAllListeners();
            mainframeButton.onClick.AddListener(() => { MakeMonster(index, effect); });
            mainframeButton.gameObject.SetActive(checkActive && combineData.CheckActive());

            var animator = mainframeButton.transform.GetChild(0).GetComponent<Animator>();
            if (animator.gameObject.activeInHierarchy == true)
                animator.Play("max");
        }
        else
        {
            if(combineData.combineItem != null)
            {
                var mainframeIcon = combineViews[index].GetChild(0).GetChild(1).GetComponent<Image>();
                mainframeIcon.color = checkActive ? Color.white : Color.black;
                mainframeIcon.sprite = combineData.combineItem.itemImage;
                mainframeIcon.SetNativeSize();
                mainframeIcon.rectTransform.sizeDelta = (mainframeIcon.rectTransform.sizeDelta.y <= 35f) ? mainframeIcon.rectTransform.sizeDelta * 3.5f : mainframeIcon.rectTransform.sizeDelta / 1.5f;

                var effect = combineViews[index].GetChild(0).GetChild(3).GetComponent<Animator>();

                var mainframeButton = combineViews[index].GetChild(0).GetChild(2).GetComponent<Button>();
                mainframeButton.onClick.RemoveAllListeners();
                mainframeButton.onClick.AddListener(() => { MakeItem(index, effect); });
                mainframeButton.gameObject.SetActive(checkActive && combineData.CheckActive());

                var animator = mainframeButton.transform.GetChild(0).GetComponent<Animator>();
                if (animator.gameObject.activeInHierarchy == true)
                    animator.Play("max");
            }
        }
        

        int childCount = combineViews[index].childCount;

        var objs = combineData.GetDatas();
        for(int i = 1; i < childCount; i++)
        {
            if(i < objs.Count + 1)
            {
                combineViews[index].GetChild(i).gameObject.SetActive(true);
                var frameIcon = combineViews[index].GetChild(i).GetChild(1).GetComponent<Image>();
                frameIcon.color = checkActive ? Color.white : Color.black;

                var countText = combineViews[index].GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
                countText.color = checkActive ? Color.white : new Color(countText.color.r, countText.color.g, countText.color.b, 0f);

                var checkLock = combineViews[index].GetChild(i).GetChild(3).GetComponent<Animator>();
                checkLock.enabled = checkActive;
                checkLock.GetComponent<Image>().color = checkActive ? Color.white : new Color(countText.color.r, countText.color.g, countText.color.b, 0f); 

                MonsterIndigrident monsterData = objs[i - 1] as MonsterIndigrident;
                CostItemIndigrident costData = objs[i - 1] as CostItemIndigrident;
                ConsumeItemIndigrident consumeData = objs[i - 1] as ConsumeItemIndigrident;

                if (monsterData != null)
                {
                    frameIcon.sprite = monsterData.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
                    frameIcon.SetNativeSize();

                    bool check = (monsterData.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (monsterData.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (monsterData.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
                    float value = (monsterData.monsterData.monsterWeight != MonsterWeight.Big) ? 1.5f : 3.25f;
                    if (check == true)
                        value = 1.5f;

                    frameIcon.rectTransform.sizeDelta = frameIcon.rectTransform.sizeDelta / value;

                    countText.text = $"<b><size=24>x<b><size=28>{monsterData.count}";

                    check = combineData.CheckActiveMonster(monsterData);
                    if (check == true)
                        checkLock.Play("open");
                    else
                        checkLock.Play("lock");
                }
                else
                {
                    if(costData != null)
                    {
                        frameIcon.sprite = ItemInventory.Instance.GetSprite(costData.itemType);
                        frameIcon.SetNativeSize();
                        frameIcon.rectTransform.sizeDelta = new Vector2(Mathf.Min(84, frameIcon.rectTransform.sizeDelta.x * 3.5f), Mathf.Min(84, frameIcon.rectTransform.sizeDelta.y * 3.5f));

                        countText.text = $"<b><size=24>x<b><size=28>{costData.count}";

                        bool check = combineData.CheckActiveCostItem(costData);
                        if (check == true)
                            checkLock.Play("open");
                        else
                            checkLock.Play("lock");
                    }
                    else
                    {
                        if(consumeData != null)
                        {
                            frameIcon.sprite = consumeData.consumeItem.itemImage;
                            frameIcon.SetNativeSize();
                            frameIcon.rectTransform.sizeDelta = (frameIcon.rectTransform.sizeDelta.y <= 35f) ? frameIcon.rectTransform.sizeDelta * 2f : frameIcon.rectTransform.sizeDelta / 2f;

                            countText.text = $"<b><size=24>x<b><size=28>{consumeData.count}";

                            bool check = combineData.CheckActiveConsumeItem(consumeData);
                            if (check == true)
                                checkLock.Play("open");
                            else
                                checkLock.Play("lock");
                        }
                    }
                }
            }
            else
            {
                combineViews[index].GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void ActiveCombineView(int index, bool active)
    {
        if (active)
        {
            var mainframe = combineViews[index].GetChild(0);
            var background = mainframe.GetComponent<Image>();
            background.color = Color.white;

            mainframe.GetChild(1).gameObject.SetActive(true);
            mainframe.GetChild(2).gameObject.SetActive(true);

            int childCount = combineViews[index].childCount;
            for (int i = 1; i < childCount; i++)
            {
                var frame = combineViews[index].GetChild(i);

                background = frame.GetComponent<Image>();
                background.color = Color.white;

                frame.GetChild(1).gameObject.SetActive(true);
                frame.GetChild(2).gameObject.SetActive(true);
                frame.GetChild(3).gameObject.SetActive(true);
            }
        }
        else
        {
            var mainframe = combineViews[index].GetChild(0);
            var background = mainframe.GetComponent<Image>();
            background.color = new Color(0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f, 1f);

            mainframe.GetChild(1).gameObject.SetActive(false);
            mainframe.GetChild(2).gameObject.SetActive(false);

            int childCount = combineViews[index].childCount;
            for(int i = 1; i < childCount; i++)
            {
                var frame = combineViews[index].GetChild(i);

                background = frame.GetComponent<Image>();
                background.color = new Color(0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f, 1f);

                frame.GetChild(1).gameObject.SetActive(false);
                frame.GetChild(2).gameObject.SetActive(false);
                frame.GetChild(3).gameObject.SetActive(false);
            }
        }
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

    private IEnumerator EffectRoutine(Animator effect)
    {
        effect.Play("start");

        for (int i = 0; i < descButtons.Length; ++i)
        {
            descButtons[i].interactable = false;
        }

        for (int i = 0; i < combineViews.Length; ++i)
        {
            var button = combineViews[i].GetChild(0).GetChild(2).GetComponent<Button>();
            if (button != null)
                button.gameObject.SetActive(false);
        }
        yield return null;
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);

        SettingData();
    }
}
