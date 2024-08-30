using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public struct SternStatusStructure
{
    public Coroutine currentRoutine;
    public GameObject effect;
    public GameObject prefab;

    public IEnumerator remaningRoutine;
    public float remaningPercent;
    public void Start(EntityMonster player, GameObject effectPrefab, Transform effectSpawnPosition, float sternTime, bool reverseScale = false)
    {
        float iceReflextionValue = ((effectPrefab == SkillManager.Instance.iceCrystalEffect) && player.battleInstance.abilities.Contains(AbilityType.IceReflexion)) ? 0.25f : 1f; 
        if(currentRoutine == null)
        {
            player.ResetPassiveSkill();

            Animator effectClone = Object.Instantiate(effectPrefab, effectSpawnPosition.position, Quaternion.identity).GetComponent<Animator>();
            effect = effectClone.gameObject;

            prefab = effectPrefab;
            currentRoutine = player.StartCoroutine(SternRoutine(player, effectClone, sternTime * iceReflextionValue));
            player.battleInstance.repeatRatio -= 10f;

            float monScale = player.bulletCreatePoint.localScale.x;
            if (reverseScale == true)
                effectClone.transform.localScale = new Vector3(-1 * effectClone.transform.localScale.x * monScale, effectClone.transform.localScale.y * monScale, effectClone.transform.localScale.z);
            else
                effectClone.transform.localScale = new Vector3(effectClone.transform.localScale.x * monScale, effectClone.transform.localScale.y * monScale, effectClone.transform.localScale.z);
        }
        else
        {
            player.ResetPassiveSkill();

            Stop(player);
            Animator effectClone = Object.Instantiate(effectPrefab, effectSpawnPosition.position, Quaternion.identity).GetComponent<Animator>();
            effect = effectClone.gameObject;
            prefab = effectPrefab;
            currentRoutine = player.StartCoroutine(SternRoutine(player, effectClone, sternTime * iceReflextionValue));
            player.battleInstance.repeatRatio -= 10f;

            float monScale = player.bulletCreatePoint.localScale.x;
            if (reverseScale == true)
                effectClone.transform.localScale = new Vector3(-1 * effectClone.transform.localScale.x * monScale, effectClone.transform.localScale.y * monScale, effectClone.transform.localScale.z);
            else
                effectClone.transform.localScale = new Vector3(effectClone.transform.localScale.x * monScale, effectClone.transform.localScale.y * monScale, effectClone.transform.localScale.z);
        }
    }
    public void Start(EntityMonster player, GameObject effectPrefab, Transform effectSpawnPosition, IEnumerator remaningRoutine, float perent, bool reverseScale = false, bool dontScale = false)
    {
        player.ResetPassiveSkill();

        Stop(player);
        Animator effectClone = Object.Instantiate(effectPrefab, effectSpawnPosition.position, Quaternion.identity).GetComponent<Animator>();
        effect = effectClone.gameObject;
        prefab = effectPrefab;
        player.battleInstance.repeatRatio -= 10f;
        player.StartCoroutine(RemanningSternStopMonsterRoutine(player, effectClone));

        this.remaningRoutine = remaningRoutine;
        this.remaningPercent = perent;

        if(dontScale == false)
        {
            float monScale = player.bulletCreatePoint.localScale.x;
            if (reverseScale == true)
                effectClone.transform.localScale = new Vector3(-1 * effectClone.transform.localScale.x * monScale, effectClone.transform.localScale.y * monScale, effectClone.transform.localScale.z);
            else
                effectClone.transform.localScale = new Vector3(effectClone.transform.localScale.x * monScale, effectClone.transform.localScale.y * monScale, effectClone.transform.localScale.z);
        }
    }
    public bool Check() { return (currentRoutine != null) || (currentRoutine == null && remaningRoutine != null); }
    public bool CheckRemainningStern() { return currentRoutine == null && remaningRoutine != null; }
    public void Stop(EntityMonster player)
    {
        if(currentRoutine != null)
        {

            player.StopCoroutine(currentRoutine);
            player.battleInstance.repeatRatio += 10f;
            currentRoutine = null;
            prefab = null;
            if(effect != null)
                Object.Destroy(effect);

            player.GetComponent<Animator>().enabled = true;
        }
        if (remaningRoutine != null)
        {
            remaningRoutine = null;
            remaningPercent = 0f;
            prefab = null;
            player.battleInstance.repeatRatio += 10f;
            if (effect != null)
                Object.Destroy(effect);

            player.GetComponent<Animator>().enabled = true;
        }
    }
    private IEnumerator SternRoutine(EntityMonster player, Animator effect, float sternTime)
    {
        if(sternTime == 0f)
        {
            player.GetComponent<Animator>().enabled = false;
            CombatManager.Instance.battleQueue.RemoveAll(x => x == player);
            float dex = player.battleInstance.dex;
            while(true)
            {
                player.battleInstance.dex = dex;
                yield return null;
            }
        }
        else
        {
            player.GetComponent<Animator>().enabled = false;
            CombatManager.Instance.battleQueue.RemoveAll(x => x == player);
            float time = 0f;
            float dex = player.battleInstance.dex;

            while (time < sternTime)
            {
                if (SkillManager.Instance.isBlock == true)
                {
                    player.battleInstance.dex = dex;
                    yield return null;
                }
                else
                {
                    player.battleInstance.dex = dex;
                    time += Time.deltaTime;
                    yield return null;
                }
            }
            effect.Play("exit");
            player.battleInstance.repeatRatio += 10f;
            player.sternStatus.currentRoutine = null;
            player.sternStatus.prefab = null;
            player.GetComponent<Animator>().enabled = true;
        }
    }

    private IEnumerator RemanningSternStopMonsterRoutine(EntityMonster player, Animator effect)
    {
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        player.GetComponent<Animator>().enabled = false;
    }
}

public struct DotDmgStatusStructure
{
    public bool burnFlag;
    public bool poisenFlag;
    public bool deadlyPoisonFlag;
    public bool elecFlag;
    public bool unableFlag;
    public bool recoveryFlag;
  
    public int burnCount;
    public int poisenCount;
    public int deadlyPoisonCount;
    public int elecCount;
    public int unableCount;
   
    public void Initialize()
    {
        burnFlag = poisenFlag = elecFlag = unableFlag = deadlyPoisonFlag = false;
        burnCount = poisenCount = elecCount = unableCount = deadlyPoisonCount = 0;
    }
    
    public GameObject AddBurnFlag(EntityMonster player, GameObject effect)
    {
        if(player.battleInstance.abilities.Contains(AbilityType.BurnReflexion))
        {
            return null;
        }
        else
        {
            if (!CombatManager.Instance.dotDmgObjList.Contains(player))
                CombatManager.Instance.dotDmgObjList.Add(player);

            burnFlag = true;
            burnCount = player.battleInstance.abilities.Contains(AbilityType.Cell) ? 1 : 3;

            var clone = Object.Instantiate(effect, player.center.position, Quaternion.identity);
            return clone;
        }
    }

    public void RemoveBurnFlag(EntityMonster player)
    {
        burnCount = 0;
        burnFlag = false;
        Remove(player);
    }

    public void DecreaseBurnCount(EntityMonster player)
    {
        burnCount = burnCount - 1;
        if (burnCount <= 0)
            RemoveBurnFlag(player);
    }

    public GameObject AddPoisenFlag(EntityMonster player, GameObject effect)
    {
        if(player.battleInstance.abilities.Contains(AbilityType.PoisonReflexion))
        {
            return null;
        }
        else
        {
            if (!CombatManager.Instance.dotDmgObjList.Contains(player))
                CombatManager.Instance.dotDmgObjList.Add(player);
            poisenFlag = true;
            poisenCount = player.battleInstance.abilities.Contains(AbilityType.Cell) ? 1 : 5;

            var clone = Object.Instantiate(effect, player.center.position, Quaternion.identity);
            return clone;
        }
    }
    public void RemovePoisenFlag(EntityMonster player)
    {
        poisenFlag = false;
        poisenCount = 0;
        Remove(player);
    }

    public void DecreasePosienCount(EntityMonster player)
    {
        poisenCount = poisenCount - 1;
        if (poisenCount <= 0)
            RemovePoisenFlag(player);
    }

    public GameObject AddDeadlyPoisonFlag(EntityMonster player, GameObject effect)
    {
        if(deadlyPoisonFlag == false)
        {
            if (!CombatManager.Instance.dotDmgObjList.Contains(player))
                CombatManager.Instance.dotDmgObjList.Add(player);
            deadlyPoisonFlag = true;
            deadlyPoisonCount = player.battleInstance.abilities.Contains(AbilityType.Cell) ? 1 : 4;
        }
        var clone = Object.Instantiate(effect, player.center.position, Quaternion.identity);
        return clone;
    }
    public void RemoveDeadlyPoisonFlag(EntityMonster player)
    {
        deadlyPoisonFlag = false;
        deadlyPoisonCount = 0;
        Remove(player);
    }
    public void DecreaseDeadlyPoisonCount(EntityMonster player)
    {
        deadlyPoisonCount = deadlyPoisonCount - 1;
        if (deadlyPoisonCount <= 0)
            RemoveDeadlyPoisonFlag(player);
    }

    public GameObject AddElecFlag(EntityMonster player, GameObject effect)
    {
        if (!CombatManager.Instance.dotDmgObjList.Contains(player))
            CombatManager.Instance.dotDmgObjList.Add(player);
        elecCount = player.battleInstance.abilities.Contains(AbilityType.Cell) ? 1 : 3;
        elecFlag = true;

        var clone = Object.Instantiate(effect, player.center.position, Quaternion.identity);
        return clone;
    }
    public void RemoveElecFlag(EntityMonster player)
    {
        elecCount = 0;
        elecFlag = false;
        Remove(player);
    }
    public void DecreaseElecCount(EntityMonster player)
    {
        elecCount = elecCount - 1;
        if (elecCount <= 0)
            RemoveElecFlag(player);
    }

    public GameObject AddUnableFlag(EntityMonster player, GameObject effect)
    {
        if (!CombatManager.Instance.dotDmgObjList.Contains(player))
            CombatManager.Instance.dotDmgObjList.Add(player);

        unableCount = player.battleInstance.abilities.Contains(AbilityType.Cell) ? 1 : 3;
        unableFlag = true;

        var clone = Object.Instantiate(effect, player.transform.position, Quaternion.identity);
        return clone;
    }

    public void RemoveUnableFlag(EntityMonster player)
    {
        unableCount = 0;
        unableFlag = false;
        Remove(player);
    }
    public void DecreaseUnableCount(EntityMonster player)
    {
        unableCount = unableCount - 1;
        if (unableCount <= 0)
            RemoveUnableFlag(player);
    }
    public void AddRecoveryFlag(EntityMonster player)
    {
        if (!CombatManager.Instance.dotDmgObjList.Contains(player))
            CombatManager.Instance.dotDmgObjList.Add(player);
        recoveryFlag = true;
    }
    public void RemoveRecoveryFlag(EntityMonster player)
    {
        recoveryFlag = false;
        Remove(player);
    }
    public void Clear(EntityMonster player)
    {
        burnFlag = poisenFlag = elecFlag = unableFlag = false;
        burnCount = poisenCount = elecCount = unableCount = 0;
        CombatManager.Instance.dotDmgObjList.Remove(player);
    }
    public bool CheckBadStatus()
    {
        return (burnFlag == true) || (poisenFlag == true) || (elecFlag == true) || (unableFlag == true) || (deadlyPoisonFlag == true);
    }
    public void ClearBadStatus(EntityMonster player)
    {
        RemoveBurnFlag(player);
        RemovePoisenFlag(player);
        RemoveElecFlag(player);
        RemoveUnableFlag(player);
        RemoveDeadlyPoisonFlag(player);
    }

    private void Remove(EntityMonster player)
    {
        if((burnFlag == false) && (poisenFlag == false) && (elecFlag == false)  && (unableFlag == false) && (deadlyPoisonFlag == false) && (recoveryFlag == false))
            CombatManager.Instance.dotDmgObjList.Remove(player);
    }
}

public class EntityMonster : MonoBehaviour
{
    public Transform bulletCreatePoint;
    public Transform center;
    public GameObject shieldCreatePoint;
    public bool checkDead2;
 
    public MonsterInstance battleInstance;
    public MonsterInstance originInstance;
    public HurtObject hurtObject { get; private set; }

    [System.NonSerialized]
    public int width = -1;

    [System.NonSerialized]
    public int height = -1;

    [System.NonSerialized]
    public bool allowAttack = false;

    [System.NonSerialized]
    public SternStatusStructure sternStatus = new SternStatusStructure();

    [System.NonSerialized]
    public DotDmgStatusStructure dogDmgStatus = new DotDmgStatusStructure();

    [System.NonSerialized]
    public bool checkSwapPosition = false;

    [System.NonSerialized]
    public FormationBuffUI formationBuffView;

    public bool isDead {
        get 
        {
            if (battleInstance != null && battleInstance.hp <= 0)
            {
                bool check = CheckLivingDead();
                if (check == true)
                    return false;
                else
                    return true;
            }
            else
                return false;
        } 
    }

    //아웃복서AI
    [System.NonSerialized]
    public EntityMonster shieldTarget;

    [System.NonSerialized]
    public bool passiveFlag = false;

    [System.NonSerialized]
    public PassiveName currentPassiveName;

    [System.NonSerialized]
    public int shiledCount = 0;

    [System.NonSerialized]
    public bool deadBlock = false;

    [System.NonSerialized]
    public SpriteMask mask = null;

    [System.NonSerialized]
    public int confirmSkillIndex = 0;

    [System.NonSerialized]
    public bool checkFirstTurn = false;

    [System.NonSerialized]
    public int fireVowStack = 3;

    //폼체인지
    [System.NonSerialized]
    public EntityMonster originMonster;

    //분신
    [System.NonSerialized]
    public EntityMonster mainBody;

    [System.NonSerialized]
    public bool isClone = false;

    //리빙데드
    [System.NonSerialized]
    public bool startLivingDeadFlag;

    //메테오
    [System.NonSerialized]
    public float sumStatusRatio;

    //폼체인지4 돌진스킬 한번만하게끔
    [System.NonSerialized]
    public bool isBlockHurt;

    //신에게 힘을빌려오는 스킬 신성효과버프
    [System.NonSerialized]
    public float addHolyStatus = 0f;

    //리빙데드인데 빛속성 공격에 맞았는지 체크하는 변수
    [System.NonSerialized]
    public bool checkLivingDeadHolyHurt = false;

    //언브리커블 트리거 스킬 체킹
    public EntityMonster unbreakableTarget;

    //ThunderMan
    [System.NonSerialized]
    public Transform thunderManAura;

    //마인드컨트롤리턴에사용
    [System.NonSerialized]
    public bool checkMindControl = false;

    //포메이션버프관련
    [System.NonSerialized]
    public bool isShort;
    [System.NonSerialized]
    public bool isLong;
    [System.NonSerialized]
    public bool isSide;
    [System.NonSerialized]
    public bool completeRow;
    [System.NonSerialized]
    public bool completeColum;
    //////////////////////////////////////////////

    [System.NonSerialized]
    public bool startRecovery;

    private int formChangeMonsterTurnCount = 0;
    private float formChangeMonsterMpDiscount = 0;
    private void Start()
    {
        hurtObject = GetComponent<HurtObject>();
        hurtObject.HurtStart();
        dogDmgStatus.Initialize();
    }
    private void Update()
    {
        if (CombatManager.Instance.isStart == false)
            return;
        if (passiveFlag == true && currentPassiveName == PassiveName.ShadowCrew)
            battleInstance.dex = 0f;

        hurtObject.HurtUpdate(isDead);

        if(SkillManager.Instance.isBlock == false)
            allowAttack = battleInstance.DexUpdate();
        if (allowAttack && !CombatManager.Instance.battleQueue.Contains(this) && ((sternStatus.Check() == false) || (sternStatus.CheckRemainningStern() == true)))
        {
            if(passiveFlag == true && currentPassiveName == PassiveName.ThunderMan && (center.childCount > 0))
            {
                CheckThunderManSkill();

                //기습
                CheckSurpriseAttackSkill(this);
            }
            else
            {
                //기습
                CheckSurpriseAttackSkill(this);
                CombatManager.Instance.battleQueue.AddLast(this);
            }

            allowAttack = false;

            if(originMonster != null)
            {
                formChangeMonsterTurnCount++;
                if (formChangeMonsterTurnCount > 3)
                {
                    SwapOrigin();
                    formChangeMonsterTurnCount = 0;
                }
            }
        }

        if(isClone == true)
        {
            if(mainBody == null || (mainBody != null && mainBody.isDead == true))
            {
                CloneDead();
            }
            if(mainBody != null)
            {
                bool check = CombatManager.Instance.homegroundMonsters.Contains(mainBody);
                bool check2 = CombatManager.Instance.homegroundMonsters.Contains(this);
                if (check != check2)
                {
                    CloneDead();
                }
            }
        }

        if (battleInstance.abilities.Contains(AbilityType.Recycle))
        {
            if(battleInstance.maxHp > battleInstance.hp)
            {
                if(dogDmgStatus.recoveryFlag == false)
                    dogDmgStatus.AddRecoveryFlag(this);
            }
            else
            {
                if (dogDmgStatus.recoveryFlag == true)
                    dogDmgStatus.RemoveRecoveryFlag(this);
            }
        }
    }
    private void OnDestroy()
    {
        if(isDead == true && CombatManager.Instance.isStart == true)
        {
            bool isHomeground = (transform.localScale.x > 0f);
            if (sternStatus.Check())
                sternStatus.Stop(this);
            if (isClone == false)
                SkillManager.Instance.deadCount++;
            if(battleInstance.abilities.Contains(AbilityType.Easter) && isClone == false)
            {
                EasterStructure easterStructure = new EasterStructure();
                EmptyPositionStructure emptyPosition = new EmptyPositionStructure();
                emptyPosition.width = width;
                emptyPosition.height = height;
                emptyPosition.worldPosition = transform.position;
                easterStructure.Init(this.originInstance, isHomeground, emptyPosition);
                SkillManager.Instance.easterList.Add(easterStructure);
            }
        }
    }

    public static EntityMonster CreateMonster(MonsterInstance data, Vector2 worldPosition, int width, int height , bool isHomeground, bool allowHeightDistance = true)
    {
        Vector2 heightDisOffset = allowHeightDistance ? new Vector2(CombatManager.Instance.heightDistance * (height + 1), 0) : Vector2.zero;
        var clone = Instantiate(data.monsterData.monsterPrefab, worldPosition + heightDisOffset + (Vector2)data.monsterData.monsterPrefab.transform.position, Quaternion.identity);
        clone.transform.localScale = (isHomeground == true) ? clone.transform.localScale : new Vector3(-1 * clone.transform.localScale.x, clone.transform.localScale.y, clone.transform.localScale.z);
        clone.battleInstance = MonsterInstance.Instance(data);
        clone.originInstance = data;
        clone.width = width;
        clone.height = height;

        var shieldTarget = clone.transform.GetChild(3);
        float shieldTargetScale = shieldTarget.localScale.x;
        shieldTarget.localScale = (isHomeground == true) ? shieldTarget.localScale : new Vector3(-1 * shieldTargetScale, shieldTargetScale, shieldTarget.localScale.z);
        CombatManager.Instance.SortConfirm(clone, clone.battleInstance.currentConfirmSkillPriority, clone.battleInstance.skillDatas);

        if (isHomeground == true)
            CombatManager.Instance.homegroundMonsters.Add(clone);
        else
            CombatManager.Instance.awayMonsters.Add(clone);

        CombatManager.Instance.allMonsters.Add(clone);
      
        if(isHomeground == true && CombatManager.Instance.isStart == false)
        {
            if (data.monsterWeight == MonsterWeight.Big)
                CombatManager.Instance.monsterSpawnCount = CombatManager.Instance.monsterSpawnCount + 4;
            else
                CombatManager.Instance.monsterSpawnCount = CombatManager.Instance.monsterSpawnCount + 1;
        }
        if(CombatManager.Instance.isStart == true)
        {
            CombatManager.Instance.EasterClear(clone.originInstance);

            if((clone.originMonster == null) && (CombatUI.Instance.activeEdit == false))
                CombatManager.Instance.SetFormationBuffDebuff(clone);
        }
        if(clone.battleInstance.abilities.Contains(AbilityType.Potential))
        {
            int value = Mathf.RoundToInt(clone.battleInstance.atk * 0.25f);
            clone.battleInstance.atk = Mathf.Max(0, clone.battleInstance.atk - value);
        }
        if(isHomeground && CombatManager.Instance.currentBattleType != BattleType.Gamebling)
        {
            if(ItemInventory.Instance.ContainsTreasureItem(MonsterDataBase.Instance.easterstone))
            {
                if(!clone.battleInstance.abilities.Contains(AbilityType.Easter))
                    clone.battleInstance.abilities.Add(AbilityType.Easter);
            }
        }
        return clone;
    }

    public EntityMonster SwapMonster(MonsterData swapMonsterData, float formChangeMpDiscount)
    {
        var instance = MonsterInstance.Instance(swapMonsterData);
        var monster = CreateMonster(instance, this.transform.position, this.width, this.height, CombatManager.Instance.homegroundMonsters.Contains(this), false);
        monster.originMonster = this;
        this.formChangeMonsterMpDiscount = formChangeMpDiscount;
        Remove();

        return monster;
    }
    public void SwapOrigin()
    {
        if(originMonster != null)
        {
            Animator anim = GetComponent<Animator>();
            anim.Play("exit");
       
            originMonster.gameObject.SetActive(true);
            if (CombatManager.Instance.homegroundMonsters.Contains(this))
                CombatManager.Instance.homegroundMonsters.Add(originMonster);
            if (CombatManager.Instance.awayMonsters.Contains(this))
                CombatManager.Instance.awayMonsters.Add(originMonster);

            CombatManager.Instance.allMonsters.Add(originMonster);
            originMonster.battleInstance.mp = Mathf.Max(0, originMonster.battleInstance.mp - originMonster.formChangeMonsterMpDiscount);
            originMonster.formChangeMonsterMpDiscount = 0;
            Remove();
        }
    }
    public SkillData GetSkillData(out SkillTrigger skillTrigger)
    {
        skillTrigger = SkillTrigger.Alone;
        SkillData skillData = null;
        skillData = CombatManager.Instance.CheckTrigger(this, battleInstance.triggerSkillDatas, out skillTrigger);
        if (skillData == null)
        {
            skillData = CombatManager.Instance.CheckPercent(battleInstance.percentSkillDatas);
            if (skillData == null)
            {
                return CombatManager.Instance.CheckConfirm(this, battleInstance.currentConfirmSkillPriority, battleInstance.skillDatas, ref confirmSkillIndex);
            }
            else
                return skillData;
        }
        else
            return skillData;
    }
    public void DonActiveShadowAndUI()
    {
        Transform shadow = transform.GetChild(4);
        Transform ui = transform.GetChild(0);

        shadow.gameObject.SetActive(false);
        ui.gameObject.SetActive(false);
    }
    public void ActiveShadowAndUI()
    {
        Transform shadow = transform.GetChild(4);
        Transform ui = transform.GetChild(0);

        shadow.gameObject.SetActive(true);
        ui.gameObject.SetActive(true);
    }
    public void ResetHide(Transform shaodw, Transform cavas, Transform maskTransform)
    {
        cavas.gameObject.SetActive(true);
        var rec = cavas.GetComponent<RectTransform>();
        rec.anchoredPosition = new Vector2(rec.anchoredPosition.x, maskTransform.localPosition.x);

        shaodw.gameObject.SetActive(true);

        bulletCreatePoint.localPosition = new Vector3(bulletCreatePoint.localPosition.x, maskTransform.localRotation.eulerAngles.x, bulletCreatePoint.localPosition.z);

        battleInstance.repeatRatio = Mathf.Max(0, battleInstance.repeatRatio - SkillManager.Instance.sandHideData.buffValue);
        battleInstance.skillDatas.Clear();
        battleInstance.skillDatas.AddRange(originInstance.skillDatas);
        battleInstance.percentSkillDatas.AddRange(originInstance.percentSkillDatas);
        battleInstance.triggerSkillDatas.AddRange(originInstance.triggerSkillDatas);
        battleInstance.battleAIType = originInstance.battleAIType;

        battleInstance.currentConfirmSkillPriority = (ConfirmSkillPriority)confirmSkillIndex;
        confirmSkillIndex = 0;
     
        Destroy(mask.gameObject);
        mask = null;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.maskInteraction = SpriteMaskInteraction.None;

        CombatManager.Instance.battleQueue.RemoveAll(x => x == this);
        battleInstance.dex = 0f;

        passiveFlag = false;
        currentPassiveName = PassiveName.None;
    }
    public void Hurt(float dmg, EntityMonster player, SkillData data)
    {
        bool check = CheckNoblesseOblige(dmg);
        bool check2 = false;
        if(check == false)
            check2 = CheckDestinyOfTheTeam(dmg);

        bool check3 = CheckElementalKingsArmor(dmg, data);
        if (check == false && check2 == false && check3 == false)
        {
            //크리티컬확률
            float geniusValue = (player.battleInstance.abilities.Contains(AbilityType.Potential)) ? 2f : 0f;

            check = CreaticalCheck(dmg, player, data);
            dmg = (check) ? dmg * (2f + geniusValue) : dmg;

            float fieldStatus = CombatManager.Instance.GetFieldStatus(data);
            float stoneStatus = CombatManager.Instance.GetStoneStatus(data);
            float statusCheck = StatusCheck(data);
            //속성추가데미지
            dmg = dmg * (Mathf.Max(0.1f, statusCheck + fieldStatus + stoneStatus));

            //공격전 반격기
            CheckStrongDefenseSkill(dmg + originInstance.def);

            if (passiveFlag == true && currentPassiveName == PassiveName.ShadowCrew)
                ResetPassiveSkill();

            //색바꾸기
            if (check)
                hurtObject.SetTime(new Color(0.2627450980392157f, 0.2627450980392157f, 0.2627450980392157f), Color.white);
            else
                hurtObject.SetTime(Color.white, new Color(0.7843137254901961f, 0f, 0f));

            //딜계산
            player.CheckUnbreakable(this, dmg);
            battleInstance.hp = Mathf.Max(CheckSturdy(dmg), battleInstance.hp - dmg);
            if (isDead == true)
            {
                if (this.passiveFlag == true && this.currentPassiveName == PassiveName.DestinyBond)
                    CheckDestinyBondSkill(player, true);
                else
                    Dead();
            }
            else
            {
                if (this.passiveFlag == true && this.currentPassiveName == PassiveName.DestinyBond)
                    CheckDestinyBondSkill(player, false);

                CheckMadnessSkill(player);
            }

            if(this.battleInstance.abilities.Contains(AbilityType.Fighting))
            {
                battleInstance.atk += 1;
            }
        }
    }
    public void CriticalHurt(float dmg, EntityMonster player, SkillData data)
    {
        bool check = CheckNoblesseOblige(dmg);
        bool check2 = false;
        if (check == false)
            check2 = CheckDestinyOfTheTeam(dmg);

        bool check3 = CheckElementalKingsArmor(dmg, data);
        if (check == false && check2 == false && check3 == false)
        {
            dmg = dmg * 2f;
            dmg = (check) ? dmg * 2f : dmg;
            CheckStrongDefenseSkill(dmg);

            if (passiveFlag == true && currentPassiveName == PassiveName.ShadowCrew)
                ResetPassiveSkill();

            Instantiate(SkillManager.Instance.flowingText2, center.position, Quaternion.identity);
            hurtObject.SetTime(new Color(0.2627450980392157f, 0.2627450980392157f, 0.2627450980392157f), Color.white);

            player.CheckUnbreakable(this, dmg);
            battleInstance.hp = Mathf.Max(CheckSturdy(dmg), battleInstance.hp - dmg);

            if (isDead == true)
            {
                if (this.passiveFlag == true && this.currentPassiveName == PassiveName.DestinyBond)
                    CheckDestinyBondSkill(player, true);
                else
                    Dead();
            }
            else
            {
                if (this.passiveFlag == true && this.currentPassiveName == PassiveName.DestinyBond)
                    CheckDestinyBondSkill(player, false);

                CheckMadnessSkill(player);
            }

            if (this.battleInstance.abilities.Contains(AbilityType.Fighting))
            {
                battleInstance.atk += 1;
            }
        }
    }

    //Normal, Fire, Ice, Earth, Wind, Water, Wood, Elec, Acid, Dark, Light
    public float StatusCheck(SkillData data)
    {
        var shieldStatus = battleInstance.status;
        var attackStatus = data.status;

        if(shieldStatus == Status.Fire)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 0.5f;
                case Status.Ice:
                    return 0.5f;
                case Status.Earth:
                    return 2f;
                case Status.Wind:
                    return 1f;
                case Status.Water:
                    return 2f;
                case Status.Wood:
                    return 0.5f;
                case Status.Elec:
                    return 1f;
                case Status.Acid:
                    return 1f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Ice)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 2f;
                case Status.Ice:
                    return 0.5f;
                case Status.Earth:
                    return 0.5f;
                case Status.Wind:
                    return 1f;
                case Status.Water:
                    return 2f;
                case Status.Wood:
                    return 0.5f;
                case Status.Elec:
                    return 1f;
                case Status.Acid:
                    return 1f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Earth)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 0.5f;
                case Status.Ice:
                    return 2f;
                case Status.Earth:
                    return 0.5f;
                case Status.Wind:
                    return 1f;
                case Status.Water:
                    return 2f;
                case Status.Wood:
                    return 0.5f;
                case Status.Elec:
                    return 0.5f;
                case Status.Acid:
                    return 1f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if (shieldStatus == Status.Wind)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 1f;
                case Status.Ice:
                    return 1f;
                case Status.Earth:
                    return 0.5f;
                case Status.Wind:
                    return 0.5f;
                case Status.Water:
                    return 1f;
                case Status.Wood:
                    return 0.5f;
                case Status.Elec:
                    return 1f;
                case Status.Acid:
                    return 1f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Water)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 0.5f;
                case Status.Ice:
                    return 0.5f;
                case Status.Earth:
                    return 0.5f;
                case Status.Wind:
                    return 1f;
                case Status.Water:
                    return 0.5f;
                case Status.Wood:
                    return 2f;
                case Status.Elec:
                    return 2f;
                case Status.Acid:
                    return 1f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Wood)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 2f;
                case Status.Ice:
                    return 2f;
                case Status.Earth:
                    return 0.5f;
                case Status.Wind:
                    return 2f;
                case Status.Water:
                    return 0.5f;
                case Status.Wood:
                    return 0.5f;
                case Status.Elec:
                    return 0.5f;
                case Status.Acid:
                    return 2f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Elec)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 1f;
                case Status.Ice:
                    return 1f;
                case Status.Earth:
                    return 1f;
                case Status.Wind:
                    return 1f;
                case Status.Water:
                    return 1f;
                case Status.Wood:
                    return 1f;
                case Status.Elec:
                    return 0.5f;
                case Status.Acid:
                    return 1f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Acid)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 1f;
                case Status.Ice:
                    return 1f;
                case Status.Earth:
                    return 1f;
                case Status.Wind:
                    return 1f;
                case Status.Water:
                    return 1f;
                case Status.Wood:
                    return 2f;
                case Status.Elec:
                    return 1f;
                case Status.Acid:
                    return 0.5f;
                case Status.Dark:
                    return 1f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Dark)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 0.5f;
                case Status.Ice:
                    return 0.5f;
                case Status.Earth:
                    return 0.5f;
                case Status.Wind:
                    return 0.5f;
                case Status.Water:
                    return 0.5f;
                case Status.Wood:
                    return 0.5f;
                case Status.Elec:
                    return 0.5f;
                case Status.Acid:
                    return 0.5f;
                case Status.Dark:
                    return 0.5f;
                case Status.Light:
                    return 2f;
                default:
                    return 1f;
            }
        }
        else if(shieldStatus == Status.Light)
        {
            switch (attackStatus)
            {
                case Status.Normal:
                    return 1f;
                case Status.Fire:
                    return 1f;
                case Status.Ice:
                    return 1f;
                case Status.Earth:
                    return 1f;
                case Status.Wind:
                    return 1f;
                case Status.Water:
                    return 1f;
                case Status.Wood:
                    return 1f;
                case Status.Elec:
                    return 1f;
                case Status.Acid:
                    return 1f;
                case Status.Dark:
                    return 2f;
                case Status.Light:
                    return 0.5f;
                default:
                    return 1f;
            }
        }
        else
        {
            return 1f;
        }
    }
    public void Hurt(float dmg, EntityMonster destinyBondPlayer = null)
    {
        bool check = CheckNoblesseOblige(dmg);
        bool check2 = false;
        if (check == false)
            check2 = CheckDestinyOfTheTeam(dmg);

        if(check == false && check2 == false)
        {
            hurtObject.SetTime(Color.white, new Color(0.7843137254901961f, 0f, 0f));
            battleInstance.hp = Mathf.Max(CheckSturdy(dmg), battleInstance.hp - dmg);

            if (passiveFlag == true && currentPassiveName == PassiveName.ShadowCrew)
                ResetPassiveSkill();

            if(destinyBondPlayer != null)
            {
                if (isDead == true)
                {
                    if (this.passiveFlag == true && this.currentPassiveName == PassiveName.DestinyBond)
                        CheckDestinyBondSkill(destinyBondPlayer, true);
                    else
                        Dead();
                }
            }
            else
            {
                if (isDead == true)
                    Dead();
            }
        }
    }
    public void CheckUnbreakable(EntityMonster target, float dmg)
    {
        float maxDmg = Mathf.FloorToInt(target.battleInstance.maxHp * 0.33f);
        if (dmg <= maxDmg)
        {
            unbreakableTarget = target;
        }
        else
        {
            unbreakableTarget = null;
        }
    }
    public void ClearAbility()
    {
        if (battleInstance.abilities.Contains(AbilityType.Recycle))
        {
            dogDmgStatus.RemoveRecoveryFlag(this);
            CombatManager.Instance.dotDmgObjList.Remove(this);
        }

        this.battleInstance.abilities.Clear();
    }
    public bool DmgCheck(EntityMonster player, SkillData skillData, bool blockText = false)
    {
        float knowledgeValue = (battleInstance.abilities.Contains(AbilityType.Knowledge) && (battleInstance.hp <= battleInstance.maxHp * 0.33f)) ? 0.5f : 0f;
        float knowledgeValue2 = (player.battleInstance.abilities.Contains(AbilityType.Knowledge) && (player.battleInstance.hp <= player.battleInstance.maxHp * 0.33f)) ? 0.5f : 0f;
        float repeatValue = battleInstance.repeatRatio + skillData.repeatRatio + knowledgeValue - knowledgeValue2;

        repeatValue = Mathf.Min(repeatValue, 0.9f);

        if (repeatValue <= 0f)
            return true;

        if (player.CheckLockOn())
            return true;

        float value = Random.Range(0f, 1f);
        if ((value <= repeatValue) || (passiveFlag == true && currentPassiveName == PassiveName.ShadowCrew))
        {
            if(center != null && blockText == false)
                Instantiate(SkillManager.Instance.flowingText, center.position, Quaternion.identity);

            SoundManager.Instance.PlayEffect(146, 0.75f);
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool DmgCheck(float repeatRato)
    {
        float repeatValue = repeatRato;

      

        if (repeatValue <= 0f)
            return true;

       
        float value = Random.Range(0f, 1f);
        if ((value <= repeatValue) || (passiveFlag == true && currentPassiveName == PassiveName.ShadowCrew))
        {
            if (center != null)
                Instantiate(SkillManager.Instance.flowingText, center.position, Quaternion.identity);

            SoundManager.Instance.PlayEffect(146, 0.75f);
            return false;
        }
        else
        {
            return true;
        }
    }
    public void FlyingDead()
    {
        //부활
        bool check = CheckSturdyOneTurnKill();
        if(check == false)
        {
            if (isClone == false)
            {
                EmptyPositionStructure emptyPosition = new EmptyPositionStructure();
                emptyPosition.width = width;
                emptyPosition.height = height;
                emptyPosition.worldPosition = transform.position;
                Tuple<bool, EmptyPositionStructure, MonsterInstance> data = new Tuple<bool, EmptyPositionStructure, MonsterInstance>(CombatManager.Instance.homegroundMonsters.Contains(this), emptyPosition, originInstance);
                CombatManager.Instance.deadMonsters.Add(data);
            }

            SoundManager.Instance.PlayEffect(149, 1f);

            ResetPassiveSkill();
            deadBlock = true;

            sternStatus.Stop(this);
            dogDmgStatus.Clear(this);

            var rendererParent = Instantiate(SkillManager.Instance.flyingMoveEffect, this.transform.position, Quaternion.identity).transform;
            var renderer = rendererParent.GetChild(0).GetComponent<SpriteRenderer>();
            renderer.sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
            rendererParent.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(this) == true) ? transform.localScale.x : -1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            CombatManager.Instance.battleQueue.RemoveAll(x => x == this);

            rendererParent.GetComponent<FlyingMove>().Move(CombatManager.Instance.homegroundMonsters.Contains(this));

            if (CombatManager.Instance.homegroundMonsters.Contains(this))
                CombatManager.Instance.homegroundMonsters.Remove(this);
            else
                CombatManager.Instance.awayMonsters.Remove(this);

            CombatManager.Instance.allMonsters.Remove(this);

            Destroy(this.gameObject);
        }
    }
    public void DissolveDead()
    {
        bool check = CheckSturdyOneTurnKill();
        if(check == false)
        {
            ResetPassiveSkill();
            deadBlock = true;

            sternStatus.Stop(this);
            dogDmgStatus.Clear(this);

            SoundManager.Instance.PlayEffect(148, 1f);

            CombatManager.Instance.battleQueue.RemoveAll(x => x == this);

            if (CombatManager.Instance.homegroundMonsters.Contains(this))
                CombatManager.Instance.homegroundMonsters.Remove(this);
            else
                CombatManager.Instance.awayMonsters.Remove(this);

            CombatManager.Instance.allMonsters.Remove(this);

            var effect = Instantiate(SkillManager.Instance.dissolveObject, transform.position, Quaternion.identity);
            effect.StartCoroutine(effect.StartDissolve(GetComponent<SpriteRenderer>()));

            Destroy(this.gameObject);
        }
    }
    public void DeathMatchDead()
    {
        ResetPassiveSkill();
        deadBlock = true;

        sternStatus.Stop(this);
        dogDmgStatus.Clear(this);

        CombatManager.Instance.battleQueue.RemoveAll(x => x == this);

        if (CombatManager.Instance.homegroundMonsters.Contains(this))
            CombatManager.Instance.homegroundMonsters.Remove(this);
        else
            CombatManager.Instance.awayMonsters.Remove(this);

        CombatManager.Instance.allMonsters.Remove(this);

        var effect = Instantiate(SkillManager.Instance.dissolveObject, transform.position, Quaternion.identity);
        effect.StartCoroutine(effect.StartDissolve(GetComponent<SpriteRenderer>()));

        Destroy(this.gameObject);
    }
    public void CloneDead()
    {
        ResetPassiveSkill();
        deadBlock = true;

        sternStatus.Stop(this);
        dogDmgStatus.Clear(this);

        Instantiate(SkillManager.Instance.windClone, transform.position, Quaternion.identity);
        CombatManager.Instance.battleQueue.RemoveAll(x => x == this);

        if (CombatManager.Instance.homegroundMonsters.Contains(this))
            CombatManager.Instance.homegroundMonsters.Remove(this);
        else
            CombatManager.Instance.awayMonsters.Remove(this);

        CombatManager.Instance.allMonsters.Remove(this);

        Destroy(this.gameObject);
    }
    public bool CreaticalCheck(float dmg, EntityMonster player, SkillData skillData)
    {
        if (dmg <= this.battleInstance.def)
        {
            return false;
        }
        else
        {
            float miasmaValue = (player.battleInstance.abilities.Contains(AbilityType.Miasma) && (player.battleInstance.hp <= player.battleInstance.maxHp * 0.33f)) ? 1f : 0f;
            float creaticalValue = Mathf.Min(0.9f, player.battleInstance.creaticalRatio + skillData.creaticalRatio + miasmaValue);
            if (creaticalValue <= 0f)
                return false;
            float value = Random.Range(0f, 1f);
            if (value <= creaticalValue)
            {
                if(center != null)
                    Instantiate(SkillManager.Instance.flowingText2, center.position, Quaternion.identity);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public void SetOrigin()
    {
        battleInstance.Copy(originInstance);
        fireVowStack = 3;
    }
    public float GetDefenseDeal()
    {
        return battleInstance.abilities.Contains(AbilityType.UltimateHardness) ? 0f : 1f;
    }
    public void AddPassiveSkill(SkillData data)
    {
        passiveFlag = true;
        shiledCount = data.shieldCount;
        currentPassiveName = data.passiveName;
    }
    public bool PlayPassiveSkill()
    {
        if(shieldTarget != null)
        {
            if(currentPassiveName == PassiveName.Driven)
            {
                SkillManager.Instance.PlayPassive(currentPassiveName, this);
                shiledCount--;
                if (shiledCount <= 0)
                {
                    passiveFlag = false;
                    shieldCreatePoint.gameObject.SetActive(false);
                    currentPassiveName = PassiveName.None;
                }
            }
            else if(currentPassiveName == PassiveName.FireBraces)
            {
                SkillManager.Instance.PlayPassive(currentPassiveName, this);

                var braces = GetComponentInChildren<FireBraces>();
                if(braces.transform.childCount <= 0)
                {
                    passiveFlag = false;
                    Destroy(braces.gameObject);
                    currentPassiveName = PassiveName.None;
                }
            }
            else if(currentPassiveName == PassiveName.Madness)
            {
                SkillManager.Instance.PlayPassive(currentPassiveName, this);
                passiveFlag = false;
                shieldCreatePoint.gameObject.SetActive(false);
                currentPassiveName = PassiveName.None;
            }
            else if(currentPassiveName == PassiveName.SandHide)
            {
                SkillManager.Instance.PlayPassive(currentPassiveName, this);
                passiveFlag = false;
                currentPassiveName = PassiveName.None;
            }
            else if(currentPassiveName == PassiveName.Surprise_Attack)
            {
                SkillManager.Instance.PlayPassive(currentPassiveName, this);
                shiledCount--;
                if (shiledCount <= 0)
                {
                    passiveFlag = false;
                    shieldCreatePoint.gameObject.SetActive(false);
                    currentPassiveName = PassiveName.None;
                    SkillManager.Instance.supriseAttackList.Remove(this);
                }
            }
            else if(currentPassiveName == PassiveName.ShadowCrew)
            {
                SkillManager.Instance.PlayPassive(currentPassiveName, this);
                passiveFlag = false;
                shieldCreatePoint.gameObject.SetActive(false);
                currentPassiveName = PassiveName.None;
                SkillManager.Instance.shadowCrewList.Remove(this);
            }
            else if(currentPassiveName == PassiveName.DestinyBond)
            {
                SkillManager.Instance.PlayPassive(currentPassiveName, this);
                passiveFlag = false;
                shieldCreatePoint.gameObject.SetActive(false);
                currentPassiveName = PassiveName.None;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ResetPassiveSkill()
    {
        if (currentPassiveName == PassiveName.ShadowCrew)
        {
            GetComponent<SpriteRenderer>().color = battleInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().color;
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else if(currentPassiveName == PassiveName.NoblesseOblige)
        {
            if (CombatManager.Instance.homegroundMonsters.Contains(this))
                SkillManager.Instance.homegrounsNoblesseOblige = null;
            else
                SkillManager.Instance.awayNoblessOblige = null;
        }

        shiledCount = 0;
        passiveFlag = false;
        currentPassiveName = PassiveName.None;

        shieldCreatePoint.gameObject.SetActive(false);

        var braces = GetComponentInChildren<FireBraces>();
        if(braces != null)
            Destroy(braces.gameObject);

        if (mask != null)
        {
            Transform shaodw = transform.GetChild(4);
            Transform cavas = transform.GetChild(0);
            Transform maskTransform = transform.GetChild(5);

            ResetHide(shaodw, cavas, maskTransform);
        }

        if (thunderManAura != null)
        {
            Destroy(thunderManAura.gameObject);
            thunderManAura = null;
        }


        SkillManager.Instance.supriseAttackList.Remove(this);
        SkillManager.Instance.shadowCrewList.Remove(this);
    }

    public void UpdateGardner()
    {
        int random = Random.Range(0, 5);
        if(random == 0)
        {
            this.shieldCreatePoint.gameObject.SetActive(true);
            this.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = SkillManager.Instance.madnessSprite;
            AddPassiveSkill(SkillManager.Instance.madnessSkillData);
            shiledCount = 0;
        }
        else if(random == 1)
        {
            this.shieldCreatePoint.gameObject.SetActive(true);
            this.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = SkillManager.Instance.counterAttackSprite;
            AddPassiveSkill(SkillManager.Instance.drivenSkillData);
        }
        else if(random == 2)
        {
            this.shieldCreatePoint.gameObject.SetActive(true);
            this.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = SkillManager.Instance.surpriseAttackSprite;
            AddPassiveSkill(SkillManager.Instance.surpriseAttackSkillData);
            SkillManager.Instance.supriseAttackList.Add(this);
        }
        else if (random == 3)
        {
            this.shieldCreatePoint.gameObject.SetActive(true);
            this.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = SkillManager.Instance.strongDefenseSprite;
            AddPassiveSkill(SkillManager.Instance.strongDefenseSkillData);
        }
        else if (random == 4)
        {
            this.shieldCreatePoint.gameObject.SetActive(true);
            this.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = SkillManager.Instance.destinyBodSprite;
            AddPassiveSkill(SkillManager.Instance.destinyBondSkillData);
        }
    }
    public bool CheckDrivenSkill(EntityMonster currentTarget)
    {
        if (passiveFlag == true)
        {
            if (currentPassiveName == PassiveName.Driven)
            {
                if (currentTarget.passiveFlag == true && currentTarget.currentPassiveName == PassiveName.ThunderMan && (currentTarget.center.childCount > 0))
                {
                    ResetPassiveSkill();
                    return false;
                }
                else
                {
                    shieldTarget = currentTarget;
                    CombatManager.Instance.battleQueue.AddFirst(this);
                    return true;

                }
            }
            else
                return false;
        }
        else
            return false;
    }
    public void CheckFireBaracesSkill(EntityMonster currentTarget)
    {
        CombatManager.Instance.battleQueue.AddFirst(this);
        shieldTarget = currentTarget;
    }
    public bool CheckMadnessSkill(EntityMonster currentTarget)
    {
        if (passiveFlag == true)
        {
            if (currentPassiveName == PassiveName.Madness)
            {
                shiledCount++;
                if (shiledCount >= 3)
                {
                    if(shieldTarget == null)
                    {
                        CombatManager.Instance.battleQueue.AddFirst(this);
                        shieldTarget = currentTarget;
                    }
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;
    }
    public void CheckStrongDefenseSkill(float dmg)
    {
        if (passiveFlag == true)
        {
            if (currentPassiveName == PassiveName.StrongDefense)
            {
                if(battleInstance.def >= dmg)
                    SkillManager.Instance.PlayPassive(PassiveName.StrongDefense, this);
                else
                {
                    passiveFlag = false;
                    shieldCreatePoint.gameObject.SetActive(false);
                    battleInstance.def = originInstance.def;
                    currentPassiveName = PassiveName.None;
                }
            }
        }
    }
    public float CheckSturdy(float dmg)
    {
        if (battleInstance.abilities.Contains(AbilityType.Sturdy))
        {
            if ((battleInstance.hp >= battleInstance.maxHp) && (dmg >= battleInstance.hp))
            {
                Instantiate(SkillManager.Instance.strongDefenseEffect, transform.position, Quaternion.identity);
                return 0.5f;
            }
            else
                return 0f;
        }
        else
            return 0f;
    }
    public bool CheckLivingDead()
    {
        if (battleInstance.abilities.Contains(AbilityType.LivingDead))
        {
            if (startLivingDeadFlag == false)
            {
                Instantiate(SkillManager.Instance.livingDeadStartEffect, center.position, Quaternion.identity);

                bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(this);
                var monsters = isHomeground ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.allMonsters;

                if(CombatManager.Instance.livingDeadAllDeadFlag == false)
                {
                    bool check = false;
                    for (int i = 0; i < monsters.Count; ++i)
                    {
                        if (monsters[i].battleInstance.hp > 0)
                        {
                            check = true;
                            break;
                        }
                    }

                    if (check == false)
                    {
                        for (int i = 0; i < monsters.Count; ++i)
                        {
                            monsters[i].ResetPassiveSkill();
                            CombatManager.Instance.battleQueue.RemoveAll(x => x == monsters[i]);
                        }

                        CombatManager.Instance.livingDeadAllDeadFlag = true;
                    }
                    startLivingDeadFlag = true;
                }
            }

            return true;
        }
        else
            return false;
    }
    public bool CheckLockOn()
    {
        if (passiveFlag == true)
        {
            if (currentPassiveName == PassiveName.LockOn)
            {
                shiledCount--;
                if(shiledCount <= 0)
                {
                    ResetPassiveSkill();
                    return false;
                }
                else
                    return true;
            }
            else
                return false;
        }
        else
            return false;
    }
    public bool CheckSturdyOneTurnKill()
    {
        if (battleInstance.abilities.Contains(AbilityType.Sturdy))
        {
            int random = Random.Range(0, 100);
            if (random < 70)
            {
                Instantiate(SkillManager.Instance.strongDefenseEffect, transform.position, Quaternion.identity);
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }
    public bool CheckElementalKingsArmor(float dmg, SkillData data)
    {
        if (passiveFlag == true)
        {
            if (currentPassiveName == PassiveName.ElementalKingsArmor)
            {
                float value = StatusCheck(data);
                if (2f <= value)
                {
                    ResetPassiveSkill();
                    return false;
                }
                else
                {
                    shiledCount--;
                    if (shiledCount <= 0)
                    {
                        ResetPassiveSkill();
                        return false;
                    }
                    else
                    {
                        Instantiate(SkillManager.Instance.elemantalText, center.position, Quaternion.identity);
                        return true;
                    }
                }
            }
            else
                return false;
        }
        else
            return false;
    }
    public void CheckSandHideSkill(EntityMonster currentTarget)
    {
        if (passiveFlag == true)
        {
            if (currentPassiveName == PassiveName.SandHide)
            {
                shieldTarget = currentTarget;
                CombatManager.Instance.battleQueue.AddFirst(this);
            }
        }
    }
    public void CheckThunderManSkill()
    {
        if(shiledCount <= 0)
        {
            passiveFlag = false;
            currentPassiveName = PassiveName.None;
            shieldCreatePoint.gameObject.SetActive(false);

            var child = center.GetChild(0);
            Destroy(child.gameObject);
        }
        else
        {
            shiledCount = Mathf.Max(0, shiledCount - 1);
            CombatManager.Instance.battleQueue.RemoveAll(x => x == this);
            CombatManager.Instance.battleQueue.AddFirst(this);
            CombatManager.Instance.battleQueue.AddFirst(this);
        }
    }
    public void CheckSurpriseAttackSkill(EntityMonster currentTarget)
    {
        var surpriseObjs = SkillManager.Instance.supriseAttackList;

        if (surpriseObjs.Count > 0)
        {
            bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(currentTarget);
            for (int i = 0; i < surpriseObjs.Count; ++i)
            {
                bool isHomeground2 = CombatManager.Instance.homegroundMonsters.Contains(surpriseObjs[i]);
                if (isHomeground != isHomeground2)
                {
                    surpriseObjs[i].shieldTarget = currentTarget;
                    CombatManager.Instance.battleQueue.AddFirst(surpriseObjs[i]);
                }
            }

            //surpriseObjs.RemoveAll(x => CombatManager.Instance.battleQueue.Contains(x));
        }
    }
    public void CheckShaodwCrew(EntityMonster currentTarget)
    {
        shieldTarget = currentTarget;
        CombatManager.Instance.battleQueue.AddFirst(this);
    }
    public void CheckDestinyBondSkill(EntityMonster currentTarget, bool isDead)
    {
        if(isDead == true)
        {
            this.shieldTarget = currentTarget;
            CombatManager.Instance.battleQueue.AddFirst(this);
        }
        else
        {
            shiledCount--;
            if (shiledCount <= 0)
                ResetPassiveSkill();
        }
    }
    public bool CheckDestinyOfTheTeam(float dmg)
    {
        if (passiveFlag == true && currentPassiveName == PassiveName.DestinyOfTheTeam)
        {
            var targets = CombatManager.Instance.homegroundMonsters.Contains(this) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
            if (targets.Count > 1)
            {
                targets = targets.FindAll(x => !(x.passiveFlag == true && x.currentPassiveName == PassiveName.DestinyOfTheTeam));
                if(targets.Count > 0)
                {
                    var currentTarget = targets[Random.Range(0, targets.Count)];
                    currentTarget.Hurt(dmg);
                    shiledCount--;
                    if (shiledCount <= 0)
                        ResetPassiveSkill();

                    return true;
                }
                else
                {
                    ResetPassiveSkill();
                    return false;
                }
            }
            else
            {
                ResetPassiveSkill();
                return false;
            }
        }
        else
            return false;
    }
    public bool CheckNoblesseOblige(float dmg)
    {
        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(this);
        if(isHomeground)
        {
            if (this == SkillManager.Instance.homegrounsNoblesseOblige)
                return false;
        }
        else
        {
            if (this == SkillManager.Instance.awayNoblessOblige)
                return false;
        }
        var targets = isHomeground ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        if (targets.Count > 1)
        {
            var target = isHomeground ? SkillManager.Instance.homegrounsNoblesseOblige : SkillManager.Instance.awayNoblessOblige;
            if(target != null)
            {
                target.Hurt(dmg);
                if (target != null && target.isDead == false)
                {
                    target.shiledCount--;
                    if (target.shiledCount <= 0)
                        target.ResetPassiveSkill();
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        else
            return false;
    }
    public float CheckPoisonSkill()
    {
        if (passiveFlag == true && currentPassiveName == PassiveName.PoisonBarrior)
        {
            if (shiledCount <= 0)
            {
                passiveFlag = false;
                currentPassiveName = PassiveName.None;
                shieldCreatePoint.gameObject.SetActive(false);
                return 1f;
            }
            else
            {
                shiledCount = Mathf.Max(0, shiledCount - 1);
                return 2f;
            }
        }
        else
            return 1f;
    }
    public void Dead()
    {
        if (deadBlock == false)
        {
            if (battleInstance.abilities.Contains(AbilityType.WillPower))
            {
                ResetPassiveSkill();
                deadBlock = true;

                sternStatus.Stop(this);
                dogDmgStatus.Clear(this);

                var deadPrefab = checkDead2 ? SkillManager.Instance.monsterDeadEffect2 : SkillManager.Instance.monsterDeadEffect;
                var rendererParent = Instantiate(deadPrefab, this.transform.position, Quaternion.identity).transform;
                rendererParent.GetComponent<Animator>().Play("knockDown");
                var renderer = rendererParent.GetChild(0).GetComponent<SpriteRenderer>();
                renderer.sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                rendererParent.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(this) == true) ? transform.localScale.x : -1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                CombatManager.Instance.battleQueue.RemoveAll(x => x == this);

                bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(this);
                if (isHomeground)
                    CombatManager.Instance.homegroundMonsters.Remove(this);
                else
                    CombatManager.Instance.awayMonsters.Remove(this);

                CombatManager.Instance.allMonsters.Remove(this);
                this.gameObject.SetActive(false);

                SkillManager.Instance.willPowerStructure.objs.Add(this);
                SkillManager.Instance.willPowerStructure.effects.Add(rendererParent.gameObject);
                SkillManager.Instance.willPowerStructure.isHomegrounds.Add(isHomeground);
                SkillManager.Instance.willPowerStructure.checkSkillEnds.Add(false);
                SkillManager.Instance.willPowerStructure.isStart = true;
            }
            else
            {
                if (isClone == true)
                    CloneDead();
                else
                {
                    //부활준비
                    if (isClone == false)
                    {
                        EmptyPositionStructure emptyPosition = new EmptyPositionStructure();
                        emptyPosition.width = width;
                        emptyPosition.height = height;
                        emptyPosition.worldPosition = transform.position;
                        Tuple<bool, EmptyPositionStructure, MonsterInstance> data = new Tuple<bool, EmptyPositionStructure, MonsterInstance>(CombatManager.Instance.homegroundMonsters.Contains(this), emptyPosition, originInstance);
                        CombatManager.Instance.deadMonsters.Add(data);
                    }

                    SoundManager.Instance.PlayEffect(147, 1f);

                    ResetPassiveSkill();
                    deadBlock = true;

                    sternStatus.Stop(this);
                    dogDmgStatus.Clear(this);

                    var deadPrefab = checkDead2 ? SkillManager.Instance.monsterDeadEffect2 : SkillManager.Instance.monsterDeadEffect;
                    var rendererParent = Instantiate(deadPrefab, this.transform.position, Quaternion.identity).transform;
                    var renderer = rendererParent.GetChild(0).GetComponent<SpriteRenderer>();
                    renderer.sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                    rendererParent.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(this)) ? transform.localScale.x : -1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    CombatManager.Instance.battleQueue.RemoveAll(x => x == this);

                    bool itemCheck = false;
                    if(CombatManager.Instance.homegroundMonsters.Contains(this))
                    {
                        if(CombatManager.Instance.homegroundMonsters.Count > 1)
                            itemCheck = true;
                    }
                    else
                    {
                        if (CombatManager.Instance.homegroundMonsters.Count > 0)
                            itemCheck = true;
                    }
                    
                    if (CombatManager.Instance.homegroundMonsters.Contains(this))
                        CombatManager.Instance.homegroundMonsters.Remove(this);
                    else
                        CombatManager.Instance.awayMonsters.Remove(this);

                    CombatManager.Instance.allMonsters.Remove(this);


                
                    //아이템스폰
                    if (battleInstance.abilities.Contains(AbilityType.SpawnItem) && (CombatManager.Instance.currentBattleType != BattleType.Gamebling) && itemCheck)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= 0.5f)
                        {
                            var itemData = originInstance.monsterData.spawnItems[Random.Range(0, originInstance.monsterData.spawnItems.Length)];
                            var clone = Instantiate(SkillManager.Instance.worldItem, this.transform.position, Quaternion.identity);
                            clone.Init(itemData, (CombatManager.Instance.homegroundMonsters.Contains(this)));
                        }
                    }

                    Destroy(this.gameObject);
                }
            }
        }
    }
    public IEnumerator RecoverRoutine(float startHp, float endHp, float speed = 1f, bool isHp = true)
    {
        startRecovery = true;
        float lerpSpeed = speed;
        float currentTime = 0f;
        float lerpTime = 1f;

        if(isHp == true)
        {
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                this.battleInstance.hp = Mathf.Lerp(startHp, endHp, currentSpeed);
                yield return null;
            }
        }
        else
        {
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                this.battleInstance.mp = Mathf.Lerp(startHp, endHp, currentSpeed);
                yield return null;
            }
        }
        
        startRecovery = false;
    }
    public IEnumerator DexRoutine(float startHp, float endHp, float speed = 1f)
    {
        startRecovery = true;
        float lerpSpeed = speed;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            this.battleInstance.dex = Mathf.Lerp(startHp, endHp, currentSpeed);
            yield return null;
        }

        startRecovery = false;
    }
    public IEnumerator HolyRoutine()
    {
        SkillManager.Instance.isBlock = true;
        battleInstance.dex = 0f;

        var effect = Instantiate(SkillManager.Instance.holyEffect, center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        battleInstance.hpRecoveryRatio = Mathf.Max(0, battleInstance.hpRecoveryRatio - 0.1f);
        battleInstance.manaRecoveryRatio = Mathf.Max(0, battleInstance.hpRecoveryRatio - 0.1f);

        SkillManager.Instance.isBlock = false;
    }
    public void Remove()
    {
        this.gameObject.SetActive(false);
        sternStatus.Stop(this);
        dogDmgStatus.Clear(this);
        CombatManager.Instance.battleQueue.RemoveAll(x => x == this);
        if (CombatManager.Instance.homegroundMonsters.Contains(this))
            CombatManager.Instance.homegroundMonsters.Remove(this);
        if (CombatManager.Instance.awayMonsters.Contains(this))
            CombatManager.Instance.awayMonsters.Remove(this);

        CombatManager.Instance.allMonsters.Remove(this);

        SkillManager.Instance.supriseAttackList.Remove(this);
        SkillManager.Instance.shadowCrewList.Remove(this);
    }

    public void Return(bool isHomeground, EmptyPositionStructure emptyPosition)
    {
        this.gameObject.SetActive(true);
        transform.position = emptyPosition.worldPosition;

        if ((width != emptyPosition.width) || (height != emptyPosition.height))
            CombatManager.Instance.SetFormationBuffDebuffs();

        width = emptyPosition.width;
        height = emptyPosition.height;
        if (isHomeground)
            CombatManager.Instance.homegroundMonsters.Add(this);
        else
            CombatManager.Instance.awayMonsters.Add(this);
        CombatManager.Instance.allMonsters.Add(this);

        battleInstance.dex = 0;
    }
}
