using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class SelectTeamUI : MonoBehaviour
{
    public Scrollbar scrollbar;
    public Button[] teamImages;
    public TeamData[] teamDatas;
    public Sprite star;
   
    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public TeamData selectTeam;

    private static SelectTeamUI instance = null;
    public static SelectTeamUI Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        Init();
        SettingData();
        UpdateTeamLockeds();
    }
    public void Init()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
        SettingTeam();
    }
    public void PopUp()
    {
        if(isPopUp == false)
        {
            SoundManager.Instance.PlayBgm(43, 1f);

            scrollbar.value = 1f;
            isPopUp = true;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    public void Closed()
    {
        if (isPopUp == true)
        {
            SoundManager.Instance.StopBgm();
            isPopUp = false;
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void SettingData()
    {
        for(int i = 0; i < teamImages.Length; i++)
        {
            teamImages[i].image.sprite = teamDatas[i].teamIcon;

            var text = teamImages[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = teamDatas[i].name;
        }
    }

    public void SelectTeamAram(int index)
    {
        selectTeam = teamDatas[index];
        AlarmUI.Instance.PopUpForKey(selectTeam.name, SelectTeam, null);
    }

    public void SelectTeamAramProuduct(int index)
    {
        selectTeam = teamDatas[index];
        AlarmUI.Instance.PopUpForKey(selectTeam.name, SelectTeam, null);
    }
    public void UpdateTeamLockeds()
    {
       
    }
    public void SelectTeam()
    {
        ItemInventory.Instance.SetCount(selectTeam.startMoney, selectTeam.startDiamond);
        StoryUI.Instance.SetStageSelectSlots();
        SystemUI.Instance.InitValues();

        TranningUI.Instance.playerInventory.Clear();

        TranningUI.Instance.playerInventory.AddItems(selectTeam.startMonsters);

        CombatUI.Instance.UpdateMonsterImages();

        LeagueManager.Instance.StartLeague(selectTeam, teamDatas);
  
        Closed();
        CalendarUI.Instance.UpdateCalander(LeagueManager.Instance.league_3);
        MainSystemUI.Instance.PopUp();
        AlarmUI.Instance.Closed();
    }

    private void SettingTeam()
    {
        for(int i = 0; i < teamImages.Length;++i)
        {
            int teamLevel = Mathf.Min(teamDatas[i].teamLevel, 5);
            for(int j = 1; j < teamLevel + 1; ++j)
            {
                var image = teamImages[i].transform.GetChild(j).GetComponent<Image>();
                image.sprite = star;
            }
        }
    }
}
