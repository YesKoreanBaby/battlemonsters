using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreagureInventoryUI : MonoBehaviour
{
    public TreagureItemSlot[] itemImages;
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public RectTransform fixedView;
    private static TreagureInventoryUI instance = null;
    public static TreagureInventoryUI Instance { get { return instance; } }

    [System.NonSerialized]
    public GameObject currentSelectBox;

    [System.NonSerialized]
    public bool isPopUp = false;
    private void Awake()
    {
        instance = this;
        Init();
    }
    public void PopUp()
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            SettingData();
            isPopUp = true;
        }
    }

    public void Closed()
    {
        if (isPopUp == true)
        {
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }

            isPopUp = false;
        }
    }

    public void SetView(TreagureItemSlot itemImage)
    {
        if (itemImage != null)
        {
            fixedView.gameObject.SetActive(true);
            this.itemImage.sprite = itemImage.itemData.itemImage;
            this.itemImage.SetNativeSize();
            this.itemImage.rectTransform.sizeDelta = this.itemImage.rectTransform.sizeDelta / 3.5f;
            this.itemName.text = TextManager.Instance.GetString(itemImage.itemData.name + "Name");
            this.itemDescription.text = TextManager.Instance.GetString(itemImage.itemData.name + "Desc");
        }
        else
            fixedView.gameObject.SetActive(false);
    }

    private void Init()
    {
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < itemImages.Length; ++i)
            itemImages[i].Init();
    }

    private void SettingData()
    {
        fixedView.gameObject.SetActive(false);
        if (currentSelectBox != null)
        {
            currentSelectBox.SetActive(false);
            currentSelectBox = null;
        }

        for (int i = 0; i < itemImages.Length; ++i)
            itemImages[i].DontActive();

        var itemDatas = ItemInventory.Instance.treasureDatas;
        for (int i = 0; i < itemDatas.Length; ++i)
        {
            if (itemDatas[i] == null)
                continue;

            var itemImage = Array.Find(itemImages, x => x.treagureImage.sprite == itemDatas[i].itemImage);
            if (itemImage != null)
                itemImage.Active(itemDatas[i]);
        }
    }
}
