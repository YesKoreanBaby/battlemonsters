using UnityEngine;

public class StoryUI : MonoBehaviour
{
    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public StageSelectSlot currentSlot;

    [System.NonSerialized]
    public int changeSoundIndex = 0;

    private static StoryUI instance = null;
    public static StoryUI Instance { get { return instance; } }

    public StageSelectSlot level_1;
    public StageSelectSlot level_1_hidden;
    public StageSelectSlot level_1_hidden_2;

    public StageSelectSlot level_2;
    public StageSelectSlot level_3;

    public StageSelectSlot level_4;
    public StageSelectSlot level_4_hidden;
    public StageSelectSlot level_4_hidden_2;

    public StageSelectSlot level_5;

    public StageSelectSlot level_6;
    public StageSelectSlot level_6_hidden;
    public StageSelectSlot level_6_hidden2;
    public StageSelectSlot level_6_hidden2_1;

    public StageSelectSlot special_1;
    public StageSelectSlot special_2;
    public StageSelectSlot special_3;

    public RectTransform level1Tolevel2;
    public RectTransform level2Tolevel3;
    public RectTransform level3Tolevel4;
    public RectTransform level4Tolevel5;
    public RectTransform level5Tolevel6;
    public RectTransform level_1Tolevel_1_hidden;
    public RectTransform level_1_hiddenTolevel_1_hidden_2;
    public RectTransform level_4Tolevel_4_hidden;
    public RectTransform level_4_hiddenTolevel_4_hidden_2;
    public RectTransform level_6Tolevel_6_hidden;
    public RectTransform level_6Tolevel_6_hidden2;
    public RectTransform level_6Tolevel_6_hidden2_1;

    public int nextLevel = 1;
    private void Awake()
    {
        instance = this;
        Init();
    }
    public void SetStageSelectSlots()
    {
        level1Tolevel2.gameObject.SetActive(false);
        level2Tolevel3.gameObject.SetActive(false);
        level3Tolevel4.gameObject.SetActive(false);
        level4Tolevel5.gameObject.SetActive(false);
        level5Tolevel6.gameObject.SetActive(false);
        level_1Tolevel_1_hidden.gameObject.SetActive(false);
        level_1_hiddenTolevel_1_hidden_2.gameObject.SetActive(false);
        level_4Tolevel_4_hidden.gameObject.SetActive(false);
        level_4_hiddenTolevel_4_hidden_2.gameObject.SetActive(false);
        level_6Tolevel_6_hidden.gameObject.SetActive(false);
        level_6Tolevel_6_hidden2.gameObject.SetActive(false);
        level_6Tolevel_6_hidden2_1.gameObject.SetActive(false);

        level_1.SetUp(StoryTransitionType.Open, null, null);
        level_1_hidden.SetUp(StoryTransitionType.Block, null, null);
        level_1_hidden_2.SetUp(StoryTransitionType.Blind, null, null);
        level_2.SetUp(StoryTransitionType.Block, null, null);
        level_3.SetUp(StoryTransitionType.Block, null, null);
        level_4.SetUp(StoryTransitionType.Block, null, null);
        level_4_hidden.SetUp(StoryTransitionType.Block, null, null);
        level_4_hidden_2.SetUp(StoryTransitionType.Blind, null, null);
        level_5.SetUp(StoryTransitionType.Block, null, null);
        level_6.SetUp(StoryTransitionType.Block, null, null);
        level_6_hidden.SetUp(StoryTransitionType.Block, null, null);
        level_6_hidden2.SetUp(StoryTransitionType.Blind, null, null);
        level_6_hidden2_1.SetUp(StoryTransitionType.Blind, null, null);

        special_1.SetUp(StoryTransitionType.Blind, null, null);
        special_2.SetUp(StoryTransitionType.Blind, null, null);
        special_3.SetUp(StoryTransitionType.Blind, null, null);

        currentSlot = level_1;
    }

    public void SetStageSelectSlotsForLoading()
    {
        level1Tolevel2.gameObject.SetActive(false);
        level2Tolevel3.gameObject.SetActive(false);
        level3Tolevel4.gameObject.SetActive(false);
        level4Tolevel5.gameObject.SetActive(false);
        level5Tolevel6.gameObject.SetActive(false);
        level_1Tolevel_1_hidden.gameObject.SetActive(false);
        level_1_hiddenTolevel_1_hidden_2.gameObject.SetActive(false);
        level_4Tolevel_4_hidden.gameObject.SetActive(false);
        level_4_hiddenTolevel_4_hidden_2.gameObject.SetActive(false);
        level_6Tolevel_6_hidden.gameObject.SetActive(false);
        level_6Tolevel_6_hidden2.gameObject.SetActive(false);
        level_6Tolevel_6_hidden2_1.gameObject.SetActive(false);

        level_1.SetUp(level_1.transitionType, level_2, null);
        level_1_hidden.SetUp(level_1_hidden.transitionType, null, null);
        level_1_hidden_2.SetUp(level_1_hidden_2.transitionType, null, null);
        level_2.SetUp(level_2.transitionType, null, null);
        level_3.SetUp(level_3.transitionType, null, null);
        level_4.SetUp(level_4.transitionType, null, null);
        level_4_hidden.SetUp(level_4_hidden.transitionType, null, null);
        level_4_hidden_2.SetUp(level_4_hidden_2.transitionType, null, null);
        level_5.SetUp(level_5.transitionType, null, null);
        level_6.SetUp(level_6.transitionType, null, null);
        level_6_hidden.SetUp(level_6_hidden.transitionType, null, null);
        level_6_hidden2.SetUp(level_6_hidden2.transitionType, null, null);
        level_6_hidden2_1.SetUp(level_6_hidden2_1.transitionType, null, null);

        special_1.SetUp(special_1.transitionType, null, null);
        special_2.SetUp(special_2.transitionType, null, null);
        special_3.SetUp(special_3.transitionType, null, null);

        currentSlot = level_1;
    }
    public void PopUp()
    {
        if(isPopUp == false)
        {
            MainSystemUI.Instance.StopBgm();
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            level1Tolevel2.gameObject.SetActive(level_2.transitionType == StoryTransitionType.Open);
            level2Tolevel3.gameObject.SetActive(level_3.transitionType == StoryTransitionType.Open);
            level3Tolevel4.gameObject.SetActive(level_4.transitionType == StoryTransitionType.Open);
            level4Tolevel5.gameObject.SetActive(level_5.transitionType == StoryTransitionType.Open);
            level5Tolevel6.gameObject.SetActive(level_6.transitionType == StoryTransitionType.Open);
            level_1Tolevel_1_hidden.gameObject.SetActive(level_1_hidden.transitionType == StoryTransitionType.Open);
            level_1_hiddenTolevel_1_hidden_2.gameObject.SetActive(level_1_hidden_2.transitionType == StoryTransitionType.Open);
            level_4Tolevel_4_hidden.gameObject.SetActive(level_4_hidden.transitionType == StoryTransitionType.Open);
            level_4_hiddenTolevel_4_hidden_2.gameObject.SetActive(level_4_hidden_2.transitionType == StoryTransitionType.Open);
            level_6Tolevel_6_hidden.gameObject.SetActive(level_6_hidden.transitionType == StoryTransitionType.Open);
            level_6Tolevel_6_hidden2.gameObject.SetActive(level_6_hidden2.transitionType == StoryTransitionType.Open);
            level_6Tolevel_6_hidden2_1.gameObject.SetActive(level_6_hidden2_1.transitionType == StoryTransitionType.Open);

            level_1.SetUp();
            level_1_hidden.SetUp();
            level_1_hidden_2.SetUp();
            level_2.SetUp();
            level_3.SetUp();
            level_4.SetUp();
            level_4_hidden.SetUp();
            level_4_hidden_2.SetUp();
            level_5.SetUp();
            level_6.SetUp();
            level_6_hidden.SetUp();
            level_6_hidden2.SetUp();
            level_6_hidden2_1.SetUp();

            special_1.SetUp();
            special_2.SetUp();
            special_3.SetUp();

           

            //if(level_4_hidden_2.transitionType == StoryTransitionType.Open)
            //{
            //    if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false)
            //        AdmobManager.Instance.DestroyBannerAds();
            //}
        }
    }

    public void Closed()
    {
        if (isPopUp == true)
        {
            isPopUp = false;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void Rest()
    {
        Closed();
        MainSystemUI.Instance.PopUp();
    }
    public void Next_1_HiddenStage()
    {
        level_1.SetUp(StoryTransitionType.Transition, level_1_hidden, level_1Tolevel_1_hidden);
    }
    public void Next_1_Hidden2Stage()
    {
        level_1.SetUp(StoryTransitionType.Transition, level_1_hidden_2, level_1_hiddenTolevel_1_hidden_2);
    }
    public void Next2Stage()
    {
        level_1.SetUp(StoryTransitionType.Transition, level_2, level1Tolevel2);
    }
    public void Next3Stage()
    {
        level_2.SetUp(StoryTransitionType.Transition, level_3, level2Tolevel3);
    }

    public void Next4Stage()
    {
        level_3.SetUp(StoryTransitionType.Transition, level_4, level3Tolevel4);
    }
    public void Next_4_HiddenStage()
    {
        level_4.SetUp(StoryTransitionType.Transition, level_4_hidden, level_4Tolevel_4_hidden);
    }
    public void Next_4_Hidden2Stage()
    {
        level_4.SetUp(StoryTransitionType.Transition, level_4_hidden_2, level_4_hiddenTolevel_4_hidden_2);
    }

    public void Next5Stage()
    {
        level_4.SetUp(StoryTransitionType.Transition, level_5, level4Tolevel5);
    }

    public void Next6Stage()
    {
        level_5.SetUp(StoryTransitionType.Transition, level_6, level5Tolevel6);
    }
    public void Next_6_HiddenStage()
    {
        level_6.SetUp(StoryTransitionType.Transition, level_6_hidden, level_6Tolevel_6_hidden);
    }
    public void Next_6_Hidden2Stage()
    {
        level_6.SetUp(StoryTransitionType.Transition, level_6_hidden2, level_6Tolevel_6_hidden2);
    }
    public void Next_6_Hidden2_1Stage()
    {
        level_6_hidden2.SetUp(StoryTransitionType.Transition, level_6_hidden2_1, level_6Tolevel_6_hidden2_1);
    }
    public void NextSpecial_1()
    {
        special_1.SetUp(StoryTransitionType.HiddenTransition, null, null);
    }
    public void NextSpecial_2()
    {
        special_2.SetUp(StoryTransitionType.HiddenTransition, null, null);
    }
    public void NextSpecial_3()
    {
        special_3.SetUp(StoryTransitionType.HiddenTransition, null, null);
    }
    public void Earn1StageReward()
    {
        RewardUI.Instance.AddConsumeItem(MonsterDataBase.Instance.evolutionCandy, 1);
        RewardUI.Instance.AddConsumeItem(MonsterDataBase.Instance.tranningBooks[4], 1);
    }
    public void Earn2StageReward()
    {
        RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.fireStone);
    }
    public void Earn3StageReward()
    {
        RewardUI.Instance.AddCostItem(CostItemType.Fame, 50);
    }
    public void Earn4StageReward()
    {
        RewardUI.Instance.AddCostItem(CostItemType.Fame, 100);
    }
    public void Earn5StageReward()
    {
        RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.iceStone);
    }
    public void EarnSpecial3Reward()
    {
        RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.symbolofSwordMaster);
    }
    public void EarnSpecial2Reward()
    {
        RewardUI.Instance.AddMonster(MonsterInstance.Instance(MonsterDataBase.Instance.livinglegendData));
    }
    public void EarnSpecialReward()
    {
        RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.piggyBank);
        RewardUI.Instance.AddCostItem(CostItemType.Fame, 50);
    }
    public void Earn1HiddenStage2()
    {
        RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.easterstone);
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
