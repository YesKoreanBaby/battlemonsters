using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    private static StatUI instance = null;
    public static StatUI Instance { get { return instance; } }
    public Image monsterImage;

    public TextMeshProUGUI monsterNameText;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI hpElementText;

    public TextMeshProUGUI mpText;
    public TextMeshProUGUI mpElementText;

    public TextMeshProUGUI atkText;
    public TextMeshProUGUI atkElementText;

    public TextMeshProUGUI defText;
    public TextMeshProUGUI defElementText;

    public TextMeshProUGUI dexText;
    public TextMeshProUGUI dexElementText;

    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI dodgeElementText;

    public TextMeshProUGUI criticalText;
    public TextMeshProUGUI criticalElementText;

    public TextMeshProUGUI hpRecoveryText;
    public TextMeshProUGUI hpRecoveryElementText;

    public TextMeshProUGUI mpRecoveryText;
    public TextMeshProUGUI mpRecoveryElementText;

    public TextMeshProUGUI tranningText;
    public TextMeshProUGUI battleTranningText;

    public TextMeshProUGUI monsterDescriptionText;

    public TextMeshProUGUI battlePercentText;
    public TextMeshProUGUI battleRecodeText;

    public Button nextButton;
    public Button prevButton;
    public Button exitButton;
    public Button monsterSellButton;

    public RectTransform[] fixedViews;

    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public int fixedViewIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Closed();
    }

    public void PopUp(MonsterInstance currentInstance, bool readOnly)
    {
        if(isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            fixedViewIndex = 0;
            CheckInterect();
            SettingFixedView();

            SettingData(currentInstance, readOnly);

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData != null && AdmobManager.Instance.adData.adsBlock == false)
            //    AdmobManager.Instance.DestroyBannerAds();
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

            fixedViewIndex = 0;
            CheckInterect();
            SettingFixedView();

            SettingData(monsterData);

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData != null && AdmobManager.Instance.adData.adsBlock == false)
            //    AdmobManager.Instance.DestroyBannerAds();

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

        //if (DialougeUI.Instance.isPopUp == true)
        //{
        //    if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
        //        AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);
        //}

        //if (ExternalUI.Instance.isPopUp == true)
        //{
        //    if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
        //        AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Bottom);
        //}
    }

    public void Next()
    {
        if(fixedViewIndex < fixedViews.Length)
        {
            SoundManager.Instance.PlayEffect(166, 1f);

            fixedViewIndex++;
            SettingFixedView();
        }

        CheckInterect();
    }

    public void Prev()
    {
        if (fixedViewIndex > 0)
        {
            SoundManager.Instance.PlayEffect(166, 1f);

            fixedViewIndex--;
            SettingFixedView();
        }

        CheckInterect();
    }

    public void Exit()
    {
        Closed();
    }

    public void SettingData(MonsterInstance currentInstance, bool readOnly)
    {
        bool editCheck = CombatUI.Instance.activeEdit;
        var tranningData = currentInstance.tranningCicleInstance;
        monsterImage.sprite = currentInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

        bool check = (currentInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (currentInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (currentInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
        float value = (currentInstance.monsterWeight != MonsterWeight.Big) ? 1 / 1.5f : 1 / 3.5f;
        if (check == true)
            value = 1 / 1.5f;
        monsterImage.SetNativeSize();
        monsterImage.rectTransform.sizeDelta = new Vector2(Mathf.Min(81.11113f, monsterImage.rectTransform.sizeDelta.x * value), Mathf.Min(81.11113f, monsterImage.rectTransform.sizeDelta.y * value));

        string monsterName = TextManager.Instance.GetString(currentInstance.monsterData.dictionaryData.name + "Name");
        monsterNameText.text = $"{monsterName} / {currentInstance.monsterData.status} / {currentInstance.monsterData.battleAIType}";


        float buffValue = editCheck ? 0 : tranningData.tranningData.hpData.buffValue * tranningData.hpLevel;

        float deadHp = 0, deadMp = 0, deadAtk = 0, deadDef = 0, deadDex = 0;
        float battleHp = 0, battleMp = 0, battleAtk = 0, battleDef = 0;

        if(editCheck == false)
        {
            currentInstance.SplitDeadTranning(out deadHp, out deadMp, out deadAtk, out deadDef, out deadDex);
            currentInstance.SplitBattleTranning(out battleHp, out battleMp, out battleAtk, out battleDef);
        }
      
        hpText.text = $"HP: {currentInstance.hp} / {currentInstance.maxHp}";
        hpElementText.text = $"({currentInstance.monsterData.hp} + {buffValue} + {deadHp} + {battleHp})";

        buffValue = editCheck ? 0 : tranningData.tranningData.mpData.buffValue * tranningData.mpLevel;
        mpText.text = $"MP: {currentInstance.mp} / {currentInstance.maxMp}";
        mpElementText.text = $"({currentInstance.monsterData.mp} + {buffValue} + {deadMp} + {battleMp})";

        buffValue = editCheck ? 0 : tranningData.tranningData.atkData.buffValue * tranningData.atkLevel;
        atkText.text = $"ATK: {currentInstance.atk}";
        atkElementText.text = $"({currentInstance.monsterData.atk} + {buffValue} + {deadAtk} + {battleAtk})";

        buffValue = editCheck ? 0 : tranningData.tranningData.defData.buffValue * tranningData.defLevel;
        defText.text = $"DEF: {currentInstance.def}";
        defElementText.text = $"({currentInstance.monsterData.def} + {buffValue} + {deadDef} + {battleDef})";

        buffValue = editCheck ? 0 : tranningData.tranningData.dexData.buffValue * tranningData.dexLevel;
        dexText.text = $"DEX: {currentInstance.maxDex}";
        dexElementText.text = $"({currentInstance.monsterData.dex} - {buffValue} - {deadDex} - {0})";

        buffValue = editCheck ? 0 : (tranningData.tranningData.dodgeData.buffValue * tranningData.dodgeLevel) * 100f;
        dodgeText.text = $"DDE: {(int)(currentInstance.repeatRatio * 100f)}%";
        dodgeElementText.text = $"({(int)(currentInstance.monsterData.repeatRatio * 100f)} + {(int)buffValue} + {0} + {0})";

        buffValue = editCheck ? 0 : (tranningData.tranningData.criticalData.buffValue * tranningData.criticalLevel) * 100f;
        criticalText.text = $"CRI: {(int)(currentInstance.creaticalRatio * 100f)}%";
        criticalElementText.text = $"({(int)(currentInstance.monsterData.creaticalRatio * 100f)} + {(int)buffValue} + {0} + {0})";

        buffValue = editCheck ? 0 : (tranningData.tranningData.hpRecoveryData.buffValue * tranningData.hpRecoveryLevel) * 100f;
        hpRecoveryText.text = $"HRC: {(int)(currentInstance.hpRecoveryRatio * 100f)}%";
        hpRecoveryElementText.text = $"({(int)(currentInstance.monsterData.hpRecoveryRatio * 100f)} + {(int)buffValue} + {0} + {0})";

        buffValue = editCheck ? 0 : (tranningData.tranningData.mpRecoveryData.buffValue * tranningData.mpRecoveryLevel) * 100f;
        mpRecoveryText.text = $"MRC: {(int)(currentInstance.manaRecoveryRatio * 100f)}%";
        mpRecoveryElementText.text = $"({(int)(currentInstance.monsterData.manaRecoveryRatio * 100f)} + {(int)buffValue} + {0} + {0})";

        //  monsterDescriptionText.text = currentInstance.monsterData.dictionaryData.description;

        monsterDescriptionText.text = TextManager.Instance.GetString(currentInstance.monsterData.dictionaryData.name + "Desc");

        tranningText.text = editCheck ? "Tranning: Read-Only" :  $"Tranning: {TextManager.Instance.GetString(currentInstance.tranningCicleInstance.tranningData.name)}";
        battleTranningText.text = editCheck ? "0/0" : $"Exp: <b><size=28>{Array.FindIndex(currentInstance.battleTrannings, x => x != BattleTranningType.None + 1)}/5";

        if(currentInstance.battleRecode != null)
        {

            string battleCount = (TextManager.Instance.language == SystemLanguage.Korean) ? "°æ±â ¼ö" : "<b><size=24>Battle Count";
            string winningPercent = (TextManager.Instance.language == SystemLanguage.Korean) ? "½Â·ü" : "<b><size=24>Winning Percent";
            int percent = Mathf.RoundToInt(((float)currentInstance.battleRecode.winCount / (float)currentInstance.battleRecode.battleCount) * 100f);
            battleRecodeText.text = $"{battleCount}: <b><size=28>{currentInstance.battleRecode.battleCount}";
            battlePercentText.text = $"{winningPercent}: <b><size=28>{percent}%";
        }
        else
        {
            string battleCount = (TextManager.Instance.language == SystemLanguage.Korean) ? "°æ±â ¼ö" : "<b><size=24>Battle Count";
            string winningPercent = (TextManager.Instance.language == SystemLanguage.Korean) ? "½Â·ü" : "<b><size=24>Winning Percent";

            battleRecodeText.text = $"{battleCount}: <b><size=28>{0}";
            battlePercentText.text = $"{winningPercent}: <b><size=28>{100}%";
        }

        if (readOnly == true)
        {
            monsterSellButton.gameObject.SetActive(false);
        }
        else
        {
            monsterSellButton.gameObject.SetActive(true);
            monsterSellButton.onClick.RemoveAllListeners();

            int price = (currentInstance.battleRecode == null) ? 0 : currentInstance.battleRecode.price;

            string text = $"¸ó½ºÅÍ¸¦ ÆÇ¸ÅÇÏ½Ã°Ú½À´Ï±î?\r\n\r\n<color=black><b><size=32>(+{price}$)";
            monsterSellButton.onClick.AddListener(() => { AlarmUI.Instance.PopUp(text, () => { SellMonster(currentInstance, price); }); });
        }
    }

    public void SettingData(MonsterData currentInstance)
    {
        monsterImage.sprite = currentInstance.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

        bool check = (currentInstance.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (currentInstance.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (currentInstance.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
        float value = (currentInstance.monsterWeight != MonsterWeight.Big) ? 1 / 1.5f : 1 / 3.5f;
        if (check == true)
            value = 1 / 1.5f;
        monsterImage.SetNativeSize();
        monsterImage.rectTransform.sizeDelta = new Vector2(Mathf.Min(81.11113f, monsterImage.rectTransform.sizeDelta.x * value), Mathf.Min(81.11113f, monsterImage.rectTransform.sizeDelta.y * value));

        string monsterName = TextManager.Instance.GetString(currentInstance.dictionaryData.name + "Name");
        monsterNameText.text = $"{monsterName} / {currentInstance.status} / {currentInstance.battleAIType}";

        hpText.text = $"HP: {currentInstance.hp} / {currentInstance.hp}";
        hpElementText.text = $"({currentInstance.hp} + {0} + {0} + {0})";

        mpText.text = $"MP: {currentInstance.mp} / {currentInstance.mp}";
        mpElementText.text = $"({currentInstance.mp} + {0} + {0} + {0})";

        atkText.text = $"ATK: {currentInstance.atk}";
        atkElementText.text = $"({currentInstance.atk} + {0} + {0} + {0})";

        defText.text = $"DEF: {currentInstance.def}";
        defElementText.text = $"({currentInstance.def} + {0} + {0} + {0})";

        dexText.text = $"DEX: {currentInstance.dex}";
        dexElementText.text = $"({currentInstance.dex} - {0} - {0} - {0})";

        dodgeText.text = $"DDE: {(int)(currentInstance.repeatRatio * 100f)}%";
        dodgeElementText.text = $"({(int)(currentInstance.repeatRatio * 100f)} + {0} + {0} + {0})";

        criticalText.text = $"CRI: {(int)(currentInstance.creaticalRatio * 100f)}%";
        criticalElementText.text = $"({(int)(currentInstance.creaticalRatio * 100f)} + {0} + {0} + {0})";

        hpRecoveryText.text = $"HRC: {(int)(currentInstance.hpRecoveryRatio * 100f)}%";
        hpRecoveryElementText.text = $"({(int)(currentInstance.hpRecoveryRatio * 100f)} + {0} + {0} + {0})";

        mpRecoveryText.text = $"MRC: {(int)(currentInstance.manaRecoveryRatio * 100f)}%";
        mpRecoveryElementText.text = $"({(int)(currentInstance.manaRecoveryRatio * 100f)} + {0} + {0} + {0})";

        //  monsterDescriptionText.text = currentInstance.monsterData.dictionaryData.description;

        monsterDescriptionText.text = TextManager.Instance.GetString(currentInstance.dictionaryData.name + "Desc");

        string empty = "";
        if (currentInstance.tranningType == TranningType.Late_Bloomer)
            empty = "<b><size=24>";
        tranningText.text = $"Tranning: {empty}{currentInstance.tranningType}";
        battleTranningText.text = $"Exp: <b><size=28>{0}/5";

        string battleCount = (TextManager.Instance.language == SystemLanguage.Korean) ? "°æ±â ¼ö" : "<b><size=24>Battle Count";
        string winningPercent = (TextManager.Instance.language == SystemLanguage.Korean) ? "½Â·ü" : "<b><size=24>Winning Percent";

        battleRecodeText.text = $"{battleCount}: <b><size=28>{0}";
        battlePercentText.text = $"{winningPercent}: <b><size=28>{100}%";

        monsterSellButton.gameObject.SetActive(false);
    }
    public void SettingFixedView()
    {
        for(int i = 0; i < fixedViews.Length; ++i)
        {
            fixedViews[i].gameObject.SetActive(false);
        }

        fixedViews[fixedViewIndex].gameObject.SetActive(true);
    }

    public void SellMonster(MonsterInstance currentInstance, int price)
    {
        SoundManager.Instance.PlayEffect(179, 1f);

        ItemInventory.Instance.money += price;
        var inventory = TranningUI.Instance.playerInventory;
        int index = inventory.FindIndex(currentInstance);
        inventory.RemoveItem(index);

        CombatUI.Instance.UpdateMonsterImages();
        TranningUI.Instance.SetMaxIndex();
        index = Array.FindIndex(inventory.monsterDatas, x => x != null);
        if(index != -1)
        {
            TranningUI.Instance.Origin();
            AlarmUI.Instance.Closed();
            Closed();
        }
        else
        {
            TranningUI.Instance.Closed2();
            AlarmUI.Instance.Closed();
            Closed();
            MainSystemUI.Instance.PopUp();
        }
    }

    private void CheckInterect()
    {
        nextButton.interactable = fixedViewIndex < (fixedViews.Length - 1);
        prevButton.interactable = fixedViewIndex > 0;
    }
}
