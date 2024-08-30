using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RulletUI : MonoBehaviour
{
    public Transform rullet;
    public Image timeImage;
    public Image returnMovingObjectPrefab;
    public RulletObject[] rulletObjs;
    public Button playButton;
    public Button stopButton;
    public Button rulletInventory;
    public Button exitButton;
    public TextMeshProUGUI moneyText;
    public float stoppedTime;
    public float decreaseTime = 1f;
    public float rotationSpeed;

    public bool isPopUp { get; private set; }
    public bool isPlaying { get; private set; }
    public bool isStopped { get; private set; }
    public float currentTime { get; private set; }
    public float currentRotationSpeed { get; private set; }

    [System.NonSerialized]
    public int payValue = 100;

    public RulletObject currentRulletObject { get; private set; }

    private static RulletUI instance = null;
    public static RulletUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isPopUp = false;
        var childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(isPlaying == true)
        {
            timeImage.fillAmount = currentTime / stoppedTime;
        }
    }

    public void PopUp()
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }

            timeImage.fillAmount = 0f;
            moneyText.text = $"<b><size=24>$<b><size=32>{ItemInventory.Instance.money}";
            UpdateData();

            for(int i = 0; i < rulletObjs.Length; ++i)
            {
                rulletObjs[i].UpdateRulletAnimData();
            }
        }
    }

    public void Closed()
    {
        if (isPopUp == true)
        {
            isPopUp = false;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(false);
            }

            SoundManager.Instance.StopEffect(189);
            SoundManager.Instance.StopEffect(190);
        }
    }
    public void Clear()
    {
        for (int i = 0; i < rulletObjs.Length; ++i)
        {
            rulletObjs[i].Clear();
        }

        rullet.transform.rotation = Quaternion.Euler(0f, 0f, 22f);
    }

    public void Init()
    {
        for (int i = 0; i < rulletObjs.Length; ++i)
        {
            rulletObjs[i].Init();
        }
    }
    public void Play()
    {
        SoundManager.Instance.StopEffect(189);
        SoundManager.Instance.StopEffect(190);
        SoundManager.Instance.PlayEffect(166, 1f);
        StartCoroutine(DescreaseMoneyRoutine(payValue));
        StartCoroutine(RotationRoutine());
    }
    public void Stop()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        if (isPlaying == true)
            isStopped = true;
    }
    public void SetRulletObject(RulletObject rulletObject)
    {
        currentRulletObject = rulletObject;
    }
    public void PopUpRulletInventory()
    {
        BatUI.Instance.PopUp(ItemInventory.Instance.rulletItemDatas);
    }
    public void Exit()
    {
        var objs = ItemInventory.Instance.rulletItemDatas;
        for(int i = 0; i < objs.Count; ++i)
        {
            if (objs[i] is CostItemIndigrident)
            {
                var data = objs[i] as CostItemIndigrident;

                int count = (data.itemType == CostItemType.Money) ? Random.Range(100, 501) : Random.Range(1, 11);
                RewardUI.Instance.AddCostItem(data.itemType, count);
            }
            else if (objs[i] is ItemElementalData)
            {
                var data = objs[i] as ItemElementalData;
                RewardUI.Instance.AddConsumeItem(data, 1);
            }
            else if (objs[i] is MonsterData)
            {
                var data = objs[i] as MonsterData;
                RewardUI.Instance.AddMonster(MonsterInstance.Instance(data));
            }
        }

        ItemInventory.Instance.rulletItemDatas.Clear();

        Closed();
      //  MainSystemUI.Instance.PopUp();
    }
    private IEnumerator RotationRoutine()
    {
        isPlaying = true;
        isStopped = false;
        currentTime = stoppedTime;
        currentRotationSpeed = rotationSpeed;
        UpdateData();

        while (currentTime > 0f)
        {
            rullet.Rotate(-1 * Vector3.forward * currentRotationSpeed * Time.deltaTime);
            currentTime -= Time.deltaTime;
            if (isStopped == true)
                goto jump;
            yield return null;
        }
    jump:
        currentTime = 0f;
        isStopped = true;
        UpdateData();
        var coroutine = StartCoroutine(LerpSpeedRoutine());
        while (currentRotationSpeed > 0f)
        {
            rullet.Rotate(-1 * Vector3.forward * currentRotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return StartCoroutine(RewardRoutine());

        StopCoroutine(coroutine);
        isPlaying = false;
        isStopped = false;
        UpdateData();
    }
    private IEnumerator LerpSpeedRoutine()
    {
        float lerpTime = decreaseTime + Random.Range(0.5f, 1.5f);
        float lerpSpeed = 1f;
        float currentTime = 0f;

        float startValue = currentRotationSpeed;
        float endValue = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            currentRotationSpeed = Mathf.Lerp(startValue, endValue, currentSpeed);
            yield return null;
        }
    }

    private IEnumerator RewardRoutine()
    {
        if (currentRulletObject == null)
        {
            SoundManager.Instance.PlayEffect(190, 1f);
            yield break;
        }

        yield return StartCoroutine(currentRulletObject.RewardRoutine());
    }

    private void UpdateData()
    {
        playButton.interactable = (isPlaying == false) && (ItemInventory.Instance.money >= payValue) && CheckActive();
        stopButton.interactable = (isPlaying == true && isStopped == false);
        exitButton.interactable = rulletInventory.interactable = (isPlaying == false);

        var tmp_0 = playButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tmp_0.color = (playButton.interactable == true) ? new Color(tmp_0.color.r, tmp_0.color.g, tmp_0.color.b, 255f) : new Color(tmp_0.color.r, tmp_0.color.g, tmp_0.color.b, 50f);

        var tmp_1 = stopButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tmp_1.color = (playButton.interactable == true) ? new Color(tmp_1.color.r, tmp_1.color.g, tmp_1.color.b, 255f) : new Color(tmp_1.color.r, tmp_1.color.g, tmp_1.color.b, 50f);
    }

    private bool CheckActive()
    {
        for(int i = 0; i < rulletObjs.Length; ++i)
        {
            if (rulletObjs[i].isBlock == false)
                return true;
        }
        return false;
    }

    private IEnumerator DescreaseMoneyRoutine(int descreaseValue)
    {
        SoundManager.Instance.PlayEffect(172, 1f);

        int start = ItemInventory.Instance.money;
        int end = Mathf.Max(0, ItemInventory.Instance.money - descreaseValue);

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(start, end, currentSpeed);

            ItemInventory.Instance.money = Mathf.FloorToInt(value);
            moneyText.text = $"<b><size=24>$<b><size=32>{ItemInventory.Instance.money}";
            yield return null;
        }
    }
}
