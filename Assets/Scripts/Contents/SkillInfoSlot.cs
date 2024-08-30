using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillInfoSlot : MonoBehaviour, IPointerDownHandler
{
    [System.NonSerialized]
    public object skillData;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(skillData != null)
        {
            SoundManager.Instance.PlayEffect(166, 1f);

            var obj = this.transform.GetChild(0).gameObject;

            if (obj.activeInHierarchy == false)
            {
                if (SkillUI.Instance.currentSelectBox != null)
                {
                    SkillUI.Instance.currentSelectBox.SetActive(false);
                    SkillUI.Instance.currentSelectBox = obj;
                }
                else
                    SkillUI.Instance.currentSelectBox = obj;

                obj.gameObject.SetActive(true);

                SkillUI.Instance.SettingGeneratorSkillView(skillData);
            }
        }
    }
}
