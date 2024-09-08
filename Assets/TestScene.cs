using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TestScene : MonoBehaviour
{
    public List<SkillData> skillDatas;

    public Image skillIcon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDesc;

    private int currentIndex = 0;
    private bool isRunning = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isRunning == false)
            {
                StartCoroutine(TestRoutine());
            }
        }
    }

    private void UpdateData()
    {
        SettingData();
        currentIndex++;
        if (currentIndex >= skillDatas.Count)
            currentIndex = 0;
    }
    private void SettingData()
    {
        skillIcon.sprite = skillDatas[currentIndex].skillDictionary.skillIcon;
        skillName.text = $"{TextManager.Instance.GetString(skillDatas[currentIndex].skillDictionary.name + "Name")} / {skillDatas[currentIndex].status}";
        skillDesc.text = TextManager.Instance.GetString(skillDatas[currentIndex].skillDictionary.name + "Desc");
    }

    private IEnumerator TestRoutine()
    {
        isRunning = true;
        WaitForSeconds waitTime = new WaitForSeconds(10f);
        UpdateData();
        yield return waitTime;

        while (currentIndex != 0)
        {
            UpdateData();
            yield return waitTime;
        }
        isRunning = false;
    }
}
