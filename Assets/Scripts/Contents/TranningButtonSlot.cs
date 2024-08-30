using UnityEngine;
using UnityEngine.EventSystems;

public class TranningButtonSlot : MonoBehaviour, IPointerDownHandler
{
    [System.NonSerialized]
    public int index;

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlayEffect(166, 1f);

        var obj = this.transform.GetChild(1).gameObject;
        bool active = TranningUI.Instance.CheckActvie(index);
        if(obj.activeInHierarchy == false)
        {
            TranningUI.Instance.ActiveBox(index, true);
            if (TranningUI.Instance.selectButtons[index].interactable == true && active == true)
            {
                TranningUI.Instance.ViewActive(index, true);
            }
            else
            {
                TranningUI.Instance.ViewActive(index, false);
                TranningUI.Instance.ActiveBox(index, false);
            }
        }
        else
        {
            if (TranningUI.Instance.selectButtons[index].interactable == true && active == true)
            {
                TranningUI.Instance.UpdateData(index);
            }
            else
                TranningUI.Instance.ActiveBox(index, false);
        }
    }
}
