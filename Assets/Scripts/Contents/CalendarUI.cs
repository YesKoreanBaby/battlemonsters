using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class CalendarUI : MonoBehaviour
{
    public RectTransform[] images;

    public RectTransform selectImage;

    public RectTransform cancelButton;

    public Vector2 selectImageStartPosition;

    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public int xIndex = 0;

    [System.NonSerialized]
    public int yIndex = 0;

    [System.NonSerialized]
    public bool blackSound = true;

    private static CalendarUI instance = null;
    public static CalendarUI Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        Init();
    }
    public void PopUp(bool activeCancel = true)
    {
        if(isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }

            cancelButton.gameObject.SetActive(activeCancel);
        }
    }
    public void Closed()
    {
        if (isPopUp == true)
        {
            isPopUp = false;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(false);
            }
        }
    }
    public void UpdateCalander(Dictionary<int, List<Tuple<TeamData, TeamData>>> league)
    {
        xIndex = 0;
        yIndex = 0;
        LeagueManager.Instance.SetLeagueTarget(league);
        LeagueManager.Instance.SetFinalMatch();
        for (int i = 0; i < images.Length; ++i)
        {
            var team = FindMatchingTeam(i);
            UpdateDay(team, images[i]);
        }

        selectImage.anchoredPosition = selectImageStartPosition;
    }

    public void UpdateCalander()
    {
        for (int i = 0; i < images.Length; ++i)
        {
            var team = FindMatchingTeam(i);
            UpdateDay(team, images[i]);
        }

        selectImage.anchoredPosition = GetCurrentIndexPosition();
    }

    public TeamData FindMatchingTeam(int timecount)
    {
        var league = LeagueManager.Instance.currentLeague;

        List<Tuple<TeamData, TeamData>> teamData = null;
        league.TryGetValue(timecount, out teamData);

        if (teamData != null)
        {
            var team = LeagueManager.Instance.FindPlayMatchTeam(teamData);
            if (team != null)
                return team;
            else
                return null;
        }
        else
            return null;
    }
    public IEnumerator NextMoveRoutine()
    {
        blackSound = true;

        Vector2 startPosition = selectImage.anchoredPosition;
        Vector2 endPosition = GetNextPosition();

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        SoundManager.Instance.PlayEffect(146, 1f);

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            selectImage.anchoredPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        blackSound = false;

        yield return new WaitForSeconds(2f);
        blackSound = true;
        yield return new WaitForSeconds(0.4f);
    }
    public void Cancel()
    {
        Closed();
    }
    private void UpdateDay(TeamData teamData, RectTransform image)
    {
        var background = image.transform.GetChild(0).GetComponent<Image>();
        var icon = image.transform.GetChild(1).GetComponent<Image>();
        if (teamData == null)
        {
            background.color = Color.white;
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.sprite = teamData.teamIcon;

            background.color = new Color(0.0745098039215686f, 0.8509803921568627f, 0.7490196078431373f, 1f);
        }
    }
    private void Init()
    {
        isPopUp = false;
        var childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(false);
        }
    }
    //private Vector2 GetIndexPosition(int currentIndex)
    //{
    //    if (((currentIndex - 1) > 0) && (currentIndex - 1) % 6 == 0)
    //    {
    //        return new Vector2(selectImageStartPosition.x, selectImageStartPosition.y - (123 * ((currentIndex - 1) / 6)));
    //    }
    //    else
    //        return new Vector2(selectImageStartPosition.x + (123 * currentIndex), selectImageStartPosition.y);
    //}

    private Vector2 GetNextPosition()
    {
        xIndex++;
        if ((xIndex > 0) && (xIndex % 6 == 0))
        {
            xIndex = 0;
            yIndex++;
        }
       
        var position = new Vector2(selectImageStartPosition.x + (123 * xIndex), selectImageStartPosition.y - (123 * yIndex));

        return position;
    }

    private Vector2 GetCurrentIndexPosition()
    {
        var position = new Vector2(selectImageStartPosition.x + (123 * xIndex), selectImageStartPosition.y - (123 * yIndex));
        return position;
    }
}
