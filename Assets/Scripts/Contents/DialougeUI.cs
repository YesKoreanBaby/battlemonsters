using System;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;
using DS.Enumerations;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Linq;
using System.Collections;

[System.Serializable]
public class ConversationData
{
    public Sprite npcImage;
    public string npcName;
    public DSDialogueContainerSO converstionData;
    public int imageIndex;
}

[System.Serializable]
public class MonsterShopData
{
    public MonsterData monster;
    public CostItemType costType;
    public int count;
}

[System.Serializable]
public class ItemShopData
{
    public ItemElementalData item;
    public CostItemType costType;
    public int count;
}
public class DialougeUI : MonoBehaviour
{
    public RectTransform singleDialogueView;
    public RectTransform TwoDialogueView;
    public RectTransform ThreeDialogueView;
    public RectTransform FourDialogueView;
    public RectTransform fixedView;
    public RectTransform fixedView2;
    public Image npcImage;
    public TextMeshProUGUI npcName;
    public Slider slider;
    public TextMeshProUGUI quantityValueText;
    public Image quantityValueImage;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;

    public BoxInfoSlot[] monsterBoxInfos;
    public BoxInfoSlot[] itemBoxInfos;
    public Button[] monsterBoxButtons;
    public Button[] itemBoxButtons;
    public RectTransform monsterResetButton;
    public RectTransform itemResetButton;
    public RectTransform itemResetAdsButton;
    public RectTransform monsterResetAdsButton;

    public bool isPopUp { get; private set; }
    public int changedValue { get; private set; }
    public int changedValue2 { get; private set; }
    public bool goMainSystem { get; private set; }
    public Animator animator { get; private set; }
    public DSDialogueSO currentDialogueData { get; private set; }
    public DSDialogueSO currentChoiceData { get; private set; }

    public List<Func<bool>> dsFuncDic { get; private set; }
    public bool dsFuncParse { get; private set; }

    public DialougeInfoSlot currentSelectInfo { get; private set; }
    public BoxInfoSlot currentBoxInfo { get; private set; }
  
    private BoxInfoSlot sellBoxInfo;

    private bool randomSoundBlock = false;

    [System.NonSerialized]
    public int previousBattleBgmIndex = -1;

    private static DialougeUI instance = null;
    public static DialougeUI Instance { get { return instance; } }


    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        Init();
    }
    private void Update()
    {
        if(isPopUp == true)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("convertmonsteridle"))
            {
                if(randomSoundBlock == false)
                    StartCoroutine(RandomSound());
            }
        }
    }
    public void PopUp(ConversationData converstionData, bool checkFixedView = false, bool goMainSystem = true)
    {
        if (isPopUp == false)
        {
            MainSystemUI.Instance.StopBgm();

            isPopUp = true;
            animator.enabled = true;
            animator.Play("idle");

            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }

            DSDialogueSO dialoueData = converstionData.converstionData.UngroupedDialogues.Find(x => x.IsStartingDialogue == true);
            npcImage.sprite = converstionData.npcImage;
            npcImage.rectTransform.sizeDelta = (converstionData.imageIndex == 0) ? new Vector2(170.6667f, 170.6667f) : new Vector2(144f, 144f);
            npcName.text = converstionData.npcName;
            SettingData(dialoueData);
            SettingFixedView(checkFixedView);

            this.goMainSystem = goMainSystem;

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
            //    AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);
        }
    }
    public void PopUp(bool checkFixedView = false, bool goMainSystem = true)
    {
        if (isPopUp == false)
        {
            MainSystemUI.Instance.StopBgm();

            isPopUp = true;
            animator.enabled = true;
            animator.Play("idle");
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }
            SettingFixedView(checkFixedView);

            this.goMainSystem = goMainSystem;

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
            //    AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);
        }
    }
    public void Closed()
    {
        if (isPopUp == true)
        {
            isPopUp = false;
            animator.enabled = false;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(false);
            }

            //if(CombatUI.Instance.isPopUp == true)
            //{
            //    if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false)
            //        AdmobManager.Instance.DestroyBannerAds();
            //}
            SetBgm();
        }
    }
    public void SetCurrentDialogueSlot(DialougeInfoSlot newInfo)
    {
        if (currentSelectInfo != null)
            currentSelectInfo.Active(false);

        currentSelectInfo = newInfo;
        currentSelectInfo.Active(true);
        currentChoiceData = newInfo.nextData;

    }
    public void SetCurrentBoxInfo(BoxInfoSlot newInfo)
    {
        if (currentBoxInfo != null)
            currentBoxInfo.Active(false);

        currentBoxInfo = newInfo;
        currentBoxInfo.Active(true);
    }

    public void ResetCurrentBoxInfo()
    {
        currentBoxInfo = null;
    }
    public void SetMonsterDatas()
    {
        float random = Random.Range(0f, 1f);

        if(random <= 0.5f)
        {
            var uncommons = MonsterDataBase.Instance.uncomonDatas.ToList();

            var monster_0 = uncommons[Random.Range(0, uncommons.Count)];
            uncommons.Remove(monster_0);

            var monster_1 = uncommons[Random.Range(0, uncommons.Count)];
            uncommons.Remove(monster_1);

            var monster_2 = uncommons[Random.Range(0, uncommons.Count)];

            random = Random.Range(0f, 1f);
            var hightDatas = (random <= 0.1f) ? MonsterDataBase.Instance.lairDatas : MonsterDataBase.Instance.comonDatas;

            var monster_3 = hightDatas[Random.Range(0, hightDatas.Count)];

            monsterBoxInfos[0].SetUp(monster_0, false);
            monsterBoxInfos[1].SetUp(monster_1, false);
            monsterBoxInfos[2].SetUp(monster_2, false);
            monsterBoxInfos[3].SetUp(monster_3, true);
        }
        else
        {
            var uncommons = MonsterDataBase.Instance.uncomonDatas.ToList();

            var monster_0 = uncommons[Random.Range(0, uncommons.Count)];
            uncommons.Remove(monster_0);

            var monster_1 = uncommons[Random.Range(0, uncommons.Count)];
            uncommons.Remove(monster_1);

            var monster_2 = uncommons[Random.Range(0, uncommons.Count)];
            uncommons.Remove(monster_2);

            var monster_3 = uncommons[Random.Range(0, uncommons.Count)];

            monsterBoxInfos[0].SetUp(monster_0, false);
            monsterBoxInfos[1].SetUp(monster_1, false);
            monsterBoxInfos[2].SetUp(monster_2, false);
            monsterBoxInfos[3].SetUp(monster_3, false);
        }
    }
    public void SetItemDatas()
    {
        var itemDatas = MonsterDataBase.Instance.itemDatas.ToList();

        var item_0 = itemDatas[Random.Range(0, itemDatas.Count)];
    

        var item_1 = itemDatas[Random.Range(0, itemDatas.Count)];
   

        var item_2 = itemDatas[Random.Range(0, itemDatas.Count)];
      

        var item_3 = itemDatas[Random.Range(0, itemDatas.Count)];
     

        itemBoxInfos[0].SetUp(item_0, false);
        itemBoxInfos[1].SetUp(item_1, false);
        itemBoxInfos[2].SetUp(item_2, false);
        itemBoxInfos[3].SetUp(item_3, false);
    }
    public void ExitCurrentDialougeSlot()
    {
        if(currentSelectInfo != null)
        {
            currentSelectInfo.Active(false);
            currentSelectInfo = null;
        }
    }
    public void ExitCurrentBoxSlot()
    {
        if(currentBoxInfo != null)
        {
            currentBoxInfo.Active(false);
            currentBoxInfo = null;
        }
    }
    public void Next()
    {
        if(currentChoiceData != null)
        {
            int n;
            bool check = int.TryParse(currentChoiceData.Text, out n);

            if(check == false)
                SettingData(currentChoiceData);
            else
            {
                check = dsFuncDic[n].Invoke();
                if (!check)
                    SettingData(currentChoiceData.Choices[0].NextDialogue);
            }
        }
        else
        {
            Closed();

            if(goMainSystem == true)
            {
                SoundManager.Instance.StopBgm();
                MainSystemUI.Instance.PopUp();
            }
        }
    }
    public void PopUpMonsterStatInfo()
    {
        //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false)
        //    AdmobManager.Instance.DestroyBannerAds();

        SoundManager.Instance.PlayEffect(164, 1f);
        StatUI.Instance.PopUp(currentBoxInfo.selectMonster, true);
    }
    public void PopUpMonsterSkillInfo()
    {
        //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false)
        //    AdmobManager.Instance.DestroyBannerAds();

        SoundManager.Instance.PlayEffect(164, 1f);
        SkillUI.Instance.PopUp(currentBoxInfo.selectMonster, true);
    }
    public void PopUpItemInfo()
    {
        SoundManager.Instance.PlayEffect(164, 1f);
        ItemInventoryUI.Instance.PopUp(currentBoxInfo.selectItem);

        //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false)
        //    AdmobManager.Instance.DestroyBannerAds();
    }

    public void MonsterReset()
    {
        SoundManager.Instance.PlayEffect(164, 1f);
        for (int i = 0; i < monsterBoxInfos.Length; ++i)
        {
            monsterBoxInfos[i].animator.Play("closedidle");
        }
        SetMonsterDatas();

        ItemInventory.Instance.diamond -= 20;
        monsterResetButton.gameObject.SetActive(ItemInventory.Instance.diamond >= 20);

        monsterResetAdsButton.gameObject.SetActive((ItemInventory.Instance.diamond < 20) && (LeagueManager.Instance.blockResetMonsterForAds == false));

        moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
        diamondText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.diamond}";

        var buttons = monsterBoxButtons;

        buttons[0].interactable = false;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[1].interactable = false;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[2].interactable = false;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[3].interactable = true;
        text = buttons[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }
    public void ItemReset()
    {
        SoundManager.Instance.StopAllEffect(180);
        SoundManager.Instance.StopAllEffect(181);
        SoundManager.Instance.PlayEffect(164, 1f);
        for (int i = 0; i < itemBoxInfos.Length; ++i)
        {
            itemBoxInfos[i].animator.Play("closedidle");
        }
        SetItemDatas();

        ItemInventory.Instance.diamond -= 20;
        itemResetButton.gameObject.SetActive(ItemInventory.Instance.diamond >= 20);
        itemResetAdsButton.gameObject.SetActive((ItemInventory.Instance.diamond < 20) && (LeagueManager.Instance.blockResetItemForAds == false));

        moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
        diamondText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.diamond}";

        var buttons = itemBoxButtons;

        buttons[0].interactable = false;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[1].interactable = false;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[2].interactable = true;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }

    public void ItemResetAds()
    {
        LeagueManager.Instance.blockResetItemForAds = true;
        ItemReset();
    }

    public void MonsterResetAds()
    {
        LeagueManager.Instance.blockResetMonsterForAds = true;
        MonsterReset();
    }
    public void MonsterSell2()
    {
        int money = ItemInventory.Instance.GetData(currentBoxInfo.monsterShopData.costType);

        if(money < currentBoxInfo.monsterShopData.count)
        {
            string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "<b><size=42>자금이 부족합니다" : "<b><size=42>I'm short on funds";
            AlarmUI.Instance.PopUp(text, null);
            return;
        }
        int emptyIndex = TranningUI.Instance.playerInventory.FindEmptyIndex();
        if(emptyIndex == -1)
        {
            AlarmUI.Instance.PopUpForKey("fullmonster", null);
            return;
        }
        sellBoxInfo = currentBoxInfo;

        string value = (TextManager.Instance.language == SystemLanguage.Korean) ? "<color=black>몬스터를 \r\n\r\n구매하시겠습니까?" : "<b><size=38><color=black>Would you like \r\n\r\nto purchase the monster?";
        AlarmUI.Instance.PopUp(value, MonsterSell);
    }
    public void ItemSell2()
    {
        int money = ItemInventory.Instance.GetData(currentBoxInfo.itemShopData.costType);
        if (money < currentBoxInfo.itemShopData.count)
        {
            string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "<b><size=42>자금이 부족합니다" : "<b><size=42>I'm short on funds";
            AlarmUI.Instance.PopUp(text, null);
            return;
        }

        if(!ItemInventory.Instance.ContainsTreasureItem(currentBoxInfo.selectItem))
        {
            int emptyIndex = ItemInventory.Instance.FindConsumeItemEmptyIndex();
            if (emptyIndex == -1)
            {
                AlarmUI.Instance.PopUpForKey("fullitem", null);
                return;
            }
        }

        sellBoxInfo = currentBoxInfo;

        string value = (TextManager.Instance.language == SystemLanguage.Korean) ? "<color=black>아이템을 \r\n\r\n구매하시겠습니까?" : "<b><size=38><color=black>Would you like \r\n\r\nto purchase the item?";
        AlarmUI.Instance.PopUp(value, ItemSell);
    }
    public void OnDiamondChanged(float value)
    {
        if(slider.value > 0)
        {
            int maxDiamond = Mathf.Min(99999 - ItemInventory.Instance.diamond, ItemInventory.Instance.money / LeagueManager.Instance.diamondToGold);
            int currentDiamond = ItemInventory.Instance.diamond + Mathf.FloorToInt(maxDiamond * slider.value);
            int currentMoney = ItemInventory.Instance.money - (Mathf.FloorToInt(maxDiamond * slider.value) * LeagueManager.Instance.diamondToGold);

            quantityValueText.text = currentDiamond.ToString();

            moneyText.text = $"<b><size=48>x<b><size=52>{currentMoney}";
            diamondText.text = $"<b><size=48>x<b><size=52>{currentDiamond}";

            changedValue = currentMoney;
            changedValue2 = currentDiamond;
        }
        else
        {
            changedValue = ItemInventory.Instance.money;
            changedValue2 = ItemInventory.Instance.diamond;

            moneyText.text = $"<b><size=48>x<b><size=52>{changedValue}";
            diamondText.text = $"<b><size=48>x<b><size=52>{changedValue2}";
            quantityValueText.text = changedValue2.ToString();
        }
    }
    public void OnMoneyChanged(float value)
    {
        if(slider.value > 0)
        {
            int maxMoney = Mathf.Min(99999 - ItemInventory.Instance.money, LeagueManager.Instance.diamondToGold * ItemInventory.Instance.diamond);
            int currentMoney = ItemInventory.Instance.money + Mathf.FloorToInt(maxMoney * slider.value);
            int currentDiamond = ItemInventory.Instance.diamond - (Mathf.FloorToInt(maxMoney * slider.value) / LeagueManager.Instance.diamondToGold);

            quantityValueText.text = currentMoney.ToString();

            moneyText.text = $"<b><size=48>x<b><size=52>{currentMoney}";
            diamondText.text = $"<b><size=48>x<b><size=52>{currentDiamond}";

            changedValue = currentMoney;
            changedValue2 = currentDiamond;
        }
        else
        {
            changedValue = ItemInventory.Instance.money;
            changedValue2 = ItemInventory.Instance.diamond;

            moneyText.text = $"<b><size=48>x<b><size=52>{changedValue}";
            diamondText.text = $"<b><size=48>x<b><size=52>{changedValue2}";
            quantityValueText.text = changedValue.ToString();
        }
    }

    public void SaveMoney(float value)
    {
        int maxMoney = ItemInventory.Instance.money;
        int currentSaveMoney = ItemInventory.Instance.saveMoney + Mathf.FloorToInt(maxMoney * slider.value);
        int currentMoney = ItemInventory.Instance.money - Mathf.FloorToInt(maxMoney * slider.value);
        quantityValueText.text = currentSaveMoney.ToString();

        moneyText.text = $"<b><size=48>x<b><size=52>{currentMoney}";

        changedValue = currentMoney;
        changedValue2 = currentSaveMoney;
    }

    public void LoadMoney(float value)
    {
        if(slider.value > 0)
        {
            int maxMoney = Mathf.Min(99999 - ItemInventory.Instance.money, ItemInventory.Instance.saveMoney);
            int currentMoney = ItemInventory.Instance.money + Mathf.FloorToInt(maxMoney * slider.value);
            int currentSaveMoney = ItemInventory.Instance.saveMoney - Mathf.FloorToInt(maxMoney * slider.value);

            quantityValueText.text = currentSaveMoney.ToString();

            moneyText.text = $"<b><size=48>x<b><size=52>{currentMoney}";

            changedValue = currentMoney;
            changedValue2 = currentSaveMoney;
        }
    }
    public void DickerMoney(float value, int dickerMoney, float percent, out bool check)
    {
        if(slider.value > 0)
        {
            int currentDickerMoney = Mathf.FloorToInt(dickerMoney * slider.value);

            quantityValueText.text = currentDickerMoney.ToString();

            moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money + currentDickerMoney}";
            changedValue = ItemInventory.Instance.money + currentDickerMoney;

            check = value <= percent;
        }
        else
        {
            changedValue = ItemInventory.Instance.money;

            quantityValueText.text = 0.ToString();

            moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
            check = true;
        }
        
    }
    public void Exit()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("convertmonsteridle"))
            animator.Play("convertmonsterexit");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("convertitemidle"))
            animator.Play("convertitemexit");

        //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
        //    AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);
    }
    public void SettingFixedView(bool active)
    {
        if (active == true)
        {
            fixedView.gameObject.SetActive(true);
            moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
            diamondText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.diamond}";
        }
        else
        {
            fixedView.gameObject.SetActive(false);
        }
    }
    private void SettingData(DSDialogueSO dialoueData)
    {
        dsFuncParse = false;
        currentDialogueData = dialoueData;
        currentChoiceData = (dialoueData.Choices.Count > 0) ? currentDialogueData.Choices[0].NextDialogue : null;
        currentSelectInfo = null;
        SettingDialogueView(dialoueData);
    }
    private void SettingDialogueView(DSDialogueSO dialoueData)
    {
        int choiceCount = (dialoueData.DialogueType == DSDialogueType.SingleChoice) ? 0 : dialoueData.Choices.Count;

        singleDialogueView.gameObject.SetActive(false);
        TwoDialogueView.gameObject.SetActive(false);
        ThreeDialogueView.gameObject.SetActive(false);
        FourDialogueView.gameObject.SetActive(false);

        if (choiceCount == 0)
        {
            singleDialogueView.gameObject.SetActive(true);
            var text = singleDialogueView.GetChild(0).GetComponent<TypingText>();
            var button = text.GetComponent<Button>();
            var typingEndObj = singleDialogueView.GetChild(1);

            button.interactable = false;
            typingEndObj.gameObject.SetActive(false);
            text.ResetTyping();

            string txt = TextManager.Instance.GetString(dialoueData.Text);

            //float value = (TextManager.Instance.language == SystemLanguage.Korean) ? 1f : 0.75f;
            //text.Play(txt, 0.075f * value, () =>
            //{
            //    button.interactable = true;
            //    typingEndObj.gameObject.SetActive(true);
            //});

            text.PlayLerp(txt, 1f, () =>
            {
                button.interactable = true;
                typingEndObj.gameObject.SetActive(true);
            });
        }
        else if (choiceCount == 2)
        {
            TwoDialogueView.gameObject.SetActive(true);
            var texts = TwoDialogueView.GetComponentsInChildren<DialougeInfoSlot>();
            for (int i = 0; i < texts.Length; ++i)
                texts[i].SetUp(currentDialogueData.Choices[i]);
        }
        else if(choiceCount == 3)
        {
            ThreeDialogueView.gameObject.SetActive(true);
            var texts = ThreeDialogueView.GetComponentsInChildren<DialougeInfoSlot>();
            for (int i = 0; i < texts.Length; ++i)
                texts[i].SetUp(currentDialogueData.Choices[i]);
        }
        else if(choiceCount == 4)
        {
            FourDialogueView.gameObject.SetActive(true);
            var texts = FourDialogueView.GetComponentsInChildren<DialougeInfoSlot>();
            for (int i = 0; i < texts.Length; ++i)
                texts[i].SetUp(currentDialogueData.Choices[i]);
        }
        else
            throw new NotImplementedException();
    }
    private void Init()
    {
        isPopUp = false;
        animator.enabled = false;
        var childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(false);
        }

        dsFuncDic = new List<Func<bool>>();

        Func<bool> buyLottery = () => {
            bool check = ItemInventory.Instance.money >= 50;
            if(check == true)
            {
                ItemInventory.Instance.money -= 50;
                Closed();
                RewardUI.Instance.PopUpMoney(MonsterDataBase.Instance.lotteryTable[Random.Range(0, MonsterDataBase.Instance.lotteryTable.Count)], 
                () =>
                {
                    RewardUI.Instance.Closed();
                    PopUp(true);
                });
            }
            return check;
        };

        Func<bool> showDiamond = () =>
        {
            if(dsFuncParse == false)
            {
                singleDialogueView.gameObject.SetActive(false);
                TwoDialogueView.gameObject.SetActive(false);
                ThreeDialogueView.gameObject.SetActive(false);
                FourDialogueView.gameObject.SetActive(false);

                singleDialogueView.gameObject.SetActive(true);
                var text = singleDialogueView.GetChild(0).GetComponent<TypingText>();
                var button = text.GetComponent<Button>();
                var typingEndObj = singleDialogueView.GetChild(1);

                button.interactable = false;
                typingEndObj.gameObject.SetActive(false);

                string value = (TextManager.Instance.language == SystemLanguage.Korean) ? $"현재 다이아의 시세는 {LeagueManager.Instance.diamondToGold}gold입니다" : $"The current price of diamond is {LeagueManager.Instance.diamondToGold}gold.";
                text.PlayLerp(value, 1f, () => {
                    button.interactable = true;
                    typingEndObj.gameObject.SetActive(true);
                });

                dsFuncParse = true;
            }
            else
            {
                SettingData(currentChoiceData.Choices[0].NextDialogue);
                dsFuncParse = false;
            }
            return true;
        };

        Func<bool> showMoneyToDiamond = () =>
        {
            bool check = ItemInventory.Instance.money > LeagueManager.Instance.diamondToGold;
            if(check == true)
            {
                singleDialogueView.gameObject.SetActive(false);
                TwoDialogueView.gameObject.SetActive(false);
                ThreeDialogueView.gameObject.SetActive(false);
                FourDialogueView.gameObject.SetActive(false);

                fixedView2.gameObject.SetActive(true);
                quantityValueImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Diamond);
                quantityValueText.text = ItemInventory.Instance.diamond.ToString();
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener(OnDiamondChanged);
                slider.value = 0f;

                var text = fixedView2.GetChild(1).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[1]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    ItemInventory.Instance.money = changedValue;
                    ItemInventory.Instance.diamond = changedValue2;
                    moneyText.text = $"<b><size=48>x<b><size=52>{changedValue}";
                    diamondText.text = $"<b><size=48>x<b><size=52>{changedValue2}";
                    fixedView2.gameObject.SetActive(false);

                });

                text = fixedView2.GetChild(2).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[2]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
                    diamondText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.diamond}";
                    fixedView2.gameObject.SetActive(false);

                });

                changedValue = ItemInventory.Instance.money;
                changedValue2 = ItemInventory.Instance.diamond;
            }
            return check;
        };

        Func<bool> showDiamondToMoney = () =>
        {
            bool check = ItemInventory.Instance.diamond > 0;
            if (check == true)
            {
                singleDialogueView.gameObject.SetActive(false);
                TwoDialogueView.gameObject.SetActive(false);
                ThreeDialogueView.gameObject.SetActive(false);
                FourDialogueView.gameObject.SetActive(false);

                fixedView2.gameObject.SetActive(true);
                quantityValueImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Money);
                quantityValueText.text = ItemInventory.Instance.money.ToString();
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener(OnMoneyChanged);
                slider.value = 0f;

                var text = fixedView2.GetChild(1).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[1]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    ItemInventory.Instance.money = changedValue;
                    ItemInventory.Instance.diamond = changedValue2;
                    moneyText.text = $"<b><size=48>x<b><size=52>{changedValue}";
                    diamondText.text = $"<b><size=48>x<b><size=52>{changedValue2}";
                    fixedView2.gameObject.SetActive(false);

                });

                text = fixedView2.GetChild(2).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[2]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
                    diamondText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.diamond}";
                    fixedView2.gameObject.SetActive(false);

                });

                changedValue = ItemInventory.Instance.money;
                changedValue2 = ItemInventory.Instance.diamond;
            }
            return check;
        };

        Func<bool> convertMonster = () =>
        {
            bool check = false;
            for(int i = 0; i < monsterBoxInfos.Length; ++i)
            {
                if (monsterBoxInfos[i].isSell == false)
                {
                    check = true;
                    break;
                }    
            }

            if(check == true)
            {
                animator.Play("convertmonster");

                var buttons = monsterBoxButtons;

                buttons[0].interactable = false;
                var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

                buttons[1].interactable = false;
                text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

                buttons[2].interactable = false;
                text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

                buttons[3].interactable = true;
                text = buttons[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

                bool resetCheck = ItemInventory.Instance.diamond >= 20;
                monsterResetButton.gameObject.SetActive(resetCheck);

                monsterResetAdsButton.gameObject.SetActive((ItemInventory.Instance.diamond < 20) && (LeagueManager.Instance.blockResetMonsterForAds == false));
            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
            //    {
            //        if (resetCheck)
            //            AdmobManager.Instance.DestroyBannerAds();
            //        else
            //            AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);

            //    }
            }

            return check;
        };

        Func<bool> convertItem = () =>
        {
            bool check = false;
            for (int i = 0; i < itemBoxInfos.Length; ++i)
            {
                if (itemBoxInfos[i].isSell == false)
                {
                    check = true;
                    break;
                }
            }

            if (check == true)
            {
                animator.Play("convertitem");

                var buttons = itemBoxButtons;

                buttons[0].interactable = false;
                var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

                buttons[1].interactable = false;
                text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

                buttons[2].interactable = true;
                text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);

                bool resetCheck = ItemInventory.Instance.diamond >= 20;
                itemResetButton.gameObject.SetActive(resetCheck);

                itemResetAdsButton.gameObject.SetActive((ItemInventory.Instance.diamond < 20) && (LeagueManager.Instance.blockResetItemForAds == false));

                //if(AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
                //{
                //    if (resetCheck)
                //        AdmobManager.Instance.DestroyBannerAds();
                //    else
                //        AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);

                //}
            }
                

            return check;
        };

        Func<bool> combine = () =>
        {
            bool check = SelectTeamUI.Instance.selectTeam.startCombineDatas.Count > 0;

            if (check == true)
            {
                CombineUI.Instance.PopUp();
            }

            return check;
        };

        Func<bool> gambling = () =>
        {
            bool check = LeagueManager.Instance.blockGamebling;
            if (check == true)
            {
                currentChoiceData = currentChoiceData.Choices[1].NextDialogue;
                Next();
            }
            else
            {
                check = ItemInventory.Instance.money >= 1;

                if (check == true)
                {
                    CombatManager.Instance.battleDataObject = CombatManager.Instance.ai.singleGameblingData;
                    CombatManager.Instance.ReturnGamebling();
                    Closed();
                }
            }
            return check;
        };

        Func<bool> saveMoney = () =>
        {
            bool check = ItemInventory.Instance.money > 0;
            if (check == true)
            {
                singleDialogueView.gameObject.SetActive(false);
                TwoDialogueView.gameObject.SetActive(false);
                ThreeDialogueView.gameObject.SetActive(false);
                FourDialogueView.gameObject.SetActive(false);

                fixedView2.gameObject.SetActive(true);
                quantityValueImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Money);
                quantityValueText.text = ItemInventory.Instance.saveMoney.ToString();
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener(SaveMoney);
                slider.value = 0f;

                var text = fixedView2.GetChild(1).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[1]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    ItemInventory.Instance.money = changedValue;
                    ItemInventory.Instance.saveMoney = changedValue2;
                    moneyText.text = $"<b><size=48>x<b><size=52>{changedValue}";
                    fixedView2.gameObject.SetActive(false);

                });

                text = fixedView2.GetChild(2).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[2]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
                    fixedView2.gameObject.SetActive(false);

                });

                changedValue = ItemInventory.Instance.money;
                changedValue2 = ItemInventory.Instance.saveMoney;
            }
            return check;
        };

        Func<bool> loadMoney = () =>
        {
            bool check = ItemInventory.Instance.saveMoney > 0;
            if (check == true)
            {
                singleDialogueView.gameObject.SetActive(false);
                TwoDialogueView.gameObject.SetActive(false);
                ThreeDialogueView.gameObject.SetActive(false);
                FourDialogueView.gameObject.SetActive(false);

                fixedView2.gameObject.SetActive(true);
                quantityValueImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Money);
                quantityValueText.text = ItemInventory.Instance.saveMoney.ToString();
                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener(LoadMoney);
                slider.value = 0f;

                var text = fixedView2.GetChild(1).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[1]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    ItemInventory.Instance.money = changedValue;
                    ItemInventory.Instance.saveMoney = changedValue2;
                    moneyText.text = $"<b><size=48>x<b><size=52>{changedValue}";
                    fixedView2.gameObject.SetActive(false);

                });

                text = fixedView2.GetChild(2).GetComponent<DialougeInfoSlot>();
                text.SetUp(currentChoiceData.Choices[2]);
                text.addEvt.RemoveAllListeners();
                text.addEvt.AddListener(() =>
                {
                    moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
                    fixedView2.gameObject.SetActive(false);

                });

                changedValue = ItemInventory.Instance.money;
                changedValue2 = ItemInventory.Instance.saveMoney;
            }
            return check;
        };

        Func<bool> surrend = () =>
        {
            StoryUI.Instance.PopUp();
            AlarmUI.Instance.Closed();
            CombatManager.Instance.maxLevel = 0;
            CombatManager.Instance.currentLevel = 0;
            Closed();
            return true;
        };

        Func<bool> wolfDicker = () =>
        {
            int dickerMoney = 2000;
            float maxPercent = Random.Range(0.25f, 1f);
            var failData = currentChoiceData.Choices[0].NextDialogue;
            bool check = false;
            singleDialogueView.gameObject.SetActive(false);
            TwoDialogueView.gameObject.SetActive(false);
            ThreeDialogueView.gameObject.SetActive(false);
            FourDialogueView.gameObject.SetActive(false);

            fixedView.gameObject.SetActive(true);
            fixedView2.gameObject.SetActive(true);
            quantityValueImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Money);
            quantityValueText.text = "0";
            changedValue = ItemInventory.Instance.money;
            changedValue2 = ItemInventory.Instance.diamond;
            moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
            diamondText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.diamond}";
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener((x) => { DickerMoney(x, dickerMoney, maxPercent, out check); });
        
            var text = fixedView2.GetChild(1).GetComponent<DialougeInfoSlot>();
            text.SetUp(currentChoiceData.Choices[1]);
            text.addEvt.RemoveAllListeners();
            text.addEvt.AddListener(() =>
            {
                if(check == true)
                {
                    ItemInventory.Instance.money = changedValue;
                    moneyText.text = $"<b><size=48>x<b><size=52>{changedValue}";
                    fixedView2.gameObject.SetActive(false);
                    fixedView.gameObject.SetActive(false);
                }
                else
                {
                    SettingData(failData);
                    fixedView2.gameObject.SetActive(false);
                    fixedView.gameObject.SetActive(false);
                }
            });

            text = fixedView2.GetChild(2).GetComponent<DialougeInfoSlot>();
            text.SetUp(currentChoiceData.Choices[2]);
            text.addEvt.RemoveAllListeners();
            text.addEvt.AddListener(() =>
            {
                fixedView2.gameObject.SetActive(false);

            });
            return true;
        };

        Func<bool> setdesertstage = () =>
        {
            StoryUI.Instance.PopUp();
            StoryUI.Instance.Next3Stage();
            AlarmUI.Instance.Closed();
            CombatManager.Instance.maxLevel = 0;
            CombatManager.Instance.currentLevel = 0;
            Closed();
            return true;
        };

        Func<bool> earnFairy = () =>
        {
            var inventory = TranningUI.Instance.playerInventory;
            int findIndex = Array.FindIndex(inventory.monsterDatas, x => x == null);
            if (findIndex == -1)
                return false;

            findIndex = Array.FindIndex(inventory.monsterDatas, x => (x != null) && (x.monsterData == MonsterDataBase.Instance.stage_1RewardMonster));
            if (findIndex != -1)
                return false;

            var instance = MonsterInstance.Instance(MonsterDataBase.Instance.stage_1RewardMonster);
            TranningUI.Instance.playerInventory.AddItem(instance);
            instance.currentConfirmSkillPriority = instance.confirmSkillPrioritys[0];
            instance.currentSelectDetailTargetType = instance.selectDetailTargetTypes[0];
            CombatUI.Instance.UpdateMonsterImages();

            CombatUI.Instance.surrendButton.interactable = false;
            Closed();
            return true;
        };

        Func<bool> dontActiveItem = () =>
        {
            CombatUI.Instance.itemFlag = false;
            CombatUI.Instance.Closed();
            CombatUI.Instance.PopUp();
            Closed();
            return true;
        };

        Func<bool> earn10000gold = () =>
        {
            StoryUI.Instance.currentSlot.clearEvent.AddListener(() => { RewardUI.Instance.AddCostItem(CostItemType.Money, 10000); });
            Closed();
            return true;
        };

        Func<bool> earnHunterTreagure = () =>
        {
            StoryUI.Instance.currentSlot.clearEvent.AddListener(() => { RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.symbolofGunslinger); });
            Closed();
            return true;
        };

        Func<bool> dontActiveSurrend = () =>
        {
            CombatUI.Instance.surrendFlag = false;
            CombatUI.Instance.Closed();
            CombatUI.Instance.PopUp();
            Closed();
            return true;
        };

        Func<bool> unlockParliament = () =>
        {
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_1);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_2);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_3);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.Next_6_Hidden2Stage);

            StoryUI.Instance.currentSlot.clearEvent.AddListener(StoryUI.Instance.NextSpecial_1);
            currentChoiceData = currentChoiceData.Choices[0].NextDialogue;
            Next();
            MonsterDead();
            return true;
        };

        Func<bool> unlockMine = () =>
        {
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_1);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_2);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_3);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.Next_6_Hidden2Stage);

            StoryUI.Instance.currentSlot.clearEvent.AddListener(StoryUI.Instance.Next_6_Hidden2Stage);
            currentChoiceData = currentChoiceData.Choices[0].NextDialogue;
            Next();
            MonsterDead();
            return true;
        };

        Func<bool> unlockEndWorld = () =>
        {
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_1);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_2);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_3);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.Next_6_Hidden2Stage);

            StoryUI.Instance.currentSlot.clearEvent.AddListener(StoryUI.Instance.NextSpecial_3);
            currentChoiceData = currentChoiceData.Choices[0].NextDialogue;
            Next();
            MonsterDead();
            return true;
        };

        Func<bool> unlockHallOfFame = () =>
        {
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_1);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_2);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.NextSpecial_3);
            StoryUI.Instance.currentSlot.clearEvent.RemoveListener(StoryUI.Instance.Next_6_Hidden2Stage);

            StoryUI.Instance.currentSlot.clearEvent.AddListener(StoryUI.Instance.NextSpecial_2);
            currentChoiceData = currentChoiceData.Choices[0].NextDialogue;
            Next();
            MonsterDead();
            return true;
        };

        Func<bool> lostFame = () =>
        {
            ItemInventory.Instance.fame -= 20;
            currentChoiceData = currentChoiceData.Choices[0].NextDialogue;
            Next();
            return true;
        };

        Func<bool> tollfee = () =>
        {
            bool check = ItemInventory.Instance.money >= 2000;
            if (check == true)
            {
                currentChoiceData = currentChoiceData.Choices[1].NextDialogue;
                Next();
                ItemInventory.Instance.money -= 2000;
                CombatManager.Instance.battleDataObject = StoryUI.Instance.level_6_hidden2_1.stageData;
                CombatManager.Instance.ReturnGame();
                StoryUI.Instance.currentSlot.clearBlock = true;
            }
            return check;
        };

        Func<bool> famefee = () =>
        {
            bool check = ItemInventory.Instance.fame >= 150;
            if(check == true)
            {
                currentChoiceData = currentChoiceData.Choices[1].NextDialogue;
                Next();
            }

            return check;
        };

        Func<bool> rankingFee = () =>
        {
            bool check = ItemInventory.Instance.ranking >= 1;
            if (check == true)
            {
                currentChoiceData = currentChoiceData.Choices[1].NextDialogue;
                Next();
            }

            return check;
        };

        Func<bool> enterDeathGround = () =>
        {
            CombatUI.Instance.itemFlag = false;
            CombatUI.Instance.surrendFlag = false;
            LightMonsterObstacle();
            CombatUI.Instance.Closed();
            CombatUI.Instance.PopUp();
            Closed();
            return true;
        };

        Func<bool> showRulletUI = () =>
        {
            bool check = ItemInventory.Instance.money > 0;
            if(check == true)
            {
                RulletUI.Instance.PopUp();
                //Closed();
            }
            return check;
        };

        Func<bool> officalWin = () =>
        {
            CombatManager.Instance.startEvent = () =>
            {
                var homegrounds = CombatManager.Instance.homegroundMonsters;
                for(int i = 0; i < homegrounds.Count; ++i)
                {
                    var instance = homegrounds[i].battleInstance;
                    instance.atk += Mathf.Ceil((instance.atk * 0.25f));
                    instance.def += Mathf.Ceil((instance.def * 0.25f));
                }
            };

            CombatManager.Instance.endEvent = (x) =>
            {
                if (x == -1)
                    ItemInventory.Instance.fame -= 5;
            };
            Closed();
            return true;
        };

        Func<bool> officalLose = () =>
        {
            CombatManager.Instance.startEvent = () =>
            {
                var homegrounds = CombatManager.Instance.homegroundMonsters;
                for (int i = 0; i < homegrounds.Count; ++i)
                {
                    var instance = homegrounds[i].battleInstance;
                    instance.atk = Mathf.Max(0, instance.atk - Mathf.Ceil((instance.atk * 0.25f)));
                    instance.def = Mathf.Max(0, instance.def - Mathf.Ceil((instance.def * 0.25f)));
                }
            };

            CombatManager.Instance.endEvent = (x) =>
            {
                if (x == 1)
                    ItemInventory.Instance.fame += 5;
            };
            Closed();
            return true;
        };

        Func<bool> nocomment = () =>
        {
            CombatManager.Instance.startEvent = null;
            CombatManager.Instance.endEvent = null;
            Closed();
            return true;
        };

        Func<bool> showfriendShopLevel = () =>
        {
            if (dsFuncParse == false)
            {
                singleDialogueView.gameObject.SetActive(false);
                TwoDialogueView.gameObject.SetActive(false);
                ThreeDialogueView.gameObject.SetActive(false);
                FourDialogueView.gameObject.SetActive(false);

                singleDialogueView.gameObject.SetActive(true);
                var text = singleDialogueView.GetChild(0).GetComponent<TypingText>();
                var button = text.GetComponent<Button>();
                var typingEndObj = singleDialogueView.GetChild(1);

                button.interactable = false;
                typingEndObj.gameObject.SetActive(false);

                string value = (TextManager.Instance.language == SystemLanguage.Korean) ? $"현재 레벨은 {LeagueManager.Instance.friendshipLevel}입니다" : $"Current level is {LeagueManager.Instance.friendshipLevel}";
                text.PlayLerp(value, 1f, () => {
                    button.interactable = true;
                    typingEndObj.gameObject.SetActive(true);
                });

                dsFuncParse = true;
            }
            else
            {
                SettingData(currentChoiceData.Choices[0].NextDialogue);
                dsFuncParse = false;
            }
            return true;
        };

        Func<bool> selectInfightMonster = () =>
        {
            var playerInven = TranningUI.Instance.playerInventory;
            playerInven.Clear();
            playerInven.AddItem(MonsterDataBase.Instance.infightTutorialData);
            playerInven.monsterDatas[0].maxHp = playerInven.monsterDatas[0].hp = Mathf.Max(playerInven.monsterDatas[0].hp, 100);
            CombatUI.Instance.UpdateMonsterImages();
            SettingData(currentChoiceData.Choices[0].NextDialogue);
            return true;
        };

        Func<bool> selectOutfightMonster = () =>
        {
            var playerInven = TranningUI.Instance.playerInventory;
            playerInven.Clear();
            playerInven.AddItem(MonsterDataBase.Instance.outFightTutorialData);
            playerInven.monsterDatas[0].maxHp = playerInven.monsterDatas[0].hp = Mathf.Max(playerInven.monsterDatas[0].hp, 100);
            CombatUI.Instance.UpdateMonsterImages();
            SettingData(currentChoiceData.Choices[0].NextDialogue);
            return true;
        };

        Func<bool> selectSlergerMonster = () =>
        {
            var playerInven = TranningUI.Instance.playerInventory;
            playerInven.Clear();
            playerInven.AddItem(MonsterDataBase.Instance.slergerTutorialData);
            playerInven.monsterDatas[0].maxHp = playerInven.monsterDatas[0].hp = Mathf.Max(playerInven.monsterDatas[0].hp, 100);
            CombatUI.Instance.UpdateMonsterImages();
            SettingData(currentChoiceData.Choices[0].NextDialogue);
            return true;
        };

        Func<bool> checkClearDevilStage = () =>
        {
            bool check = StoryUI.Instance.special_1.isCleard;

            if (check == true)
            {
                currentChoiceData = currentChoiceData.Choices[1].NextDialogue;
                Next();
            }

            return check;
        };

        dsFuncDic.Add(buyLottery);
        dsFuncDic.Add(showDiamond);
        dsFuncDic.Add(showMoneyToDiamond);
        dsFuncDic.Add(showDiamondToMoney);
        dsFuncDic.Add(convertMonster);
        dsFuncDic.Add(convertItem);
        dsFuncDic.Add(combine);
        dsFuncDic.Add(gambling);
        dsFuncDic.Add(saveMoney);
        dsFuncDic.Add(loadMoney);
        dsFuncDic.Add(surrend);
        dsFuncDic.Add(wolfDicker);
        dsFuncDic.Add(setdesertstage);
        dsFuncDic.Add(earnFairy);
        dsFuncDic.Add(dontActiveItem);
        dsFuncDic.Add(earn10000gold);
        dsFuncDic.Add(earnHunterTreagure);
        dsFuncDic.Add(dontActiveSurrend);
        dsFuncDic.Add(unlockParliament);
        dsFuncDic.Add(unlockMine);
        dsFuncDic.Add(unlockEndWorld);
        dsFuncDic.Add(unlockHallOfFame);
        dsFuncDic.Add(lostFame);
        dsFuncDic.Add(tollfee);
        dsFuncDic.Add(famefee);
        dsFuncDic.Add(rankingFee);
        dsFuncDic.Add(enterDeathGround);
        dsFuncDic.Add(showRulletUI);
        dsFuncDic.Add(officalWin);
        dsFuncDic.Add(officalLose);
        dsFuncDic.Add(nocomment);
        dsFuncDic.Add(showfriendShopLevel);
        dsFuncDic.Add(selectInfightMonster);
        dsFuncDic.Add(selectOutfightMonster);
        dsFuncDic.Add(selectSlergerMonster); //34
        dsFuncDic.Add(checkClearDevilStage); // 35
    }
    private void SetBgm()
    {
        if (CombatUI.Instance.isPopUp == false)
            return;

        if (StoryUI.Instance.isPopUp == true)
            return;

        var currentBattleType = CombatManager.Instance.currentBattleType;
        var currentLevel = CombatManager.Instance.currentLevel;
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
        else if(currentBattleType == BattleType.Official)
        {
            SoundManager.Instance.StopBgm();

            if(CombatManager.Instance.battleDataObject == CombatManager.Instance.ai.tutorialData)
            {
                SoundManager.Instance.PlayBgm(25, 1f);
            }
            else
            {
                int index = Random.Range(35, 41);
                if (index == 39)
                    index = 45;
                if (index == 40)
                    index = 46;
                while (index == previousBattleBgmIndex)
                {
                    index = Random.Range(35, 41);
                    if (index == 39)
                        index = 45;
                    if (index == 40)
                        index = 46;
                }
                SoundManager.Instance.PlayBgm(index, 1f);
                previousBattleBgmIndex = index;
            }
        }
        else if (currentBattleType == BattleType.Gamebling)
        {
            SoundManager.Instance.StopBgm();
            SoundManager.Instance.PlayBgm(42, 1f);
        }
    }
    private void MonsterSell()
    {
        if (currentBoxInfo == sellBoxInfo)
            currentBoxInfo = null;

        var instance = sellBoxInfo.selectMonster;

        TranningUI.Instance.playerInventory.AddItem(instance);
        instance.currentConfirmSkillPriority = instance.confirmSkillPrioritys[0];
        instance.currentSelectDetailTargetType = instance.selectDetailTargetTypes[0];

        sellBoxInfo.animator.Play("sell");
        sellBoxInfo.SetSell(true);
        sellBoxInfo.SetBlock(true);
        StartCoroutine(DescreaseMoneyRoutine(sellBoxInfo.monsterShopData.costType, sellBoxInfo.monsterShopData.count));
        sellBoxInfo = null;

        SoundManager.Instance.PlayEffect(179, 1f);

        if (AlarmUI.Instance.isPopUp == true)
            AlarmUI.Instance.Closed();

        var buttons = monsterBoxButtons;

        buttons[0].interactable = false;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[1].interactable = false;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[2].interactable = false;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[3].interactable = true;
        text = buttons[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }
    private void ItemSell()
    {
        if (currentBoxInfo == sellBoxInfo)
            currentBoxInfo = null;

        var item = sellBoxInfo.selectItem;
        if(ItemInventory.Instance.ContainsTreasureItem(item))
            ItemInventory.Instance.AddTreasureItem(item);
        else
            ItemInventory.Instance.AddConsumeItem(item);

        sellBoxInfo.animator.Play("sell");
        sellBoxInfo.SetSell(true);
        sellBoxInfo.SetBlock(true);
        StartCoroutine(DescreaseMoneyRoutine(sellBoxInfo.itemShopData.costType, sellBoxInfo.itemShopData.count));
        sellBoxInfo = null;

        SoundManager.Instance.StopEffect(181);
        SoundManager.Instance.PlayEffect(179, 1f);

        if (AlarmUI.Instance.isPopUp == true)
            AlarmUI.Instance.Closed();

        var buttons = itemBoxButtons;

        buttons[0].interactable = false;
        var text = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[1].interactable = false;
        text = buttons[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5019607843137255f);

        buttons[2].interactable = true;
        text = buttons[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }
    private void MonsterDead()
    {
        var inven = TranningUI.Instance.playerInventory.monsterDatas;
        var monsters = Array.FindAll(inven, x => (x != null) && (x.heathState != MonsterHeathState.Faint));
        if (monsters.Length > 1)
        {
            Array.Sort(monsters, (a, b) =>
            {
                float aSumValue = a.atk + a.def + a.hp;
                float bSumValue = b.atk + b.def + b.hp;
                return bSumValue.CompareTo(aSumValue);
            });

            if (monsters[0].monsterData.isDeadLock == false)
            {
                var nextMonsterInstance = MonsterInstance.Instance(MonsterDataBase.Instance.deadTable[Random.Range(0, MonsterDataBase.Instance.deadTable.Length)]);
                nextMonsterInstance.previousMonsterData = monsters[0];
                int index = TranningUI.Instance.playerInventory.FindIndex(monsters[0]);
                TranningUI.Instance.playerInventory.monsterDatas[index] = nextMonsterInstance;
                CombatUI.Instance.UpdateMonsterImages();
            }
        }
    }
    private void LightMonsterObstacle()
    {
        var inven = TranningUI.Instance.playerInventory.monsterDatas;
        var monsters = Array.FindAll(inven, x => (x != null) && (x.heathState != MonsterHeathState.Faint) && (x.status == Status.Light));
        for(int i = 0; i < monsters.Length; ++i)
            monsters[i].heathState = MonsterHeathState.CrippedStrong;

        CombatUI.Instance.UpdateMonsterImages();
    }
    private IEnumerator DescreaseMoneyRoutine(CostItemType type, int descreaseValue)
    {
        int start = ItemInventory.Instance.GetData(type);
        int end = Mathf.Max(0, ItemInventory.Instance.GetData(type) - descreaseValue);

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(start, end, currentSpeed);

            if (type == CostItemType.Money)
            {
                ItemInventory.Instance.money = Mathf.FloorToInt(value);
                moneyText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.money}";
            }
            else if (type == CostItemType.Diamond)
            {
                ItemInventory.Instance.diamond = Mathf.FloorToInt(value);
                diamondText.text = $"<b><size=48>x<b><size=52>{ItemInventory.Instance.diamond}";
            }
            yield return null;
        }
    }
    private IEnumerator RandomSound()
    {
        randomSoundBlock = true;
        int index = Random.Range(174, 178);
        SoundManager.Instance.PlayEffect(index, 1f);
        yield return null;

        var audio = SoundManager.Instance.FindEffect(index);
        yield return new WaitUntil(() => audio.isPlaying == false);

        yield return new WaitForSeconds(Random.Range(4f, 8f));

        randomSoundBlock = false;
    }
}
