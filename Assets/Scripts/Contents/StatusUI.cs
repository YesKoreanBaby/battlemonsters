using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public EntityMonster monster;
    public Image hp;
    public Image subHp;
    public Image mp;
    public Image subMp;
    public Image dex;

    private MonsterInstance monsterInstance;

    private bool blockSubHp = false;
    private bool blockSubMp = false;
    private void Start()
    {
        monsterInstance = monster.battleInstance;

        Init();

    }
    void Update()
    {
        hp.fillAmount = monsterInstance.hp / monsterInstance.maxHp;
        if(blockSubHp == false && hp.fillAmount != subHp.fillAmount)
            StartCoroutine(SubHpRoutine());

        mp.fillAmount = monsterInstance.mp / monsterInstance.maxMp;
        if (blockSubMp == false && mp.fillAmount != subMp.fillAmount)
            StartCoroutine(SubMpRoutine());


        dex.fillAmount = monsterInstance.dex / monsterInstance.maxDex;
    }

    private void OnDisable()
    {
        if(CombatManager.Instance.isStart == true)
        {
            subHp.fillAmount = hp.fillAmount = monsterInstance.hp / monsterInstance.maxHp;
            subMp.fillAmount = mp.fillAmount = monsterInstance.mp / monsterInstance.maxMp;
            dex.fillAmount = monsterInstance.dex / monsterInstance.maxDex;

            blockSubHp = false;
            blockSubMp = false;
            StopAllCoroutines();
        }
    }

    public void Init()
    {
        dex.rectTransform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(monster)) ? Mathf.Abs(dex.rectTransform.localScale.x) : -1 * Mathf.Abs(dex.rectTransform.localScale.x), dex.rectTransform.localScale.y, dex.rectTransform.localScale.z);
        mp.rectTransform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(monster)) ? Mathf.Abs(mp.rectTransform.localScale.x) : -1 * Mathf.Abs(mp.rectTransform.localScale.x), mp.rectTransform.localScale.y, mp.rectTransform.localScale.z);
        hp.rectTransform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(monster)) ? Mathf.Abs(hp.rectTransform.localScale.x) : -1 * Mathf.Abs(hp.rectTransform.localScale.x), hp.rectTransform.localScale.y, hp.rectTransform.localScale.z);
        subHp.rectTransform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(monster)) ? Mathf.Abs(subHp.rectTransform.localScale.x) : -1 * Mathf.Abs(subHp.rectTransform.localScale.x), subHp.rectTransform.localScale.y, subHp.rectTransform.localScale.z);
        subMp.rectTransform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(monster)) ? Mathf.Abs(subMp.rectTransform.localScale.x) : -1 * Mathf.Abs(subMp.rectTransform.localScale.x), subMp.rectTransform.localScale.y, subMp.rectTransform.localScale.z);
    }
    private IEnumerator SubHpRoutine()
    {
        blockSubHp = true;

        float startValue = subHp.fillAmount;
        float endValue = hp.fillAmount;

        float startAlphaValue = 1f;
        float endAlphaValue = 0f;

        float lerpTime = 1f;
        float lerpSpeed = 1f;
        float currentTime = 0f;

        while(currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            subHp.fillAmount = Mathf.Lerp(startValue, endValue, currentSpeed);

            float a = Mathf.Lerp(startAlphaValue, endAlphaValue, currentSpeed);
            subHp.color = new Color(subHp.color.r, subHp.color.g, subHp.color.b, a);
            yield return null;
        }

        blockSubHp = false;
    }
    private IEnumerator SubMpRoutine()
    {
        blockSubMp = true;

        float startValue = subMp.fillAmount;
        float endValue = mp.fillAmount;

        float startAlphaValue = 1f;
        float endAlphaValue = 0f;

        float lerpTime = 1f;
        float lerpSpeed = 1f;
        float currentTime = 0f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            subMp.fillAmount = Mathf.Lerp(startValue, endValue, currentSpeed);

            float a = Mathf.Lerp(startAlphaValue, endAlphaValue, currentSpeed);
            subMp.color = new Color(subMp.color.r, subMp.color.g, subMp.color.b, a);
            yield return null;
        }

        blockSubMp = false;
    }
    
}
