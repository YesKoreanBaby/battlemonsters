using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public struct BuffMessage
{
    public string value;
    public bool buff;
}
public class FormationBuffUI : MonoBehaviour
{
    public TextMeshProUGUI[] buffDebuffTexts;

    private Stack<BuffMessage> buffStack = new Stack<BuffMessage>();

    private void Start()
    {
       
    }

    public void AddData(string value, bool buff)
    {
        BuffMessage msg = new BuffMessage();
        msg.value = value;
        msg.buff = buff;
        buffStack.Push(msg);
    }
    public void SettingView()
    {
        for (int i = 0; i < buffDebuffTexts.Length; i++)
        {
            buffDebuffTexts[i].gameObject.SetActive(false);
        }

        if(buffStack.Count >= buffDebuffTexts.Length)
        {
            List<BuffMessage> list = buffStack.ToList();

            var tmp = list[1];
            list[1] = list[2];
            list[2] = tmp;

            list.Reverse();
            buffStack.Clear();
            for (int i = 0; i < list.Count; ++i)
                buffStack.Push(list[i]);
        }

        for (int i = buffDebuffTexts.Length - 1; i >= 0; i--)
        {
            if (buffStack.Count > 0)
            {
                var msg = buffStack.Pop();
                string value = msg.buff ? "++" : "--";

                buffDebuffTexts[i].gameObject.SetActive(true);
                buffDebuffTexts[i].text = $"{msg.value} {value}";
                buffDebuffTexts[i].font = msg.buff ? MonsterDataBase.Instance.buffFont : MonsterDataBase.Instance.debuffFont;
            }
        }
    }
    public void SettingAtk(bool buff)
    {
        string value = buff ? "++" : "--";
        buffDebuffTexts[0].text = $"ATK<b><size=96> {value}";
        buffDebuffTexts[0].font = buff ? MonsterDataBase.Instance.buffFont : MonsterDataBase.Instance.debuffFont;
    }
    public void SettingDef(bool buff)
    {
        string value = buff ? "++" : "--";
        buffDebuffTexts[1].text = $"DEF<b><size=96> {value}";
        buffDebuffTexts[1].font = buff ? MonsterDataBase.Instance.buffFont : MonsterDataBase.Instance.debuffFont;
    }
    public void SettingCri(bool buff)
    {
        string value = buff ? "++" : "--";
        buffDebuffTexts[2].text = $"CRI<b><size=96> {value}";
        buffDebuffTexts[2].font = buff ? MonsterDataBase.Instance.buffFont : MonsterDataBase.Instance.debuffFont;
    }
    public void SettingDde(bool buff)
    {
        string value = buff ? "++" : "--";
        buffDebuffTexts[3].text = $"DDE<b><size=96> {value}";
        buffDebuffTexts[3].font = buff ? MonsterDataBase.Instance.buffFont : MonsterDataBase.Instance.debuffFont;
    }
}
