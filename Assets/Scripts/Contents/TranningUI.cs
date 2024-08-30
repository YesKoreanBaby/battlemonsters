using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TranningUI : MonoBehaviour
{
    private static TranningUI instance = null;
    public static TranningUI Instance { get { return instance; } }

    public MonsterInventory playerInventory;
    public Image monsterImage;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goalPercentText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI[] statTexts;
    public RectTransform assetTransform;
    public Button prevButton;
    public Button nextButton;
    public Button cancelButton;
    public TextMeshProUGUI flowingTextPrefab;
    public Image flowingImagePrefab;
    public Image hpImage;
    public Image heathImage;
    public Animator itemEffectImage;

    public Button[] selectButtons;

    public TextMeshProUGUI countText;


    [System.NonSerialized]
    public bool isPopUp = false;
    [System.NonSerialized]
    public bool isReadOnly = false;
    [System.NonSerialized]
    public MonsterInstance currentMonsterInstance;

    [System.NonSerialized]
    public bool[] isAnables;

    [System.NonSerialized]
    public string[] originString;

    private float dontActiveColorPercent = 0.5098039215686275f;

    private float activeColorPercent;

    private int currentMonsterIndex;
    private int maxIndex = 0;
    public int currentMonsterEditIndex;
    private int maxEditIndex = 0;

    private Image assetImage;
    private TextMeshProUGUI assetText;
    private Animator animator;
  
    [System.NonSerialized]
    public GameObject currentActiveBox = null;

    private void Awake()
    {
        instance = this;

        isAnables = new bool[selectButtons.Length];
        originString = new string[selectButtons.Length];
        for (int i = 0; i < selectButtons.Length; ++i)
        {
            isAnables[i] = false;
            originString[i] = selectButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            var slot = selectButtons[i].GetComponent<TranningButtonSlot>();
            slot.index = i;
        }

        activeColorPercent = selectButtons[0].GetComponent<Image>().color.a;

        assetImage = assetTransform.GetComponent<Image>();
        assetText = assetTransform.GetChild(0).GetComponent<TextMeshProUGUI>();

        animator = GetComponent<Animator>();

        Init();

        for (int i = 0; i < isAnables.Length; ++i)
            isAnables[i] = true;
    }
    public void Active(int index, bool isAnable)
    {
        var childTmp = selectButtons[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        isAnables[index] = isAnable;

        if(isAnable == false)
        {
            childTmp.text = "-";
            statTexts[index].gameObject.SetActive(false);
            childTmp.GetComponent<Animator>().Play("idle");
            childTmp.color = new Color(childTmp.color.r, childTmp.color.g, childTmp.color.b, dontActiveColorPercent);
            selectButtons[index].interactable = false;
        }
        else
        {
            childTmp.text = originString[index];
            statTexts[index].gameObject.SetActive(true);

            bool isActvie = CheckActvie(index);
            if(isActvie == false)
            {
                selectButtons[index].interactable = false;

                statTexts[index].text = "max";
                childTmp.GetComponent<Animator>().Play("max");
                ActiveBox(index, false);
                ViewActive(index, false);
            }
            else
            {
                childTmp.text = originString[index];
                selectButtons[index].interactable = true;
                childTmp.GetComponent<Animator>().Play("idle");
                childTmp.color = new Color(childTmp.color.r, childTmp.color.g, childTmp.color.b, activeColorPercent);
            }
        }
    }

    public void ViewActive(int index, bool isActvie)
    {
        if(isActvie == true)
        {
            levelText.gameObject.SetActive(true);
            goalPercentText.gameObject.SetActive(true);
            costText.gameObject.SetActive(true);
            assetTransform.gameObject.SetActive(true);
            SettingData(index);

        }
        else
        {
            levelText.gameObject.SetActive(false);
            goalPercentText.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            assetTransform.gameObject.SetActive(false);
        }
    }
    public void ViewActive(bool isActvie)
    {
        if (isActvie == true)
        {
            levelText.gameObject.SetActive(true);
            goalPercentText.gameObject.SetActive(true);
            costText.gameObject.SetActive(true);
            assetTransform.gameObject.SetActive(true);

        }
        else
        {
            levelText.gameObject.SetActive(false);
            goalPercentText.gameObject.SetActive(false);
            costText.gameObject.SetActive(false);
            assetTransform.gameObject.SetActive(false);
        }
    }
    public void PrevNextActive()
    {
        prevButton.interactable = CheckPrev();
        nextButton.interactable = CheckNext();
    }

    public void EditPrevNextActive(MonsterInstance[] datas)
    {
        prevButton.interactable = CheckEditPrev(datas);
        nextButton.interactable = CheckEditNext(datas);
    }
    public void PopUp(bool battlemode = false)
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(163, 1f);

            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            SetMaxIndex();

            monsterImage.sprite = playerInventory.monsterDatas[currentMonsterIndex].monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

            var prefab = playerInventory.monsterDatas[currentMonsterIndex].monsterData.monsterPrefab;
            bool check = (prefab == MonsterDataBase.Instance.checkDrake) || (prefab == MonsterDataBase.Instance.checkBasilisk) || (prefab == MonsterDataBase.Instance.checkLivingLegend);
            float value = (playerInventory.monsterDatas[currentMonsterIndex].monsterWeight != MonsterWeight.Big) ? 1 / 1.1f : 1 / 2.75f;
            if (check == true)
                value = 1 / 1.1f;
            monsterImage.SetNativeSize();
            monsterImage.rectTransform.sizeDelta = new Vector2(Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.x * value), Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.y * value));

            hpImage.fillAmount = playerInventory.monsterDatas[currentMonsterIndex].hp / playerInventory.monsterDatas[currentMonsterIndex].maxHp;

            var heathState = playerInventory.monsterDatas[currentMonsterIndex].heathState;
            heathImage.gameObject.SetActive(heathState != MonsterHeathState.None);
            if (heathState != MonsterHeathState.None)
                heathImage.sprite = MonsterDataBase.Instance.heathIcons[heathState];

            cancelButton.onClick.RemoveAllListeners();
            if (battlemode == false)
                cancelButton.onClick.AddListener(Cancel);
            else
                cancelButton.onClick.AddListener(Closed);

            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(Next);

            prevButton.onClick.RemoveAllListeners();
            prevButton.onClick.AddListener(Prev);

            isReadOnly = battlemode;

            currentMonsterInstance = playerInventory.monsterDatas[currentMonsterIndex];

            for(int i = 0; i < selectButtons.Length; ++i)
            {
                SettingData(i);
                Active(i, isAnables[i]);
            }

            animator.Play("popup");

            ViewActive(false);
            PrevNextActive();

            SetCountText();
        }
    }
    public void PopUpEdit(MonsterInstance[] datas)
    {
        SoundManager.Instance.PlayEffect(163, 1f);

        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }

        SetMaxIndexEdit(datas, CombatUI.Instance.activeEdit);
       

        var currentMonsterInstance = datas[currentMonsterEditIndex];
        monsterImage.sprite = currentMonsterInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

        var prefab = currentMonsterInstance.monsterData.monsterPrefab;
        bool check = (prefab == MonsterDataBase.Instance.checkDrake) || (prefab == MonsterDataBase.Instance.checkBasilisk) || (prefab == MonsterDataBase.Instance.checkLivingLegend);
        float value = (currentMonsterInstance.monsterWeight != MonsterWeight.Big) ? 1 / 1.1f : 1 / 2.75f;
        if (check == true)
            value = 1 / 1.1f;
        monsterImage.SetNativeSize();
        monsterImage.rectTransform.sizeDelta = new Vector2(Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.x * value), Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.y * value));

        hpImage.fillAmount = currentMonsterInstance.hp / currentMonsterInstance.maxHp;

        heathImage.gameObject.SetActive(false);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(Closed);
     

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() => { EditNext(datas); });

        prevButton.onClick.RemoveAllListeners();
        prevButton.onClick.AddListener(() => { EditPrev(datas); });

        isReadOnly = true;

        this.currentMonsterInstance = currentMonsterInstance;

        for (int i = 0; i < selectButtons.Length; ++i)
            Active(i, false);

        animator.Play("popup");

        ViewActive(false);
        EditPrevNextActive(datas);

        SetEditCountText();
    }

    public void Init()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void Closed()
    {
        if(isPopUp == true)
            animator.Play("closed");
    }
    public void Closed2()
    {
        if (isPopUp == true)
            StartCoroutine(SetClosed());
    }

    public bool CheckActvie(int index)
    {
        var tranningData = currentMonsterInstance.tranningCicleInstance.tranningData;
        if(index == 0)
        {
            return currentMonsterInstance.tranningCicleInstance.hpLevel < tranningData.hpData.level;
        }
        else if(index == 1)
        {
            return currentMonsterInstance.tranningCicleInstance.mpLevel < tranningData.mpData.level;
        }
        else if(index == 2)
        {
            return currentMonsterInstance.tranningCicleInstance.dexLevel < tranningData.dexData.level;
        }
        else if(index == 3)
        {
            return currentMonsterInstance.tranningCicleInstance.atkLevel < tranningData.atkData.level;
        }
        else if (index == 4)
        {
            return currentMonsterInstance.tranningCicleInstance.defLevel < tranningData.defData.level;
        }
        else if (index == 5)
        {
            return currentMonsterInstance.tranningCicleInstance.dodgeLevel < tranningData.dodgeData.level;
        }
        else if (index == 6)
        {
            return currentMonsterInstance.tranningCicleInstance.criticalLevel < tranningData.criticalData.level;
        }
        else if (index == 7)
        {
            return currentMonsterInstance.tranningCicleInstance.hpRecoveryLevel < tranningData.hpRecoveryData.level;
        }
        else if (index == 8)
        {
            return currentMonsterInstance.tranningCicleInstance.mpRecoveryLevel < tranningData.mpRecoveryData.level;
        }
        else if (index == 9)
        {
            return currentMonsterInstance.tranningCicleInstance.skillLevel < currentMonsterInstance.tranningCicleInstance.skillMaxLevel;
        }
        else if (index == 10)
        {
            return currentMonsterInstance.tranningCicleInstance.iqLevel < currentMonsterInstance.tranningCicleInstance.iqMaxLevel;
        }
        else
        {
            return currentMonsterInstance.tranningCicleInstance.abilityLevel < currentMonsterInstance.tranningCicleInstance.abilityMaxLevel;
        }
    }
    public bool CheckPrev()
    {
        if (currentMonsterInstance == playerInventory.monsterDatas[0])
            return false;
        else
            return true;
    }
    public bool CheckNext()
    {
        if (currentMonsterInstance == playerInventory.monsterDatas[maxIndex - 1])
            return false;
        else
            return true;
    }

    public bool CheckEditPrev(MonsterInstance[] datas)
    {
        if (currentMonsterInstance == datas[0])
            return false;
        else
            return true;
    }

    public bool CheckEditNext(MonsterInstance[] datas)
    {

        if (currentMonsterInstance == datas[maxEditIndex - 1])
            return false;
        else
            return true;
    }

    public void Next()
    {
        if(currentMonsterIndex < maxIndex)
        {
            SoundManager.Instance.PlayEffect(166, 1f);
            if (currentActiveBox != null)
            {
                currentActiveBox.gameObject.SetActive(false);
                currentActiveBox = null;
            }
            currentMonsterIndex++;
            UpdateMonsterStatus();
        }
    }
    public void Prev()
    {
        if(currentMonsterIndex > 0)
        {
            SoundManager.Instance.PlayEffect(166, 1f);
            if (currentActiveBox != null)
            {
                currentActiveBox.gameObject.SetActive(false);
                currentActiveBox = null;
            }

            currentMonsterIndex--;
            UpdateMonsterStatus();
        }
    }
    public void EditNext(MonsterInstance[] datas)
    {
        if (currentMonsterEditIndex < maxEditIndex)
        {
            SoundManager.Instance.PlayEffect(166, 1f);
            currentMonsterEditIndex++;

            UpdateEditMonsterStatus(datas);
        }
    }

    public void EditPrev(MonsterInstance[] datas)
    {
        if (currentMonsterEditIndex > 0)
        {
            SoundManager.Instance.PlayEffect(166, 1f);
            currentMonsterEditIndex--;

            UpdateEditMonsterStatus(datas);
        }
    }

    public void SetMaxIndex()
    {
        maxIndex = playerInventory.FindEmptyIndex();
        if (maxIndex == -1)
            maxIndex = playerInventory.monsterDatas.Length;
    }

    public void SetMaxIndexEdit(MonsterInstance[] datas, bool editCheck)
    {
        if(editCheck == true)
            maxEditIndex = datas.Length;
        else
        {
            maxEditIndex = Array.FindIndex(datas, x => x == null);
            if(maxEditIndex == -1)
                maxEditIndex = datas.Length;
        }
        currentMonsterEditIndex = 0;
    }

    public void Origin()
    {
        if (currentActiveBox != null)
        {
            currentActiveBox.gameObject.SetActive(false);
            currentActiveBox = null;
        }

        currentMonsterIndex = 0;
        int index = Array.FindIndex(playerInventory.monsterDatas, x => x != null);

        if(index != -1)
            UpdateMonsterStatus();
    }

    public void SkillOrigin()
    {
        if (currentActiveBox != null)
        {
            currentActiveBox.gameObject.SetActive(false);
            currentActiveBox = null;

            ViewActive(false);
        }
    }

    public void UpdateMonsterStatus()
    {
        monsterImage.sprite = playerInventory.monsterDatas[currentMonsterIndex].monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
        var prefab = playerInventory.monsterDatas[currentMonsterIndex].monsterData.monsterPrefab;
        bool check = (prefab == MonsterDataBase.Instance.checkDrake) || (prefab == MonsterDataBase.Instance.checkBasilisk) || (prefab == MonsterDataBase.Instance.checkLivingLegend);
        float value = (playerInventory.monsterDatas[currentMonsterIndex].monsterWeight != MonsterWeight.Big) ? 1 / 1.1f : 1 / 2.75f;
        if (check == true)
            value = 1 / 1.1f;
        monsterImage.SetNativeSize();
        monsterImage.rectTransform.sizeDelta = new Vector2(Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.x * value), Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.y * value));

        hpImage.fillAmount = playerInventory.monsterDatas[currentMonsterIndex].hp / playerInventory.monsterDatas[currentMonsterIndex].maxHp;
        currentMonsterInstance = playerInventory.monsterDatas[currentMonsterIndex];

        var heathState = playerInventory.monsterDatas[currentMonsterIndex].heathState;
        heathImage.gameObject.SetActive(heathState != MonsterHeathState.None);
        if (heathState != MonsterHeathState.None)
            heathImage.sprite = MonsterDataBase.Instance.heathIcons[heathState];

        for (int i = 0; i < selectButtons.Length; ++i)
        {
            SettingData(i);
            Active(i, isAnables[i]);
        }
        ViewActive(false);
        PrevNextActive();

        SetCountText();
    }

    public void UpdateEditMonsterStatus(MonsterInstance[] datas)
    {
        this.currentMonsterInstance = datas[currentMonsterEditIndex];

        monsterImage.sprite = currentMonsterInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

        var prefab = currentMonsterInstance.monsterData.monsterPrefab;
        bool check = (prefab == MonsterDataBase.Instance.checkDrake) || (prefab == MonsterDataBase.Instance.checkBasilisk) || (prefab == MonsterDataBase.Instance.checkLivingLegend);
        float value = (currentMonsterInstance.monsterWeight != MonsterWeight.Big) ? 1 / 1.1f : 1 / 2.75f;
        if (check == true)
            value = 1 / 1.1f;
        monsterImage.SetNativeSize();
        monsterImage.rectTransform.sizeDelta = new Vector2(Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.x * value), Mathf.Min(110.7143f, monsterImage.rectTransform.sizeDelta.y * value));

        hpImage.fillAmount = currentMonsterInstance.hp / currentMonsterInstance.maxHp;

        heathImage.gameObject.SetActive(false);

        for (int i = 0; i < selectButtons.Length; ++i)
        {
            Active(i, false);
        }
        ViewActive(false);
        EditPrevNextActive(datas);

        SetEditCountText();
    }

    public void PopUpInfo()
    {
        StatUI.Instance.PopUp(currentMonsterInstance, isReadOnly);
        itemEffectImage.Play("idle");
    }

    public void PopUpSkillInfo()
    {
        SkillUI.Instance.PopUp(currentMonsterInstance);
        itemEffectImage.Play("idle");
    }

    public void PopUpItemInfo()
    {
        ItemInventoryUI.Instance.PopUp();
        itemEffectImage.Play("idle");
    }

    public void SettingData(int index)
    {
        var tranningData = currentMonsterInstance.tranningCicleInstance.tranningData;
        if (index == 0)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.hpLevel}/{tranningData.hpData.level}";

            //퍼센트텍스트
            int goalPercent = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.hpGoalPercent * 100);
            goalPercentText.text = $"{goalPercent}%";
            goalPercentText.color = (goalPercent > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.hpCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.hpData.costItemType) < currentMonsterInstance.tranningCicleInstance.hpCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = ((int)currentMonsterInstance.maxHp).ToString();

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.hpData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.hpData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.hpData.costItemType) < currentMonsterInstance.tranningCicleInstance.hpCost) ? Color.red : Color.blue;
        }
        else if(index == 1)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.mpLevel}/{tranningData.mpData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.mpGoalPercent* 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.mpCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.mpData.costItemType) < currentMonsterInstance.tranningCicleInstance.mpCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = ((int)currentMonsterInstance.maxMp).ToString();

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.mpData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.mpData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.mpData.costItemType) < currentMonsterInstance.tranningCicleInstance.mpCost) ? Color.red : Color.blue;
        }
        else if (index == 2)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.dexLevel}/{tranningData.dexData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.dexGoalPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.dexCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.dexData.costItemType) < currentMonsterInstance.tranningCicleInstance.dexCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = (currentMonsterInstance.maxDex.ToString());

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.dexData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.dexData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.dexData.costItemType) < currentMonsterInstance.tranningCicleInstance.dexCost) ? Color.red : Color.blue;
        }
        else if(index == 3)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.atkLevel}/{tranningData.atkData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.atkGoalPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.atkCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.atkData.costItemType) < currentMonsterInstance.tranningCicleInstance.atkCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = ((int)currentMonsterInstance.atk).ToString();

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.atkData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.atkData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.atkData.costItemType) < currentMonsterInstance.tranningCicleInstance.atkCost) ? Color.red : Color.blue;
        }
        else if (index == 4)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.defLevel}/{tranningData.defData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.defGoalPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.defCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.defData.costItemType) < currentMonsterInstance.tranningCicleInstance.defCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = ((int)currentMonsterInstance.def).ToString();

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.defData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.defData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.defData.costItemType) < currentMonsterInstance.tranningCicleInstance.defCost) ? Color.red : Color.blue;
        }
        else if (index == 5)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.dodgeLevel}/{tranningData.dodgeData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.dodgeGoalPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.dodgeCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.dodgeData.costItemType) < currentMonsterInstance.tranningCicleInstance.dodgeCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = (currentMonsterInstance.repeatRatio.ToString());

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.dodgeData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.dodgeData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.dodgeData.costItemType) < currentMonsterInstance.tranningCicleInstance.dodgeCost) ? Color.red : Color.blue;
        }
        else if (index == 6)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.criticalLevel}/{tranningData.criticalData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.criticalGoalPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.criticalCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.criticalData.costItemType) < currentMonsterInstance.tranningCicleInstance.criticalCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = (currentMonsterInstance.creaticalRatio.ToString());

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.criticalData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.criticalData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.criticalData.costItemType) < currentMonsterInstance.tranningCicleInstance.criticalCost) ? Color.red : Color.blue;
        }
        else if(index == 7)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.hpRecoveryLevel}/{tranningData.hpRecoveryData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.hpRecoveryGoalPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.hpRecoveryCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.hpRecoveryData.costItemType) < currentMonsterInstance.tranningCicleInstance.hpRecoveryCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = (currentMonsterInstance.hpRecoveryRatio.ToString());

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.hpRecoveryData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.hpRecoveryData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.hpRecoveryData.costItemType) < currentMonsterInstance.tranningCicleInstance.hpRecoveryCost) ? Color.red : Color.blue;
        }
        else if (index == 8)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.mpRecoveryLevel}/{tranningData.mpRecoveryData.level}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.mpRecoveryGoalPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.mpRecoveryCost}";
            costText.color = (ItemInventory.Instance.GetData(tranningData.mpRecoveryData.costItemType) < currentMonsterInstance.tranningCicleInstance.mpRecoveryCost) ? Color.red : Color.blue;

            //스탯텍스트
            statTexts[index].text = (currentMonsterInstance.manaRecoveryRatio.ToString());

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(tranningData.mpRecoveryData.costItemType);
            ItemInventory.Instance.PlayDecrease(assetText, tranningData.mpRecoveryData.costItemType);
            assetText.color = (ItemInventory.Instance.GetData(tranningData.mpRecoveryData.costItemType) < currentMonsterInstance.tranningCicleInstance.mpRecoveryCost) ? Color.red : Color.blue;
        }
        else if(index == 9)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.skillLevel}/{currentMonsterInstance.tranningCicleInstance.skillMaxLevel}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.skillPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.skillCost}";
            costText.color = (ItemInventory.Instance.GetData(CostItemType.Diamond) < currentMonsterInstance.tranningCicleInstance.skillCost) ? Color.red : Color.blue;

            //스탯텍스트
            int currentSkillCount = (currentMonsterInstance.skillDatas.Count + currentMonsterInstance.triggerSkillDatas.Count + currentMonsterInstance.percentSkillDatas.Count);
            int maxSkillCount = currentSkillCount + (currentMonsterInstance.possibleSkillDatas.Count + currentMonsterInstance.possibleTriggerSkillDatas.Count + currentMonsterInstance.possiblePercentSkillDatas.Count);
            statTexts[index].text = $" {currentSkillCount} / {maxSkillCount}";

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Diamond);
            ItemInventory.Instance.PlayDecrease(assetText, CostItemType.Diamond);
            assetText.color = (ItemInventory.Instance.GetData(CostItemType.Diamond) < currentMonsterInstance.tranningCicleInstance.skillCost) ? Color.red : Color.blue;
        }
        else if(index == 10)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.iqLevel}/{currentMonsterInstance.tranningCicleInstance.iqMaxLevel}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.iqPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.iqCost}";
            costText.color = (ItemInventory.Instance.GetData(CostItemType.Money) < currentMonsterInstance.tranningCicleInstance.iqCost) ? Color.red : Color.blue;

            //스탯텍스트
            int currentSkillCount = (currentMonsterInstance.selectDetailTargetTypes.Count + currentMonsterInstance.confirmSkillPrioritys.Count);
            int maxSkillCount = currentSkillCount + (currentMonsterInstance.possibleSelectDetailTargetTypeDatas.Count + currentMonsterInstance.possibleConfirmSkillPriorityDatas.Count);
            statTexts[index].text = $" {currentSkillCount} / {maxSkillCount}";

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Money);
            ItemInventory.Instance.PlayDecrease(assetText, CostItemType.Money);
            assetText.color = (ItemInventory.Instance.GetData(CostItemType.Money) < currentMonsterInstance.tranningCicleInstance.iqCost) ? Color.red : Color.blue;
        }
        else if(index == 11)
        {
            //레벨텍스트
            levelText.text = $"{currentMonsterInstance.tranningCicleInstance.abilityLevel}/{currentMonsterInstance.tranningCicleInstance.abilityMaxLevel}";

            //퍼센트텍스트
            int goalPercnet = Mathf.RoundToInt(currentMonsterInstance.tranningCicleInstance.abilityPercent * 100);
            goalPercentText.text = $"{goalPercnet}%";
            goalPercentText.color = (goalPercnet > 50) ? Color.blue : Color.red;

            //코스트텍스트
            costText.text = $"-{currentMonsterInstance.tranningCicleInstance.abilityCost}";
            costText.color = (ItemInventory.Instance.GetData(CostItemType.Diamond) < currentMonsterInstance.tranningCicleInstance.abilityCost) ? Color.red : Color.blue;

            //스탯텍스트
            int currentSkillCount = currentMonsterInstance.abilities.Count;
            int maxSkillCount = currentSkillCount + currentMonsterInstance.possibleAbilitieDatas.Count;
            statTexts[index].text = $" {currentSkillCount} / {maxSkillCount}";

            //에셋텍스트
            assetImage.sprite = ItemInventory.Instance.GetSprite(CostItemType.Diamond);
            ItemInventory.Instance.PlayDecrease(assetText, CostItemType.Diamond);
            assetText.color = (ItemInventory.Instance.GetData(CostItemType.Diamond) < currentMonsterInstance.tranningCicleInstance.abilityCost) ? Color.red : Color.blue;
        }
    }

    public void UpdateData(int index)
    {
        float value = 0f;
        string textValue = "";
        Sprite imageValue = null;
        bool isActvie = CheckActvie(index);
        if(isActvie == true)
        {
            var tranningData = currentMonsterInstance.tranningCicleInstance;
            if (index == 0)
            {
                value = tranningData.TranningHp();
            }
            else if (index == 1)
            {
                value = tranningData.TranningMp();
            }
            else if (index == 2)
            {
                value = tranningData.TranningDex();
            }
            else if (index == 3)
            {
                value = tranningData.TranningAtk();
            }
            else if (index == 4)
            {
                value = tranningData.TranningDef();
            }
            else if (index == 5)
            {
                value = tranningData.TranningDodge();
            }
            else if (index == 6)
            {
                value = tranningData.TranningCritical();
            }
            else if (index == 7)
            {
                value = tranningData.TranningHpRecovery();
            }
            else if (index == 8)
            {
                value = tranningData.TranningMpRecovery();
            }
            else if (index == 9)
            {
                value = tranningData.TranningSkill(out imageValue);
            }
            else if (index == 10)
            {
                value = tranningData.TranningIq(out textValue);
            }
            else if (index == 11)
            {
                value = tranningData.TranningAbility(out imageValue);
            }

            SettingData(index);
            Active(index, isAnables[index]);

            if(imageValue != null)
                StartCoroutine(FlowingImageRoutine(imageValue));
            else
                StartCoroutine(FlowingTextRoutine(value, textValue));

        }
    }
    public void ActiveBox(int index, bool active)
    {
        var obj = selectButtons[index].transform.GetChild(1).gameObject;
        if (active)
        {
            if(currentActiveBox != null)
                currentActiveBox.gameObject.SetActive(false);

            obj.SetActive(true);
            currentActiveBox = obj;
        }
        else
        {
            obj.SetActive(false);
            currentActiveBox = null;
        }
    }

    public void SetItemEffect()
    {
        itemEffectImage.Play("start");
    }
    public void SetFailed()
    {
        StartCoroutine(FlowingTextRoutine(-1f, ""));
    }

    public void Cancel()
    {
        StartCoroutine(CancelRoutine());
    }

    private void PopUpSetting()
    {
        isPopUp = true;
    }

    private void ClosedSetting()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (currentActiveBox != null)
        {
            currentActiveBox.gameObject.SetActive(false);
            currentActiveBox = null;
        }
    }

    private void SetCountText()
    {
        countText.text = (currentMonsterIndex + 1).ToString();
    }
    private void SetEditCountText()
    {
        countText.text = (currentMonsterEditIndex + 1).ToString();
    }
    private IEnumerator FlowingTextRoutine(float value, string skillValue)
    {
        var clone = Instantiate(flowingTextPrefab, transform);
        clone.rectTransform.localPosition = new Vector2(-320f, 405f);

        clone.text = (value == -1f) ? "Failed" : (skillValue == "") ? $"+{value}" : skillValue;
        clone.color = (value == -1f) ? Color.red : Color.blue;

        if(value != -1f)
            SoundManager.Instance.PlayEffect(171, 1f);
        else
            SoundManager.Instance.PlayEffect(172, 1f);

        Vector2 startPosition = clone.rectTransform.localPosition;
        Vector2 endPosition = clone.rectTransform.localPosition + new Vector3(0f, 100f);

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            clone.rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        Destroy(clone.gameObject);
    }

    private IEnumerator FlowingImageRoutine(Sprite image)
    {
        SoundManager.Instance.PlayEffect(171, 1f);

        var clone = Instantiate(flowingImagePrefab, transform);
        clone.sprite = image;
        clone.rectTransform.localPosition = new Vector2(-320f, 405f);

        Vector2 startPosition = clone.rectTransform.localPosition;
        Vector2 endPosition = clone.rectTransform.localPosition + new Vector3(0f, 100f);

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            clone.rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        Destroy(clone.gameObject);
    }

    private IEnumerator CancelRoutine()
    {
        Closed();

        yield return new WaitForSeconds(0.45f);

        MainSystemUI.Instance.PopUp(false);
    }

    private IEnumerator SetClosed()
    {
        animator.Play("closedidle");
        yield return null;
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (currentActiveBox != null)
        {
            currentActiveBox.gameObject.SetActive(false);
            currentActiveBox = null;
        }
    }
}
