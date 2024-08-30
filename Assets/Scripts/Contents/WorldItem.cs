using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [System.NonSerialized]
    public ItemData itemData;

    [System.NonSerialized]
    public ItemElementalData itemData2;

    private SpriteRenderer spriteRenderer = null;
    private bool isRunning = false;
    private bool isHomeground = false;

    private void Update()
    {
        if ((CombatManager.Instance.isEnd == true) && isRunning == false)
        {
            if (itemData.isAlive == true)
                EarnItem();
            else
                DestroyItem();
        }
    }
    public void Init(ItemData itemData, bool isHomeground)
    {
        if(itemData.effectType == ItemEffectType.EarnNormalStone)
        {
            if(ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.normalStone))
            {
                itemData = MonsterDataBase.Instance.treagureInsteadData;
            }
            else
                transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if(itemData.effectType == ItemEffectType.EarnElecStone)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.elecStone))
            {
                itemData = MonsterDataBase.Instance.treagureInsteadData;
            }
            else
                transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.EarnPoisonStone)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.poisonStone))
            {
                itemData = MonsterDataBase.Instance.treagureInsteadData;
            }
            else
                transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.EarnFireStone)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.fireStone))
            {
                itemData = MonsterDataBase.Instance.treagureInsteadData;
            }
            else
                transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.EarnGunslinger)
        {
            if (ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.symbolofGunslinger))
            {
                itemData = MonsterDataBase.Instance.treagureInsteadData;
            }
            else
                transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.ThorHammer)
        {
            transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.OldBook)
        {
            transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.HeroBook)
        {
            transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.MagicalLeaf)
        {
            transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }
        else if (itemData.effectType == ItemEffectType.MiracleNeck)
        {
            transform.localScale = new Vector3(0.36f, 0.36f, 1f);
        }

        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.itemData = itemData;
        spriteRenderer.sprite = itemData.itemImage;
        this.isHomeground = isHomeground;
        CombatManager.Instance.worldItemList.Add(this);
    }

    public void EarnItem()
    {
        if(isRunning == false)
        {
            isRunning = true;
            var targets = CombatManager.Instance.homegroundMonsters;
            if (targets.Count <= 0)
            {
                DestroyItem();
                return;
            }
            else
            {
                if (targets.Count > 1)
                {
                    var target = GetNearTarget(targets);

                    GetComponent<Animator>().Play("suction");
                    spriteRenderer.sortingLayerName = "EB";
                    spriteRenderer.sortingOrder = 0;
                    StartCoroutine(MoveRoutine(target));
                }
                else
                {
                    var target = targets[0];
                    GetComponent<Animator>().Play("suction");
                    spriteRenderer.sortingLayerName = "EB";
                    spriteRenderer.sortingOrder = 0;
                    StartCoroutine(MoveRoutine(target));
                }
            }
        }
    }

    public void DestroyItem()
    {
        CombatManager.Instance.worldItemList.Remove(this);
        Destroy(this.gameObject);
        isRunning = true;
    }
    private EntityMonster GetNearTarget(List<EntityMonster> targets)
    {
        EntityMonster nearTarget = targets[Random.Range(0, targets.Count)];
        for(int i = 0; i < targets.Count - 1; i++)
        {
            for(int j = i + 1; j < targets.Count; j++)
            {
                float a = (targets[i].transform.position - this.transform.position).magnitude;
                float b = (targets[j].transform.position - this.transform.position).magnitude;

                if(a <= b)
                    nearTarget = targets[i];
            }
        }

        return nearTarget;
    }

    private IEnumerator Effect(EntityMonster target)
    {
        if(itemData.effectType == ItemEffectType.Earn5Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, Random.Range(1, 11));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.Earn10Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, Random.Range(10, 51));
            yield return null;
        }
        else if(itemData.effectType == ItemEffectType.Earn15Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, Random.Range(50, 101));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.Earn25Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, Random.Range(100, 201));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.Earn50Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, Random.Range(100, 501));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.Earn100Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, Random.Range(501, 1001));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.Earn500Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, 500);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.Earn1000Gold)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Money, Random.Range(1000, 5001));
            yield return null;
        }
        else if(itemData.effectType == ItemEffectType.EarnHp)
        {
            target.battleInstance.hp = Mathf.Min(target.battleInstance.maxHp, target.battleInstance.hp + (target.battleInstance.maxHp * 0.25f));
            yield return null;
        }
        else if(itemData.effectType == ItemEffectType.EarnMp)
        {
            target.battleInstance.mp = Mathf.Min(target.battleInstance.maxMp, target.battleInstance.mp + (target.battleInstance.maxMp * 0.25f));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.Earndex)
        {
            target.battleInstance.dex = Mathf.Min(target.battleInstance.maxDex, target.battleInstance.dex + (target.battleInstance.maxDex * 0.25f));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.DamageHp)
        {
            target.Hurt((target.battleInstance.maxHp * 0.25f));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.DamageMp)
        {
            target.battleInstance.mp = Mathf.Max(0f, target.battleInstance.mp - (target.battleInstance.maxMp * 0.25f));
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.DamageDex)
        {
            target.battleInstance.dex = Mathf.Max(0f, target.battleInstance.dex - (target.battleInstance.maxDex * 0.25f));
            CombatManager.Instance.battleQueue.RemoveAll(x => x == target);
            yield return null;
        }
        else if(itemData.effectType == ItemEffectType.AddPoison)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            var effect = target.dogDmgStatus.AddPoisenFlag(target, SkillManager.Instance.poisenEffect);
            if(effect != null)
            {
                yield return new WaitUntil(() => effect == null);
                yield return new WaitForSeconds(0.25f);
            }
            else
                yield return null;
        }
        else if(itemData.effectType == ItemEffectType.Earn1Diamond)
        {
            RewardUI.Instance.AddCostItem(CostItemType.Diamond, Random.Range(1, 21));
            yield return null;
        }
        else if(itemData.effectType == ItemEffectType.EarnNormalStone)
        {
            RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.normalStone);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.EarnElecStone)
        {
            RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.elecStone);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.EarnPoisonStone)
        {
            RewardUI.Instance.AddTreagureItem(MonsterDataBase.Instance.poisonStone);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.ThorHammer)
        {
            RewardUI.Instance.AddConsumeItem(MonsterDataBase.Instance.thorHammer, 1);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.OldBook)
        {
            RewardUI.Instance.AddConsumeItem(MonsterDataBase.Instance.oldBook, 1);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.HeroBook)
        {
            RewardUI.Instance.AddConsumeItem(MonsterDataBase.Instance.heroBook, 1);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.MagicalLeaf)
        {
            RewardUI.Instance.AddConsumeItem(MonsterDataBase.Instance.magicalLeaf, 1);
            yield return null;
        }
        else if (itemData.effectType == ItemEffectType.MiracleNeck)
        {
            RewardUI.Instance.AddConsumeItem(MonsterDataBase.Instance.miracleNeck, 1);
            yield return null;
        }
    }

    private IEnumerator MoveRoutine(EntityMonster target)
    {
        isRunning = true;

        //ø¨√‚
        Vector2 startPosition = this.transform.position;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = (CombatManager.Instance.isEnd == true) ? 4f : 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        yield return StartCoroutine(Effect(target));
        SoundManager.Instance.PlayEffect(155, 1f);
        DestroyItem();
    }
}
