using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemInventoryUI : MonoBehaviour
{
    public ItemInfoSlot[] itemImages;
    public Image itemImage;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    public RectTransform fixedView;

    public RectTransform singleItemDescriptionView;
    public RectTransform multiItemDescriptionView;
    public Image singleItemImage;
    public TextMeshProUGUI singleItemName;
    public TextMeshProUGUI singleItemDescription;
    public Button itemSellButton;

    private static ItemInventoryUI instance = null;
    public static ItemInventoryUI Instance { get { return instance; } }

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
        if(isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
            singleItemDescriptionView.gameObject.SetActive(false);
            SettingData();
            isPopUp = true;
        }
    }

    public void PopUp(ItemElementalData itemData)
    {
        if (isPopUp == false)
        {
            int childCount = this.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
            multiItemDescriptionView.gameObject.SetActive(false);

            this.singleItemImage.sprite = itemData.itemImage;

            if (itemData.tranningType != TranningType.Random)
            {
                bool isKorean = TextManager.Instance.language == SystemLanguage.Korean;
                this.singleItemName.text = isKorean ? "훈련용 책" : "Tranning Book";
                this.singleItemDescription.text = isKorean ? "몬스터의 기본능력을 상승시킨다." : "Increases the monster's basic stats";
                var tranningText = GetTranningBookText(itemData);
                this.singleItemDescription.text += tranningText;
            }
            else
            {
                this.singleItemName.text = TextManager.Instance.GetString(itemData.name + "Name");

                if (itemData.effectType == ItemElementalEffectType.Combine)
                {
                    bool isKorean = TextManager.Instance.language == SystemLanguage.Korean;
                    this.singleItemDescription.text = isKorean ? "특정 몬스터를 조합시키는데\r\n사용된다." : "Used to combine specific monsters";
                }
                else
                    this.singleItemDescription.text = TextManager.Instance.GetString(itemData.name + "Desc");
            }
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

        //if (CombineUI.Instance.isPopUp == false && DialougeUI.Instance.isPopUp == true)
        //{
        //    if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
        //        AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Top);
        //}
    }
    public void SetView(ItemInfoSlot itemImage)
    {
        if(itemImage != null)
        {
            fixedView.gameObject.SetActive(true);
            this.itemImage.sprite = itemImage.itemData.itemImage;
           // this.itemName.text = itemImage.itemData.ItemName;
           // this.itemDescription.text = itemImage.itemData.ItemComment;
            if(itemImage.itemData.tranningType != TranningType.Random)
            {
                bool isKorean = TextManager.Instance.language == SystemLanguage.Korean;
                this.itemName.text = isKorean ? "훈련용 책" : "Tranning Book";
                this.itemDescription.text = isKorean ? "몬스터의 기본능력을 상승시킨다." : "Increases the monster's basic stats";
                var tranningText = GetTranningBookText(itemImage.itemData);
                this.itemDescription.text += tranningText;
            }
            else
            {
                this.itemName.text = TextManager.Instance.GetString(itemImage.itemData.name + "Name");

                if(itemImage.itemData.effectType == ItemElementalEffectType.Combine)
                {
                    bool isKorean = TextManager.Instance.language == SystemLanguage.Korean;
                    this.itemDescription.text = isKorean ? "특정 몬스터를 조합시키는데\r\n사용된다." : "Used to combine specific monsters";
                }
                else
                    this.itemDescription.text = TextManager.Instance.GetString(itemImage.itemData.name + "Desc");
            }
        }
        else
            fixedView.gameObject.SetActive(false);
    }

    public void UpdateSellItemButton(int index)
    {
        itemSellButton.onClick.RemoveAllListeners();

        int value = (ItemInventory.Instance.consumeItemDatas[index].effectType == ItemElementalEffectType.Combine) ? 500 : 20;
        string text = $"아이템을 판매하시겠습니까?\r\n\r\n<color=black><b><size=32>(+{value}$)";
        itemSellButton.onClick.AddListener(() => { AlarmUI.Instance.PopUp(text, () => { SellItem(index); }); });
    }
    private void SellItem(int index)
    {
        SoundManager.Instance.PlayEffect(179, 1f);

        int value = (ItemInventory.Instance.consumeItemDatas[index].effectType == ItemElementalEffectType.Combine) ? 500 : 20;
        ItemInventory.Instance.money += value;
        ItemInventory.Instance.RemoveConsumItem(index);
        SettingData();
        AlarmUI.Instance.Closed();
    }
    private void Init()
    {
        int childCount = this.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void SettingData()
    {
        fixedView.gameObject.SetActive(false);
        if (currentSelectBox != null)
        {
            currentSelectBox.SetActive(false);
            currentSelectBox = null;
        }
        var itemDatas = ItemInventory.Instance.consumeItemDatas;
        for(int i = 0; i < itemDatas.Length; ++i)
        {
            itemImages[i].UpdateSlot(itemDatas[i]);
            itemImages[i].index = i;
        }
    }

    private string GetTranningBookText(ItemElementalData itemData)
    {
        float hp, mp, atk, def, dex, hrc, mrc, cri, ddg;
        itemData.GetTranningData(out hp, out mp, out atk, out def, out dex, out hrc, out mrc, out cri, out ddg);
        string value = $"\n\n------------------------------------\n\nhp:   +{hp}\nmp:   +{mp}\natk:  +{atk}\ndef:  +{def}\ndex:  -{dex}\nhrc:  +{hrc * 100}%\nmrc:  +{mrc * 100}%\ncri:  +{cri * 100}%\nddg:  +{ddg * 100}%";

        return value;
        
    }
}
