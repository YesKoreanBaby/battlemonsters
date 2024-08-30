using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxInfoSlot : MonoBehaviour, IPointerDownHandler
{
    public Animator animator;
    public Animator backgroundAnim;
    public Image costItemImage;
    public Image objectImage;
    public TextMeshProUGUI costItemText;
    public bool isBlock { get; private set; }

    [System.NonSerialized]
    public bool isSell;
    public bool checkActive { get; private set; }

    private bool isRunning = false;
    private bool isRunning2 = false;

    [System.NonSerialized]
    public bool isHidden;

    [System.NonSerialized]
    public MonsterShopData monsterShopData;
    public MonsterInstance selectMonster { get; private set; }

    [System.NonSerialized]
    public ItemShopData itemShopData;
    public ItemElementalData selectItem { get; private set; }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(isBlock == false)
        {
            if (DialougeUI.Instance.currentBoxInfo == this)
                DialougeUI.Instance.ExitCurrentBoxSlot();
            else
            {
                if(monsterShopData != null)
                    SoundManager.Instance.PlayEffect(178, 1f);

                DialougeUI.Instance.SetCurrentBoxInfo(this);
            }
        }
    }
    private void Update()
    {
        if (isBlock == true && isRunning == false)
        {
            StartCoroutine(ReturnBlockRoutine());
        }

        if(backgroundAnim != null && (backgroundAnim.gameObject.activeInHierarchy == true) && isRunning2 == false)
        {
            if (isHidden == true)
                backgroundAnim.Play("hidden");
            else
                backgroundAnim.Play("normal");
            isRunning2 = true;
        }
    }
    private void OnEnable()
    {
        if(DialougeUI.Instance != null && DialougeUI.Instance.currentBoxInfo != null)
            DialougeUI.Instance.ResetCurrentBoxInfo();

        if(backgroundAnim != null)
        {
            if (isHidden == true)
                backgroundAnim.Play("hidden");
            else
                backgroundAnim.Play("normal");
        }
        checkActive = false;
        if (isSell == true)
            animator.Play("sell");
        else
            isBlock = false;
    }
    public void Active(bool check)
    {
        if(check == true)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("popupidle"))
            {
                animator.Play("popup");

                if(isBlock == false)
                    isBlock = true;

                checkActive = true;
            }
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("closedidle"))
            {
                animator.Play("closed");

                if (isBlock == false)
                    isBlock = true;

                checkActive = false;
            }
        }
    }

    public void SetUp()
    {
        if (isSell == true)
        {
            isBlock = true;
            return;
        }
        if(monsterShopData != null)
        {
            isSell = false;
            isBlock = false;
            selectMonster = MonsterInstance.Instance(monsterShopData.monster);

            costItemImage.sprite = ItemInventory.Instance.GetSprite(monsterShopData.costType);
            costItemText.text = $"<b><size=32>x<b><size=36>{monsterShopData.count}";

            float value = 1f;
            if (selectMonster.monsterWeight == MonsterWeight.Small)
                value = 0.5f;
            else if (selectMonster.monsterWeight == MonsterWeight.Middle)
            {
                value = 1.25f;

                var sprite = selectMonster.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
                if (sprite.bounds.size.y * sprite.pixelsPerUnit >= 32)
                    value = 0.75f;
            }
            else if (selectMonster.monsterWeight == MonsterWeight.Big)
                value = 0.75f;

            bool check = (selectMonster.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (selectMonster.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (selectMonster.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
            if (check)
                value = 1.25f;



            objectImage.sprite = selectMonster.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
            objectImage.SetNativeSize();
            objectImage.rectTransform.sizeDelta = objectImage.rectTransform.sizeDelta * (133.3333f * value);

            isRunning = false;
            isRunning2 = false;
        }
        else
        {
            if(itemShopData != null)
            {
                isSell = false;
                isBlock = false;
                isRunning = false;
                selectItem = itemShopData.item;

                costItemImage.sprite = ItemInventory.Instance.GetSprite(itemShopData.costType);
                costItemText.text = $"<b><size=24>x<b><size=28>{itemShopData.count}";

                objectImage.sprite = selectItem.itemImage;
                objectImage.SetNativeSize();
                objectImage.rectTransform.sizeDelta = objectImage.rectTransform.sizeDelta * 96f;
            }
        }
    }
    public void SetUp(object item, bool isHidden)
    {
        var monsterShopData = item as MonsterShopData;
        if(monsterShopData != null)
        {
            isSell = false;
            isBlock = false;
            selectMonster = MonsterInstance.Instance(monsterShopData.monster);
            this.monsterShopData = monsterShopData;

            costItemImage.sprite = ItemInventory.Instance.GetSprite(monsterShopData.costType);
            costItemText.text = $"<b><size=32>x<b><size=36>{monsterShopData.count}";

            float value = 1f;
            if (selectMonster.monsterWeight == MonsterWeight.Small)
                value = 0.5f;
            else if (selectMonster.monsterWeight == MonsterWeight.Middle)
            {
                value = 1.25f;

                var sprite = selectMonster.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
                if (sprite.bounds.size.y * sprite.pixelsPerUnit >= 32)
                    value = 0.75f;
            }
            else if (selectMonster.monsterWeight == MonsterWeight.Big)
                value = 0.75f;

            bool check = (selectMonster.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (selectMonster.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (selectMonster.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
            if (check)
                value = 1.25f;

            objectImage.sprite = selectMonster.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
            objectImage.SetNativeSize();
            objectImage.rectTransform.sizeDelta = objectImage.rectTransform.sizeDelta * (133.3333f * value);

            backgroundAnim.gameObject.SetActive(true);

            isRunning = false;
            isRunning2 = false;
            this.isHidden = isHidden;
        }
        else
        {
            var itemShopData = item as ItemShopData;
            if(itemShopData != null)
            {
                isSell = false;
                isBlock = false;
                isRunning = false;
                selectItem = itemShopData.item;
                this.itemShopData = itemShopData;

                costItemImage.sprite = ItemInventory.Instance.GetSprite(itemShopData.costType);
                costItemText.text = $"<b><size=24>x<b><size=28>{itemShopData.count}";

                objectImage.sprite = selectItem.itemImage;
                objectImage.SetNativeSize();
                objectImage.rectTransform.sizeDelta = objectImage.rectTransform.sizeDelta * 96f;
            }
        }
    }
    public void SetBlock(bool v)
    {
        isBlock = v;
    }
    public void SetSell(bool v)
    {
        isSell = v;
    }
    private IEnumerator ReturnBlockRoutine()
    {
        isRunning = true;
        yield return null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("closedidle") || animator.GetCurrentAnimatorStateInfo(0).IsName("popupidle"));
        isBlock = false;
        isRunning = false;
    }
}
