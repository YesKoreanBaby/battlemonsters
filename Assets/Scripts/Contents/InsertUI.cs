using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InsertUI : MonoBehaviour
{
    public TypingText typingText;
    public Button button;
    public Button button2;
    public RectTransform scrollingText;
    public TextMeshProUGUI scrollViewText;

    public Sprite loseSpr;
    public Sprite winSpr;
    public Sprite defendingWin;
    public Sprite victory;

    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public bool isButtonDown = false;

    private Animator animator = null;
    private static InsertUI instance = null;
    private int soundIndex = -1;
    public static InsertUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        Init();
    }
    public void PopUpLose()
    {
        Parser.Instance.RemoveData();
        soundIndex = 41;
        int previousNumber = LeagueManager.Instance.GetCurrentLeagueNumber();
        int number = Mathf.Min(4, previousNumber + 1);

        Queue<string> loseTextQueue = new Queue<string>();

        string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "당신은 패배하였습니다..." : "You are defeated...";
        loseTextQueue.Enqueue(text);
        if (number == 4)
        {
            text = (TextManager.Instance.language == SystemLanguage.Korean) ? "팀은 역사의 뒤안길로 사라져버립니다..." : "The team disappears into history...";
            loseTextQueue.Enqueue(text);

            PopUp(loseTextQueue, loseSpr, () =>
            {
                MainSystemUI.Instance.Closed2();
                SelectTeamUI.Instance.PopUp();
            });
        }
        else
        {
            if (number == 0)
                number = 1;

            text = (TextManager.Instance.language == SystemLanguage.Korean) ? $"{number}부 리그로 강등당합니다..." : $"Relegated to the {number}nd division...";
            loseTextQueue.Enqueue(text);

            PopUp(loseTextQueue, loseSpr, () =>
            {
                if (SelectTeamUI.Instance.selectTeam == LeagueManager.Instance.defendingChampionData)
                    LeagueManager.Instance.RelegationChampion();
                else
                    LeagueManager.Instance.RestartLeague();

                CalendarUI.Instance.UpdateCalander(LeagueManager.Instance.GetPlayerContainsLeague());
                MainSystemUI.Instance.PopUp();
            });

        }
    }
    public void PopUpWin()
    {
        soundIndex = 39;

        int previousNumber = LeagueManager.Instance.GetCurrentLeagueNumber();
        int number = Mathf.Max(-2, previousNumber - 1);
        Queue<string> winTextQueue = new Queue<string>();

        string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "당신은 승리하였습니다!!!" : "You have won!!!";
        winTextQueue.Enqueue(text);

        if (number == 0)
        {
            text = (TextManager.Instance.language == SystemLanguage.Korean) ? "하지만 아직 챔피언이 남았습니다..." : "But there's still a champion left...";
            winTextQueue.Enqueue(text);

            text = (TextManager.Instance.language == SystemLanguage.Korean) ? "마지막 권좌에 도전하시겠습니까?" : "Would you like to take on the final throne?";
            winTextQueue.Enqueue(text);

            PopUp(winTextQueue, null, () =>
            {
                MainSystemUI.Instance.Closed();

                CombatManager.Instance.battleDataObject = LeagueManager.Instance.defendingChampionData.championData;
                CombatManager.Instance.ReturnGame();

                CombatUI.Instance.UpdateMonsterImages();
            });
        }
        else if(number == -1)
        {
            text = (TextManager.Instance.language == SystemLanguage.Korean) ? "리그 재패에 성공하였습니다!" : "Successfully won the league!";
            winTextQueue.Enqueue(text);

            text = (TextManager.Instance.language == SystemLanguage.Korean) ? "당신의 팀은 명예의 전당에 오릅니다!" : "Your team will be in the Hall of Fame!";
            winTextQueue.Enqueue(text);
            PopUp(winTextQueue, defendingWin, () =>
            {
                LeagueManager.Instance.RestartDefendingLeage();
                CalendarUI.Instance.UpdateCalander(LeagueManager.Instance.GetPlayerContainsLeague());
                RewardUI.Instance.AddCostItem(CostItemType.Ranking, 1);
                RewardUI.Instance.AddCostItem(CostItemType.Money, 1000);
                RewardUI.Instance.AddCostItem(CostItemType.Fame, 20);
                MainSystemUI.Instance.PopUp();
            }, true);
        }
        else if(number == -2)
        {
            text = (TextManager.Instance.language == SystemLanguage.Korean) ? "성공적으로 챔피언 타이틀을 방어해냈습니다" : "Successfully defended your title as champion";
            winTextQueue.Enqueue(text);
            PopUp(winTextQueue, defendingWin, () =>
            {
                LeagueManager.Instance.RestartDefendingLeage();
                CalendarUI.Instance.UpdateCalander(LeagueManager.Instance.GetPlayerContainsLeague());
                MainSystemUI.Instance.PopUp();
            }, true);
        }
        else
        {
            text = (TextManager.Instance.language == SystemLanguage.Korean) ? $"{number}부 리그로 승격합니다!!!" : $"Promoted to {number}nd division";
            winTextQueue.Enqueue(text);
            PopUp(winTextQueue, winSpr, () =>
            {
                LeagueManager.Instance.RestartLeague();
                CalendarUI.Instance.UpdateCalander(LeagueManager.Instance.GetPlayerContainsLeague());
                RewardUI.Instance.AddCostItem(CostItemType.Money, 1000);
                RewardUI.Instance.AddCostItem(CostItemType.Fame, 20);
                MainSystemUI.Instance.PopUp();
            });
        }
    }
    public void PopUpVictory()
    {
        soundIndex = 40;

        int previousNumber = LeagueManager.Instance.GetCurrentLeagueNumber();
        Queue<string> winTextQueue = new Queue<string>();

        string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "당신은 비겼습니다." : "Victory is not decided";
        winTextQueue.Enqueue(text);

        text = (TextManager.Instance.language == SystemLanguage.Korean) ? "성공적으로 챔피언 타이틀을 방어해냈습니다" : "Successfully defended your title as champion";
        string text2 = (TextManager.Instance.language == SystemLanguage.Korean) ? $"{previousNumber}부 리그에 잔류합니다." : $"Remains in the {previousNumber}nd division";
        string txt = (SelectTeamUI.Instance.selectTeam == LeagueManager.Instance.defendingChampionData) ? text : text2;
        winTextQueue.Enqueue(txt);

        PopUp(winTextQueue, victory, () =>
        {
            if (SelectTeamUI.Instance.selectTeam == LeagueManager.Instance.defendingChampionData)
                LeagueManager.Instance.RestartDefendingLeage();
            else
                LeagueManager.Instance.RestartLeague();

            CalendarUI.Instance.UpdateCalander(LeagueManager.Instance.GetPlayerContainsLeague());
            MainSystemUI.Instance.PopUp();
        });
    }
    private void PopUp(Queue<string> textQueue, Sprite image, Action evt, bool isScrolling = false)
    {
        if(isPopUp == false)
        {
            MainSystemUI.Instance.StopBgm();
            if (MainSystemUI.Instance.isPopUp == true)
                MainSystemUI.Instance.Closed2();

            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            scrollingText.gameObject.SetActive(false);
            scrollViewText.gameObject.SetActive(false);
            button.interactable = false;
            button2.interactable = false;

            animator.enabled = true;
            animator.Play("idle");

            StartCoroutine(InsertRoutine(textQueue, image, evt, isScrolling));
            isPopUp = true;
        }
    }
    public void Closed()
    {
        if (isPopUp == true)
        {
            animator.enabled = false;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }

            isPopUp = false;
        }
    }

    public void SetButton()
    {
        isButtonDown = true;
    }

    private void Init()
    {
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        isPopUp = false;
    }
    private IEnumerator InsertRoutine(Queue<string> textQueue, Sprite image, Action evt, bool isScrolling)
    {
        SoundManager.Instance.StopBgm();

        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        while(textQueue.Count > 0)
        {
            isButtonDown = false;
            button.interactable = false;
            animator.Play("idle");
            typingText.Play(textQueue.Peek(), 0.1f);

            yield return new WaitUntil(() => typingText.isRunning == false);
            button.interactable = true;
            animator.Play("enterText");

            yield return new WaitUntil(() => isButtonDown == true);
            yield return waitTime;
            textQueue.Dequeue();
            isButtonDown = false;
        }

        if(image != null)
        {
            animator.Play("endText");
            button2.image.sprite = image;

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("endTextidle"));

            button2.interactable = true;

            SoundManager.Instance.PlayBgm(soundIndex, 1f);
            if (isScrolling == true)
            {
                yield return new WaitForSeconds(0.25f);
                StartCoroutine(TextScrolling());
            }
            yield return new WaitUntil(() => isButtonDown == true);

            SoundManager.Instance.StopBgm();
            Closed();
            evt.Invoke();
        }
        else
        {
            SoundManager.Instance.StopBgm();
            Closed();
            evt.Invoke();
        }
    }
    private IEnumerator TextScrolling()
    {
        scrollingText.gameObject.SetActive(true);

        scrollViewText.gameObject.SetActive(true);
        scrollViewText.text = LeagueManager.Instance.Write();
        var scrollBar = scrollingText.GetComponentInChildren<Scrollbar>();

        float startValue = 1f;
        float endValue = 0f;
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 20f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

             float currentSpeed = currentTime / lerpTime;
             scrollBar.value = Mathf.Lerp(startValue, endValue, currentSpeed);
             yield return null;
        }

        scrollingText.gameObject.SetActive(false);
    }
}
