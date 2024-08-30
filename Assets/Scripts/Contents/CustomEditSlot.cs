using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEditSlot : MonoBehaviour, IPointerDownHandler
{
    [System.NonSerialized]
    public SkillData skillData;

    [System.NonSerialized]
    public TextMeshProUGUI text;

    [System.NonSerialized]
    public int number = 0;
    private void Start()
    {
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (skillData != null)
        {
            SoundManager.Instance.PlayEffect(166, 1f);

            var child = transform.GetChild(0);
            if(child.gameObject.activeInHierarchy == true)
                CustomEditUI.Instance.RemoveSkillPriority(this);
            else
                CustomEditUI.Instance.AddSkillPriority(this);
        }
    }
}
