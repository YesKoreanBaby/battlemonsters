using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class VictoryUI : MonoBehaviour
{
    public TypingText text;
    public Image panel;
    public Button[] buttons;
    public TextMeshProUGUI floorText;

    public Image current;
    public Image next;
    public Image prev;

    public Image currentPvP;
    public Image nextPvP;
    public Image prevPvP;

    public RectTransform cancelButton;

    public TextMeshProUGUI moneyText;
    public RectTransform cancelButton2;
    public RectTransform cancelButton3;

    public bool isPopUp { get; private set; }
    public int windAndLose { get; private set; }
    private Animator animtor = null;
    private static VictoryUI instance = null;
    public static VictoryUI Instance { get { return instance; } }

    ConversationData popupFriendShipGradeData;
    Color originColor;
    Color disableColor = new Color(0f, 0f, 0f, 0.75f);
    private void Awake()
    {
        instance = this;
        originColor = buttons[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;

        animtor = GetComponent<Animator>();
        Init();
    } 
    public void PopUp(int windAndLose)
    {
        if (isPopUp == false)
        {
            cancelButton.gameObject.SetActive(false);
            cancelButton2.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            StartCoroutine(VictoryRoutine(windAndLose));
            this.windAndLose = windAndLose;


            CombatUI.Instance.isSpeed = false;
            CombatManager.Instance.SetResetSpeed();
            CombatUI.Instance.speedButton.image.sprite = CombatUI.Instance.speedModeSprite;

            SoundManager.Instance.StopAllEffect();

            if(CombatManager.Instance.currentBattleType == BattleType.Gamebling)
            {
                int gameblingWindLose = CombatManager.Instance.CheckWinAndLoseBetting(windAndLose);
                if (gameblingWindLose == 1)
                    SoundManager.Instance.PlayEffect(160, 0.75f);
                else
                    SoundManager.Instance.PlayEffect(162, 0.75f);
            }
            else
            {
                if (windAndLose == 1)
                    SoundManager.Instance.PlayEffect(160, 0.75f);
                else if (windAndLose == 0)
                    SoundManager.Instance.PlayEffect(161, 1f);
                else if (windAndLose == -1)
                    SoundManager.Instance.PlayEffect(162, 0.75f);
            }
        }
    }

    public void Closed()
    {
        if (isPopUp == true)
        {
            animtor.Play("idle");
            isPopUp = false;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
            for(int i = 0; i < buttons.Length; ++i)
            {
                Block(i, false);
            }

            floorText.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            cancelButton2.gameObject.SetActive(false);
            cancelButton3.gameObject.SetActive(false);
        }
    }

    public void Init()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < buttons.Length; ++i)
        {
            Block(i, false);
        }
    }

    public void Restart()
    {
        CombatManager.Instance.blockStory = true;

        CombatUI.Instance.UpdateMonsterImages();
        CombatManager.Instance.ReturnGame();
    }

    public void NextLevel()
    {
        CombatManager.Instance.blockStory = false;

        CombatUI.Instance.UpdateMonsterImages();
        CombatManager.Instance.AddLevel();
        CombatManager.Instance.ReturnGame();
    }

    public void Exit()
    {
        CombatManager.Instance.blockStory = false;
        

        if (CombatManager.Instance.currentBattleType == BattleType.Story)
        {
            bool clear = (windAndLose == 1) && (CombatManager.Instance.currentLevel >= CombatManager.Instance.maxLevel - 1);
            if ((StoryUI.Instance.currentSlot.isTransition == false) && clear == true)
            {
                if(StoryUI.Instance.currentSlot.clearBlock == false)
                {
                    StoryUI.Instance.PopUp();
                    StartCoroutine(NextStoryRoutine());

                    if (StoryUI.Instance.currentSlot.isCleard == false)
                        StoryUI.Instance.currentSlot.isCleard = true;
                }
                else
                    StoryUI.Instance.PopUp();
            }
            else
                StoryUI.Instance.PopUp();

            CombatUI.Instance.surrendFlag = true;
            CombatUI.Instance.itemFlag = true;
        }
        else if(CombatManager.Instance.currentBattleType == BattleType.FriendShip)
        {
            if (popupFriendShipGradeData != null)
                DialougeUI.Instance.PopUp(popupFriendShipGradeData, false, true);
            else
                MainSystemUI.Instance.PopUp();
        }
        else if(CombatManager.Instance.currentBattleType == BattleType.Gamebling)
        {
            if(ItemInventory.Instance.money <= 0)
                MainSystemUI.Instance.PopUp();
            else
                CombatManager.Instance.ReturnGamebling();
        }
        else if(CombatManager.Instance.currentBattleType == BattleType.Official)
        {
            if (SelectTeamUI.Instance.selectTeam == LeagueManager.Instance.defendingChampionData)
            {
                if (windAndLose == -1)
                    InsertUI.Instance.PopUpLose();
                else if (windAndLose == 1)
                    InsertUI.Instance.PopUpWin();
                else if (windAndLose == 0)
                    InsertUI.Instance.PopUpVictory();
            }
            else
            {
                if (CombatManager.Instance.battleDataObject == LeagueManager.Instance.defendingChampionData.championData)
                {
                    if (windAndLose == -1)
                        InsertUI.Instance.PopUpLose();
                    else if (windAndLose == 1)
                        InsertUI.Instance.PopUpWin();
                    else if (windAndLose == 0)
                        InsertUI.Instance.PopUpVictory();
                }
                else
                {
                    int check = LeagueManager.Instance.CheckWindAndLose();

                    if (check == -2)
                        StartCoroutine(RestRoutine());
                    else
                    {
                        if (check == -1)
                            InsertUI.Instance.PopUpLose();
                        else if (check == 0)
                            InsertUI.Instance.PopUpVictory();
                        else
                            InsertUI.Instance.PopUpWin();
                    }
                }
            }
        }
        else if(CombatManager.Instance.currentBattleType == BattleType.PvP)
        {
            MainSystemUI.Instance.PopUp();
         
        }
      
        CombatManager.Instance.maxLevel = 0;
        CombatManager.Instance.currentLevel = 0;
        Closed();
    }

    private void Block(int index, bool check)
    {
        if(check == true)
        {
            buttons[index].interactable = true;
            var text = buttons[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.color = originColor;
        }
        else
        {
            buttons[index].interactable = false;
            var text = buttons[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.color = disableColor;
        }
    }
    private void PopUpSetting()
    {
        isPopUp = true;

        if(CombatManager.Instance.currentBattleType != BattleType.PvP)
        {
            bool check = CombatManager.Instance.CheckProgressingStory() && (windAndLose == 1);
            bool check2 = LeagueManager.Instance.CheckAllFaint();

            if (check2)
            {
                Block(0, false);
                SetFloorText(false);
            }
            else
            {
                Block(0, check);
                SetFloorText(check);
            }

            if (check2)
                Block(1, false);
            else
                Block(1, windAndLose != 1);

            Block(2, true);
        }
        
    }
    private void SetFloorText(bool check)
    {
        if(check)
        {
            floorText.gameObject.SetActive(true);
            floorText.text = $"{Mathf.Min(CombatManager.Instance.maxLevel, CombatManager.Instance.currentLevel + 1)} / {CombatManager.Instance.maxLevel}";
        }
    }
    public IEnumerator DeathMatchRoutine()
    {
        SoundManager.Instance.PlayEffect(126, 1f);
        text.gameObject.SetActive(true);
        text.Play("D E A T H M A C H", 0.075f);

        yield return new WaitUntil(() => text.isRunning == false);
        yield return new WaitForSeconds(0.25f);

        SoundManager.Instance.StopEffect(126);
        text.gameObject.SetActive(false);
    }
    private IEnumerator VictoryRoutine(int windAndLose)
    {
        var battleType = CombatManager.Instance.currentBattleType;
        int gameblingWindLose = CombatManager.Instance.CheckWinAndLoseBetting(windAndLose);
        if (battleType == BattleType.Gamebling)
        {
            if(gameblingWindLose == 1)
            {
                text.Play("W I N B E T ! ! !", 0.075f);
            }
            else
                text.Play("L O S E B E T . . .", 0.075f);
        }
        else
        {
            if (windAndLose == -1)
            {
                text.Play("Y O U L O S T . . .", 0.075f);
            }
            else if (windAndLose == 0)
            {
                text.Play("D R A W G A M E", 0.075f);
            }
            else if (windAndLose == 1)
            {
                text.Play("Y O U W O N ! ! !", 0.075f);
            }
        }
        
        yield return new WaitUntil(() => text.isRunning == false);
        yield return new WaitForSeconds(0.25f);

        if (battleType == BattleType.Official)
        {
            var playerTeam = SelectTeamUI.Instance.selectTeam;
            int priviousIndex = LeagueManager.Instance.currentLeagTeam.FindIndex(x => x == playerTeam);

            LeagueManager.Instance.PlayerWinAndLose(windAndLose);

            int swapIndex = LeagueManager.Instance.GetSwapIndex();
            yield return StartCoroutine(SwapRoutine(swapIndex, priviousIndex));
            yield return new WaitForSeconds(0.5f);
            cancelButton.gameObject.SetActive(true);
        }
        else if(battleType == BattleType.FriendShip)
        {
            yield return StartCoroutine(MoneyRoutine(windAndLose));
            yield return new WaitForSeconds(0.5f);
            cancelButton2.gameObject.SetActive(true);
        }
        else if(battleType == BattleType.Story)
        {
            animtor.Play("popup");
        }
        else if(battleType == BattleType.Gamebling)
        {
            yield return StartCoroutine(MoneyRoutineToGamebling(gameblingWindLose));
            yield return new WaitForSeconds(0.5f);
            cancelButton2.gameObject.SetActive(true);
        }
    }
    private IEnumerator RestRoutine()
    {
        CalendarUI.Instance.PopUp(false);
        var monsterInstances = Array.FindAll(TranningUI.Instance.playerInventory.monsterDatas, x => x != null);
        for (int i = 0; i < monsterInstances.Length; ++i)
        {
            if (monsterInstances[i].heathState != MonsterHeathState.Faint)
                monsterInstances[i].hp = (monsterInstances[i].heathState == MonsterHeathState.Blooding) ? monsterInstances[i].hp : Mathf.Min(monsterInstances[i].maxHp, monsterInstances[i].hp + (monsterInstances[i].maxHp * monsterInstances[i].hpRecoveryRatio));
        }

        yield return new WaitForSeconds(0.25f);

        int nextTimeCount = (LeagueManager.Instance.timecount - LeagueManager.Instance.currentTimeCount) + 1;
        yield return StartCoroutine(CalendarUI.Instance.NextMoveRoutine());

        ItemInventory.Instance.money += (200 + (ItemInventory.Instance.fame * 5));
        ItemInventory.Instance.friendShipBattleCount = ItemInventory.Instance.friendShipBattleWin = 0;
        LeagueManager.Instance.friendshipLevel = FriendShipLevel.Begginer;
        LeagueManager.Instance.currentTimeCount = LeagueManager.Instance.timecount - nextTimeCount;
        LeagueManager.Instance.diamondToGold = MonsterDataBase.Instance.diamondPayTable[Random.Range(0, MonsterDataBase.Instance.diamondPayTable.Count)];
        LeagueManager.Instance.blockGamebling = false;
        LeagueManager.Instance.blockResetItemForAds = LeagueManager.Instance.blockResetMonsterForAds = false;

        DialougeUI.Instance.SetMonsterDatas();
        DialougeUI.Instance.SetItemDatas();
        RulletUI.Instance.Clear();
        CalendarUI.Instance.Closed();
        MainSystemUI.Instance.PopUp();
    }
    private IEnumerator SwapRoutine(int swapIndex, int previousIndex)
    {
        if (swapIndex == previousIndex)
        {
            Active(current, SelectTeamUI.Instance.selectTeam, previousIndex);
            if (previousIndex == 0)
            {
                DontActive(next);
                Active(prev, LeagueManager.Instance.currentLeagTeam[previousIndex + 1], previousIndex + 1);
            }
            else if (previousIndex == (LeagueManager.Instance.currentLeagTeam.Count - 1))
            {
                DontActive(prev);
                Active(next, LeagueManager.Instance.currentLeagTeam[previousIndex - 1], previousIndex - 1);
            }
            else
            {
                Active(prev, LeagueManager.Instance.currentLeagTeam[previousIndex + 1], previousIndex + 1);
                Active(next, LeagueManager.Instance.currentLeagTeam[previousIndex - 1], previousIndex - 1);
            }

            animtor.Play("draw");
            yield return new WaitUntil(() => animtor.GetCurrentAnimatorStateInfo(0).IsName("drawidle"));
        }
        else if (swapIndex > previousIndex)
        {
            Active(current, SelectTeamUI.Instance.selectTeam, previousIndex);
            if (previousIndex == 0)
            {
                DontActive(next);
                Active(prev, LeagueManager.Instance.currentLeagTeam[swapIndex], swapIndex);
            }
            else if (previousIndex == (LeagueManager.Instance.currentLeagTeam.Count - 1))
            {
                DontActive(prev);
                Active(next, LeagueManager.Instance.currentLeagTeam[swapIndex], swapIndex);
            }
            else
            {
                Active(prev, LeagueManager.Instance.currentLeagTeam[swapIndex], swapIndex);
                Active(next, LeagueManager.Instance.currentLeagTeam[previousIndex - 1], previousIndex - 1);
            }
            animtor.Play("lose");
            yield return new WaitUntil(() => animtor.GetCurrentAnimatorStateInfo(0).IsName("loseidle"));

            UpdateNumber(current, swapIndex);
            UpdateNumber(prev, previousIndex);
        }
        else if (swapIndex < previousIndex)
        {
            Active(current, SelectTeamUI.Instance.selectTeam, previousIndex);
            if (previousIndex == 0)
            {
                DontActive(next);
                Active(prev, LeagueManager.Instance.currentLeagTeam[swapIndex], swapIndex);
            }
            else if (previousIndex == (LeagueManager.Instance.currentLeagTeam.Count - 1))
            {
                DontActive(prev);
                Active(next, LeagueManager.Instance.currentLeagTeam[swapIndex], swapIndex);
            }
            else
            {
                Active(prev, LeagueManager.Instance.currentLeagTeam[previousIndex + 1], previousIndex + 1);
                Active(next, LeagueManager.Instance.currentLeagTeam[swapIndex], swapIndex);
            }
            animtor.Play("win");
            yield return new WaitUntil(() => animtor.GetCurrentAnimatorStateInfo(0).IsName("winidle"));

            UpdateNumber(current, swapIndex);
            UpdateNumber(next, previousIndex);
        }
    }

  
    private IEnumerator MoneyRoutine(int winAndLose)
    {
        int moneyValue = CombatManager.Instance.GetFriendShipReward();

        if (windAndLose == 1)
        {
            var currentLevel = LeagueManager.Instance.friendshipLevel;
            ItemInventory.Instance.friendShipBattleCount++;
            ItemInventory.Instance.friendShipBattleWin++;

            var nextLevel = CombatManager.Instance.GetFriendShipLevel();
            if (currentLevel != nextLevel)
            {
                popupFriendShipGradeData = ((int)currentLevel > (int)nextLevel) ? MonsterDataBase.Instance.friendShipGradeLose : MonsterDataBase.Instance.friendShipGradeWin;
                LeagueManager.Instance.friendshipLevel = nextLevel;
            }
            else
                popupFriendShipGradeData = null;
        }
        else if (windAndLose == -1)
        {
            var currentLevel = CombatManager.Instance.GetFriendShipLevel();
            ItemInventory.Instance.friendShipBattleCount++;

            var nextLevel = CombatManager.Instance.GetFriendShipLevel();
            if (currentLevel != nextLevel)
            {
                popupFriendShipGradeData = ((int)currentLevel > (int)nextLevel) ? MonsterDataBase.Instance.friendShipGradeLose : MonsterDataBase.Instance.friendShipGradeWin;
                LeagueManager.Instance.friendshipLevel = nextLevel;
            }
            else
                popupFriendShipGradeData = null;
        }
        else
            popupFriendShipGradeData = null;

        string stringValue = ItemInventory.Instance.GetSplitMoneyText();
        char[] charArray = stringValue.ToCharArray();
        string _0 = GetColorText(0, charArray);
        string _1 = GetColorText(1, charArray);
        string _2 = GetColorText(2, charArray);
        string _3 = GetColorText(3, charArray);
        string _4 = GetColorText(4, charArray);
        moneyText.text = $"{_0}{charArray[0]} {_1}{charArray[1]} {_2}{charArray[2]} {_3}{charArray[3]} {_4}{charArray[4]}";
        animtor.Play("money");

        yield return new WaitUntil(() => animtor.GetCurrentAnimatorStateInfo(0).IsName("moneyidle"));
        yield return new WaitForSeconds(0.25f);

        int endValue = 0;
        if (winAndLose == 0)
            yield break;
        else if(winAndLose == 1)
            endValue = ItemInventory.Instance.money + moneyValue;
        else if(winAndLose == -1)
            endValue = ItemInventory.Instance.money - 100;

        int startValue = ItemInventory.Instance.money;
        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        if (endValue > startValue)
            SoundManager.Instance.PlayEffect(171, 1f);
        else if (endValue < startValue)
            SoundManager.Instance.PlayEffect(172, 1f);
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            ItemInventory.Instance.money = Mathf.FloorToInt(Mathf.Lerp(startValue, endValue, currentSpeed));
            yield return null;

            stringValue = ItemInventory.Instance.GetSplitMoneyText();
            charArray = stringValue.ToCharArray();

            _0 = GetColorText(0, charArray);
            _1 = GetColorText(1, charArray);
            _2 = GetColorText(2, charArray);
            _3 = GetColorText(3, charArray);
            _4 = GetColorText(4, charArray);
            moneyText.text = $"{_0}{charArray[0]} {_1}{charArray[1]} {_2}{charArray[2]} {_3}{charArray[3]} {_4}{charArray[4]}";
        }
    }
  
    private IEnumerator MoneyRoutineToGamebling(int winAndLose)
    {
        string stringValue = ItemInventory.Instance.GetSplitMoneyText();
        char[] charArray = stringValue.ToCharArray();
        string _0 = GetColorText(0, charArray);
        string _1 = GetColorText(1, charArray);
        string _2 = GetColorText(2, charArray);
        string _3 = GetColorText(3, charArray);
        string _4 = GetColorText(4, charArray);
        moneyText.text = $"{_0}{charArray[0]} {_1}{charArray[1]} {_2}{charArray[2]} {_3}{charArray[3]} {_4}{charArray[4]}";
        animtor.Play("money");

        yield return new WaitUntil(() => animtor.GetCurrentAnimatorStateInfo(0).IsName("moneyidle"));
        yield return new WaitForSeconds(0.25f);

        int endValue = 0;
        int moneyValue = ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.piggyBank) ? 500 : 100;
        if (winAndLose == 1)
            endValue = ItemInventory.Instance.money + BatUI.Instance.GetData();
        else if (winAndLose == -1)
            endValue = ItemInventory.Instance.money - CombatUI.Instance.batMoney;

        int startValue = ItemInventory.Instance.money;
        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        if(endValue > startValue)
            SoundManager.Instance.PlayEffect(171, 1f);
        else if(endValue < startValue)
            SoundManager.Instance.PlayEffect(172, 1f);
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            ItemInventory.Instance.money = Mathf.FloorToInt(Mathf.Lerp(startValue, endValue, currentSpeed));
            yield return null;

            stringValue = ItemInventory.Instance.GetSplitMoneyText();
            charArray = stringValue.ToCharArray();

            _0 = GetColorText(0, charArray);
            _1 = GetColorText(1, charArray);
            _2 = GetColorText(2, charArray);
            _3 = GetColorText(3, charArray);
            _4 = GetColorText(4, charArray);
            moneyText.text = $"{_0}{charArray[0]} {_1}{charArray[1]} {_2}{charArray[2]} {_3}{charArray[3]} {_4}{charArray[4]}";
        }
    }
    private IEnumerator NextStoryRoutine()
    {
        if (StoryUI.Instance.currentSlot.clearEvent == null && StoryUI.Instance.currentSlot.clearStoryEvent.npcImage == null)
            yield break;

        if(StoryUI.Instance.currentSlot.clearStoryEvent.npcImage != null)
        {
            DialougeUI.Instance.PopUp(StoryUI.Instance.currentSlot.clearStoryEvent, false, false);
            yield return new WaitUntil(() => DialougeUI.Instance.isPopUp == false);
        }

        if(StoryUI.Instance.currentSlot.clearEvent != null)
        {
            StoryUI.Instance.currentSlot.isTransition = true;
            yield return new WaitForSeconds(0.25f);
            StoryUI.Instance.currentSlot.clearEvent.Invoke();
        }
      
    }
    private void Active(Image background, TeamData teamData, int index)
    {
        var teamMark = background.transform.GetChild(0).GetComponent<Image>();
        teamMark.gameObject.SetActive(true);
        teamMark.sprite = teamData.teamIcon;

        var scoreText = background.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"<color=black><b><size=28>{teamData.win}<color=blue><b><size=24>win <color=black><b><size=28>{teamData.draw}<color=#808080><b><size=24>draw <color=black><b><size=28>{teamData.lose}<color=red><b><size=24>lose <color=black><b><size=28>{teamData.score}<color=#A32CC4><b><size=24>pt";

        var numberText = background.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        numberText.gameObject.SetActive(true);

        string text = "";
        int number = index + 1;
        if (number == 1)
            text = "st";
        else if (number == 2)
            text = "nd";
        else if (number == 3)
            text = "rd";
        else
            text = "th";

        numberText.text =$"{number}<b><size=40>{text}";

        background.color = Color.white;
    }
    private void DontActive(Image background)
    {
        int childCount = background.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            background.transform.GetChild(i).gameObject.SetActive(false);
        }

        background.color = new Color(0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f);
    }

    private void DontActivePvP(Image background)
    {
        int childCount = background.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            background.transform.GetChild(i).gameObject.SetActive(false);
        }

        background.color = new Color(0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f);
    }

    private void UpdateNumber(Image background, int index)
    {
        var numberText = background.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        string text = "";
        int number = index + 1;
        if (number == 1)
            text = "st";
        else if (number == 2)
            text = "nd";
        else if (number == 3)
            text = "rd";
        else
            text = "th";

        numberText.text = $"{number}<b><size=40>{text}";

        background.color = Color.white;
    }
    private string GetColorText(int index, char[] array)
    {
        bool check = false;
        for(int i = 0; i <= index; ++i)
        {
            int previousNumber = array[i] - '0';
            if (previousNumber > 0)
            {
                check = true;
                break;
            }
        }
        return (check == false) ? "<color=#808080>" : "<color=black>";
    }
}
