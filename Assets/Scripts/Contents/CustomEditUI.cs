using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomEditUI : MonoBehaviour
{
    [System.NonSerialized]
    public bool isPopUp = false;

    private static CustomEditUI instance = null;
    public static CustomEditUI Instance { get { return instance; } }

    public Image[] icons;
    public CustomEditSlot[] slots;

    private List<SkillData> skillDatas = new List<SkillData>();
    private List<SkillData> originSkillDatas;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Closed();
    }

    public void PopUp(List<SkillData> skillDatas)
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

            SettingData(skillDatas);
        }
    }

    private void SettingData(List<SkillData> skillDatas)
    {
        this.skillDatas.Clear();
        originSkillDatas = skillDatas;
        for (int i = 0; i < icons.Length; ++i)
        {
            icons[i].gameObject.SetActive(false);
            icons[i].transform.GetChild(0).gameObject.SetActive(false);
            icons[i].transform.GetChild(1).gameObject.SetActive(false);
            slots[i].skillData = null;
        }
        for (int i = 0; i < skillDatas.Count; ++i)
        {
            icons[i].gameObject.SetActive(true);
            icons[i].sprite = skillDatas[i].skillDictionary.skillIcon;
            slots[i].skillData = skillDatas[i];
        }
    }

    public void AddSkillPriority(CustomEditSlot slot)
    {
        if(!skillDatas.Contains(slot.skillData))
        {
            skillDatas.Add(slot.skillData);
            slot.transform.GetChild(0).gameObject.SetActive(true);
            slot.transform.GetChild(1).gameObject.SetActive(true);
            slot.number = skillDatas.Count;
            slot.text.text = slot.number.ToString();

        }
    }

    public void RemoveSkillPriority(CustomEditSlot slot)
    {
        if(skillDatas.Contains(slot.skillData))
        {
            skillDatas.Remove(slot.skillData);
            slot.transform.GetChild(0).gameObject.SetActive(false);
            slot.transform.GetChild(1).gameObject.SetActive(false);

            var newSlots = Array.FindAll(slots, x => x.transform.GetChild(0).gameObject.activeInHierarchy == true);
            for (int i = 0; i < newSlots.Length; ++i)
            {
                newSlots[i].number = Mathf.Max(1, newSlots[i].number - 1);
                newSlots[i].text.text = newSlots[i].number.ToString();
            }
        }
    }

    public void Enter()
    {
        var notContainsDatas = originSkillDatas.FindAll(x => !skillDatas.Contains(x));

        originSkillDatas.Clear();
        originSkillDatas.AddRange(skillDatas);
        originSkillDatas.AddRange(notContainsDatas);
        SkillUI.Instance.ReSettingConfirmSkill();
        Closed();
    }

    public void Cancel()
    {
        Closed();
    }

    public void Closed()
    {
        isPopUp = false;
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
