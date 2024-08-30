using DS.Data;
using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialougeInfoSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent addEvt;
    public bool isBlock { get; private set; }
    public TextMeshProUGUI textUI { get; private set; }

    public DSDialogueSO nextData { get; private set; }  

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isBlock == false)
        {
            SoundManager.Instance.PlayEffect(166, 1f);

            DialougeUI.Instance.Next();
            if (addEvt != null)
                addEvt.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isBlock == false)
        {
            DialougeUI.Instance.SetCurrentDialogueSlot(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isBlock == false)
        {
            DialougeUI.Instance.ExitCurrentDialougeSlot();
        }
    }

    public void SetUp(DSDialogueChoiceData choiceData)
    {
        if(textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();
        // textUI.text = choiceData.Text;
        textUI.text = TextManager.Instance.GetString(choiceData.Text);
         nextData = choiceData.NextDialogue;
        textUI.color = Color.black;
    }

    public void Block()
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();
        isBlock = true;
        textUI.color = Color.red;
    }

    public void UnBlock()
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();
        isBlock = false;
        textUI.color = Color.black;
    }

    public void Active(bool check)
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();
        textUI.color = check ? Color.blue : Color.black;
    }
}
