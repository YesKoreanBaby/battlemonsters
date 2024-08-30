using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class RulletObject : MonoBehaviour
{
    public Vector2 destPosition;

    private Animator animator;
    private RectTransform movingObject;
    private Transform deathObject;
    private Transform devilObject;
    private Image image;
    private Image childImage;
    private Vector2 startPosition;

    public object rewardData;

    [System.NonSerialized]
    public bool isBlock;

    [System.NonSerialized]
    public bool isDevil = false;
    private bool soundBlock = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(soundBlock == false)
        {
            SoundManager.Instance.PlayEffect(134, 1f);
            soundBlock = true;
        }
        RulletUI.Instance.SetRulletObject(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        soundBlock = false;
        RulletUI.Instance.SetRulletObject(null);
    }

    public IEnumerator RewardRoutine()
    {
        if (isDevil == true)
        {
            yield return StartCoroutine(DevilRoutine());
            yield break;
        }

        SoundManager.Instance.PlayEffect(189, 1f);

        ItemInventory.Instance.rulletItemDatas.Add(rewardData);
        animator.Play("reward");
        yield return null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));
       // yield return new WaitForSeconds(0.25f);

        movingObject.gameObject.SetActive(true);

        Coroutine deathRoutine = null;

        int deathRandom = Random.Range(0, 4);
        if (deathRandom == 0)
        {
            deathRoutine = StartCoroutine(DeathRoutine());
        }
        else
            SettingRulletData();

        float lerpTime = 1f;
        float lerpSpeed = 2f;
        float currentTime = 0f;

        movingObject.anchoredPosition = this.startPosition;
        movingObject.SetParent(movingObject.transform.root, true);

        Vector2 startPosition = movingObject.anchoredPosition;
        Vector2 endPosition = new Vector2(723f, 412f);

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            movingObject.anchoredPosition = Vector2.Lerp(startPosition, endPosition, currentSpeed);
            movingObject.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 1f), currentSpeed);
            yield return null;
        }

        SoundManager.Instance.PlayEffect(164, 1f);

        movingObject.SetParent(this.transform, true);
        SettingChildImage();
        movingObject.gameObject.SetActive(false);

        if (deathRoutine != null)
            yield return new WaitUntil(() => isBlock == true);
    }
    private IEnumerator DevilRoutine()
    {
        isDevil = false;
 
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        SoundManager.Instance.PlayEffect(124, 1f);

        animator.Play("devil");
        yield return new WaitForSeconds(1.75f);

        if (ItemInventory.Instance.rulletItemDatas.Count <= 0)
            goto jump;

        var objs = ItemInventory.Instance.rulletItemDatas.ToList();
        ItemInventory.Instance.rulletItemDatas.Clear();

        Vector2 startPosition = new Vector2(723f, 412f);

        Transform parent = this.transform.parent;
        this.transform.SetParent(this.transform.root, true);
        Vector2 endPosition = this.transform.GetComponent<RectTransform>().anchoredPosition;
        this.transform.SetParent(parent, true);

        Vector2 prevPosition = startPosition;

        List<RectTransform> transforms = new List<RectTransform>();
        for(int i = 0; i < objs.Count; ++i)
        {
            var clone = Instantiate(RulletUI.Instance.returnMovingObjectPrefab, this.transform.root);

            Vector2 position = prevPosition + new Vector2(Random.Range(20, 51), Random.Range(20, 51));
            clone.rectTransform.anchoredPosition = position;

            prevPosition = position;
            transforms.Add(clone.rectTransform);

            if (objs[i] is CostItemIndigrident)
            {
                var data = objs[i] as CostItemIndigrident;
                clone.sprite = ItemInventory.Instance.GetSprite(data.itemType);
                clone.SetNativeSize();
                clone.rectTransform.sizeDelta *= 5.25f;
            }
            else if (objs[i] is ItemElementalData)
            {
                var data = objs[i] as ItemElementalData;
                if (data.effectType == ItemElementalEffectType.None)
                {
                    clone.sprite = data.itemImage;
                    clone.SetNativeSize();
                    clone.rectTransform.sizeDelta *= 2.25f;
                }
                else
                {
                    clone.sprite = data.itemImage;
                    clone.SetNativeSize();
                    clone.rectTransform.sizeDelta *= 3.5f;
                }
            }
            else if (objs[i] is MonsterData)
            {
                var data = objs[i] as MonsterData;
                clone.sprite = data.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

                bool check = (data.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (data.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (data.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
                float value = (data.monsterWeight != MonsterWeight.Big) ? 4f : 2f;
                if (check == true)
                    value = 4f;
                clone.SetNativeSize();
                clone.rectTransform.sizeDelta = new Vector2(Mathf.Min(64f, clone.rectTransform.sizeDelta.x * value), Mathf.Min(64f, clone.rectTransform.sizeDelta.y * value));
            }
            yield return waitTime;
        }

        var trans = transforms[0].gameObject;
        for (int i = 0; i < transforms.Count; ++i)
        {
            StartCoroutine(ReturnMoveRoutine(transforms[i], transforms[i].anchoredPosition, endPosition));
        }

        yield return new WaitUntil(() => trans == null);

    jump:
        animator.Play("idle");
        yield return null;
        SettingRulletData();
    }

    private IEnumerator ReturnMoveRoutine(RectTransform obj, Vector2 startPosition, Vector2 endPosition)
    {
        float lerpTime = 1f;
        float lerpSpeed = 2f;
        float currentTime = 0f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.anchoredPosition = Vector2.Lerp(startPosition, endPosition, currentSpeed);
            obj.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 1f), currentSpeed);
            yield return null;
        }

        SoundManager.Instance.PlayEffect(164, 1f);
        Destroy(obj.gameObject);
    }
    private IEnumerator DeathRoutine()
    {
        SoundManager.Instance.PlayEffect(154, 1f);
        animator.Play("death");
        yield return null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        isBlock = true;

        animator.Play("idle");
        this.gameObject.SetActive(false);
        deathObject.gameObject.SetActive(false);
    }

    public void Clear()
    {
        isDevil = false;
        isBlock = false;
        soundBlock = false;
        movingObject.gameObject.SetActive(false);
        deathObject.gameObject.SetActive(false);
        devilObject.gameObject.SetActive(false);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        SettingRulletData();
        SettingChildImage();

        this.transform.gameObject.SetActive(true);
    }

    public void ReClear()
    {
        soundBlock = false;
        deathObject.gameObject.SetActive(false);
        devilObject.gameObject.SetActive(false);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        SettingReRulletData();
        SettingChildImage();
    }

    public void SettingRulletData()
    {
        int random = Random.Range(0, 101);
        if(random < 5)
        {
            image.gameObject.SetActive(false);
            isDevil = true;
        }
        else if (random >= 5 && random < 25)
        {
            image.gameObject.SetActive(true);
            CostItemIndigrident itemIndigrident = new CostItemIndigrident();
            itemIndigrident.count = Random.Range(1, 4);
            itemIndigrident.itemType = (Random.Range(0, 2) == 0) ? CostItemType.Money : CostItemType.Diamond;

            rewardData = itemIndigrident;
            image.sprite = ItemInventory.Instance.GetSprite(itemIndigrident.itemType);
            image.SetNativeSize();
            image.rectTransform.sizeDelta = image.rectTransform.sizeDelta * 1.5f;
        }
        else if (random >= 25 && random < 75)
        {
            image.gameObject.SetActive(true);
            List<ItemElementalData> itemDatas = new List<ItemElementalData>() { MonsterDataBase.Instance.potion, MonsterDataBase.Instance.statusPotion, MonsterDataBase.Instance.deadPotion, MonsterDataBase.Instance.fullCondition };
            ItemElementalData data = itemDatas[Random.Range(0, itemDatas.Count)];

            rewardData = data;
            image.sprite = data.itemImage;
            image.SetNativeSize();
        }
        else if (random >= 75 && random < 85)
        {
            image.gameObject.SetActive(true);
            List<ItemElementalData> itemDatas = new List<ItemElementalData>() { MonsterDataBase.Instance.evolutionCandy, MonsterDataBase.Instance.karmaCandy, MonsterDataBase.Instance.tranningBooks[Random.Range(0, MonsterDataBase.Instance.tranningBooks.Length)] };
            ItemElementalData data = itemDatas[Random.Range(0, itemDatas.Count)];

            rewardData = data;
            image.sprite = data.itemImage;
            image.SetNativeSize();
        }
        else if (random >= 85 && random < 95)
        {
            image.gameObject.SetActive(true);
            List<ItemElementalData> itemDatas = new List<ItemElementalData>() { MonsterDataBase.Instance.heroBook, MonsterDataBase.Instance.oldBook, MonsterDataBase.Instance.magicalLeaf, MonsterDataBase.Instance.thorHammer, MonsterDataBase.Instance.miracleNeck };
            ItemElementalData data = itemDatas[Random.Range(0, itemDatas.Count)];

            rewardData = data;
            image.sprite = data.itemImage;
            image.SetNativeSize();
            image.rectTransform.sizeDelta = image.rectTransform.sizeDelta / 1.5f;
        }
        else if (random >= 95 && random < 99)
        {
            image.gameObject.SetActive(true);
            List<MonsterData> monsterDatas = new List<MonsterData>() { MonsterDataBase.Instance.uncomonDatas[Random.Range(0, MonsterDataBase.Instance.uncomonDatas.Count)].monster, MonsterDataBase.Instance.comonDatas[Random.Range(0, MonsterDataBase.Instance.comonDatas.Count)].monster };
            MonsterData data = monsterDatas[Random.Range(0, monsterDatas.Count)];

            rewardData = data;
            image.sprite = data.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
            bool check = (data.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (data.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (data.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
            float value = (data.monsterWeight != MonsterWeight.Big) ? 1.25f : 1 / 1.5f;
            if (check == true)
                value = 1.25f;
            image.SetNativeSize();
            image.rectTransform.sizeDelta = new Vector2(Mathf.Min(133.33f, image.rectTransform.sizeDelta.x * value), Mathf.Min(133.33f, image.rectTransform.sizeDelta.y * value));
        }
        else if (random >= 99 && random < 100)
        {
            image.gameObject.SetActive(true);
            MonsterData data = MonsterDataBase.Instance.lairDatas[Random.Range(0, MonsterDataBase.Instance.lairDatas.Count)].monster;

            rewardData = data;
            image.sprite = data.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
            bool check = (data.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (data.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (data.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
            float value = (data.monsterWeight != MonsterWeight.Big) ? 1.25f : 1 / 1.5f;
            if (check == true)
                value = 1.25f;
            image.SetNativeSize();
            image.rectTransform.sizeDelta = new Vector2(Mathf.Min(133.33f, image.rectTransform.sizeDelta.x * value), Mathf.Min(133.33f, image.rectTransform.sizeDelta.y * value));

        }
    }

    public void SettingReRulletData()
    {
        if(isBlock == true)
        {
            this.gameObject.SetActive(false);
            deathObject.gameObject.SetActive(false);
        }
        else if(isDevil == true)
        {
            animator.Play("devilidle");
            image.gameObject.SetActive(false);
        }
        else
        {
            if (rewardData is CostItemIndigrident)
            {
                var itemIndigrident = rewardData as CostItemIndigrident;
                image.sprite = ItemInventory.Instance.GetSprite(itemIndigrident.itemType);
                image.SetNativeSize();
                image.rectTransform.sizeDelta = image.rectTransform.sizeDelta * 1.5f;
            }
            else if (rewardData is ItemElementalData)
            {
                ItemElementalData data = rewardData as ItemElementalData;
                rewardData = data;
                image.sprite = data.itemImage;
                image.SetNativeSize();

                if (data.effectType == ItemElementalEffectType.Combine)
                    image.rectTransform.sizeDelta = image.rectTransform.sizeDelta / 1.5f;
            }
            else if (rewardData is MonsterData)
            {
                MonsterData data = rewardData as MonsterData;

                image.sprite = data.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
                bool check = (data.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (data.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (data.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
                float value = (data.monsterWeight != MonsterWeight.Big) ? 1.25f : 1 / 1.5f;
                if (check == true)
                    value = 1.25f;
                image.SetNativeSize();
                image.rectTransform.sizeDelta = new Vector2(Mathf.Min(133.33f, image.rectTransform.sizeDelta.x * value), Mathf.Min(133.33f, image.rectTransform.sizeDelta.y * value));
            }
        }
    }

    public void SettingChildImage()
    {
        childImage.sprite = image.sprite;
        childImage.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
    }

    public void UpdateRulletAnimData()
    {
        if(isDevil == true)
        {
            animator.Play("devilidle");
        }
        else
        {
            animator.Play("idle");
        }
    }

    public void Init()
    {
        image = GetComponent<Image>();
        childImage = transform.GetChild(0).GetComponent<Image>();
        animator = GetComponent<Animator>();

        movingObject = childImage.rectTransform;
        startPosition = movingObject.anchoredPosition;
        movingObject.gameObject.SetActive(false);

        deathObject = transform.GetChild(1);

        devilObject = transform.GetChild(2);
    }
}
