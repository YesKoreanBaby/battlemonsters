using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class MainSystemUI : MonoBehaviour
{
    public Button[] selectButton;
    public RectTransform basicView;

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI verseText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI fameText;
    public TextMeshProUGUI rankingText;

    public Button scoreButton;
    public Button treagureButton;
    public Button scheduleButton;
    public Button externalButton;

    private float dontActiveColorPercent = 0.5098039215686275f;

    private float activeColorPercent;

    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public TeamData machingTeam;

    private bool checkStartSound = false;
    private int currentSoundSwapIndex = 0;
    private int currentSoundIndex = 0;

    private Coroutine bgmRoutine = null;

    private static MainSystemUI instance = null;
    public static MainSystemUI Instance { get { return instance; } }

    private Animator animator;
    private bool checkTutorialActive = false;
    private Action popUpEvent = null;
    private bool checkNetWork = false;
    private void Awake()
    {
        instance = this;

        animator = GetComponent<Animator>();
      

        var childTmp = selectButton[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        activeColorPercent = childTmp.color.a;

        Active(0, true);
        Active(1, true);
        Active(2, true);
        Active(3, true);
        Active(4, true);
        Active(5, true);
        Active(6, true);

        Init();
    }
    public void PopUp(bool stopBgm = true)
    {
        if(RewardUI.Instance.rewardQueue.Count > 0)
        {
            RewardUI.Instance.PopUp();
            return;
        }
        if(isPopUp == false)
        {
            //var audioSource = SoundManager.Instance.bgm;
            //if (audioSource.isPlaying == false || audioSource.clip == null)
            //    bgmRoutine = StartCoroutine(MainBgmRoutine());

            if (stopBgm == true)
            {
                StopBgm();
                bgmRoutine = StartCoroutine(MainBgmRoutine());
            }

            SoundManager.Instance.PlayEffect(163, 1f);
            Active(0, true);
            Active(1, true);
            Active(2, true);
            Active(3, true);
            Active(4, true);
            Active(5, true);
            Active(6, true);

            animator.Play("popup");
            int childCount = this.transform.childCount;

            for (int i = 0; i < selectButton.Length; ++i)
            {
                selectButton[i].interactable = true;
            }

            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            SettingData();

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
            //    AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Bottom);

            if (popUpEvent != null)
            {
                popUpEvent.Invoke();
                popUpEvent = null;
            }
        }
    }
    public void Closed()
    {
        if(isPopUp == true)
        {
            animator.Play("closed");
            Active(0, false);
            Active(1, false);
            Active(2, false);
            Active(3, false);
            Active(4, false);
            Active(5, false);
            Active(6, false);
        }
    }
    public void Closed2()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
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
    }

    public void CommunityButton()
    {
        string text = TextManager.Instance.language == SystemLanguage.Korean ? "<b><size=48>커뮤니티를 오픈 시키는\r\n\r\n기능으로 변경됨\r\n\r\n지금은 준비중입니다" : "<b><size=48>Changed to a function  \r\n\r\nthat opens the  community\r\n \r\n\r\nI'm preparing now";
        AlarmUI.Instance.PopUp(text, null);
    }
    public void SetTextFormats()
    {
        for (int i = 0; i < selectButton.Length; ++i)
        {
            TextMeshProUGUI text = selectButton[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            if (TextManager.Instance.language == SystemLanguage.English)
            {
                text.font = MonsterDataBase.Instance.ehFont;
                text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, -2.9f);
                text.fontStyle = FontStyles.Normal;
                text.fontSize = 36;

                if (i == 0)
                {
                    text.text = "Official Match";
                }
                else if (i == 1)
                {
                    text.text = "Endless Mode";
                }
                else if (i == 2)
                {
                    text.text = "Shop";
                }
                else if (i == 3)
                {
                    text.text = "Bar";
                }
                else if (i == 4)
                {
                    text.text = "Monster";
                }
                else if (i == 5)
                {
                    text.text = "Story";
                }
                else if (i == 6)
                {
                    text.text = "Rest";
                }
            }
            else
            {
                text.font = MonsterDataBase.Instance.krFont;
                text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, 3.7f);
                text.fontStyle = FontStyles.Bold;
                text.fontSize = 42;

                if (i == 0)
                {
                    text.text = "공식 경기";
                }
                else if (i == 1)
                {
                    text.text = "무한 모드";
                }
                else if (i == 2)
                {
                    text.text = "상점";
                }
                else if (i == 3)
                {
                    text.text = "주점";
                }
                else if (i == 4)
                {
                    text.text = "몬스터";
                }
                else if (i == 5)
                {
                    text.text = "스토리";
                }
                else if (i == 6)
                {
                    text.text = "휴식";
                }
            }
        }

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
    }

    public void Active(int index ,bool isActvie)
    {
        var childTmp = selectButton[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (isActvie == false)
        {
            childTmp.color = new Color(childTmp.color.r, childTmp.color.g, childTmp.color.b, dontActiveColorPercent);
            selectButton[index].interactable = false;
        }
        else
        {
            childTmp.color = new Color(childTmp.color.r, childTmp.color.g, childTmp.color.b, activeColorPercent);
            selectButton[index].interactable = true;
        }
    }
    public void SettingData()
    {
        dayText.text = $"D - {LeagueManager.Instance.currentTimeCount}";

        var temData = SelectTeamUI.Instance.selectTeam;
        verseText.text = $"{temData.win} win {temData.draw} draw {temData.lose} lose";

        moneyText.text = ItemInventory.Instance.money.ToString();
        diamondText.text = ItemInventory.Instance.diamond.ToString();
        fameText.text = ItemInventory.Instance.fame.ToString();
        rankingText.text = ItemInventory.Instance.ranking.ToString();

        int index = (LeagueManager.Instance.timecount - LeagueManager.Instance.currentTimeCount);
        machingTeam = CalendarUI.Instance.FindMatchingTeam(index);
      
        scheduleButton.interactable = scoreButton.interactable = treagureButton.interactable = true;
        externalButton.interactable = checkNetWork;
       
        Active(4, Array.Find(TranningUI.Instance.playerInventory.monsterDatas, x => x != null) != null);
        Active(0, machingTeam != null);

        bool check = (LeagueManager.Instance.currentLeague == LeagueManager.Instance.league_0) && (machingTeam != null);
        Active(6, !check);
    }

    
    public void FriendlyButtonSetUp()
    {
        AlarmUI.Instance.PopUpForKey("consumitem", null);
    }
    public void OfficialMatch()
    {
        SoundManager.Instance.PlayEffect(166, 1f);


        var mon = Array.Find(TranningUI.Instance.playerInventory.monsterDatas, x => x != null);
        if (mon == null)
        {
            AlarmUI.Instance.PopUpForKey("notmonster", null, null);
            return;
        }

        bool check = LeagueManager.Instance.CheckAllFaint();
        if (check == true)
        {
            AlarmUI.Instance.PopUpForKey("dontmatching", null, null);
            return;
        }

        var homegroundTeam = SelectTeamUI.Instance.selectTeam;
        AlarmUI.Instance.PopUpForMathcing(() => { OfficialMatch2(); AlarmUI.Instance.Closed(); }, null, homegroundTeam.teamIcon, homegroundTeam.teamName, machingTeam.teamIcon, machingTeam.teamName);
    }
    public void FriendShipMatch()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        var mon = Array.Find(TranningUI.Instance.playerInventory.monsterDatas, x => x != null);
        if (mon == null)
        {
            AlarmUI.Instance.PopUpForKey("notmonster", null, null);
            return;
        }

        bool check = LeagueManager.Instance.CheckAllFaint();
        if (check == true)
        {
            AlarmUI.Instance.PopUpForKey("dontmatching", null, null);
            return;
        }

        check = ItemInventory.Instance.money < 100;
        if (check == true)
        {
            AlarmUI.Instance.PopUpForKey("participation fee", null, null);
            return;
        }

        string text = TextManager.Instance.language == SystemLanguage.Korean ? "<b><size=48><color=black>100 gold를 걸고 \r\n\r\n대결을 펼치시겠습니까?" : "<b><size=52><color=black>Would you like to \r\n\r\ncompete for \r\n\r\n100 gold?";

        AlarmUI.Instance.PopUp(text, () => { FriendshipMatch2(); AlarmUI.Instance.Closed(); }, null);
    }
    public void Shop()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        Closed2();
        DialougeUI.Instance.PopUp(MonsterDataBase.Instance.shopKeeperData, true);

    }
    public void Bar()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        Closed2();
        DialougeUI.Instance.PopUp(MonsterDataBase.Instance.bartenderData, true);
    }
    public void Story()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        var mon = Array.Find(TranningUI.Instance.playerInventory.monsterDatas, x => x != null);
        if (mon == null)
        {
            AlarmUI.Instance.PopUpForKey("notmonster", null, null);
            return;
        }

        bool check = LeagueManager.Instance.CheckAllFaint();
        if (check == true)
        {
            AlarmUI.Instance.PopUpForKey("dontmatching", null, null);
            return;
        }

        string text = TextManager.Instance.language == SystemLanguage.Korean ? "<b><size=48><color=black>몬스터와 보물의 행방을 \r\n\r\n찾아 여행하시겠습니까?" : "<b><size=52><color=black>Would you like to go \r\n\r\nlooking for treasure \r\n\r\nwith monsters?";
        AlarmUI.Instance.PopUp(text, () => { Story2(); AlarmUI.Instance.Closed(); }, null);
    }
    public void Shedule()
    {
        CalendarUI.Instance.PopUp();
    }

    public void Score()
    {
        ScoreUI.Instance.PopUp();
    }
    public void Treagure()
    {
        TreagureInventoryUI.Instance.PopUp();
    }
    public void Rest()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        List<Tuple<TeamData, TeamData>> machingDatas = null;
        LeagueManager.Instance.currentLeague.TryGetValue(LeagueManager.Instance.timecount - LeagueManager.Instance.currentTimeCount, out machingDatas);
        if (machingDatas != null)
            AlarmUI.Instance.PopUpForKey("abstention", Rest2, null);
        else
            Rest2();
    }
    
    public void Rest2()
    {
        int check = LeagueManager.Instance.CheckWindAndLose();

        if(check == -2)
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
        if (AlarmUI.Instance.isPopUp == true)
            AlarmUI.Instance.Closed();
    }
    public void FriendshipMatch2()
    {
        StartCoroutine(FriendMatch2Routine());
    }
    public void OfficialMatch2()
    {
        StartCoroutine(OfficialMatch2Routine());
    }
    public void Story2()
    {
        Closed();
        StoryUI.Instance.PopUp();
        if (AlarmUI.Instance.isPopUp == true)
            AlarmUI.Instance.Closed();
    }
    public void Tranning()
    {
        StartCoroutine(TranningRoutine());
    }
    public void StopBgm()
    {
        if(bgmRoutine != null)
        {
            StopCoroutine(bgmRoutine);
            SoundManager.Instance.StopBgm();
            bgmRoutine = null;
        }
    }
    public void PopUpSytemUI()
    {
        SystemUI.Instance.PopUp();
    }

 

    public void AddPopUpEvent(Action evt)
    {
        popUpEvent = evt;
    }
    private IEnumerator RestRoutine()
    {
        Closed();

        //몬스터 회복
        var monsterInstances = Array.FindAll(TranningUI.Instance.playerInventory.monsterDatas, x => x != null);
        for (int i = 0; i < monsterInstances.Length; ++i)
        {
            monsterInstances[i].RestStateUpdate();
            if (monsterInstances[i].heathState != MonsterHeathState.Faint)
                monsterInstances[i].hp = (monsterInstances[i].heathState == MonsterHeathState.Blooding) ? monsterInstances[i].hp : Mathf.Min(monsterInstances[i].maxHp, monsterInstances[i].hp + (monsterInstances[i].maxHp * monsterInstances[i].hpRecoveryRatio));
        }

        //기권판정
        List<Tuple<TeamData, TeamData>> machingDatas = null;
        LeagueManager.Instance.currentLeague.TryGetValue(LeagueManager.Instance.timecount - LeagueManager.Instance.currentTimeCount, out machingDatas);
        if (machingDatas != null)
        {
            var enemyTeam = LeagueManager.Instance.FindPlayMatchTeam(machingDatas);
            var playerTeam = SelectTeamUI.Instance.selectTeam;
            enemyTeam.win += 1;
            playerTeam.lose += 1;
            LeagueManager.Instance.SetRandomAIScores(machingDatas);
            LeagueManager.Instance.Report(-1);
        }
        yield return new WaitForSeconds(0.45f);

        CalendarUI.Instance.PopUp(false);
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
        PopUp(false);
    }
    private IEnumerator TranningRoutine()
    {
        Closed();

        yield return new WaitForSeconds(0.45f);

        TranningUI.Instance.PopUp();
    }
    private IEnumerator FriendMatch2Routine()
    {
        Closed();
        yield return new WaitForSeconds(0.45f);
        CombatManager.Instance.battleDataObject = CombatManager.Instance.ai.friendshipData;
        //CombatManager.Instance.battleDataObject = CombatManager.Instance.ai.testData;
        CombatManager.Instance.ReturnGame();

        CombatUI.Instance.UpdateMonsterImages();
        if (AlarmUI.Instance.isPopUp == true)
            AlarmUI.Instance.Closed();
    }
    private IEnumerator OfficialMatch2Routine()
    {
        Closed();
        yield return new WaitForSeconds(0.45f);

        int number = LeagueManager.Instance.GetCurrentLeagueNumber();

        if (number == 3)
            CombatManager.Instance.battleDataObject = machingTeam.league_3Data;
        else if (number == 2)
            CombatManager.Instance.battleDataObject = machingTeam.league_2Data;
        else if (number == 1)
            CombatManager.Instance.battleDataObject = machingTeam.league_1Data;
        else if (number == -1)
            CombatManager.Instance.battleDataObject = machingTeam.championData;

        CombatManager.Instance.ReturnGame();
        List<Tuple<TeamData, TeamData>> machingDatas = null;
        LeagueManager.Instance.currentLeague.TryGetValue(LeagueManager.Instance.timecount - LeagueManager.Instance.currentTimeCount, out machingDatas);

        LeagueManager.Instance.SetRandomAIScores(machingDatas);
        DialougeUI.Instance.PopUp(MonsterDataBase.Instance.repoterData, false, false);
        CombatUI.Instance.UpdateMonsterImages();
    }

    private IEnumerator MainBgmRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.75f);

        if (checkStartSound == false)
        {
            currentSoundIndex = 49;
            checkStartSound = true;
        }
        else
        {
            SetBgmSwapIndex();
            currentSoundIndex = currentSoundSwapIndex;
        }
        var audioSource = SoundManager.Instance.bgm;

        if (currentSoundIndex == 48)
            yield return waitTime;
        while (true)
        {
            SoundManager.Instance.PlayBgm(currentSoundIndex, 1f, false);
            SetBgmSwapIndex();

            yield return null;
            yield return new WaitUntil(() => audioSource.isPlaying == false);
            yield return waitTime;
            currentSoundIndex = currentSoundSwapIndex;
        }
    }
    private void SetBgmSwapIndex()
    {
        currentSoundSwapIndex = Random.Range(0, 4);
        if (currentSoundSwapIndex == 0)
            currentSoundSwapIndex = 44;
        else if (currentSoundSwapIndex == 1)
            currentSoundSwapIndex = 47;
        else if (currentSoundSwapIndex == 2)
            currentSoundSwapIndex = 48;
        else if (currentSoundSwapIndex == 3)
            currentSoundSwapIndex = 49;
        while (currentSoundSwapIndex == currentSoundIndex)
        {
            currentSoundSwapIndex = Random.Range(0, 4);
            if (currentSoundSwapIndex == 0)
                currentSoundSwapIndex = 44;
            else if (currentSoundSwapIndex == 1)
                currentSoundSwapIndex = 47;
            else if (currentSoundSwapIndex == 2)
                currentSoundSwapIndex = 48;
            else if (currentSoundSwapIndex == 3)
                currentSoundSwapIndex = 49;
        }
    }

  
}
