using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreagureItemSlot : MonoBehaviour, IPointerDownHandler
{
    [System.NonSerialized]
    public ItemElementalData itemData;
  
    [System.NonSerialized]
    public Image treagureImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemData != null)
        {
            var obj = this.transform.GetChild(0).gameObject;
            SoundManager.Instance.PlayEffect(166, 1f);

            if (obj.activeInHierarchy == false)
            {
                if (TreagureInventoryUI.Instance.currentSelectBox != null)
                {
                    TreagureInventoryUI.Instance.currentSelectBox.SetActive(false);
                    TreagureInventoryUI.Instance.currentSelectBox = obj;
                }
                else
                    TreagureInventoryUI.Instance.currentSelectBox = obj;

                obj.gameObject.SetActive(true);
                TreagureInventoryUI.Instance.SetView(this);
            }
        }
    }

    public void Active(ItemElementalData itemData)
    {
        treagureImage.color = Color.white;
        this.itemData = itemData;
    }

    public void DontActive()
    {
        treagureImage.color = Color.black;
        itemData = null;
    }

    public void Init()
    {
        treagureImage = GetComponent<Image>();
    }
}
