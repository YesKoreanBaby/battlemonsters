using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum StoryTransitionType { Block, Blind, Open, Transition, HiddenTransition }
public class StageSelectSlot : MonoBehaviour, IPointerDownHandler
{
    public List<BattleData> stageData;
    public SerializableDictionary<int, int> bgmCilps;
    public UnityEvent clearEvent;
    public ConversationData clearStoryEvent;

    [System.NonSerialized]
    public bool clearBlock = false;

    private Image tile;
    private Image obj;
    private Color originColor;

    [System.NonSerialized]
    public bool isNext;

    [System.NonSerialized]
    public StoryTransitionType transitionType;

    public RectTransform connect { get; private set; }

    [System.NonSerialized]
    public bool isTransition = false;

    [System.NonSerialized]
    public bool isCleard;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(transitionType == StoryTransitionType.Open)
        {
            SoundManager.Instance.PlayEffect(166, 1f);

            clearBlock = false;
            StoryUI.Instance.currentSlot = this;
            PlayStage();
        }
    }
    public void SetUp(StoryTransitionType type, StageSelectSlot nextSlot, RectTransform connect)
    {
        if(tile == null)
        {
            tile = transform.GetChild(0).GetComponent<Image>();
            obj = transform.GetChild(1).GetComponent<Image>();
            originColor = tile.color;
        }
        if (type == StoryTransitionType.Block)
        {
            Closed();
            transitionType = type;
        }
        else if (type == StoryTransitionType.Blind)
        {
            Closed2();
            transitionType = type;
        }
        else if(type == StoryTransitionType.Open)
        {
            PopUp();

            if(this.connect != null)
                this.connect.gameObject.SetActive(true);

            transitionType = type;
        }
        else if(type == StoryTransitionType.Transition)
        {
            Next(nextSlot, connect);
            nextSlot.connect = connect;
            nextSlot.transitionType = StoryTransitionType.Open;
        }
        else if(type == StoryTransitionType.HiddenTransition)
        {
            PopUp2();
            this.transitionType = StoryTransitionType.Open;
        }
    }
    public void SetUp()
    {
        if (transitionType == StoryTransitionType.Block)
            Closed();
        else if (transitionType == StoryTransitionType.Blind)
            Closed2();
        else if (transitionType == StoryTransitionType.Open)
        {
            PopUp();

            if (this.connect != null)
                this.connect.gameObject.SetActive(true);
        }
    }
    public void Closed()
    {
        tile.color = obj.color = new Color(0.392156862745098f, 0.392156862745098f, 0.392156862745098f, 1f);
    }
    public void Closed2()
    {
        tile.color = obj.color = new Color(0.1607843137254902f, 0.1607843137254902f, 0.1607843137254902f, 1f);
        this.gameObject.SetActive(false);
    }
    public void PopUp()
    {
        obj.color = Color.white;
        tile.color = originColor;
    }
    public void PopUp2()
    {
        var audioSource = SoundManager.Instance.FindEffect(185);
        if (audioSource == null)
            SoundManager.Instance.PlayEffect(185, 1f);
        this.gameObject.SetActive(true);
        StartCoroutine(PopUp2Routine());
    }

    public void Next(StageSelectSlot nextSlot, RectTransform transition)
    {
        var audioSource = SoundManager.Instance.FindEffect(185);
        if (audioSource == null)
            SoundManager.Instance.PlayEffect(185, 1f);

        StartCoroutine(NextRoutine(nextSlot, transition));
    }

    public IEnumerator PopUpRoutine()
    {
        var color = tile.color;

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;

            float r = Mathf.Lerp(color.r, originColor.r, currentSpeed);
            float g = Mathf.Lerp(color.g, originColor.g, currentSpeed);
            float b = Mathf.Lerp(color.b, originColor.b, currentSpeed);
            float r2 = Mathf.Lerp(color.r, 1, currentSpeed);
            float g2 = Mathf.Lerp(color.g, 1, currentSpeed);
            float b2 = Mathf.Lerp(color.b, 1, currentSpeed);
            tile.color = new Color(r, g, b, 1f);
            obj.color = new Color(r2, g2, b2, 1f);
            yield return null;
        }
    }

    public IEnumerator PopUp2Routine()
    {
        var color = tile.color;

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;

            float r = Mathf.Lerp(color.r, originColor.r, currentSpeed);
            float g = Mathf.Lerp(color.g, originColor.g, currentSpeed);
            float b = Mathf.Lerp(color.b, originColor.b, currentSpeed);
            float r2 = Mathf.Lerp(color.r, 1, currentSpeed);
            float g2 = Mathf.Lerp(color.g, 1, currentSpeed);
            float b2 = Mathf.Lerp(color.b, 1, currentSpeed);
            float a = Mathf.Lerp(0, 1, currentSpeed);
            tile.color = new Color(r, g, b, a);
            obj.color = new Color(r2, g2, b2, a);
            yield return null;
        }

        isNext = true;
    }

    private void PlayStage()
    {
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

        StoryUI.Instance.Closed();

        CombatManager.Instance.battleDataObject = stageData;

        CombatManager.Instance.ReturnGame();

        CombatUI.Instance.UpdateMonsterImages();
        if (AlarmUI.Instance.isPopUp == true)
            AlarmUI.Instance.Closed();


    }
    private IEnumerator NextRoutine(StageSelectSlot nextSlot, RectTransform transition)
    {
        var size = transition.sizeDelta;

        transition.sizeDelta = Vector2.zero;
        transition.gameObject.SetActive(true);

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;

            float width = Mathf.Lerp(0, size.x, currentSpeed);
            float height = Mathf.Lerp(0, size.y, currentSpeed);
            transition.sizeDelta = new Vector2(width, height);
            yield return null;
        }

        if(nextSlot.gameObject.activeInHierarchy == true)
            StartCoroutine(nextSlot.PopUpRoutine());
        else
        {
            nextSlot.gameObject.SetActive(true);
            StartCoroutine(nextSlot.PopUpRoutine());
        }

        yield return new WaitForSeconds(0.25f);

        isNext = true;
    }
}
