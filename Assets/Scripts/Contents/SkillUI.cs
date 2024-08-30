using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    public Button selectDetailTargetPrevButton;
    public Button selectDetailTargetNextButton;
    public TextMeshProUGUI selectDetailTargetText;

    public Button confirmSkillPriorityPrevButton;
    public Button confirmSkillPriorityNextButton;
    public TextMeshProUGUI confirmSkillPriorityText;

    public Image[] confirmSkillImages;
    public Image[] percentSkillImages;
    public Image[] triggerSkillImages;
    public Image[] abilityImages;

    public RectTransform skillInfoView;
    public RectTransform backgroundView;
    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI dmgType;
    public TextMeshProUGUI effectType;
    public TextMeshProUGUI rangeType;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI eft;
    public TextMeshProUGUI cri;
    public TextMeshProUGUI acc;
    public TextMeshProUGUI pay;
    public TextMeshProUGUI description;
    public TextMeshProUGUI emptyText;
    public Button skillInfoPrev;
    public Button skillInfoNext;
    public Image currentIndexImage;

    public RectTransform strategyView;
    public TextMeshProUGUI strategyName;
    public TextMeshProUGUI strategyDescription;

    public GameObject customEditObject;
    public GameObject skillRemoveObject;
    public object cancelObj;

    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public int selectDetailTargetTypeMaxIndex;

    [System.NonSerialized]
    public int selectDetailTargetCurrentIndex;

    [System.NonSerialized]
    public int confirmSkillPriorityMaxIndex;

    [System.NonSerialized]
    public int confirmSkillPriorityCurrentIndex;

    [System.NonSerialized]
    public MonsterInstance currentMonsterInstance;

    [System.NonSerialized]
    public GameObject currentSelectBox;

    [System.NonSerialized]
    public SkillInfoSlot[] confirmSkillInfos;

    [System.NonSerialized]
    public SkillInfoSlot[] percentSkillInfos;

    [System.NonSerialized]
    public SkillInfoSlot[] triggerSkillInfos;

    [System.NonSerialized]
    public AbilityInfoSlot[] abilityInfos;

    [System.NonSerialized]
    public bool readOnly = false;

    private static SkillUI instance = null;
    public static SkillUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        confirmSkillInfos = new SkillInfoSlot[confirmSkillImages.Length];
        for(int i = 0; i < confirmSkillImages.Length; ++i)
            confirmSkillInfos[i] = confirmSkillImages[i].GetComponent<SkillInfoSlot>();

        percentSkillInfos = new SkillInfoSlot[percentSkillImages.Length];
        for (int i = 0; i < percentSkillImages.Length; ++i)
            percentSkillInfos[i] = percentSkillImages[i].GetComponent<SkillInfoSlot>();

        triggerSkillInfos = new SkillInfoSlot[triggerSkillImages.Length];
        for (int i = 0; i < triggerSkillImages.Length; ++i)
            triggerSkillInfos[i] = triggerSkillImages[i].GetComponent<SkillInfoSlot>();

        abilityInfos = new AbilityInfoSlot[abilityImages.Length];
        for (int i = 0; i < abilityImages.Length; ++i)
            abilityInfos[i] = abilityImages[i].GetComponent<AbilityInfoSlot>();


        Closed();
    }
    public void PopUp(MonsterInstance currentInstance, bool readOnly = false)
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            this.readOnly = readOnly;
            SettingData(currentInstance);
        }
    }

    public void PopUp(MonsterData monsterData)
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            this.readOnly = true;
            SettingData(monsterData);
        }
    }
    public void Closed()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        //if (CombineUI.Instance.isPopUp == false && DialougeUI.Instance.isPopUp == true)
        //{
        //    if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
        //        AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);
        //}
        //if(ExternalUI.Instance.isPopUp == true)
        //{
        //    if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
        //        AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Bottom);
        //}
    }

    public void SettingData(MonsterInstance currentInstance)
    {
        currentMonsterInstance = currentInstance;
        skillInfoView.gameObject.SetActive(false);
        strategyView.gameObject.SetActive(false);

        selectDetailTargetCurrentIndex = currentInstance.selectDetailTargetTypes.FindIndex(x => x == currentInstance.currentSelectDetailTargetType);
        selectDetailTargetTypeMaxIndex = currentInstance.selectDetailTargetTypes.Count;
        CheckSelectDetailTargetInterect();
        selectDetailTargetText.text = currentInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex].ToString();

        confirmSkillPriorityCurrentIndex = currentInstance.confirmSkillPrioritys.FindIndex(x => x == currentInstance.currentConfirmSkillPriority);
        confirmSkillPriorityMaxIndex = currentInstance.confirmSkillPrioritys.Count;
        CheckConfirmSkillPriorityInterect();
        confirmSkillPriorityText.text = currentInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex].ToString();

        for(int i = 0; i < confirmSkillImages.Length; ++i)
        {
            confirmSkillImages[i].gameObject.SetActive(false);
            confirmSkillInfos[i].skillData = null;
            confirmSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentInstance.skillDatas.Count; ++i)
        {
            confirmSkillImages[i].gameObject.SetActive(true);
            confirmSkillImages[i].sprite = currentInstance.skillDatas[i].skillDictionary.skillIcon;
            confirmSkillInfos[i].skillData = currentInstance.skillDatas[i];
        }

        for (int i = 0; i < triggerSkillImages.Length; ++i)
        {
            triggerSkillImages[i].gameObject.SetActive(false);
            triggerSkillInfos[i].skillData = null;
            triggerSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentInstance.triggerSkillDatas.Count; ++i)
        {
            triggerSkillImages[i].gameObject.SetActive(true);
            triggerSkillImages[i].sprite = currentInstance.triggerSkillDatas[i].Item2.skillDictionary.skillIcon;
            triggerSkillInfos[i].skillData = currentInstance.triggerSkillDatas[i];
        }

        for (int i = 0; i < percentSkillImages.Length; ++i)
        {
            percentSkillImages[i].gameObject.SetActive(false);
            percentSkillInfos[i].skillData = null;
            percentSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentInstance.percentSkillDatas.Count; ++i)
        {
            percentSkillImages[i].gameObject.SetActive(true);
            percentSkillImages[i].sprite = currentInstance.percentSkillDatas[i].Item2.skillDictionary.skillIcon;
            percentSkillInfos[i].skillData = currentInstance.percentSkillDatas[i];
        }

        for (int i = 0; i < abilityImages.Length; ++i)
        {
            abilityImages[i].gameObject.SetActive(false);
            abilityInfos[i].abilityData = null;
            abilityInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        int abilCount = Mathf.Min(5, currentInstance.abilities.Count);
        for (int i = 0; i < abilCount; ++i)
        {
            abilityImages[i].gameObject.SetActive(true);
            abilityImages[i].sprite = MonsterDataBase.Instance.abilityDatas[currentInstance.abilities[i]].icon;
            abilityInfos[i].abilityData = MonsterDataBase.Instance.abilityDatas[currentInstance.abilities[i]];
        }
    }

    public void SettingData(MonsterData currentInstance)
    {
        currentMonsterInstance = null;
        skillInfoView.gameObject.SetActive(false);
        strategyView.gameObject.SetActive(false);

        selectDetailTargetCurrentIndex = currentInstance.selectDetailTargetTypes.FindIndex(x => x == currentInstance.selectDetailTargetTypes[0]);
        selectDetailTargetTypeMaxIndex = currentInstance.selectDetailTargetTypes.Count;
        CheckSelectDetailTargetInterect();
        selectDetailTargetText.text = currentInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex].ToString();

        confirmSkillPriorityCurrentIndex = currentInstance.confirmSkillPrioritys.FindIndex(x => x == currentInstance.confirmSkillPrioritys[0]);
        confirmSkillPriorityMaxIndex = currentInstance.confirmSkillPrioritys.Count;
        CheckConfirmSkillPriorityInterect();
        confirmSkillPriorityText.text = currentInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex].ToString();

        for (int i = 0; i < confirmSkillImages.Length; ++i)
        {
            confirmSkillImages[i].gameObject.SetActive(false);
            confirmSkillInfos[i].skillData = null;
            confirmSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentInstance.datas.Count; ++i)
        {
            confirmSkillImages[i].gameObject.SetActive(true);
            confirmSkillImages[i].sprite = currentInstance.datas[i].skillDictionary.skillIcon;
            confirmSkillInfos[i].skillData = currentInstance.datas[i];
        }

        for (int i = 0; i < triggerSkillImages.Length; ++i)
        {
            triggerSkillImages[i].gameObject.SetActive(false);
            triggerSkillInfos[i].skillData = null;
            triggerSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < currentInstance.triggerSkillDatas.Count; ++i)
        {
            triggerSkillImages[i].gameObject.SetActive(true);
            triggerSkillImages[i].sprite = currentInstance.triggerSkillDatas[i].skillData.skillDictionary.skillIcon;
            triggerSkillInfos[i].skillData = new Tuple<SkillTrigger, SkillData>(currentInstance.triggerSkillDatas[i].skillTrigger, currentInstance.triggerSkillDatas[i].skillData);
        }

        for (int i = 0; i < percentSkillImages.Length; ++i)
        {
            percentSkillImages[i].gameObject.SetActive(false);
            percentSkillInfos[i].skillData = null;
            percentSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < currentInstance.percentSkillDatas.Count; ++i)
        {
            percentSkillImages[i].gameObject.SetActive(true);
            percentSkillImages[i].sprite = currentInstance.percentSkillDatas[i].skillData.skillDictionary.skillIcon;
            percentSkillInfos[i].skillData = new Tuple<int, SkillData>(currentInstance.percentSkillDatas[i].percent, currentInstance.percentSkillDatas[i].skillData);
        }

        for (int i = 0; i < abilityImages.Length; ++i)
        {
            abilityImages[i].gameObject.SetActive(false);
            abilityInfos[i].abilityData = null;
            abilityInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentInstance.abilities.Count; ++i)
        {
            abilityImages[i].gameObject.SetActive(true);
            abilityImages[i].sprite = MonsterDataBase.Instance.abilityDatas[currentInstance.abilities[i]].icon;
            abilityInfos[i].abilityData = MonsterDataBase.Instance.abilityDatas[currentInstance.abilities[i]];
        }
    }

    public void ReSettingConfirmSkill()
    {
        for (int i = 0; i < confirmSkillImages.Length; ++i)
        {
            confirmSkillImages[i].gameObject.SetActive(false);
            confirmSkillInfos[i].skillData = null;
            confirmSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentMonsterInstance.skillDatas.Count; ++i)
        {
            confirmSkillImages[i].gameObject.SetActive(true);
            confirmSkillImages[i].sprite = currentMonsterInstance.skillDatas[i].skillDictionary.skillIcon;
            confirmSkillInfos[i].skillData = currentMonsterInstance.skillDatas[i];
        }
    }
    public void ReSettingPercentSkill()
    {
        for (int i = 0; i < percentSkillImages.Length; ++i)
        {
            percentSkillImages[i].gameObject.SetActive(false);
            percentSkillInfos[i].skillData = null;
            percentSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentMonsterInstance.percentSkillDatas.Count; ++i)
        {
            percentSkillImages[i].gameObject.SetActive(true);
            percentSkillImages[i].sprite = currentMonsterInstance.percentSkillDatas[i].Item2.skillDictionary.skillIcon;
            percentSkillInfos[i].skillData = currentMonsterInstance.percentSkillDatas[i];
        }
    }
    public void ReSettingTriggerSkill()
    {
        for (int i = 0; i < triggerSkillImages.Length; ++i)
        {
            triggerSkillImages[i].gameObject.SetActive(false);
            triggerSkillInfos[i].skillData = null;
            triggerSkillInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentMonsterInstance.triggerSkillDatas.Count; ++i)
        {
            triggerSkillImages[i].gameObject.SetActive(true);
            triggerSkillImages[i].sprite = currentMonsterInstance.triggerSkillDatas[i].Item2.skillDictionary.skillIcon;
            triggerSkillInfos[i].skillData = currentMonsterInstance.triggerSkillDatas[i];
        }
    }
    public void ReSettingAbility()
    {
        for (int i = 0; i < abilityImages.Length; ++i)
        {
            abilityImages[i].gameObject.SetActive(false);
            abilityInfos[i].abilityData = null;
            abilityInfos[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < currentMonsterInstance.abilities.Count; ++i)
        {
            abilityImages[i].gameObject.SetActive(true);
            abilityImages[i].sprite = MonsterDataBase.Instance.abilityDatas[currentMonsterInstance.abilities[i]].icon;
            abilityInfos[i].abilityData = MonsterDataBase.Instance.abilityDatas[currentMonsterInstance.abilities[i]];
        }
    }

    public void SettingGeneratorSkillView(object skillDataParent)
    {
        SkillData data = skillDataParent as SkillData;
        if(data != null)
        {
            SettingSkillView(data);
            //  description.text = data.skillDictionary.description;
            description.text = TextManager.Instance.GetString(data.skillDictionary.name + "Desc");
        }
        else
        {
            Tuple<SkillTrigger, SkillData> triggerData = skillDataParent as Tuple<SkillTrigger, SkillData>;
            if(triggerData != null)
            {
                SettingSkillView(triggerData.Item2);
                //   description.text = $"{triggerData.skillData.skillDictionary.description}\n\n<color=red>◈{MonsterDataBase.Instance.skillTriggerDic[triggerData.skillTrigger]}◈";
                bool checkTriggerRemove = CombatManager.Instance.CheckRemoveTrigger(triggerData.Item1);
                
                if(checkTriggerRemove == false)
                {
                    string continiousText = (TextManager.Instance.language == SystemLanguage.Korean) ? "지속 효과" : "Continuous Effect";
                    description.text = $"{TextManager.Instance.GetString(triggerData.Item2.skillDictionary.name + "Desc")}\n\n<color=red><b><size=38>◈{TextManager.Instance.GetString(triggerData.Item1.ToString())}◈\n\n<color=red><b><size=38>◈{continiousText}◈";
                }
                else
                    description.text = $"{TextManager.Instance.GetString(triggerData.Item2.skillDictionary.name + "Desc")}\n\n<color=red><b><size=38>◈{TextManager.Instance.GetString(triggerData.Item1.ToString())}◈";

            }
            else
            {
                Tuple<int, SkillData> percentData = skillDataParent as Tuple<int, SkillData>;
                if(percentData != null)
                {
                    SettingSkillView(percentData.Item2);
                    //   description.text = $"{percentData.skillData.skillDictionary.description}\n\n◈{percentData.percent}% 확률로◈";

                    string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "확률로" : "with probability";

                    description.text = $"{TextManager.Instance.GetString(percentData.Item2.skillDictionary.name + "Desc")}\n\n<color=red><b><size=38>◈{percentData.Item1}% {text}◈";
                }
            }
        }

        skillRemoveObject.SetActive((readOnly == false) && CheckSkillRemove(skillDataParent));
        cancelObj = skillDataParent;
    }

    public void SettingAbilityView(AbilityDictionaryData data)
    {
        strategyView.gameObject.SetActive(false);
        skillInfoView.gameObject.SetActive(true);
        backgroundView.gameObject.SetActive(false);
        description.gameObject.SetActive(true);
        dmgType.gameObject.SetActive(false);
        effectType.gameObject.SetActive(false);
        rangeType.gameObject.SetActive(false);
        eft.gameObject.SetActive(false);
        cri.gameObject.SetActive(false);
        acc.gameObject.SetActive(false);
        pay.gameObject.SetActive(false);
        atk.gameObject.SetActive(false);
        emptyText.gameObject.SetActive(false);

        skillRemoveObject.SetActive(readOnly == false);

        skillIcon.sprite = data.icon;

        //  skillName.text = data.abilityName;
        skillName.text = TextManager.Instance.GetString(data.name + "Name");

        // description.text = data.description;
        description.text = TextManager.Instance.GetString(data.name + "Desc");

        skillRemoveObject.SetActive(readOnly == false);
        cancelObj = data;
    }

    public void SettingSkillView(SkillData data)
    {
        strategyView.gameObject.SetActive(false);
        skillInfoView.gameObject.SetActive(true);
        backgroundView.gameObject.SetActive(true);

        description.gameObject.SetActive(true);
        dmgType.gameObject.SetActive(false);
        effectType.gameObject.SetActive(false);
        rangeType.gameObject.SetActive(false);
        eft.gameObject.SetActive(false);
        cri.gameObject.SetActive(false);
        acc.gameObject.SetActive(false);
        pay.gameObject.SetActive(false);
        atk.gameObject.SetActive(false);
        emptyText.gameObject.SetActive(false);

        skillIcon.sprite = data.skillDictionary.skillIcon;

       // skillName.text = $"{data.skillDictionary.skillName} / {data.status}";

        skillName.text = $"{TextManager.Instance.GetString(data.skillDictionary.name + "Name")} / {data.status}";

        //dmgType.text = $"DmgType : {data.skillDictionary.skillDmageType}";
        dmgType.text = $"DmgType : {GetDmgTypeText(data.skillDictionary.skillDmgType)}";
        // effectType.text = $"Effect  : {data.skillDictionary.addEffect}";
        effectType.text = $"Effect  : {GetSkillEffectText(data.skillDictionary.skillEffectType)}";
        rangeType.text = $"Range   : {data.skillDictionary.targetRange}";
        atk.text = $"ATK     : {data.atk}";
        eft.text = $"EPT     : {Mathf.FloorToInt(data.statusRatio * 100f)}%";
        cri.text = $"CRI     : {Mathf.FloorToInt(data.creaticalRatio * 100f)}%";
        acc.text = $"ACC     : {100 - Mathf.FloorToInt((data.repeatRatio) * 100f)}%";
        pay.text = $"PAY     : {data.consumMpAmount}";

        skillInfoPrev.interactable = false;
        skillInfoNext.interactable = true;
        currentIndexImage.sprite = MonsterDataBase.Instance.oneIcon;
    }

    //public void SettingStrageView(MonsterDictionaryData data)
    //{
    //    skillInfoView.gameObject.SetActive(false);
    //    strategyView.gameObject.SetActive(true);

    //    // strategyName.text = data.monsterName;

    //    strategyName.text = TextManager.Instance.GetString(data.name + "Name");

    //    // strategyDescription.text = data.description;
    //    strategyDescription.text = TextManager.Instance.GetString(data.name + "Desc");
    //}

    public void SettingSelectDetailTargetTypeView(SelectDetailTargetType type)
    {
        skillInfoView.gameObject.SetActive(false);
        strategyView.gameObject.SetActive(true);

        strategyName.text = TextManager.Instance.GetString("S_" + type.ToString() + "Name");
        strategyDescription.text = TextManager.Instance.GetString("S_" + type.ToString() + "Desc");
    }
    public void SettingConfirmSkillPriorityView(ConfirmSkillPriority type)
    {
        skillInfoView.gameObject.SetActive(false);
        strategyView.gameObject.SetActive(true);

        strategyName.text = TextManager.Instance.GetString("C_" + type.ToString() + "Name");
        strategyDescription.text = TextManager.Instance.GetString("C_" + type.ToString() + "Desc");
    }
    public void SelectDetailTargetNaxt()
    {
        if(selectDetailTargetCurrentIndex <= selectDetailTargetTypeMaxIndex)
        {
            selectDetailTargetCurrentIndex++;
            CheckSelectDetailTargetInterect();
            selectDetailTargetText.text = currentMonsterInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex].ToString();

            // SettingStrageView(MonsterDataBase.Instance.selectDetailTargetTypes[currentMonsterInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex]]);

            SettingSelectDetailTargetTypeView(currentMonsterInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex]);
            if (currentSelectBox != null)
            {
                currentSelectBox.SetActive(false);
                currentSelectBox = null;
            }
        }
    }
    public void SelectDetailTargetPrev()
    {
        if (selectDetailTargetCurrentIndex > 0)
        {
            selectDetailTargetCurrentIndex--;
            CheckSelectDetailTargetInterect();
            selectDetailTargetText.text = currentMonsterInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex].ToString();

            // SettingStrageView(MonsterDataBase.Instance.selectDetailTargetTypes[currentMonsterInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex]]);
            SettingSelectDetailTargetTypeView(currentMonsterInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex]);
            if (currentSelectBox != null)
            {
                currentSelectBox.SetActive(false);
                currentSelectBox = null;
            }
        }
    }

    public void ConfirmSkillPriorityNext()
    {
        if (confirmSkillPriorityCurrentIndex <= confirmSkillPriorityMaxIndex)
        {
            confirmSkillPriorityCurrentIndex++;
            CheckConfirmSkillPriorityInterect();
            confirmSkillPriorityText.text = currentMonsterInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex].ToString();

            //SettingStrageView(MonsterDataBase.Instance.confirmSkillDatas[currentMonsterInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex]]);
            SettingConfirmSkillPriorityView(currentMonsterInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex]);
            if (currentSelectBox != null)
            {
                currentSelectBox.SetActive(false);
                currentSelectBox = null;
            }
        }
    }
    public void ConfirmSkillPriorityPrev()
    {
        if (confirmSkillPriorityCurrentIndex > 0)
        {
            confirmSkillPriorityCurrentIndex--;
            CheckConfirmSkillPriorityInterect();
            confirmSkillPriorityText.text = currentMonsterInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex].ToString();

            // SettingStrageView(MonsterDataBase.Instance.confirmSkillDatas[currentMonsterInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex]]);
            SettingConfirmSkillPriorityView(currentMonsterInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex]);
            if (currentSelectBox != null)
            {
                currentSelectBox.SetActive(false);
                currentSelectBox = null;
            }
        }
    }
    
    public void SkillInfoViewNext()
    {
        skillInfoPrev.interactable = true;
        skillInfoNext.interactable = false;
        currentIndexImage.sprite = MonsterDataBase.Instance.twoIcon;

        description.gameObject.SetActive(false);
        dmgType.gameObject.SetActive(true);
        effectType.gameObject.SetActive(true);
        rangeType.gameObject.SetActive(true);
        eft.gameObject.SetActive(true);
        cri.gameObject.SetActive(true);
        acc.gameObject.SetActive(true);
        pay.gameObject.SetActive(true);
        atk.gameObject.SetActive(true);
        emptyText.gameObject.SetActive(true);
    }
    public void SkillInfoViewPrev()
    {
        skillInfoPrev.interactable = false;
        skillInfoNext.interactable = true;
        currentIndexImage.sprite = MonsterDataBase.Instance.oneIcon;

        description.gameObject.SetActive(true);
        dmgType.gameObject.SetActive(false);
        effectType.gameObject.SetActive(false);
        rangeType.gameObject.SetActive(false);
        eft.gameObject.SetActive(false);
        cri.gameObject.SetActive(false);
        acc.gameObject.SetActive(false);
        pay.gameObject.SetActive(false);
        atk.gameObject.SetActive(false);
        emptyText.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Closed();
    }

    public void PopUpAlarm()
    {
        if(ItemInventory.Instance.diamond >= 50)
        {
            AlarmUI.Instance.PopUpForKey("skillcancel", Cancel);
        }
        else
        {
            string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "<size=48>다이아몬드 50개가 \r\n\r\n필요합니다" : "<size=44>Requires 50 diamonds";
            AlarmUI.Instance.PopUp(text, null);
        }
    }
    public void Cancel()
    {
        SkillData data = cancelObj as SkillData;
        if(data != null)
        {
            currentMonsterInstance.skillDatas.Remove(data);
            ReSettingConfirmSkill();
        }
        else
        {
            Tuple<SkillTrigger, SkillData> triggerData = cancelObj as Tuple<SkillTrigger, SkillData>;
            if(triggerData != null)
            {
                currentMonsterInstance.triggerSkillDatas.RemoveAll(x => x.Item1 == triggerData.Item1);
                ReSettingTriggerSkill();

            }
            else
            {
                Tuple<int, SkillData> percentData = cancelObj as Tuple<int, SkillData>;
                if(percentData != null)
                {
                    currentMonsterInstance.percentSkillDatas.RemoveAll(x => x.Item2 == percentData.Item2);
                    ReSettingPercentSkill();
                }
                else
                {
                    AbilityDictionaryData abilityData = cancelObj as AbilityDictionaryData;
                    if(abilityData != null)
                    {
                        foreach (var pair in MonsterDataBase.Instance.abilityDatas)
                        {
                            if (pair.Value == abilityData)
                            {
                                currentMonsterInstance.abilities.Remove(pair.Key);
                                ReSettingAbility();
                                break;
                            }
                        }
                    }
                }
            }
        }

        skillInfoView.gameObject.SetActive(false);
        strategyView.gameObject.SetActive(false);
        ItemInventory.Instance.diamond -= 50;
        AlarmUI.Instance.Closed();
        
        if(TranningUI.Instance.isPopUp == true)
            TranningUI.Instance.SkillOrigin();
    }

    public void PopUpCustomEditUI()
    {
        CustomEditUI.Instance.PopUp(currentMonsterInstance.skillDatas);
    }

    private string GetDmgTypeText(SkillDmgType type)
    {
        string text = TextManager.Instance.GetString(type.ToString());

        return text;
    }
    private string GetSkillEffectText(SkillEffectType type)
    {
        string text = TextManager.Instance.GetString(type.ToString());

        return text;
    }
    private void CheckSelectDetailTargetInterect()
    {
        if(currentMonsterInstance != null)
        {
            selectDetailTargetNextButton.interactable = selectDetailTargetCurrentIndex < (selectDetailTargetTypeMaxIndex - 1);
            selectDetailTargetPrevButton.interactable = selectDetailTargetCurrentIndex > 0;
            currentMonsterInstance.currentSelectDetailTargetType = currentMonsterInstance.selectDetailTargetTypes[selectDetailTargetCurrentIndex];

            
        }
        else
        {
            selectDetailTargetNextButton.interactable = false;
            selectDetailTargetPrevButton.interactable = false;
        }
    }

    private void CheckConfirmSkillPriorityInterect()
    {
        if (currentMonsterInstance != null)
        {
            confirmSkillPriorityNextButton.interactable = confirmSkillPriorityCurrentIndex < (confirmSkillPriorityMaxIndex - 1);
            confirmSkillPriorityPrevButton.interactable = confirmSkillPriorityCurrentIndex > 0;
            currentMonsterInstance.currentConfirmSkillPriority = currentMonsterInstance.confirmSkillPrioritys[confirmSkillPriorityCurrentIndex];

            customEditObject.SetActive(currentMonsterInstance.currentConfirmSkillPriority == ConfirmSkillPriority.Custom);

            
        }
        else
        {
            confirmSkillPriorityNextButton.interactable = false;
            confirmSkillPriorityPrevButton.interactable = false;
            customEditObject.SetActive(false);
        }
            
    }

    private bool CheckSkillRemove(object skillDataParent)
    {
        SkillData data = skillDataParent as SkillData;
        if (data != null)
        {
            return currentMonsterInstance.skillDatas.Count > 1;
        }
        else
        {
            return true;
        }
    }
}

