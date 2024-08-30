using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    public MonsterItemSlot[] slots;
    public Animator[] blindAnims;
    public Button battleButton;
    public Button speedButton;
    public Button tranningButton;
    public Button surrendButton;
    public RectTransform speedButtonBackground;
    public TextMeshProUGUI battleTimerText;
    public Scrollbar scoller;
  

    public RectTransform battleView;
    public RectTransform gameblingView;
    public RectTransform gameblingView2;
    public Button[] gameblingButton;
    public TextMeshProUGUI currentMoneyText;
    public TextMeshProUGUI batMoneyText;
    public Image moneyPrefab;
    public RectTransform background4;

    public TextMeshProUGUI fieldTypeText;
    public TextMeshProUGUI saveTypeText;
    public Button saveButton;
    public Button deleteButton;
    public Button fieldNextButton;
    public Button fieldPrevButton;
    public Button saveNextButton;
    public Button savePrevButton;

    public Sprite speedModeSprite;
    public Sprite originModeSprite;
    public bool isPopUp { get; private set; }
    public bool isBlind { get; private set; }
    public bool isLocked { get; private set; }
    [System.NonSerialized]
    public bool isSpeed;

    [System.NonSerialized]
    public int batMoney;

    [System.NonSerialized]
    public int playerMoney;

    [System.NonSerialized]
    public bool itemFlag = true;

    [System.NonSerialized]
    public bool surrendFlag = true;

    [System.NonSerialized]
    public bool startTimeFlag = false;

 

    [System.NonSerialized]
    public bool saveButtonBlockFlag = false;

    [System.NonSerialized]
    public bool activeEdit = false;

    [System.NonSerialized]
    public List<Image> moneyIcons = new List<Image>();

    private List<float> alphaList;
    private Image[] alphaImages;
    private bool battleTimerTextBlock = false;
    private int moneySoundCount = 0;
    public Animator animator { get; private set; }

    private static CombatUI instance = null;
    public static CombatUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();

        for (int i = 0; i < slots.Length; ++i)
        {
            slots[i].Init();
        }
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(startTimeFlag == true && CombatManager.Instance.isStart == true && CombatManager.Instance.isEnd == false)
        {
            if (SkillManager.Instance.isBlock == true)
            {
                if (battleTimerTextBlock == true)
                {
                    battleTimerText.color = new Color(battleTimerText.color.r, battleTimerText.color.g, battleTimerText.color.b, 0.392156862745098f);
                    battleTimerTextBlock = false;
                }
            }
            else
            {
                if (battleTimerTextBlock == false)
                {
                    battleTimerText.color = new Color(battleTimerText.color.r, battleTimerText.color.g, battleTimerText.color.b, 1f);
                    battleTimerTextBlock = true;
                }

                RenderBattleTimer();
            }
        }
        else
        {
            if (battleTimerTextBlock == true)
            {
                battleTimerText.color = new Color(battleTimerText.color.r, battleTimerText.color.g, battleTimerText.color.b, 0.392156862745098f);
                battleTimerTextBlock = false;
            }
        }

        
    }

    public void RenderBattleTimer()
    {
        float time = CombatManager.Instance.battleTimer;
        int second = (int)time % 60;
        battleTimerText.text = second.ToString();
    }
    public void UpdateMonsterImages()
    {
        var datas = TranningUI.Instance.playerInventory.monsterDatas;
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i] != null)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].SettingData(i, datas[i]);
                if (datas[i].heathState == MonsterHeathState.Faint)
                    slots[i].Block();
                else
                {
                    if (datas[i].hp > 0)
                    {
                        slots[i].UnBlock();
                    }
                }
            }
            else
                slots[i].gameObject.SetActive(false);
        }
    }

   

  

   

    

    

   

    

    public void PopUp()
    {
        scoller.value = 0;
      
       

        moneySoundCount = 0;
        startTimeFlag = false;
        MainSystemUI.Instance.StopBgm();
        animator.Play("popup2");
        isPopUp = true;

        tranningButton.interactable = itemFlag;
        tranningButton.onClick.RemoveAllListeners();
        tranningButton.onClick.AddListener(PopUpTranningUI);
        surrendButton.interactable = surrendFlag;
        StartCoroutine(ZoomInRoutine());
    }
    public void PopUp2()
    {
        scoller.value = 0;
     
       

        moneySoundCount = 0;
        startTimeFlag = false;
        MainSystemUI.Instance.StopBgm();

        GameblingClearData();

        animator.Play("popup");
        isPopUp = true;
        playerMoney = ItemInventory.Instance.money;

        tranningButton.interactable = false;
        surrendButton.interactable = true;
        UpdateGameblingView();
        StartCoroutine(ZoomInRoutine());
    }
    public void PopUp3()
    {
        scoller.value = 0;
       
       

        moneySoundCount = 0;
        startTimeFlag = false;
        MainSystemUI.Instance.StopBgm();
        animator.Play("popup3");
        isPopUp = true;

        tranningButton.interactable = false;
        tranningButton.onClick.RemoveAllListeners();
   
        tranningButton.onClick.AddListener(() => 
        {
            var homegroundMonsters = CombatManager.Instance.homegroundMonsters;
            MonsterInstance[] datas = new MonsterInstance[homegroundMonsters.Count];
            for(int i = 0; i < homegroundMonsters.Count; ++i)
                datas[i] = homegroundMonsters[i].battleInstance;

            TranningUI.Instance.PopUpEdit(datas);
        });
        surrendButton.interactable = true;
        StartCoroutine(ZoomInRoutine());
    }

    public void PopUp4(MonsterInstance[] tranningButtonDatas)
    {
        scoller.value = 0;
      
      

        moneySoundCount = 0;
        startTimeFlag = false;
        MainSystemUI.Instance.StopBgm();
        animator.Play("popup2");
        isPopUp = true;

        tranningButton.interactable = itemFlag;
        tranningButton.onClick.RemoveAllListeners();
        tranningButton.onClick.AddListener(() =>
        {
            TranningUI.Instance.PopUpEdit(tranningButtonDatas);
        });
        surrendButton.interactable = surrendFlag;
        StartCoroutine(ZoomInRoutine());
    }

    public void Closed()
    {
        isPopUp = false;
        animator.Play("closed2");
        tranningButton.interactable = false;
        surrendButton.interactable = false;
        ClearBettingMoney();
    }
    public void Closed2()
    {
        isPopUp = false;
        animator.Play("closed");
        tranningButton.interactable = false;
        surrendButton.interactable = false;
    }
    public void Blind()
    {
        if (isBlind == false)
        {
            isBlind = true;
            alphaImages = GetComponentsInChildren<Image>();

            if (alphaList == null)
                alphaList = new List<float>();
            else
                alphaList.Clear();

            for(int i = 0; i < blindAnims.Length; ++i)
            {
                blindAnims[i].enabled = false;
            }

            for (int i = 0; i < alphaImages.Length; i++)
            {
                alphaList.Add(alphaImages[i].color.a);
                if (alphaImages[i].rectTransform.position.z <= 0)
                {
                    alphaImages[i].color = new Color(alphaImages[i].color.r, alphaImages[i].color.g, alphaImages[i].color.b, 0f);
                    alphaImages[i].raycastTarget = false;
                }
            }

           
        }
    }
    public void UnBlind()
    {
        if (isBlind == true)
        {
            isBlind = false;
            //var childs = GetComponentsInChildren<Image>();

            for (int i = 0; i < blindAnims.Length; ++i)
            {
                blindAnims[i].enabled = true;
            }
            for (int i = 0; i < alphaImages.Length; i++)
            {
                alphaImages[i].color = new Color(alphaImages[i].color.r, alphaImages[i].color.g, alphaImages[i].color.b, alphaList[i]);
                alphaImages[i].raycastTarget = true;
            }

            alphaImages = null;
            alphaList = null;
        }
    }

    public void LockSlot(MonsterItemSlot currentSlot = null)
    {
        if (isLocked == false)
        {
            isLocked = true;

            if (currentSlot == null)
            {
                var childs = GetComponentsInChildren<MonsterItemSlot>();
                for (int i = 0; i < childs.Length; i++)
                {
                    childs[i].isLock = true;
                }
            }
            else
            {
                var childs = GetComponentsInChildren<MonsterItemSlot>();
                for (int i = 0; i < childs.Length; i++)
                {
                    if (childs[i] != currentSlot)
                        childs[i].isLock = true;
                    else
                        childs[i].isLock = false;
                }
            }
        }
    }
    public void UnLockSlot()
    {
        if (isLocked == true)
        {
            isLocked = false;
            var childs = GetComponentsInChildren<MonsterItemSlot>();
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i].isLock = false;
            }
        }
    }

    public void SetSpeed()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        if (isSpeed == false)
        {
            isSpeed = true;
            CombatManager.Instance.SetDoubleSpeed();
            speedButton.image.sprite = originModeSprite;
        }
        else
        {
            isSpeed = false;
            CombatManager.Instance.SetOriginSpeed();
            speedButton.image.sprite = speedModeSprite;
        }
    }

    public void Bat_1()
    {
        moneySoundCount = 0;
        SoundManager.Instance.PlayEffect(166, 1f);

        playerMoney -= 1;
        batMoney += 1;
        StartCoroutine(MoveRoutine(gameblingButton[0].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(-272f, -373f)));
        UpdateGameblingView();
    }

    public void Bat_10()
    {
        moneySoundCount = 0;
        SoundManager.Instance.PlayEffect(166, 1f);

        playerMoney -= 10;
        batMoney += 10;
        StartCoroutine(MoveRoutine(gameblingButton[1].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(-119f, -366f)));
        UpdateGameblingView();
    }

    public void Bat_100()
    {
        moneySoundCount = 0;
        SoundManager.Instance.PlayEffect(166, 1f);

        playerMoney -= 100;
        batMoney += 100;
        StartCoroutine(MoveRoutine(gameblingButton[2].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(52f, -366f)));
        UpdateGameblingView();
    }

    public void Bat_1000()
    {
        moneySoundCount = 0;
        SoundManager.Instance.PlayEffect(166, 1f);

        playerMoney -= 1000;
        batMoney += 1000;
        StartCoroutine(MoveRoutine(gameblingButton[3].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(249f, -366f)));
        UpdateGameblingView();
    } 

    public void Allin()
    {
        moneySoundCount = 0;
        SoundManager.Instance.PlayEffect(166, 1f);

        int count = playerMoney / 1000;
        int value = playerMoney - (count * 1000);
        StartCoroutine(MoveRoutines(count, 3));

        count = value / 100;
        value = value - (count * 100);
        StartCoroutine(MoveRoutines(count, 2));

        count = value / 10;
        value = value - (count * 10);
        StartCoroutine(MoveRoutines(count, 1));

        count = value;
        StartCoroutine(MoveRoutines(count, 0));

        batMoney += playerMoney;
        playerMoney = 0;

        UpdateGameblingView();
    }

    public void PopUpTranningUI()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        TranningUI.Instance.PopUp(true);
    }
    public void Surrend()
    {
        moneySoundCount = 0;
        SoundManager.Instance.PlayEffect(166, 1f);
        var battleType = CombatManager.Instance.currentBattleType;

        if (battleType == BattleType.Story)
        {
            string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "정말 기권하시겠습니까?" : "Should I really abstain?";
            AlarmUI.Instance.PopUp(text, () =>
            {
                SoundManager.Instance.StopBgm();
                StoryUI.Instance.PopUp();
                AlarmUI.Instance.Closed();
            });
        }
        else if (battleType == BattleType.Official)
        {
            string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "정말 기권하시겠습니까?" : "Should I really abstain?";
            AlarmUI.Instance.PopUp(text, () =>
            {
                SoundManager.Instance.StopBgm();
                VictoryUI.Instance.PopUp(-1);
                AlarmUI.Instance.Closed();
            });
        }
        else if (battleType == BattleType.FriendShip)
        {

            string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "정말 기권하시겠습니까?" : "Should I really abstain?";
            AlarmUI.Instance.PopUp(text, () =>
            {
                SoundManager.Instance.StopBgm();
                VictoryUI.Instance.PopUp(-1);
                AlarmUI.Instance.Closed();
            });
        }
        else if (battleType == BattleType.Gamebling)
        {
            string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "하루동안 참관할 수 없습니다\r\n\r\n종료하시겠습니까?" : "<b><size=38>You cannot visit for one day\r\n\r\nShould I quit?";
            AlarmUI.Instance.PopUp(text, () =>
            {
                LeagueManager.Instance.blockGamebling = true;
                SoundManager.Instance.StopBgm();
                DialougeUI.Instance.PopUp();
                AlarmUI.Instance.Closed();
            });
        }

        CombatManager.Instance.maxLevel = 0;
        CombatManager.Instance.currentLevel = 0;
        itemFlag = true;
        surrendFlag = true;
    }

    public void ResetBet()
    {
        SoundManager.Instance.PlayEffect(166, 1f);

        playerMoney += batMoney;
        batMoney = 0;
        UpdateGameblingView();
    }

   

    public void SelectBetting()
    {
        moneySoundCount = 0;
        if(BatUI.Instance.homegroundMonster != null)
        {
            BettingData batData = new BettingData();
            batData.homeground = BatUI.Instance.homegroundMonster;
            batData.away = BatUI.Instance.awayMonster;
            batData.homegroundPercent = BatUI.Instance.homegroundPercent;
            batData.awayPercent = BatUI.Instance.awayPercent;
            batData.tiePercent = BatUI.Instance.tiePercent;
            BatUI.Instance.PopUp(batData);
        }
        else
        {
            BettingData batData = new BettingData();
            batData.homeground = CombatManager.Instance.homegroundMonsters[0].battleInstance;
            batData.away = CombatManager.Instance.awayMonsters[0].battleInstance;

            CombatManager.Instance.GetVictoryPercent(batData.homeground, batData.away, out batData.homegroundPercent, out batData.awayPercent, out batData.tiePercent);

            BatUI.Instance.PopUp(batData);
        }
    }

    private void GameblingClearData()
    {
        BatUI.Instance.ClearData();
        batMoney = 0;
        playerMoney = 0;
    }

    private void UpdateGameblingView()
    {
        Color donActiveColor = new Color(0.6666666666666667f, 0.5529411764705882f, 0.4784313725490196f, 1f);
        Color activeColor = new Color(0.1333333333333333f, 0.1333333333333333f, 0.1333333333333333f, 1f);
        gameblingButton[0].interactable = playerMoney >= 1;
        gameblingButton[0].transform.GetChild(1).gameObject.SetActive(playerMoney >= 1);
        gameblingButton[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = playerMoney >= 1 ? activeColor : donActiveColor;

        gameblingButton[1].interactable = playerMoney >= 10;
        gameblingButton[1].transform.GetChild(1).gameObject.SetActive(playerMoney >= 10);
        gameblingButton[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = playerMoney >= 10 ? activeColor : donActiveColor;

        gameblingButton[2].interactable = playerMoney >= 100;
        gameblingButton[2].transform.GetChild(1).gameObject.SetActive(playerMoney >= 100);
        gameblingButton[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = playerMoney >= 100 ? activeColor : donActiveColor;

        gameblingButton[3].interactable = playerMoney >= 1000;
        gameblingButton[3].transform.GetChild(1).gameObject.SetActive(playerMoney >= 1000);
        gameblingButton[3].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = playerMoney >= 1000 ? activeColor : donActiveColor;

        gameblingButton[4].interactable = batMoney > 0;
        gameblingButton[4].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = batMoney > 0 ? activeColor : donActiveColor;

        gameblingButton[5].interactable = playerMoney > 0;

        gameblingButton[6].interactable = batMoney > 0;

        currentMoneyText.text = $"${playerMoney}";
        batMoneyText.text = $"<b><size=36>x<b><size=48>{batMoney}";
    }
    private void StartTimeFlag()
    {
        startTimeFlag = true;
    }
    private void ClearBettingMoney()
    {
        moneySoundCount = 0;

        for(int i = 0; i < moneyIcons.Count; ++i)
        {
            if (moneyIcons[i] != null)
            {
                Destroy(moneyIcons[i].gameObject);
            }
        }
    }

  
    private IEnumerator MoveRoutine(Sprite sprite, Vector2 startPosition)
    {
        moneySoundCount++;
        if(moneySoundCount < 32)
            SoundManager.Instance.PlayEffect(171, 1f);

        Image clone = Instantiate(moneyPrefab, this.transform);
        clone.sprite = sprite;
        clone.rectTransform.anchoredPosition = startPosition;
        Vector2 endPosition = new Vector2(533.08f, 477f);
        moneyIcons.Add(clone);

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            clone.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        Destroy(clone.gameObject);
    }
    private IEnumerator MoveRoutines(int count, int index)
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        for (int i = 0; i < count; ++i)
        {
            if (index == 3)
                StartCoroutine(MoveRoutine(gameblingButton[3].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(249f, -366f)));
            else if (index == 2)
                StartCoroutine(MoveRoutine(gameblingButton[2].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(52f, -366f)));
            else if (index == 1)
                StartCoroutine(MoveRoutine(gameblingButton[1].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(-119f, -366f)));
            else if (index == 0)
                StartCoroutine(MoveRoutine(gameblingButton[0].transform.GetChild(1).GetComponent<Image>().sprite, new Vector2(-272f, -373f)));
            yield return waitTime;
        }
    }
    private IEnumerator SetOriginAnim()
    {
        yield return null;
        battleButton.gameObject.SetActive(false);
        speedButtonBackground.gameObject.SetActive(false);
        //speedButton.gameObject.SetActive(false);
        //battleTimerText.gameObject.SetActive(false);
        battleView.gameObject.SetActive(false);

        gameblingView.gameObject.SetActive(true);
        gameblingView2.gameObject.SetActive(true);

    //    background4.gameObject.SetActive(true);
    //    background4.Play("popup");

        background4.transform.GetChild(0).GetComponent<Button>().interactable = false;

        playerMoney = ItemInventory.Instance.money;
        UpdateGameblingView();
    }
    private IEnumerator ZoomInRoutine()
    {
        var mainCam = Camera.main;

        float startValue = 10.623f;
        //    float endValue = 5.623f;

        float endValue = mainCam.orthographicSize;

        PixelPerfectCamera pixelPerfect = mainCam.GetComponent<PixelPerfectCamera>();
        pixelPerfect.enabled = false;

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 0.5f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            mainCam.orthographicSize = Mathf.Lerp(startValue, endValue, currentSpeed);
            yield return null;
        }
        pixelPerfect.enabled = true;
        mainCam.orthographicSize = endValue;

       

       
    }

   
}
