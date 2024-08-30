using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

public enum WallName { OneBreak, IceBite, ElectronicBite, Bounce, PoisonBite, DakeHoleRemove }
public enum PassiveName { None, Driven, FireBraces, Madness, StrongDefense, SandHide, ThunderMan, PoisonBarrior, Surprise_Attack, DestinyBond, DestinyOfTheTeam, ShadowCrew, NoblesseOblige, ElementalKingsArmor, LockOn }
public enum SkillName { HeadButt, ComboAttack, FireBall, Driven, Explosion, IceBall, IceSpear, IceBreak, IceBurst, IceWall, IceTrap, IceTime, IceStorm, Doublebarrel, FireStrike, FireBraces, FireBlade, FireViper, Madness, Meteor, DynamicPunch, FireBomb,
                        FormChange, EarthSpike, StrongDefense, StrongWall, EarthQuake, RockBlaster,  SandHide, MudCast, EarthThrowing, FormChange2, DoubleAttack, StaticShock, ElectronicShock, ThunderMan, Benumed, ElectronicSuction, LightingVotex, ThunderBolt,
                        ElectronicPunch, ElectronicNet, DiskDrive, ElectronicBall, ThunderBird, FairWinds, WindBlast, AirBall, Swirl, SonicBoom, AirDrive, WindSwap, Tornado, TornadoBarrior, WindClone, SharpWinds, PoisonFog, PoisonNeedle, NetBall, Cocktail, Cocktail_2,
    DeadlyPoison, TailAttack, AcidBall, PoisonPool, PoisonBarrior, PoisonSwirl, PoisonWind, Suction, MpSuction, HitPlant, HitStamp, GrassKnot, Synthesis, LeafBlade, WoodBullet, HeavyWeight, LightWeight, FormChange3, ShadowBall, DemonsEyes, SurpriseAttack, DestinyBond,
    DestinyOfTheTeam, ShadowCrew, Consume, EvilSoul, MindControl, TrickRoom, SoulSlash, SoulAttack, NaturePower, GivingTree, SkullDice, FireVow, HolyArrow, HolySword, HolyDefense, HolyLight,BlastRain, SealedSword, Easter, LazerBeam, HolyRecovery, Collaboration,
    NoblesseOblige, LiberationKnell, FormChange4, ShineSkipping, DivinePower, PowerBalance, LentPower, Wedge, SummonsMagic, Paradox, Rush, EnergyBall, ElementalKingsArmor, Pogrom, Dawn, LockOn, PotentialPower, Doppelganger, Pandora, DarkHole, Timeleap, DarkLoad, MythOfSpear, DeathRoll,
    DeathMatch, HellFire, DragonDive, ShadowPunch, SoulBlade, Slash, AcidRumble, SheerCold, FormChange5
}   
public struct BattleStructure
{
    public EntityMonster player;
 //   public List<EntityMonster> target;
    public SkillData skillData;

    public BattleStructure(EntityMonster player, SkillData skillData)
    {
        this.player = player;
        this.skillData = skillData;
    }
}

public struct StopStructure
{
    public EntityMonster stopObj;
    public Animator bullet;
    public Vector2 originPosition;
    public bool isHomeground;
    public bool dontMove;
    public bool isBlock;
    public Animator speicalBackground;
}

//부활
public struct WillPowerStructure
{
    public List<EntityMonster> objs;
    public List<GameObject> effects;
    public List<bool> isHomegrounds;
    public List<bool> checkSkillEnds;
    public bool isStart;

    public void Init()
    {
        objs = new List<EntityMonster>();
        effects = new List<GameObject>();
        isHomegrounds = new List<bool>();
        checkSkillEnds = new List<bool>();
        isStart = false;
    }
}

public class EasterStructure
{
    public MonsterInstance obj;
    public bool isHomeground;
    public EmptyPositionStructure emptyPosition;
    public float easterTimer;
    public bool complete;

    public void Init(MonsterInstance _obj, bool _isHomeground, EmptyPositionStructure _emptyPosition)
    {
        obj = _obj;
        isHomeground = _isHomeground;
        emptyPosition = _emptyPosition;
        easterTimer = 0f;
        complete = false;
    }
}

public struct EmptyPositionStructure
{
    public int width;
    public int height;
    public Vector2 worldPosition;
}

public class DarkHoleStructure
{
    public EntityMonster obj;
    public EmptyPositionStructure emptyPosition;
    public bool isHomeground;
    public float time;

    public void UpdateDarkHole()
    {
        if ((isHomeground == true) && CombatManager.Instance.homegroundMonsters.Count >= 9)
            return;
        if((isHomeground == false) && CombatManager.Instance.awayMonsters.Count >= 9)
            return;
        if (time >= 7)
        {
            time = 0;
            SkillManager.Instance.PlayDarkHoleTurn(this);
        }
        else
            time += Time.deltaTime;
    }
}

public class SkillManager : MonoBehaviour
{
    public MonsterData buffGolem;
    public MonsterData buffGolem2;
    public MonsterData forestGod;
    public MonsterData liberationWarrior;
    public MonsterData behimosformchange;

    public SelectBox selectBox;
    public GameObject iceStormEffect;
    public GameObject nearDistanceHitEffect;
    public GameObject pogromHitEffect;
    public GameObject rushTrail;
    public GameObject energyBall;
    public GameObject elementalBuff;
    public GameObject fireBallEffect;
    public GameObject fireBombEffect;
    public GameObject fireBombHitEffect;
    public GameObject fireSnap;
    public GameObject fireWide;
    public GameObject singleFireWide;
    public FireBraces fireBraces;
    public GameObject fireBrade;
    public GameObject fireViper;
    public GameObject fireBuffEffect;
    public MeteorSpawnPoints meteorEffect;
    public GameObject dynamicPunchEffect;
    public GameObject hellFireEffect;
    public GameObject iceBallEffect;
    public GameObject iceSpearEffect;
    public GameObject iceCrystalEffect;
    public GameObject iceBurstEffect;
    public GameObject iceWall;
    public GameObject icebite;
    public GameObject iceTrap;
    public SheerCold sheerColdPrefab;
    public GameObject earthBreak;
    public GameObject earthSpike_fail;
    public GameObject earthSpike_goal;
    public GameObject earthSpike_goalCri;
    public GameObject rockBlasterBullet;
    public GameObject rockBlasterBulletParent;
    public GameObject sandHideEffect;
    public GameObject strongWall;
    public Palabola earthThrowingEffect;
    public GameObject mudcastBullet;
    public GameObject drangonDive;
    public LightingLine lightingLine;
    public GameObject lightingCharge;
    public GameObject electronicShockEffect;
    public GameObject thunderManAura;
    public GameObject elecStart;
    public GameObject electronicSuction;
    public GameObject electronicSuctionCharge;
    public GameObject lightingVotexEffect;
    public GameObject thunderBoltEffect;
    public GameObject electronicPunchEffect;
    public GameObject electronicPunchEffect2;
    public GameObject electronicNet;
    public GameObject electronicNetShot;
    public GameObject diskDriveEffect;
    public GameObject electronicBall;
    public GameObject thunderBird;
    public GameObject thunderBirdHit;
    public GameObject fairWindsEffect;
    public GameObject airHit;
    public GameObject windBlast;
    public GameObject airBall;
    public GameObject sonicBoom;
    public GameObject airDrive;
    public GameObject windSwap;
    public GameObject tornado;
    public GameObject tornadoBarrior;
    public GameObject windClone;
    public GameObject sharpWindHit;
    public GameObject poisonFog;
    public GameObject poisonHit;
    public GameObject poisonNeedle;
    public GameObject netBall;
    public Palabola cocktailEffect;
    public Palabola cocktailEffect_2;
    public GameObject deadPoisonHit;
    public GameObject tailAttackEffects;
    public GameObject acidBallEffect;
    public GameObject poisonPool;
    public GameObject poisonWindBlast;
    public GameObject poisonWind;
    public AcidRumble acidRumble;
    public GameObject suctionEffect;
    public GameObject mpSuctionEffect;
    public GameObject hitPlant;
    public GameObject hitStamp;
    public GameObject grassKnot;
    public GameObject synthesis;
    public GameObject leafBlade;
    public GameObject woodStern;
    public GameObject woodBullet;
    public GameObject changeWeight;
    public GameObject woodCrash;
    public GameObject naturePower;
    public GameObject givingTree;
    public GameObject shadowBall;
    public GameObject demonsEyes;
    public GameObject demonsEyesPoint;
    public GameObject surpriseAttackEffect;
    public GameObject destinyBondEffect;
    public GameObject evilSoulParent;
    public GameObject mindControlEffect;
    public GameObject mindControlSucessEffect;
    public GameObject trickRoomEffect;
    public GameObject soulSlashEffect;
    public GameObject soulAttackEffect;
    public Dice skullDice;
    public GameObject magiceCicle;
    public GameObject paradox;
    public GameObject summonsBook;
    public GameObject pandora;
    public GameObject darkHole;
    public GameObject darkHoleRemoveObj;
    public GameObject timeleap;
    public GameObject darkLoad;
    public GameObject mythofSpear;
    public GameObject deathRollEffect;
    public GameObject lockSkillEffect;
    public GameObject deathMatchTimeEffect;
    public GameObject soulBlade;
    public GameObject shadowPunch;
    public GameObject shadowPunchSupport;
    public GameObject formchange5_hit;
    public GameObject holyArrow;
    public GameObject holySword;
    public GameObject holyDefense;
    public GameObject holyLight;
    public MeteorSpawnPoints blastRain;
    public GameObject sealedSword;
    public GameObject easter;
    public GameObject lazerBeam;
    public GameObject holyRecoveryEffect;
    public GameObject holyRecoveryParent;
    public GameObject collaboration;
    public GameObject liberationKnell;
    public GameObject divinePowerEffect;
    public GameObject powerBalance;
    public GameObject lentPower;
    public GameObject holyEffect;
    public GameObject recoveryEffect;
    public GameObject burnEffect;
    public GameObject unableEffect;
    public GameObject poisenEffect;
    public GameObject deadlyPoisonEffect;
    public GameObject palalysisEffect;
    public GameObject debuffShiledEffect;
    public GameObject debuffSpeedEffect;
    public GameObject debuffAttackEffect;
    public GameObject buffShiledEffect;
    public GameObject buffSpeedEffect;
    public GameObject buffAttackEffect;
    public GameObject meditation;
    public GameObject spicalAttackBackground;
    public GameObject spicalAttackBackground2;
    public GameObject self_destruction;
    public GameObject explosition;
    public GameObject madessBurn;
    public GameObject monsterDeadEffect;
    public GameObject monsterDeadEffect2;
    public GameObject strongDefenseEffect;
    public GameObject weakUpEffect;
    public GameObject flyingMoveEffect;
    public GameObject livingDeadStartEffect;
    public GameObject dawnDeathCountEffect;
    public GameObject sheercoldRandomDeathEffect;
    public StarParent dawnEffect;
    public WorldItem worldItem;
    public DissolveObject dissolveObject;
    public FallingGenerator[] earthQuakeEffect;
    public Sprite counterAttackSprite;
    public Sprite madnessSprite;
    public Sprite strongDefenseSprite;
    public Sprite thunderManSprite;
    public Sprite poisonBarriorSprite;
    public Sprite surpriseAttackSprite;
    public Sprite destinyBodSprite;
    public Sprite destinyOfTheTeamSprite;
    public Sprite noblesseObligeSprite;
    public Sprite elementalKingsSprite;
    public Sprite lockonSprite;
    public Material suctionMaterial;
    public Material holyMaterial;
    public Material shaodwMaterial;
    public SpriteMask spriteMask;
    public FlowingText flowingText;
    public FlowingText flowingText2;
    public FlowingText elemantalText;
    public FlowingText countText;
    public SkillData fireBracesSkillData;
    public SkillData strongDefenseSkillData;
    public SkillData sandHideData;
    public SkillData mudCast;
    public SkillData thunderMan;
    public SkillData headBut;
    public SkillData poisonPoolData;
    public SkillData surpriseAttackSkillData;
    public SkillData shadowCrewSkillData;
    public SkillData formChange4SkillData;
    public SkillData holyRecoverySkillData;
    public SkillData divinePowerSkillData;
    public SkillData madnessSkillData;
    public SkillData drivenSkillData;
    public SkillData fireBallData;
    public SkillData fireBracesData;
    public SkillData destinyBondSkillData;
    public SkillData poisonBarriorData;
    public SkillData electronicNetData;
    public SkillData sheercoldData;

    [System.NonSerialized]
    public List<FallingObject> rockClones = new List<FallingObject>();

    private Coroutine currentRoutine = null;
    private static SkillManager instance = null;
    public static SkillManager Instance { get { return instance; } }

    [System.NonSerialized]
    public int deadCount = 0;

    [System.NonSerialized]
    public bool isBlock;

    [System.NonSerialized]
    public bool isBounceBlock;

    [System.NonSerialized]
    public List<Animator> bounceDestroyObjs;

    [System.NonSerialized]
    public List<EntityMonster> supriseAttackList = new List<EntityMonster>();

    [System.NonSerialized]
    public List<EntityMonster> shadowCrewList = new List<EntityMonster>();

    [System.NonSerialized]
    public List<EasterStructure> easterList = new List<EasterStructure>();

    [System.NonSerialized]
    public List<DarkHoleStructure> darkHoleList = new List<DarkHoleStructure>();

    [System.NonSerialized]
    public int homegroundDawnCount = 0;

    [System.NonSerialized]
    public int awayDawnCount = 0;

    [System.NonSerialized]
    public float dawnTimer = 0;

    [System.NonSerialized]
    public bool arrowSoundBlock = false;

    [System.NonSerialized]
    public EntityMonster mindControlReturnTarget = null;

    //충돌된 다크홀
    [System.NonSerialized]
    public Wall collisionDarkHole;

    [System.NonSerialized]
    public StopStructure stopStructure;

    [System.NonSerialized]
    public WillPowerStructure willPowerStructure;

    [System.NonSerialized]
    public EntityMonster homegrounsNoblesseOblige;

    [System.NonSerialized]
    public EntityMonster awayNoblessOblige;

    [System.NonSerialized]
    public bool checkSheercoldHomeground = false;

    [System.NonSerialized]
    public int sheerColdCount = 0;

    [System.NonSerialized]
    public float sheerColdTime = 0;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        stopStructure = new StopStructure();
        stopStructure.isBlock = false;
        willPowerStructure = new WillPowerStructure();
        willPowerStructure.Init();

        bounceDestroyObjs = new List<Animator>();
    }
    public void Play(SkillName name, BattleStructure structure)
    {
        if (structure.player.battleInstance.mp >= structure.skillData.consumMpAmount)
            currentRoutine = StartCoroutine(name.ToString(), structure);
        else
            currentRoutine = StartCoroutine(NoManaRoutine(structure));
    }
    public void PlayPassive(PassiveName name, EntityMonster player)
    {
        currentRoutine = StartCoroutine(name.ToString(), player);
    }

    public void PlayWillPower()
    {
        StartCoroutine(WillPower());
    }

    public void PlayWorldItem()
    {
        if(isBlock == false)
            StartCoroutine(WorldItemRoutine());
    }

    public void PlayBounce()
    {
        StartCoroutine(Bounce());
    }

    public void PlayDotDmgTurn()
    {
        StartCoroutine(DotDmgTurn());
    }

    public void PlayEasterTurn(List<EasterStructure> easters)
    {
        if(isBlock == false)
            StartCoroutine(EasterRoutine(easters));
    }

    public void PlayDawnTurn(bool isHomeground)
    {
        if(isBlock == false)
            StartCoroutine(DawnTurn(isHomeground));
    }

    public void PlayLivingDeadTurn(List<EntityMonster> livingDeadTargets)
    {
        if(isBlock == false)
            StartCoroutine(LivingDeadRoutine(livingDeadTargets));
    }

    public void PlayDarkHoleTurn(DarkHoleStructure darkHole)
    {
        if(isBlock == false)
            StartCoroutine(DarkHoleTurn(darkHole));
    }

    public void SheerColdTurn()
    {
        if(isBlock == false)
            StartCoroutine(SheerColdTurnRoutine());
    }

    public void Stop(WallName wallName)
    {
        StartCoroutine(wallName.ToString());
    }
    private IEnumerator HeadButt(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);

        if (dmgCheck == true)
        {
            var effectClone = Instantiate(nearDistanceHitEffect);
            effectClone.transform.position = target.center.position;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);

            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
        }
        yield return new WaitForSeconds(0.25f);

        if (dmgCheck == true && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }

            }
        }

        isBlock = false;
    }
    private IEnumerator Rush(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        bool suctionDmgCheck = false;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        targets = targets.ToList();
        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        stopStructure.speicalBackground = anim;
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;
        Vector2 endPosition2 = structure.player.transform.position;

        bool elecFlag = false, poisonFlag = false, burnFlag = false;
        float addLerp = 1f;
        float addTime = 0.1f;
        BoxCollider2D boxCol = structure.player.GetComponent<BoxCollider2D>();

        SoundManager.Instance.PlayEffect(81, 1f);
        var effect = Instantiate(rushTrail, structure.player.center);
        effect.transform.localPosition = Vector2.zero;
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;
            //반격기 확인
            targets[i].CheckDrivenSkill(structure.player);
            //if (i == 0)
            //{
            //    var cols = Physics2D.OverlapBoxAll((Vector2)structure.player.transform.position + boxCol.offset, boxCol.size, 0f);
            //    for (int j = 0; j < cols.Length; ++j)
            //    {
            //        var wall = cols[j].GetComponent<Wall>();
            //        if (wall != null)
            //            wall.Remove();
            //    }
            //}
            lerpSpeed = Mathf.Min(6f, lerpSpeed + addLerp);
            currentTime = 0f;
            lerpTime = 1f;
            Vector2 startPosition3 = structure.player.transform.position;
            Vector2 endPosition3 = targets[i].transform.position;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(startPosition3, endPosition3, currentSpeed);
                yield return null;
            }


            bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                suctionDmgCheck = true;
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                    targets[i].FlyingDead();
                else
                {
                    Instantiate(nearDistanceHitEffect, targets[i].center.position, Quaternion.identity);
                    targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
                }
            }

            yield return new WaitForSeconds(0.25f - addTime);

            addTime = Mathf.Min(0.25f, addTime + 0.1f);


            //반격기확인
            if (targets[i] != null && targets[i].isDead == false && targets[i].passiveFlag == true)
            {
                if (targets[i].currentPassiveName == PassiveName.ThunderMan)
                    elecFlag = true;
                else if (targets[i].currentPassiveName == PassiveName.PoisonBarrior)
                    poisonFlag = true;
                else if (targets[i].currentPassiveName == PassiveName.FireBraces)
                    burnFlag = true;

            }
        }


        Vector2 startPosition2 = structure.player.transform.position;

        float lerpSpeed2 = 4f;
        float currentTime2 = 0f;
        float lerpTime2 = 1f;

        while (currentTime2 < lerpTime2)
        {
            currentTime2 += Time.deltaTime * lerpSpeed2;

            float currentSpeed = currentTime2 / lerpTime2;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition2, endPosition2, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        Destroy(effect);
        float endValue = structure.player.battleInstance.hp - structure.skillData.buffValue;
        if (endValue > 0f)
        {
            StartCoroutine(structure.player.RecoverRoutine(structure.player.battleInstance.hp, endValue));
            yield return new WaitUntil(() => (structure.player == null) || (structure.player != null && structure.player.isDead == false) || (structure.player != null && structure.player.startRecovery == false));
        }
        else
            structure.player.Hurt(structure.skillData.buffValue);

        yield return waitTime;
        if (structure.player != null && structure.player.isDead == false)
        {
            if (suctionDmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
                yield return StartCoroutine(SuctionRoutine(structure.player));
        }

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        if (structure.player != null && structure.player.isDead == false)
        {
            //반격기확인
            if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
            {
                //타겟가져오기
                targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
                if (target != null)
                    structure.player.CheckFireBaracesSkill(target);
            }
            //반격기확인
            if (elecFlag == true)
            {
                float value = Random.Range(0f, 1f);
                if (value <= thunderMan.statusRatio)
                {
                    var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                    yield return new WaitUntil(() => clone == null);
                    yield return new WaitForSeconds(0.25f);
                }
            }
            if (poisonFlag == true)
            {
                float value = Random.Range(0f, 1f);
                if (value <= poisonBarriorData.statusRatio)
                {
                    var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                    if (clone != null)
                    {
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
            }
            if (burnFlag == true)
            {
                float value = Random.Range(0f, 1f);
                if (value <= fireBracesData.statusRatio)
                {
                    var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                    if (clone != null)
                    {
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
            }
        }

        //필수
        isBlock = false;
    }
    private IEnumerator Pogrom(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        Vector2 offset = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? new Vector2(-CombatManager.Instance.spawnDistance, 0f) : new Vector2(CombatManager.Instance.spawnDistance, 0f);
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = ((Vector2)target.transform.localPosition + offset) - (Vector2)target.battleInstance.monsterData.monsterPrefab.transform.localPosition + (Vector2)structure.player.battleInstance.monsterData.monsterPrefab.transform.localPosition;
       // Vector2 endPosition = new Vector2(target.transform.localPosition.x + offset.x, structure.player.transform.localPosition.y);

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        Vector2 startPosition2 = obj.localPosition;
        Vector2 endPosition2 = target.transform.localPosition - target.battleInstance.monsterData.monsterPrefab.transform.localPosition + structure.player.battleInstance.monsterData.monsterPrefab.transform.localPosition;
        //Vector2 endPosition2 = new Vector2(target.transform.localPosition.x, structure.player.transform.localPosition.y);
        lerpSpeed = 6f;
        currentTime = 0f;
        lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition2, endPosition2, currentSpeed);
            yield return null;
        }

        var effectClone = Instantiate(pogromHitEffect);
        effectClone.transform.position = target.center.position;

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition2, startPosition2, currentSpeed);
            yield return null;
        }

        yield return new WaitUntil(() => effectClone == null);
        yield return waitTime;

        //bool check = false;
        //var teams = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        //for(int i = 0; i < teams.Count; ++i)
        //{
        //    if (teams[i].battleInstance.hp < teams[i].battleInstance.maxHp)
        //    {
        //        check = true;
        //        break;
        //    }    
        //}
        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            if ((CombatManager.Instance.battleTimer >= CombatManager.Instance.battleMaxTimer - 7))
            {
                target.Hurt(target.battleInstance.hp, structure.player);
            }
            else
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
        }

        yield return waitTime;

        lerpSpeed = 4f;
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        if (dmgCheck == true && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }

            }
        }
        isBlock = false;
    }
    private IEnumerator EnergyBall(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;
        Vector2 offset = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? new Vector2(-0.5f, 0f) : new Vector2(0.5f, 0f);
        Vector2 startPosition = structure.player.transform.position;
        Vector2 endPosition = (Vector2)structure.player.transform.position + offset;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        lerpSpeed = 8f;
        currentTime = 0f;
        yield return new WaitForSeconds(0.5f);

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.position = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        var bullet = Instantiate(energyBall, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);


        float bulletSpeed = 3f;
        float addSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                yield return new WaitUntil(() => anim == null);
                yield return waitTime;
                target.Hurt(structure.player.battleInstance.atk, structure.player, structure.skillData);
            }
            else
                Destroy(bullet);
        }

        yield return waitTime;

        var effect = Instantiate(debuffAttackEffect, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        structure.player.battleInstance.atk = Mathf.Max(0, structure.player.battleInstance.atk - structure.skillData.buffValue);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator ComboAttack(BattleStructure structure)
    {
        //필수
        isBlock = true;
        bool suctionDmgCheck = false;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        var targets = new List<EntityMonster>();
        int random = Random.Range(2, 7);
        for (int i = 0; i < random; ++i)
        {
            var sortingTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
            targets.Add(CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, sortingTargets));
        }


        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                break;
            if (targets[i].isDead == true)
                break;
            Vector2 endPosition = targets[i].transform.localPosition;

            float lerpSpeed = 6f;
            float currentTime = 0f;
            float lerpTime = 1f;

            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                yield return null;
            }

            bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                var effectClone = Instantiate(nearDistanceHitEffect);
                effectClone.transform.position = targets[i].center.position;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
                yield return null;
            }

            //필수(데미지계산)
            if (dmgCheck)
            {
                suctionDmgCheck = true;
                int randomValue = Random.Range(0, 6);
                float value = 1f;
                if (randomValue == 0)
                    value = 0.1f;
                else if (randomValue == 1)
                    value = 0.25f;
                else if (randomValue == 2)
                    value = 0.5f;
                else if (randomValue == 3)
                    value = 0.75f;
                else if (randomValue == 4)
                    value = 1f;
                else if (randomValue == 5)
                    value = 1.5f;
                value = value * (structure.player.battleInstance.atk + structure.skillData.atk);
                targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), value - targets[i].battleInstance.def), structure.player, structure.skillData);
                if (targets[i] != null && targets[i].isDead == false)
                    targets[i].CheckSandHideSkill(targets[i]);
            }

            yield return new WaitForSeconds(0.25f);

            //반격기 확인
            bool check = targets[i].CheckDrivenSkill(structure.player);
            if (check == true)
                break;
            if (check == true)
                break;
            //반격기확인
            if (targets[i] != null && targets[i].isDead == false)
            {
                if (targets[i].passiveFlag == true)
                {
                    if (targets[i].currentPassiveName == PassiveName.ThunderMan)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= thunderMan.statusRatio)
                        {
                            var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                    else if (targets[i].currentPassiveName == PassiveName.PoisonBarrior)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= poisonBarriorData.statusRatio)
                        {
                            var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                            if (clone != null)
                            {
                                yield return new WaitUntil(() => clone == null);
                                yield return new WaitForSeconds(0.25f);
                            }
                        }
                    }
                    else if (targets[i].currentPassiveName == PassiveName.FireBraces)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= fireBracesData.statusRatio)
                        {
                            var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                            if (clone != null)
                            {
                                yield return new WaitUntil(() => clone == null);
                                yield return new WaitForSeconds(0.25f);
                            }
                        }
                    }
                }
            }
        }

        if (suctionDmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator Slash(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //연출
        Vector2 offset = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? new Vector2(-CombatManager.Instance.spawnDistance, 0f) : new Vector2(CombatManager.Instance.spawnDistance, 0f);
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = ((Vector2)target.transform.localPosition + offset) - (Vector2)target.battleInstance.monsterData.monsterPrefab.transform.localPosition + (Vector2)structure.player.battleInstance.monsterData.monsterPrefab.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        var anim = structure.player.GetComponent<Animator>();

        SoundManager.Instance.PlayEffect(82, 1f);
        anim.Play("skill");
        yield return waitTime;
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle"));

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);

        if (dmgCheck == true)
        {
            var effect = Instantiate(nearDistanceHitEffect, target.center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            float value = Random.Range(0f, 1f);

            if (value <= structure.skillData.statusRatio)
                target.Hurt(target.battleInstance.hp, structure.player);
            else
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);

            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
        }

        yield return waitTime;

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        if (dmgCheck == true && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator FireBall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(fireBallEffect, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }

        float bulletSpeed = 3f;
        float addSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);
                //화상상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                    {
                        var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator FireStrike(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);
        targets.Reverse();

        //연출

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = CombatManager.Instance.trapPositionDic[index];

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;
        Vector3 upPositionStart = structure.player.transform.localPosition;
        Vector3 upPositionEnd = upPositionStart + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;



        int random = Random.Range(2, 7);

        for (int i = 0; i < random; ++i)
        {
            var checkTargets = targets.FindAll(x => x == null || (x != null && x.isDead == true));
            if (checkTargets.Count == targets.Count)
                goto jump;

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(upPositionStart, upPositionEnd, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(upPositionEnd, upPositionStart, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            // yield return waitTime;

            Vector3 offset = new Vector3(0f, -0.6f);
            Vector3 position = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.GetWorldPosition(new Vector3(1f, targets[0].height - 1)) : CombatManager.Instance.GetWorldPosition(new Vector3(-3f, targets[0].height - 1));
            position += offset;
            var effect = Instantiate(fireWide, position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            //   yield return waitTime;

            for (int j = 0; j < targets.Count; ++j)
            {
                if (targets[j] == null)
                    continue;
                if (targets[j].isDead == true)
                    continue;
                bool dmgCheck = targets[j].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck == true)
                {
                    targets[j].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                    targets[j].sumStatusRatio += structure.skillData.statusRatio;
                }
            }

            yield return waitTime;
        }

        for (int a = 0; a < targets.Count; ++a)
        {
            if (targets[a] == null)
                continue;
            if (targets[a].isDead == true)
                continue;
            //화상상태
            if (targets[a].sumStatusRatio > 0f && targets[a] != null && targets[a].isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= targets[a].sumStatusRatio)
                {
                    var clone = targets[a].dogDmgStatus.AddBurnFlag(targets[a], burnEffect);
                    if (clone != null)
                    {
                        yield return new WaitUntil(() => clone == null);
                        yield return waitTime;
                    }
                }
            }
        }

    jump:

        for (int a = 0; a < targets.Count; ++a)
            targets[a].sumStatusRatio = 0f;
        yield return waitTime;

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator FireBlade(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        Vector2 effectOffsetPosition = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? new Vector2(-1f, 0f) : new Vector2(1f, 0f);
        var effect = Instantiate(fireBrade, (Vector2)target.center.position + effectOffsetPosition, Quaternion.identity);
        effect.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? effect.transform.localScale.x : -1 * effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z);

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return new WaitUntil(() => effect == null);

        //필수(데미지계산)
        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            //화상상태
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    yield return new WaitForSeconds(0.25f);

                    var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                    if (clone != null)
                    {
                        yield return new WaitUntil(() => clone == null);
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator FireViper(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        var effect = Instantiate(fireViper, target.center.position, Quaternion.identity);

        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            //화상상태
            var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
            if (clone != null)
            {
                yield return new WaitUntil(() => clone == null);
                yield return new WaitForSeconds(0.25f);
            }

            //독상태
            clone = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);
            if (clone != null)
            {
                yield return new WaitUntil(() => clone == null);
                yield return new WaitForSeconds(0.25f);
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator Doublebarrel(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);


        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);
        targets.Reverse();

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        Vector3 position = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? targets[0].transform.position + fireSnap.transform.localPosition : targets[0].transform.position + new Vector3(-1 * fireSnap.transform.localPosition.x, fireSnap.transform.localPosition.y);
        var bullet = Instantiate(fireSnap, position, Quaternion.identity).GetComponent<Animator>();
        bullet.transform.localScale = new Vector3(CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x), bullet.transform.localScale.y, bullet.transform.localScale.z);

        yield return new WaitUntil(() => bullet == null);
        yield return new WaitForSeconds(0.25f);

        //필수(데미지계산)
        bool dmgCheck = targets[0].DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
            targets[0].Hurt(Mathf.Max(targets[0].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[0].battleInstance.def), structure.player, structure.skillData);


        targets = targets.FindAll(x => (x != null && x.isDead == false));
        if (targets.Count > 0)
        {
            yield return new WaitForSeconds(0.5f);

            Vector3 offset = new Vector3(0f, -0.6f);
            position = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.GetWorldPosition(new Vector3(1f, targets[0].height - 1)) : CombatManager.Instance.GetWorldPosition(new Vector3(-3f, targets[0].height - 1));
            position += offset;
            var effect = Instantiate(fireWide, position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < targets.Count; ++i)
            {
                if (targets[i] == null)
                    continue;
                if (targets[i].isDead == true)
                    continue;
                targets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
            }

            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < targets.Count; ++i)
            {
                //화상상태
                if (targets[i] != null && targets[i].isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                    {
                        var clone = targets[i].dogDmgStatus.AddBurnFlag(targets[i], burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator Meteor(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(meteorEffect);
        var originTargets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        yield return StartCoroutine(effect.SpawnMeteoRoutine(!CombatManager.Instance.homegroundMonsters.Contains(structure.player), originTargets, structure.player, structure.skillData));
        yield return waitTime;

        Destroy(effect.gameObject);


        var statusTargets = originTargets.FindAll(x => (x != null && x.isDead == false) && x.sumStatusRatio > 0f);
        GameObject clone = null;
        for (int i = 0; i < statusTargets.Count; ++i)
        {
            float value = Random.Range(0f, 1f);
            if (value <= statusTargets[i].sumStatusRatio)
            {
                clone = statusTargets[i].dogDmgStatus.AddBurnFlag(statusTargets[i], burnEffect);
            }

            statusTargets[i].sumStatusRatio = 0f;
        }

        if (clone != null)
        {
            yield return new WaitUntil(() => clone == null);
            yield return waitTime;
        }


        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator DynamicPunch(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
        int count = 0;
        var dialogTargets = CombatManager.Instance.GetNeighbourTarget(target, out count);

        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);

        if (dmgCheck == true)
        {
            var effectClone = Instantiate(dynamicPunchEffect);
            effectClone.transform.position = target.center.position;
            yield return new WaitUntil(() => effectClone == null);
            yield return waitTime;
        }
        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
            //화상상태
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    yield return waitTime;
                    var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                    if (clone != null)
                    {
                        yield return new WaitUntil(() => clone == null);
                    }
                }
            }
        }

        if (dmgCheck == true)
        {
            if (count != 0)
            {
                yield return waitTime;

                List<GameObject> effects = new List<GameObject>();
                for (int i = 0; i < dialogTargets.Length; ++i)
                {
                    if (dialogTargets[i] == null)
                        continue;
                    if (dialogTargets[i].isDead == true)
                        continue;

                    var effect = Instantiate(singleFireWide, dialogTargets[i].transform.position, Quaternion.identity);
                    effects.Add(effect);
                }

                for (int i = 0; i < effects.Count; ++i)
                {
                    yield return new WaitUntil(() => effects[i] == null);
                }

                yield return waitTime;

                for (int i = 0; i < dialogTargets.Length; ++i)
                {
                    if (dialogTargets[i] == null)
                        continue;

                    dialogTargets[i].Hurt(Mathf.Max(target.GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) * structure.skillData.buffValue) - dialogTargets[i].battleInstance.def), structure.player, structure.skillData);
                    //화상상태
                    if (target != null && target.isDead == false)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                        {
                            yield return waitTime;
                            var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                            if (clone != null)
                            {
                                yield return new WaitUntil(() => clone == null);
                            }
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (dmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator FireBomb(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
        int count = 0;
        var dialogueTargets = CombatManager.Instance.GetNeighbourTarget(target, out count);

        //연출
        var bullet = Instantiate(fireBombEffect, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }

        float bulletSpeed = 3f;
        float addSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                Destroy(bullet);
                var hitEffect = Instantiate(fireBombHitEffect, target.transform.position, Quaternion.identity);
                yield return new WaitUntil(() => hitEffect == null);
                yield return waitTime;

                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return waitTime;
                //화상상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                    {
                        var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return waitTime;
                        }
                    }
                }

                if (count > 0)
                {
                    yield return waitTime;

                    List<GameObject> effects = new List<GameObject>();
                    for (int i = 0; i < dialogueTargets.Length; ++i)
                    {
                        if (dialogueTargets[i] == null)
                            continue;
                        if (dialogueTargets[i].isDead == true)
                            continue;

                        var effect = Instantiate(singleFireWide, dialogueTargets[i].transform.position, Quaternion.identity);
                        effects.Add(effect);
                    }

                    for (int i = 0; i < effects.Count; ++i)
                    {
                        yield return new WaitUntil(() => effects[i] == null);
                    }

                    yield return waitTime;

                    for (int i = 0; i < dialogueTargets.Length; ++i)
                    {
                        if (dialogueTargets[i] == null)
                            continue;
                        if (dialogueTargets[i].isDead == true)
                            continue;

                        dialogueTargets[i].Hurt(Mathf.Max(target.GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) * structure.skillData.buffValue) - dialogueTargets[i].battleInstance.def), structure.player, structure.skillData);
                        //화상상태
                        if (target != null && target.isDead == false)
                        {
                            float value = Random.Range(0f, 1f);
                            if (value <= structure.skillData.statusRatio)
                            {
                                yield return waitTime;
                                var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                                if (clone != null)
                                {
                                    yield return new WaitUntil(() => clone == null);
                                }
                            }
                        }
                    }
                }
            }
            else
                Destroy(bullet);
        }

        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator FireVow(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        structure.player.fireVowStack--;
        if (structure.player.fireVowStack <= 0)
        {
            structure.player.battleInstance.skillDatas.RemoveAll(x => x == structure.skillData);
            structure.player.battleInstance.percentSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
            structure.player.battleInstance.triggerSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
            if (structure.player.battleInstance.skillDatas.Count <= 0)
                structure.player.battleInstance.skillDatas.Add(fireBallData);

        }

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        //targets = targets.FindAll(x => x.battleInstance.hp <= (x.battleInstance.maxHp * 0.33f));

        //연출
        GameObject effect = null;
        for (int i = 0; i < targets.Count; ++i)
        {
            effect = Instantiate(fireBuffEffect, targets[i].center.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            effect = Instantiate(buffAttackEffect, targets[i].center.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            effect = Instantiate(buffSpeedEffect, targets[i].center.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].battleInstance.atk += Mathf.RoundToInt(targets[i].battleInstance.atk * structure.skillData.buffValue);

            targets[i].battleInstance.maxDex = Mathf.Max(0.1f, targets[i].battleInstance.maxDex - (targets[i].battleInstance.maxDex * structure.skillData.buffValue));
            targets[i].battleInstance.dex = Mathf.Max(0f, targets[i].battleInstance.dex - (targets[i].battleInstance.dex * structure.skillData.buffValue));

            CombatManager.Instance.battleQueue.RemoveAll(x => x == targets[i]);
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator HellFire(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;
        bool isHomeground = (CombatManager.Instance.homegroundMonsters.Contains(structure.player));
        var targets = isHomeground ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);

        //연출

        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");
        stopStructure.speicalBackground = anim;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = CombatManager.Instance.trapPositionDic[index];

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;
        yield return StartCoroutine(SetCamPosition(0.25f, endPosition));

        Vector3 offset = isHomeground ? new Vector3(CombatManager.Instance.spawnDistance, 0.4f) : new Vector3(-1 * CombatManager.Instance.spawnDistance, 0.4f);
        var effect = Instantiate(hellFireEffect, structure.player.transform.position + offset, Quaternion.identity);
        effect.transform.localScale = isHomeground ? effect.transform.localScale : new Vector3(effect.transform.localScale.x * -1f, effect.transform.localScale.y, effect.transform.localScale.z);

      
      
        StartCoroutine(ZoomInOut(4.1f, Camera.main.orthographicSize * 0.75f, false));
        yield return new WaitForSeconds(4.1f);

        for (int i = 0; i < targets.Count; ++i)
        {
            var dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                Instantiate(nearDistanceHitEffect, targets[i].center.position, Quaternion.identity);
                targets[i].Hurt(structure.player.battleInstance.atk + structure.skillData.atk, structure.player, structure.skillData);
                targets[i].startRecovery = true;
            }
        }
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;
            if (targets[i].startRecovery == false)
                continue;


            float value = Random.Range(0f, 1f);
            if (value <= structure.skillData.statusRatio)
            {
                effect = targets[i].dogDmgStatus.AddBurnFlag(targets[i], burnEffect);
                targets[i].startRecovery = false;
            }
        }
        if (effect != null)
        {
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }

        yield return StartCoroutine(SetCamPosition(0.25f, Vector2.zero));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator IceBall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(iceBallEffect, structure.player.bulletCreatePoint.position, Quaternion.identity);
        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }

        float bulletSpeed = 5f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);
                //얼음상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                        target.sternStatus.Start(target, iceCrystalEffect, target.center, 10f);
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator IceBreak(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);

        //연출
        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = CombatManager.Instance.trapPositionDic[index];

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        yield return waitTime;

        List<Animator> anims = new List<Animator>();
        Vector2 endPosition2 = endPosition + new Vector3(0f, 0.2f);

        targets.Reverse();
        for (int i = 0; i < targets.Count; ++i)
        {
            currentTime = 0f;
            Transform shaodw = structure.player.transform.GetChild(4);
            Transform cavas = structure.player.transform.GetChild(0);
            Vector3 shadowOriginPosition = shaodw.position;
            Vector3 cavasPosition = cavas.position;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition, endPosition2, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition2, endPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            targets[i].startRecovery = (targets[i].sternStatus.Check() == true && targets[i].sternStatus.prefab == iceCrystalEffect) ? true : false;
            if (targets[i].sternStatus.Check() == true && targets[i].sternStatus.prefab == iceCrystalEffect)
                targets[i].sternStatus.Stop(targets[i]);

            var effect = Instantiate(iceCrystalEffect, targets[i].center.position, Quaternion.identity);
            anims.Add(effect.GetComponent<Animator>());
            yield return waitTime;
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < anims.Count; ++i)
        {
            anims[i].Play("exit");
        }
        for (int i = 0; i < anims.Count; ++i)
        {
            yield return new WaitUntil(() => anims[i] == null);
        }
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].startRecovery == true)
            {
                targets[i].CriticalHurt(structure.skillData.atk, structure.player, structure.skillData);
            }
            else
            {
                bool check = targets[i].DmgCheck(structure.player, structure.skillData);
                if (check)
                    targets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
            }

            targets[i].startRecovery = false;


        }

        yield return waitTime;

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator IceSpear(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = new List<EntityMonster>();
        int random = Random.Range(2, 5);
        for (int i = 0; i < random; ++i)
        {
            var sortingTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
            targets.Add(CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, sortingTargets));
        }

        //연출
        Animator[] anims = new Animator[targets.Count];

        Vector3 previousPosition = structure.player.bulletCreatePoint.position;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        for (int i = 0; i < targets.Count; ++i)
        {
            var bullet = Instantiate(iceSpearEffect, previousPosition, Quaternion.identity);
            float monScale = structure.player.bulletCreatePoint.localScale.x;
            bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
            anims[i] = bullet.GetComponent<Animator>();

            float dis = Random.Range(0.25f, 0.5f);
            Vector2 insidePosition = new Vector2(0f, dis) * monScale;
            previousPosition = (Vector2)previousPosition + insidePosition;

            yield return waitTime;
        }

        bounceDestroyObjs.Clear();
        bounceDestroyObjs.AddRange(anims);

        for (int i = 0; i < anims.Length; ++i)
        {
            yield return new WaitUntil(() => anims[i].GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        }

        for (int i = 0; i < anims.Length; ++i)
        {
            if (targets[i] == null)
            {
                var newTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), TargetLayer.Middle);
                if (newTargets.Count <= 0)
                {
                    for (int j = 0; j < anims.Length; ++j)
                    {
                        if (anims[j] == null)
                            continue;
                        Destroy(anims[j].gameObject);
                    }
                    isBlock = false;
                    yield break;
                }
                else
                    targets[i] = newTargets[Random.Range(0, newTargets.Count)];
            }
            if (targets[i] != null && targets[i].isDead == true)
            {
                var newTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), TargetLayer.Middle);
                if (newTargets.Count <= 0)
                {
                    for (int j = 0; j < anims.Length; ++j)
                    {
                        if (anims[j] == null)
                            continue;
                        Destroy(anims[j].gameObject);
                    }
                    isBlock = false;
                    yield break;
                }
                else
                    targets[i] = newTargets[Random.Range(0, newTargets.Count)];
            }

            //멈춤데이터 초기화
            stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            stopStructure.bullet = anims[i];
            stopStructure.stopObj = null;

            anims[i].Play("stay");

            float bulletSpeed = 10f;
            float m = (targets[i].center.position - anims[i].transform.position).magnitude;
            var dir = (targets[i].center.position - anims[i].transform.position).normalized;

            float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
            anims[i].transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
            while (m > bulletSpeed * Time.deltaTime)
            {
                if (stopStructure.dontMove == true)
                    goto jump;
                m = (targets[i].center.position - anims[i].transform.position).magnitude;
                anims[i].transform.position += dir * bulletSpeed * Time.deltaTime;
                yield return null;
            }
        jump:
            //필수(데미지계산)
            if (stopStructure.dontMove == false)
            {
                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck == true)
                {
                    anims[i].Play("exit");
                    targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);

                    //얼음상태
                    if (targets[i] != null && targets[i].isDead == false)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                            targets[i].sternStatus.Start(targets[i], iceCrystalEffect, targets[i].center, 10f);
                    }
                }
                else
                    Destroy(anims[i].gameObject);
            }

            yield return new WaitForSeconds(0.1f);
            stopStructure.dontMove = false;
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator IceWall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //타겟설정
        int index = CombatManager.Instance.GetTrapTarget(structure.player);
        Wall outWall = null;
        Animator effect = null;
        CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
        if (outWall == null)
        {
            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(iceWall, randomPosition - new Vector2(0f, 0.48f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic.Add(index, wall);
        }
        else
        {
            Destroy(outWall.gameObject);

            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(iceWall, randomPosition - new Vector2(0f, 0.48f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic[index] = wall;
        }
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator IceTime(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟설정
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //얼음상태
        target.sternStatus.Start(target, iceCrystalEffect, target.center, 10f);

        yield return new WaitForSeconds(0.5f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator IceTrap(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //타겟설정
        int index = CombatManager.Instance.GetTrapTarget(structure.player);
        Wall outWall = null;
        Animator effect = null;
        CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
        if (outWall == null)
        {
            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(iceTrap, randomPosition - new Vector2(0f, 0.36f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic.Add(index, wall);
        }
        else
        {
            Destroy(outWall.gameObject);

            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(iceTrap, randomPosition - new Vector2(0f, 0.36f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic[index] = wall;
        }
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator IceBurst(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //타겟설정
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;
        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        var bullet = Instantiate(iceBurstEffect, target.center.position + new Vector3(0f, 0.25f), Quaternion.identity);

        yield return new WaitUntil(() => bullet == null);
        yield return waitTime;

        //딜계산
        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            SoundManager.Instance.PlayEffect(79, 1f);
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            //얼음상태
            if (target != null && target.isDead == false)
            {
                yield return waitTime;
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                    target.sternStatus.Start(target, iceCrystalEffect, target.center, 10f);

                if (!target.sternStatus.Check())
                {
                    value = Mathf.Max(0, target.battleInstance.dex - (target.battleInstance.maxDex * structure.skillData.buffValue));
                    if (CombatManager.Instance.battleQueue.Contains(target))
                        CombatManager.Instance.battleQueue.RemoveAll(x => x == target);

                    yield return StartCoroutine(target.DexRoutine(target.battleInstance.dex, value, 1f));
                }
            }
        }
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator IceStorm(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //타겟설정
        List<EntityMonster> targets = null;

        if (CombatManager.Instance.homegroundMonsters.Contains(structure.player))
            targets = CombatManager.Instance.awayMonsters.ToList();
        else
            targets = CombatManager.Instance.homegroundMonsters.ToList();

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        iceStormEffect.gameObject.SetActive(true);
        SoundManager.Instance.PlayEffect(15, 1f);
        SoundManager.Instance.PlayEffect(15, 1f);
        SoundManager.Instance.PlayEffect(15, 1f);
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;
            var effect = Instantiate(nearDistanceHitEffect, targets[i].center.position, Quaternion.identity);
            targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
            yield return new WaitUntil(() => effect == null);

            //얼음상태
            if (targets[i] != null && targets[i].isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                    targets[i].sternStatus.Start(targets[i], iceCrystalEffect, targets[i].center, 10f);
            }

            yield return waitTime;
        }

        yield return waitTime;

        //타겟설정
        Wall outWall = null;
        Animator effectAnim = null;
        for (int index = 0; index < 3; ++index)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
                continue;
            CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
            if (outWall == null)
            {
                var randomPosition = CombatManager.Instance.trapPositionDic[index];
                effectAnim = Instantiate(iceWall, randomPosition - new Vector2(0f, 0.48f), Quaternion.identity).GetComponent<Animator>();

                var wall = effectAnim.GetComponent<Wall>();
                wall.index = index;
                wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
                CombatManager.Instance.trapDic.Add(index, wall);
            }
            else
            {
                Destroy(outWall.gameObject);

                var randomPosition = CombatManager.Instance.trapPositionDic[index];
                effectAnim = Instantiate(iceWall, randomPosition - new Vector2(0f, 0.48f), Quaternion.identity).GetComponent<Animator>();

                var wall = effectAnim.GetComponent<Wall>();
                wall.index = index;
                wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
                CombatManager.Instance.trapDic[index] = wall;
            }
        }

        if (effectAnim != null)
        {
            yield return new WaitUntil(() => effectAnim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
            yield return waitTime;
        }

        iceStormEffect.gameObject.SetActive(false);
        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator SheerCold(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        if(!isHomeground != checkSheercoldHomeground || sheerColdCount <= 0)
        {
            checkSheercoldHomeground = !isHomeground;
            sheerColdCount = 2;
            sheerColdTime = 12f;
        }
        //타겟설정
        List<EntityMonster> targets = null;

        if (isHomeground)
            targets = CombatManager.Instance.awayMonsters.ToList();
        else
            targets = CombatManager.Instance.homegroundMonsters.ToList();

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var position_0 = CombatManager.Instance.GetPosition(!isHomeground, 0, 2);
        var position_1 = CombatManager.Instance.GetPosition(!isHomeground, 1, 1);
        var position_2 = CombatManager.Instance.GetPosition(!isHomeground, 2, 0);

        SheerCold sheerCold_0 = null, sheerCold_1 = null, sheerCold_2 = null, tmp = null;

        var findIndex = targets.FindIndex(x => x.height == 2);

        float xOffset = isHomeground ? 1 : - 1;
        if(findIndex != -1)
            sheerCold_0 = Instantiate(sheerColdPrefab, new Vector3(position_0.x, position_0.y), Quaternion.identity);

        findIndex = targets.FindIndex(x => x.height == 1);

        if (findIndex != -1)
            sheerCold_1 = Instantiate(sheerColdPrefab, new Vector3(position_0.x, position_1.y), Quaternion.identity);

        findIndex = targets.FindIndex(x => x.height == 0);

        if (findIndex != -1)
            sheerCold_2 = Instantiate(sheerColdPrefab, new Vector3(position_0.x, position_2.y), Quaternion.identity);

        if(sheerCold_0 != null)
        {
            tmp = sheerCold_0;
            StartCoroutine(sheerCold_0.SheercoldRoutine(position_0.x, position_2.x + xOffset, position_0.y));
        }

        if (sheerCold_1 != null)
        {
            tmp = sheerCold_1;
            StartCoroutine(sheerCold_1.SheercoldRoutine(position_0.x, position_2.x + xOffset, position_1.y));
        }

        if (sheerCold_2 != null)
        {
            tmp = sheerCold_1;
            StartCoroutine(sheerCold_2.SheercoldRoutine(position_0.x, position_2.x + xOffset, position_2.y));
        }
          
        yield return new WaitUntil(() => tmp.isRunning == false);

        for(int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].sternStatus.Check())
                continue;
            targets[i].GetComponent<Animator>().enabled = false;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].sternStatus.Check())
                continue;
            targets[i].GetComponent<Animator>().enabled = true;
        }

        if (sheerCold_0 != null)
        {
            tmp = sheerCold_0;
            StartCoroutine(sheerCold_0.SheercoldExitRoutine(structure, targets));
        }

        if (sheerCold_1 != null)
        {
            tmp = sheerCold_1;
            StartCoroutine(sheerCold_1.SheercoldExitRoutine(structure, targets));
        }

        if (sheerCold_2 != null)
        {
            tmp = sheerCold_1;
            StartCoroutine(sheerCold_2.SheercoldExitRoutine(structure, targets));
        }

        yield return new WaitUntil(() => tmp.isRunning == false);

        if (sheerCold_0 != null)
            Destroy(sheerCold_0.gameObject);

        if (sheerCold_1 != null)
            Destroy(sheerCold_1.gameObject);

        if (sheerCold_2 != null)
            Destroy(sheerCold_2.gameObject);
        yield return waitTime;

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;

    }
    private IEnumerator FormChange(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);

        //민첩초기화
        structure.player.battleInstance.dex = 0;



        //연출
        Animator backgroundAnim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        stopStructure.speicalBackground = backgroundAnim;
        backgroundAnim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => backgroundAnim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var clone = structure.player.SwapMonster(buffGolem, structure.skillData.consumMpAmount);

        float buffValue = Mathf.RoundToInt(structure.player.originInstance.maxHp * 0.25f);
        clone.battleInstance.maxHp += buffValue;
        clone.battleInstance.hp += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.maxMp * 0.25f);
        clone.battleInstance.maxMp += buffValue;
        clone.battleInstance.mp += buffValue;


        buffValue = Mathf.RoundToInt(structure.player.originInstance.atk * 0.25f);
        clone.battleInstance.atk += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.def * 0.25f);
        clone.battleInstance.def += buffValue;

        structure.player = clone;

        var anim = clone.GetComponent<Animator>();
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        yield return new WaitForSeconds(0.5f);


        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = CombatManager.Instance.trapPositionDic[index];

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);

            yield return null;
        }

        anim.Play("atk");
        yield return new WaitForSeconds(0.6f);

        Vector2 position = CombatManager.Instance.trapPositionDic[index] + new Vector2(0f, 1.125f);
        var effect = Instantiate(earthBreak, position, Quaternion.identity);
        effect.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale.x : -1 * Mathf.Abs(effect.transform.localScale.x)), effect.transform.localScale.y, effect.transform.localScale.z);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);


        for (int i = 0; i < targets.Count; ++i)
        {
            bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck)
            {
                targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
            }
        }

        yield return waitTime;
        anim.Play("stay");

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        backgroundAnim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => backgroundAnim == null);
        yield return waitTime;

        //필수
        isBlock = false;
    }
    private IEnumerator FormChange2(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        Animator backgroundAnim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        backgroundAnim.Play("SpecialAttack_Background");
        stopStructure.speicalBackground = backgroundAnim;

        yield return new WaitUntil(() => backgroundAnim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var clone = structure.player.SwapMonster(buffGolem2, structure.skillData.consumMpAmount);

        float buffValue = Mathf.RoundToInt(structure.player.originInstance.maxHp * 0.25f);
        clone.battleInstance.maxHp += buffValue;
        clone.battleInstance.hp += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.maxMp * 0.25f);
        clone.battleInstance.maxMp += buffValue;
        clone.battleInstance.mp += buffValue;


        buffValue = Mathf.RoundToInt(structure.player.originInstance.atk * 0.25f);
        clone.battleInstance.atk += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.def * 0.25f);
        clone.battleInstance.def += buffValue;

        structure.player = clone;

        var anim = clone.GetComponent<Animator>();
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        yield return new WaitForSeconds(0.5f);


        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = CombatManager.Instance.trapPositionDic[index];
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);

            yield return null;
        }

        anim.Play("atk");
        yield return new WaitForSeconds(0.4f);

        Vector2 position = CombatManager.Instance.trapPositionDic[index] + new Vector2(0f, 1.125f);
        var effect = Instantiate(earthBreak, position, Quaternion.identity);
        effect.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale.x : -1 * Mathf.Abs(effect.transform.localScale.x)), effect.transform.localScale.y, effect.transform.localScale.z);
        yield return new WaitForSeconds(0.3f);
        anim.Play("stay");
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < targets.Count; ++i)
        {
            bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck)
            {
                targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
            }
        }

        yield return waitTime;

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        backgroundAnim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => backgroundAnim == null);
        yield return waitTime;

        //필수
        isBlock = false;
    }
    private IEnumerator FormChange4(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        stopStructure.isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출

        //날아와서 착지
        Animator backgroundAnim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        backgroundAnim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => backgroundAnim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var clone = structure.player.SwapMonster(liberationWarrior, structure.skillData.consumMpAmount);

        float buffValue = Mathf.RoundToInt(structure.player.originInstance.maxHp * 0.25f);
        clone.battleInstance.maxHp += buffValue;
        clone.battleInstance.hp += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.maxMp * 0.25f);
        clone.battleInstance.maxMp += buffValue;
        clone.battleInstance.mp += buffValue;


        buffValue = Mathf.RoundToInt(structure.player.originInstance.atk * 0.25f);
        clone.battleInstance.atk += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.def * 0.25f);
        clone.battleInstance.def += buffValue;

        structure.player = clone;

        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 endOffset = isHomeground ? new Vector3(-12, 7) : new Vector3(12, 7);
        Vector3 endPosition = clone.transform.position;
        Vector3 startPosition = clone.transform.position + endOffset;

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        var anim = clone.GetComponent<Animator>();
        anim.Play("land");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return new WaitForSeconds(0.8f);


        //돌진스킬
        anim.Play("skillStart");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("skillStay") == true);
        yield return waitTime;

        cavas.gameObject.SetActive(false);
        shaodw.gameObject.SetActive(false);

        lerpSpeed = 2f;
        currentTime = 0f;
        lerpTime = 1f;
        Vector3 endPosition2 = new Vector3(-1 * startPosition.x, clone.transform.position.y, clone.transform.position.z);
        Vector3 startPosition2 = clone.transform.position;
        BoxCollider2D boxCol = clone.GetComponent<BoxCollider2D>();
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition2, endPosition2, currentSpeed);

            var cols = Physics2D.OverlapBoxAll((Vector2)clone.transform.position + boxCol.offset, boxCol.size, 0f);
            for (int i = 0; i < cols.Length; ++i)
            {
                var mon = cols[i].GetComponent<EntityMonster>();
                bool isHomeground2 = CombatManager.Instance.homegroundMonsters.Contains(mon);
                if (mon != null && mon != clone && (isHomeground != isHomeground2) && mon.isBlockHurt == false)
                {
                    var dmgCheck = mon.DmgCheck(structure.player, structure.skillData);
                    if (dmgCheck == true)
                    {
                        float dmg = clone.battleInstance.atk;
                        if (dmg >= mon.battleInstance.def)
                            mon.FlyingDead();
                        else
                            mon.Hurt(clone.battleInstance.atk, structure.player, structure.skillData);
                    }

                    mon.isBlockHurt = true;
                }

                var wall = cols[i].GetComponent<Wall>();
                if (wall != null)
                    wall.Remove();
            }
            yield return null;
        }

        var originTargets = isHomeground ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        for (int i = 0; i < originTargets.Count; ++i)
        {
            originTargets[i].isBlockHurt = false;
        }

        endPosition2 = startPosition2;
        startPosition2 = clone.transform.position = new Vector3(startPosition.x, clone.transform.position.y, clone.transform.position.z);
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition2, endPosition2, currentSpeed);
            yield return null;
        }
        anim.Play("skillExit");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle2") == true);
        yield return waitTime;

        cavas.gameObject.SetActive(true);
        shaodw.gameObject.SetActive(true);

        backgroundAnim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => backgroundAnim == null);
        yield return waitTime;

        //필수
        isBlock = false;
        stopStructure.isBlock = false;
    }
    private IEnumerator ShineSkipping(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟
        var targets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        targets = targets.ToList();
        //연출
        var anim = structure.player.GetComponent<Animator>();
        var renderer = structure.player.GetComponent<SpriteRenderer>();
        var originMaterial = renderer.material;
        renderer.material = holyMaterial;
        anim.Play("skill2");

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 1f;
        string materialChar = "_FlashAmount";
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;
            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(0f, 1f, currentSpeed);
            holyMaterial.SetFloat(materialChar, value);
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;
            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(1f, 0f, currentSpeed);
            holyMaterial.SetFloat(materialChar, value);
            yield return null;
        }

        renderer.material = originMaterial;
        holyMaterial.SetFloat(materialChar, 1f);


        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle2"));

        float dmg = 1f;
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;

            var dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == false)
            {
                yield return waitTime;
                break;
            }
            else
            {
                var effect = Instantiate(nearDistanceHitEffect, targets[i].center.position, Quaternion.identity);
                yield return new WaitUntil(() => effect == null);
                yield return waitTime;
                targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def)) * dmg);
                dmg += structure.skillData.buffValue;
                yield return waitTime;

                if (targets[i] != null && targets[i].isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= (structure.skillData.statusRatio + structure.player.addHolyStatus))
                    {
                        effect = Instantiate(holyEffect, targets[i].center.position, Quaternion.identity);
                        yield return new WaitUntil(() => effect == null);
                        yield return waitTime;
                        targets[i].battleInstance.hpRecoveryRatio = Mathf.Max(0, targets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                        targets[i].battleInstance.manaRecoveryRatio = Mathf.Max(0, targets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    }
                }
                yield return waitTime;
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator DoubleAttack(BattleStructure structure)
    {
        //필수
        isBlock = true;
        bool suctionDmgCheck = false;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        Vector2 offset = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? new Vector2(-CombatManager.Instance.spawnDistance, 0f) : new Vector2(CombatManager.Instance.spawnDistance, 0f);
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = ((Vector2)target.transform.localPosition + offset) - (Vector2)target.battleInstance.monsterData.monsterPrefab.transform.localPosition + (Vector2)structure.player.battleInstance.monsterData.monsterPrefab.transform.localPosition;
        //Vector2 endPosition = new Vector2(target.transform.localPosition.x + offset.x, structure.player.transform.localPosition.y);

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        var anim = structure.player.GetComponent<Animator>();
        anim.Play("atk2");

        yield return new WaitForSeconds(0.4f);

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);

        if (dmgCheck == true)
        {
            suctionDmgCheck = true;
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);

            var effectClone = Instantiate(nearDistanceHitEffect);
            effectClone.transform.position = target.center.position;
        }

        if (target.isDead)
        {
            anim.Play("stay");
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
            anim.Play("stay");

            dmgCheck = target.DmgCheck(structure.player, structure.skillData);

            if (dmgCheck == true)
            {
                suctionDmgCheck = true;
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                if (target != null && target.isDead == false)
                    target.CheckSandHideSkill(target);

                var effectClone = Instantiate(nearDistanceHitEffect);
                effectClone.transform.position = target.center.position;
            }
        }

        yield return new WaitForSeconds(0.25f);

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        if (suctionDmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator EarthSpike(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = CombatManager.Instance.trapPositionDic[index];

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        if (structure.player.originMonster != null)
        {
            var anim = structure.player.GetComponent<Animator>();
            anim.Play("atk2");
            yield return new WaitForSeconds(0.3f);
            anim.Play("stay");
        }
        else
        {
            Transform shaodw = structure.player.transform.GetChild(4);
            Transform cavas = structure.player.transform.GetChild(0);
            Vector3 shadowOriginPosition = shaodw.position;
            Vector3 cavasPosition = cavas.position;
            Vector2 endPosition2 = endPosition + new Vector3(0f, 0.2f);
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition, endPosition2, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition2, endPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);
        }

        GameObject startEffect = null;
        List<int> rands = new List<int>();
        for (int i = 0; i < targets.Count; i++)
        {
            var rand = Random.Range(0, 3);
            rands.Add(rand);
            if (rand == 0)
                startEffect = Instantiate(earthSpike_fail, targets[i].transform.position, Quaternion.identity);
            else if (rand == 1)
                startEffect = Instantiate(earthSpike_goal, targets[i].transform.position, Quaternion.identity);
            else if (rand == 2)
                startEffect = Instantiate(earthSpike_goalCri, targets[i].transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < targets.Count; i++)
        {
            var rand = rands[i];
            if (rand == 0)
            {
                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck)
                    targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) * 0.5f) - targets[i].battleInstance.def), structure.player, structure.skillData);
            }
            else if (rand == 1)
            {
                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck)
                    targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
            }
            else if (rand == 2)
            {
                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck)
                    targets[i].CriticalHurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
            }
        }

        yield return new WaitUntil(() => startEffect == null);
        yield return new WaitForSeconds(0.25f);

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator StrongWall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //타겟설정
        int index = CombatManager.Instance.GetTrapTarget(structure.player);
        Wall outWall = null;
        Animator effect = null;
        CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
        if (outWall == null)
        {
            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(strongWall, randomPosition - new Vector2(0f, 0.4f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic.Add(index, wall);
        }
        else
        {
            Destroy(outWall.gameObject);

            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(strongWall, randomPosition - new Vector2(0f, 0.4f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic[index] = wall;
        }
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator EarthQuake(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟설정
        List<EntityMonster> targets = new List<EntityMonster>();
        targets.AddRange(CombatManager.Instance.homegroundMonsters);
        targets.AddRange(CombatManager.Instance.awayMonsters);
        targets.Remove(structure.player);

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        int value = 0;
        int value3 = 0;

        Camera cam = Camera.main;
        Vector3 camHalfSize = new Vector2(((cam.orthographicSize * 2) * cam.aspect) * 0.5f, cam.orthographicSize);
        Vector2 camMin = (cam.transform.position - camHalfSize) * 0.5f;
        Vector2 camMax = (cam.transform.position + camHalfSize) * 0.5f;

        for (int i = 0; i < 40; ++i)
        {
            int value2 = value % 2;
            int value4 = value3 % 3;
            CameraShake shake = cam.GetComponent<CameraShake>();

            if (value2 == 0)
                shake.StartShake(0.1f, 0.05f, false, ShakingMode.Left);
            else
                shake.StartShake(0.1f, 0.05f, false, ShakingMode.Right);

            if (value3 > 10 && value4 == 0)
            {
                var clone = FallingGenerator.Create(earthQuakeEffect[Random.Range(0, earthQuakeEffect.Length)], new Vector2(Random.Range(camMin.x, camMax.x), Random.Range(camMin.y, camMax.y)), 6f);
                SoundManager.Instance.PlayEffect(30, 1f);
                clone.endEvent += clone.RandomDeadorChangeLayer;
            }
            yield return new WaitUntil(() => shake.CheckEnd() == true);
            value++;
            value3++;
        }

        // yield return waitTime;

        //연출

        float startX = cam.transform.position.x;
        float startY = cam.transform.position.y;
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;

            float x = Mathf.Lerp(startX, 0f, currentSpeed);
            float y = Mathf.Lerp(startY, 0f, currentSpeed);
            cam.transform.position = new Vector3(x, y, -10f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        GameObject effect = null;
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;
            bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
                effect = Instantiate(nearDistanceHitEffect, targets[i].center.position, Quaternion.identity);
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator RockBlaster(BattleStructure structure)
    {
        if (rockClones.Count > 0)
            yield return StartCoroutine(RockBlaster_1(structure));
        else
            yield return StartCoroutine(RockBlaster_0(structure));
    }
    private IEnumerator MudCast(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(mudcastBullet, structure.player.bulletCreatePoint.position, Quaternion.identity);

        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? 0.5f : -0.5f), 0.5f, 1f);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }

        float bulletSpeed = 3f;
        float addSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator EarthThrowing(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(earthThrowingEffect, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);

        bullet.StartPalabola(target.center);

        yield return new WaitUntil(() => bullet.IsEnd == true);

        //필수(데미지계산)
        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            anim.Play("exit");
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            yield return new WaitUntil(() => anim == null);
            yield return new WaitForSeconds(0.25f);
        }
        else
            Destroy(bullet.gameObject);

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator DragonDive(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        bool suctionDmgCheck = false;
        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        targets = targets.FindAll(x => x.width == target.width);

        //연출
        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        GameObject effect = null;
        for (int i = 0; i < targets.Count; ++i)
        {
            effect = Instantiate(drangonDive, targets[i].transform.position, Quaternion.identity);
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                suctionDmgCheck = true;
                targets[i].Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                if (target != null && target.isDead == false)
                    target.CheckSandHideSkill(target);
            }
        }

        yield return waitTime;

        if (suctionDmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }

            }
        }
        isBlock = false;

    }
    private IEnumerator StaticShock(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        targets = targets.ToList();
        //연출
        var clone = Instantiate(lightingCharge, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        if (targets.Count == 1)
        {
            var effect = Instantiate(lightingCharge, targets[0].center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            targets[0].Hurt(structure.skillData.atk, structure.player, structure.skillData);
            yield return waitTime;
        }
        else if (targets.Count >= 2)
        {
            for (int i = 0; i < targets.Count - 1; ++i)
            {
                if (targets[i] == null)
                    goto jump;
                if (targets[i].isDead == true)
                    goto jump;
                var obj = LightingLine.Create(targets[i].center, targets[i + 1].center);
                var effect = Instantiate(lightingCharge, targets[i].center.position, Quaternion.identity);
                var effect2 = Instantiate(lightingCharge, targets[i + 1].center.position, Quaternion.identity);
                yield return new WaitUntil(() => obj == null);
                yield return new WaitUntil(() => effect == null);
                yield return waitTime;

                targets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);

                bool dmgCheck = targets[i + 1].DmgCheck(structure.player, structure.skillData);

                if (dmgCheck == true)
                {
                    targets[i + 1].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                }
                else
                    goto jump;
                yield return waitTime;

                if (targets[i + 1].isDead == true)
                    goto jump;
            }

            if (targets[targets.Count - 2] == targets[0])
                goto jump;

            if ((targets[0] != null && targets[0].isDead == false) && (targets[targets.Count - 1] != null && targets[targets.Count - 1].isDead == false))
            {
                bool dmgCheck = targets[targets.Count - 1].DmgCheck(structure.player, structure.skillData);

                if (dmgCheck == true)
                {
                    var obj = LightingLine.Create(targets[targets.Count - 1].center, targets[0].center);
                    if (obj == null)
                        goto jump;
                    var effect = Instantiate(lightingCharge, targets[targets.Count - 1].center.position, Quaternion.identity);
                    var effect2 = Instantiate(lightingCharge, targets[0].center.position, Quaternion.identity);
                    yield return new WaitUntil(() => obj == null);
                    yield return new WaitUntil(() => effect == null);
                    yield return waitTime;


                    targets[targets.Count - 1].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                    targets[0].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                    yield return waitTime;
                }
            }
        }
    jump:
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator ElectronicShock(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(electronicShockEffect, structure.player.bulletCreatePoint.position, Quaternion.identity);

        //  float monScale = structure.player.bulletCreatePoint.localScale.x;
        //  bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        var dir = (target.center.position - bullet.transform.position).normalized;
        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);


        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);

        float bulletSpeed = 6f;
        float addSpeed = 4f;
        float m = (target.center.position - bullet.transform.position).magnitude;

        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        anim.Play("exit");
        target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
        yield return new WaitUntil(() => anim == null);
        yield return new WaitForSeconds(0.25f);
        //마비상태
        if (target != null && target.isDead == false)
        {
            float value = Random.Range(0f, 1f);
            if (value <= structure.skillData.statusRatio)
            {
                var clone = target.dogDmgStatus.AddElecFlag(target, palalysisEffect);
                yield return new WaitUntil(() => clone == null);
                yield return new WaitForSeconds(0.25f);
            }
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator Benumed(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟설정
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //마비상태
        target.dogDmgStatus.AddElecFlag(target, palalysisEffect);

        yield return new WaitForSeconds(0.5f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator ElectronicSuction(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟설정
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);


        //연출
        var effect = Instantiate(elecStart, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        effect = Instantiate(electronicSuction, target.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            float value = Mathf.Max(0, target.battleInstance.dex - (target.battleInstance.maxDex * structure.skillData.buffValue));
            target.Hurt(structure.skillData.atk, structure.player, structure.skillData);
            yield return waitTime;

            if (target != null && target.isDead == false)
            {
                if (!target.sternStatus.Check())
                {
                    if (CombatManager.Instance.battleQueue.Contains(target))
                        CombatManager.Instance.battleQueue.RemoveAll(x => x == target);

                    yield return StartCoroutine(target.DexRoutine(target.battleInstance.dex, value, 1f));
                    yield return waitTime;
                }
            }

            if (target != null && target.isDead == false)
            {
                effect = Instantiate(electronicSuctionCharge, structure.player.transform.position, Quaternion.identity);
                yield return new WaitUntil(() => effect == null);
                yield return waitTime;

                value = Mathf.Min(structure.player.battleInstance.maxDex, structure.player.battleInstance.dex + (target.battleInstance.maxDex * structure.skillData.buffValue));
                yield return StartCoroutine(structure.player.DexRoutine(structure.player.battleInstance.dex, value, 1f));
            }
        }

        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator LightingVotex(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = new List<EntityMonster>();
        int random = Random.Range(2, 7);
        for (int i = 0; i < random; ++i)
        {
            var sortingTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
            targets.Add(CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, sortingTargets));
        }

        //연출
        var clone = Instantiate(elecStart, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;

            int count = 0;
            var neighbourTargets = CombatManager.Instance.GetNeighbourTarget(targets[i], out count);

            clone = Instantiate(lightingVotexEffect, targets[i].transform.position, Quaternion.identity);
            for (int j = 0; j < neighbourTargets.Length; ++j)
            {
                if (neighbourTargets[j] == null)
                    continue;
                if (neighbourTargets[j].isDead == true)
                    continue;
                if (neighbourTargets[j] != null)
                    clone = Instantiate(lightingVotexEffect, neighbourTargets[j].transform.position, Quaternion.identity);
            }

            yield return new WaitUntil(() => clone == null);
            yield return waitTime;

            //데미지계산
            bool check = targets[i].DmgCheck(structure.player, structure.skillData);
            if (check)
            {
                targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
            }

            for (int j = 0; j < neighbourTargets.Length; ++j)
            {
                if (neighbourTargets[j] == null)
                    continue;
                if (neighbourTargets[j].isDead == true)
                    continue;

                if (neighbourTargets[j] != null)
                {
                    check = neighbourTargets[j].DmgCheck(structure.player, structure.skillData);
                    if (check)
                    {
                        neighbourTargets[j].Hurt(Mathf.Max(neighbourTargets[j].GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) * structure.skillData.buffValue) - neighbourTargets[j].battleInstance.def), structure.player, structure.skillData);
                    }
                }
            }


            //상태체크
            //마비상태
            if (targets[i] != null)
            {
                if (targets[i].isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                    {
                        yield return waitTime;
                        clone = targets[i].dogDmgStatus.AddElecFlag(targets[i], palalysisEffect);
                    }
                }
            }

            for (int j = 0; j < neighbourTargets.Length; ++j)
            {
                if (neighbourTargets[j] != null)
                {
                    if (neighbourTargets[j].isDead == false)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                        {
                            yield return waitTime;
                            clone = neighbourTargets[j].dogDmgStatus.AddElecFlag(neighbourTargets[j], palalysisEffect);
                        }
                    }
                }
            }

            if (clone != null)
            {
                yield return new WaitUntil(() => clone == null);
                yield return waitTime;
            }
            else
                yield return waitTime;
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator ThunderBolt(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟설정
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var effect = Instantiate(lightingCharge, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        effect = Instantiate(thunderBoltEffect, target.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        target.Hurt(structure.skillData.atk, structure.player, structure.skillData);
        yield return waitTime;

        //마비상태
        if (target != null && target.isDead == false)
        {
            float value = Random.Range(0f, 1f);
            if (value <= structure.skillData.statusRatio)
            {
                var clone = target.dogDmgStatus.AddElecFlag(target, palalysisEffect);
                yield return new WaitUntil(() => clone == null);
                yield return waitTime;
            }
        }

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator ElectronicPunch(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
        int count = 0;
        var dialogTargets = CombatManager.Instance.GetNeighbourTarget(target, out count);

        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);

        if (dmgCheck == true)
        {
            var effectClone = Instantiate(electronicPunchEffect);
            effectClone.transform.position = target.center.position;
            yield return new WaitUntil(() => effectClone == null);
            yield return waitTime;
        }
        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
            //마비상태
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    var clone = target.dogDmgStatus.AddElecFlag(target, palalysisEffect);
                    yield return new WaitUntil(() => clone == null);
                    yield return waitTime;
                }
            }
        }

        if (dmgCheck == true)
        {
            if (count != 0)
            {
                yield return waitTime;

                List<GameObject> effects = new List<GameObject>();
                for (int i = 0; i < dialogTargets.Length; ++i)
                {
                    if (dialogTargets[i] == null)
                        continue;
                    if (dialogTargets[i].isDead == true)
                        continue;

                    var effect = Instantiate(electronicPunchEffect2, dialogTargets[i].transform.position, Quaternion.identity);
                    effects.Add(effect);
                }

                for (int i = 0; i < effects.Count; ++i)
                {
                    yield return new WaitUntil(() => effects[i] == null);
                }

                yield return waitTime;

                for (int i = 0; i < dialogTargets.Length; ++i)
                {
                    if (dialogTargets[i] == null)
                        continue;
                    if (dialogTargets[i].isDead == true)
                        continue;

                    dialogTargets[i].Hurt(Mathf.Max(target.GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) * structure.skillData.buffValue) - dialogTargets[i].battleInstance.def), structure.player, structure.skillData);
                    //마비상태
                    if (target.isDead == false)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                        {
                            var clone = target.dogDmgStatus.AddElecFlag(target, palalysisEffect);
                            yield return new WaitUntil(() => clone == null);
                            yield return waitTime;
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (dmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator ElectronicNet(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //타겟설정
        int index = CombatManager.Instance.GetTrapTarget(structure.player);
        Wall outWall = null;
        GameObject effect = null;
        CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
        if (outWall == null)
        {
            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(electronicNet, randomPosition - new Vector2(0f, 0.36f), Quaternion.identity);

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic.Add(index, wall);
        }
        else
        {
            Destroy(outWall.gameObject);

            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(electronicNet, randomPosition - new Vector2(0f, 0.36f), Quaternion.identity);

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic[index] = wall;
        }
        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator DiskDrive(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = new List<EntityMonster>();
        int random = Random.Range(2, 5);
        for (int i = 0; i < random; ++i)
        {
            var sortingTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
            targets.Add(CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, sortingTargets));
        }

        //연출
        Animator[] anims = new Animator[targets.Count];

        Vector3 previousPosition = structure.player.bulletCreatePoint.position;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        for (int i = 0; i < targets.Count; ++i)
        {
            var bullet = Instantiate(diskDriveEffect, previousPosition, Quaternion.identity);


            float monScale = structure.player.bulletCreatePoint.localScale.x;
            bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
            anims[i] = bullet.GetComponent<Animator>();

            float dis = Random.Range(0.25f, 0.5f);
            Vector2 insidePosition = new Vector2(0f, dis) * monScale;
            previousPosition = (Vector2)previousPosition + insidePosition;

            yield return waitTime;
        }

        bounceDestroyObjs.Clear();
        bounceDestroyObjs.AddRange(anims);

        for (int i = 0; i < anims.Length; ++i)
        {
            yield return new WaitUntil(() => anims[i].GetCurrentAnimatorStateInfo(0).IsName("stay"));
        }

        for (int i = 0; i < anims.Length; ++i)
        {
            if (targets[i] == null)
            {
                var newTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), TargetLayer.Middle);
                if (newTargets.Count <= 0)
                {
                    for (int j = 0; j < anims.Length; ++j)
                    {
                        if (anims[j] == null)
                            continue;
                        Destroy(anims[j].gameObject);
                    }
                    isBlock = false;
                    yield break;
                }
                else
                    targets[i] = newTargets[Random.Range(0, newTargets.Count)];
            }
            if (targets[i] != null && targets[i].isDead == true)
            {
                var newTargets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), TargetLayer.Middle);
                if (newTargets.Count <= 0)
                {
                    for (int j = 0; j < anims.Length; ++j)
                    {
                        if (anims[j] == null)
                            continue;
                        Destroy(anims[j].gameObject);
                    }
                    isBlock = false;
                    yield break;
                }
                else
                    targets[i] = newTargets[Random.Range(0, newTargets.Count)];
            }

            //멈춤데이터 초기화
            stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            stopStructure.bullet = anims[i];
            stopStructure.stopObj = null;

            float bulletSpeed = 10f;
            float m = (targets[i].center.position - anims[i].transform.position).magnitude;
            var dir = (targets[i].center.position - anims[i].transform.position).normalized;

            float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
            anims[i].transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
            while (m > bulletSpeed * Time.deltaTime)
            {
                if (stopStructure.dontMove == true)
                    goto jump;
                m = (targets[i].center.position - anims[i].transform.position).magnitude;
                anims[i].transform.position += dir * bulletSpeed * Time.deltaTime;
                yield return null;
            }
        jump:
            //필수(데미지계산)
            if (stopStructure.dontMove == false)
            {
                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck == true)
                {
                    anims[i].Play("exit");
                    targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);

                    //마비상태
                    if (targets[i] != null && targets[i].isDead == false)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                        {
                            var clone = targets[i].dogDmgStatus.AddElecFlag(targets[i], palalysisEffect);
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else
                    Destroy(anims[i].gameObject);
            }

            yield return new WaitForSeconds(0.1f);
            stopStructure.dontMove = false;
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator ElectronicBall(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        int count = 0;
        var dialogueTargets = CombatManager.Instance.GetNeighbourTarget(target, out count);

        //연출
        var bullet = Instantiate(electronicBall, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }
        yield return new WaitForSeconds(0.5f);

        float bulletSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            List<EntityMonster> dialogueTargetsStatusList = new List<EntityMonster>();
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                    {
                        dialogueTargetsStatusList.Add(target);
                    }
                }

                yield return new WaitUntil(() => anim == null);
                yield return waitTime;

                if (dialogueTargets.Length > 0)
                {
                    GameObject effectClone = null;
                    dialogueTargetsStatusList = new List<EntityMonster>();

                    for (int i = 0; i < dialogueTargets.Length; ++i)
                    {
                        if (dialogueTargets[i] == null)
                            continue;
                        if (dialogueTargets[i].isDead == true)
                            continue;

                        effectClone = Instantiate(electronicPunchEffect2, dialogueTargets[i].transform.position, Quaternion.identity);
                    }

                    yield return new WaitUntil(() => effectClone == null);
                    yield return waitTime;

                    for (int i = 0; i < dialogueTargets.Length; ++i)
                    {
                        if (dialogueTargets[i] == null)
                            continue;
                        if (dialogueTargets[i].isDead == true)
                            continue;

                        dialogueTargets[i].Hurt(Mathf.Max(target.GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) * structure.skillData.buffValue) - dialogueTargets[i].battleInstance.def), structure.player, structure.skillData);
                        //마비상태
                        if (dialogueTargets[i] != null && dialogueTargets[i].isDead == false)
                        {
                            float value = Random.Range(0f, 1f);
                            if (value <= structure.skillData.statusRatio)
                            {
                                dialogueTargetsStatusList.Add(dialogueTargets[i]);
                            }
                        }
                    }

                    yield return waitTime;
                }

                if (dialogueTargetsStatusList.Count > 0)
                {
                    GameObject effectClone = null;
                    for (int i = 0; i < dialogueTargetsStatusList.Count; ++i)
                    {
                        effectClone = dialogueTargetsStatusList[i].dogDmgStatus.AddElecFlag(dialogueTargetsStatusList[i], palalysisEffect);
                    }

                    yield return new WaitUntil(() => effectClone == null);
                    yield return new WaitForSeconds(0.25f);
                }
                //마비상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                    {
                        var clone = target.dogDmgStatus.AddElecFlag(target, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator ThunderBird(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var effect = Instantiate(elecStart, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;


        var startPosition = CombatManager.Instance.trapPositionDic[1];
        effect = Instantiate(thunderBird, startPosition, Quaternion.identity);
        effect.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale.x : -1 * Mathf.Abs(effect.transform.localScale.x)), effect.transform.localScale.y, effect.transform.localScale.z);

        var thunderBirdAnim = effect.GetComponent<Animator>();

        yield return new WaitUntil(() => thunderBirdAnim.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        float time = 3f;
        float time2 = 0.25f;
        var dexTargets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        dexTargets = dexTargets.ToList();
        List<Vector3> positionList = new List<Vector3>();
        var explosionList = CombatManager.Instance.GetWithinPosition(CombatManager.Instance.homegroundMonsters.Contains(structure.player), true);

        Vector3 dir = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Vector3.right : Vector3.left;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            effect.transform.position += dir * 2f * Time.deltaTime;

            if (time2 > 0)
                time2 -= Time.deltaTime;
            else
            {
                if (positionList.Count <= 0)
                {
                    for (int i = 0; i < 20; ++i)
                    {
                        var index = Random.Range(0, explosionList.Count);
                        var index2 = Random.Range(0, explosionList.Count);
                        while (index == index2)
                            index2 = Random.Range(0, explosionList.Count);
                        var tmp = explosionList[index];
                        explosionList[index] = explosionList[index2];
                        explosionList[index2] = tmp;
                    }

                    positionList = explosionList.ToList();
                }

                Instantiate(thunderBirdHit, positionList[0] + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f)), Quaternion.identity);
                positionList.RemoveAt(0);

                for (int i = 0; i < dexTargets.Count; ++i)
                {
                    if (dexTargets[i] == null)
                        continue;
                    if (dexTargets[i].isDead == true)
                        continue;

                    dexTargets[i].sumStatusRatio += structure.skillData.statusRatio;
                    dexTargets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                    if (dexTargets[i] != null && dexTargets[i].isDead == true)
                        dexTargets[i].sumStatusRatio = 0f;
                }
                time2 = 0.25f;
            }
            yield return null;
        }
        thunderBirdAnim.Play("exit");
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;


        if (dexTargets.Count > 0)
        {
            for (int i = 0; i < dexTargets.Count; ++i)
            {
                if (dexTargets[i] == null)
                    continue;
                if (dexTargets[i].isDead == true)
                    continue;
                effect = Instantiate(thunderBirdHit, dexTargets[i].center.position, Quaternion.identity);
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            EntityMonster dexTarget = null;
            for (int i = 0; i < dexTargets.Count; ++i)
            {
                if (dexTargets[i] == null)
                    continue;
                if (dexTargets[i].isDead == true)
                    continue;

                CombatManager.Instance.battleQueue.RemoveAll(x => x == dexTargets[i]);
                dexTarget = dexTargets[i];
                StartCoroutine(dexTarget.DexRoutine(dexTarget.battleInstance.dex, 0, 1f));
            }

            if (dexTarget != null)
            {
                yield return new WaitUntil(() => dexTarget.startRecovery == false);
                yield return waitTime;
            }

            bool check = false;
            for (int i = 0; i < dexTargets.Count; ++i)
            {
                if (dexTargets[i] == null)
                    continue;
                if (dexTargets[i].isDead == true)
                    continue;
                //마비상태
                if (dexTargets[i] != null && dexTargets[i].isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= dexTargets[i].sumStatusRatio)
                    {
                        check = true;
                        var clone = dexTargets[i].dogDmgStatus.AddElecFlag(dexTargets[i], palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return waitTime;
                    }
                }

                dexTargets[i].sumStatusRatio = 0f;

            }

            if (check == true)
                yield return waitTime;
        }

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator FairWinds(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        SoundManager.Instance.PlayEffect(62, 1f);
        var effect = Instantiate(fairWindsEffect, Vector2.zero, Quaternion.identity);
        effect.transform.localScale = new Vector3(CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale.x : -1 * effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z);

        var effectChild = effect.transform.GetChild(0).GetComponent<Wind>();

        yield return new WaitUntil(() => effectChild.isEnd == true);
        Destroy(effect.gameObject);
        SoundManager.Instance.StopEffect(62);
        yield return waitTime;

        for (int a = 0; a < 3; ++a)
        {
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), a);
            if (targets.Count > 0)
            {
                //데미지
                var dmgTargets = new List<EntityMonster>();
                for (int i = 0; i < targets.Count; ++i)
                {
                    bool check = targets[i].DmgCheck(structure.player, structure.skillData);
                    if (check)
                        dmgTargets.Add(targets[i]);
                }

                for (int i = 0; i < dmgTargets.Count; ++i)
                {
                    if (dmgTargets[i].passiveFlag == true && dmgTargets[i].currentPassiveName == PassiveName.SandHide)
                        effect = Instantiate(airHit, dmgTargets[i].center.position, Quaternion.identity);
                    else
                        effect = Instantiate(airHit, dmgTargets[i].transform.position, Quaternion.identity);
                }

                yield return new WaitUntil(() => effect == null);
                yield return waitTime;

                for (int i = 0; i < dmgTargets.Count; ++i)
                {
                    if (dmgTargets[i].width < 2)
                        dmgTargets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                    else
                    {
                        //즉사
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                            dmgTargets[i].FlyingDead();
                        else
                        {
                            dmgTargets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                        }
                    }
                }

                yield return waitTime;

                //날리기
                if (a < 2)
                {
                    EntityMonster checkTarget = null;
                    for (int i = 0; i < dmgTargets.Count; ++i)
                    {
                        if (dmgTargets[i] == null)
                            continue;
                        if (dmgTargets[i].isDead == true)
                            continue;
                        if (dmgTargets[i].width >= 2)
                            continue;
                        if (dmgTargets[i].passiveFlag == true && dmgTargets[i].currentPassiveName == PassiveName.SandHide)
                            continue;
                        if (dmgTargets[i].sternStatus.Check() == true)
                            continue;

                        var target2 = CombatManager.Instance.homegroundMonsters.Contains(dmgTargets[i]) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == dmgTargets[i].width + 1) && (x.height == dmgTargets[i].height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == dmgTargets[i].width + 1) && (x.height == dmgTargets[i].height));
                        if (target2 != null)
                            StartCoroutine(FlyRoutine(dmgTargets[i], target2));
                        else
                            StartCoroutine(FlyRoutine2(dmgTargets[i]));
                        checkTarget = dmgTargets[i];
                        if (dmgTargets[i] != null && dmgTargets[i].isDead == false)
                            checkTarget = dmgTargets[i];
                    }

                    if (checkTarget != null)
                    {
                        yield return new WaitUntil(() => checkTarget == null || (checkTarget != null && checkTarget.checkSwapPosition == false) || (checkTarget != null && checkTarget.isDead == true));
                        yield return waitTime;

                        CombatManager.Instance.SetFormationBuffDebuffs();
                    }
                }
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator WindBlast(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var effect = Instantiate(windBlast, structure.player.bulletCreatePoint.position, Quaternion.identity).GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = effect;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        stopStructure.speicalBackground = anim;

        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);

        Vector2 startPosition = effect.transform.position;
        Vector3 endPosition = target.center.position;

        float m = (endPosition - effect.transform.position).magnitude;
        Vector3 dir = (endPosition - effect.transform.position).normalized;
        float moveSpeed = 3f;
        while (m > moveSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;

            m = (endPosition - effect.transform.position).magnitude;
            effect.transform.position += dir * moveSpeed * Time.deltaTime;
            yield return null;
        }

        if(stopStructure.dontMove == false)
        {
            effect.Play("exit");
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            var check = target.DmgCheck(structure.player, structure.skillData);
            if (check == true)
                target.FlyingDead();

            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitUntil(() => effect == null);
            yield return new WaitForSeconds(0.5f);
        }


        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator Swirl(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(windBlast, target.center.position, Quaternion.identity).GetComponent<Animator>();
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        yield return waitTime;

        int randomCount = Random.Range(2, 7);
        WaitForSeconds waitTime2 = new WaitForSeconds(0.5f);
        for (int i = 0; i < randomCount; ++i)
        {
            if (target == null)
                break;
            if (target.isDead == true)
                break;

            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            GameObject hitEffect = null;
            if (dmgCheck == true)
            {
                int randomValue = Random.Range(0, 6);
                float value = 1f;
                if (randomValue == 0)
                    value = 0.1f;
                else if (randomValue == 1)
                    value = 0.25f;
                else if (randomValue == 2)
                    value = 0.5f;
                else if (randomValue == 3)
                    value = 0.75f;
                else if (randomValue == 4)
                    value = 1f;
                else if (randomValue == 5)
                    value = 1.5f;

                hitEffect = Instantiate(nearDistanceHitEffect, target.center.position, Quaternion.identity);
                value = value * (structure.player.battleInstance.atk + structure.skillData.atk);
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), value - target.battleInstance.def), structure.player, structure.skillData);
            }
            yield return new WaitUntil(() => hitEffect == null);
            yield return waitTime2;
        }

        effect.Play("exit");
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator SonicBoom(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(sonicBoom, target.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        var check = target.DmgCheck(structure.player, structure.skillData);
        if (check)
        {
            if (target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide)
                effect = Instantiate(airHit, target.center.position, Quaternion.identity);
            else
                effect = Instantiate(airHit, target.transform.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            //즉사
            if (target.width >= 2)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio && !(target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide))
                    target.FlyingDead();
                else
                {
                    target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                }
            }
            else
            {
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            }


            yield return waitTime;

            if (target != null && target.checkSwapPosition == false && target.isDead == false && target.width < 2 && !(target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide) && target.sternStatus.Check() == false)
            {
                if (target.width == 1)
                {
                    var target2 = CombatManager.Instance.homegroundMonsters.Contains(target) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height));
                    if (target2 != null)
                        StartCoroutine(FlyRoutine(target, target2));
                    else
                        StartCoroutine(FlyRoutine2(target));
                }
                else
                {
                    var target2 = CombatManager.Instance.homegroundMonsters.Contains(target) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height));
                    var target3 = CombatManager.Instance.homegroundMonsters.Contains(target) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == target.width + 2) && (x.height == target.height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == target.width + 2) && (x.height == target.height));

                    if (target2 != null)
                        StartCoroutine(FlyRoutine(target, target2));
                    else
                    {
                        if (target3 == null)
                            StartCoroutine(FlyRoutine2(target, 2));
                        else
                            StartCoroutine(FlyRoutine2(target));
                    }
                }
                yield return waitTime;

                CombatManager.Instance.SetFormationBuffDebuffs();
            }

        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator WindSwap(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟
        EntityMonster target_0, target_1;
        CombatManager.Instance.GetRandomDoubleTarget(structure.player, structure.skillData, out target_0, out target_1);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(windSwap, target_0.center.position, Quaternion.identity);

        if (target_1 != null)
            effect = Instantiate(windSwap, target_1.center.position, Quaternion.identity);

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        bool dmgCheck = target_0.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            target_0.Hurt(structure.skillData.atk);
        }

        bool dmgCheck2 = false;
        if (target_1 != null)
        {
            dmgCheck2 = target_1.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck2 == true)
            {
                target_1.Hurt(structure.skillData.atk);
            }
        }

        yield return waitTime;

        if ((dmgCheck == true) && (dmgCheck2 == true) && ((target_0 != null && target_0.isDead == false) && (target_1 != null && target_1.isDead == false)) && !(target_0.passiveFlag == true && target_0.currentPassiveName == PassiveName.SandHide) && !(target_1.passiveFlag == true && target_1.currentPassiveName == PassiveName.SandHide) && ((target_0.sternStatus.Check() == false) && (target_1.sternStatus.Check() == false)))
        {
            StartCoroutine(FlyRoutine3(target_0, target_1));
            StartCoroutine(FlyRoutine3(target_1, target_0));
            int tmp = target_0.width;
            target_0.width = target_1.width;
            target_1.width = tmp;

            tmp = target_0.height;
            target_0.height = target_1.height;
            target_1.height = tmp;
            yield return waitTime;

            CombatManager.Instance.SetFormationBuffDebuffs();
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator AirBall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(airBall, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }
        yield return new WaitForSeconds(0.5f);

        float bulletSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                Destroy(bullet);

                GameObject effect = null;
                if (target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide)
                    effect = Instantiate(airHit, target.center.position, Quaternion.identity);
                else
                    effect = Instantiate(airHit, target.transform.position, Quaternion.identity);

                yield return new WaitUntil(() => effect == null);
                yield return new WaitForSeconds(0.25f);

                if (target.width < 2)
                    target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                else
                {
                    //즉사
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio && !(target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide))
                        target.FlyingDead();
                    else
                    {
                        target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                    }
                }

                yield return new WaitForSeconds(0.25f);


                if (target != null && target.checkSwapPosition == false && target.isDead == false && target.width < 2 && !(target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide) && target.sternStatus.Check() == false)
                {
                    var target2 = CombatManager.Instance.homegroundMonsters.Contains(target) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height));
                    if (target2 != null)
                        StartCoroutine(FlyRoutine(target, target2));
                    else
                        StartCoroutine(FlyRoutine2(target));

                    yield return new WaitForSeconds(0.25f);

                    CombatManager.Instance.SetFormationBuffDebuffs();
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator AirDrive(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        float sumValue = 0;
        var effect = Instantiate(airDrive, structure.player.transform.position, Quaternion.identity);
        effect.GetComponent<AirDrive>().endEvent = () => sumValue += structure.skillData.buffValue;

        float effectSpeed = 7f;
        Vector3 dir = (target.transform.position - effect.transform.position).normalized;
        float m = (target.transform.position - effect.transform.position).magnitude;
        while (m > effectSpeed * Time.deltaTime)
        {
            m = (target.transform.position - effect.transform.position).magnitude;
            effect.transform.position += dir * effectSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(effect.gameObject);
        yield return waitTime;

        var check = target.DmgCheck(structure.player, structure.skillData);
        if (check)
        {
            if (target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide)
                effect = Instantiate(airHit, target.center.position, Quaternion.identity);
            else
                effect = Instantiate(airHit, target.transform.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            //즉사
            if (target.width >= 2)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                    target.FlyingDead();
                else
                {
                    target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk + sumValue) - target.battleInstance.def), structure.player, structure.skillData);
                }
            }
            else
            {
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk + sumValue) - target.battleInstance.def), structure.player, structure.skillData);
            }


            yield return waitTime;

            if (target != null && target.checkSwapPosition == false && target.isDead == false && target.width < 2 && !(target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide) && target.sternStatus.Check() == false)
            {
                var target2 = CombatManager.Instance.homegroundMonsters.Contains(target) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height));
                if (target2 != null)
                    StartCoroutine(FlyRoutine(target, target2));
                else
                    StartCoroutine(FlyRoutine2(target));
                yield return waitTime;

                CombatManager.Instance.SetFormationBuffDebuffs();
            }

        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;


    }
    private IEnumerator Tornado(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        bool suctionDmgCheck = false;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        targets = targets.ToList();
        //멈춤데이터초기화
        stopStructure.isBlock = true;

        //연출
        var effect = Instantiate(tornado, structure.player.transform.position, Quaternion.identity).GetComponent<Animator>();
        structure.player.gameObject.SetActive(false);

        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        yield return waitTime;

        bool elecFlag = false, poisonFlag = false, burnFlag = false;
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;

            targets[i].CheckDrivenSkill(structure.player);
            if (i == 0)
            {
                float bulletSpeed = 4f;
                float m = (targets[i].transform.position - effect.transform.position).magnitude;
                var dir = (targets[i].transform.position - effect.transform.position).normalized;

                while (m > bulletSpeed * Time.deltaTime)
                {
                    if (stopStructure.dontMove == true)
                        break;
                    m = (targets[i].transform.position - effect.transform.position).magnitude;
                    effect.transform.position += dir * bulletSpeed * Time.deltaTime;
                    yield return null;
                }

                yield return waitTime;

                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck == true)
                {
                    suctionDmgCheck = true;
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                        targets[i].FlyingDead();
                    else
                    {
                        Instantiate(nearDistanceHitEffect, targets[i].center.position, Quaternion.identity);
                        targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
                    }
                }
               
                yield return waitTime;
            }
            else
            {
                Transform obj = effect.transform;
                Vector2 startPosition = obj.localPosition;
                Vector2 endPosition = targets[i].transform.localPosition;

                float lerpSpeed = 2f;
                float currentTime = 0f;
                float lerpTime = 1f;

                while (currentTime < lerpTime)
                {
                    currentTime += Time.deltaTime * lerpSpeed;

                    float currentSpeed = currentTime / lerpTime;
                    obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                    yield return null;
                }

                yield return waitTime;

                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck == true)
                {
                    suctionDmgCheck = true;
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                        targets[i].FlyingDead();
                    else
                    {
                        Instantiate(nearDistanceHitEffect, targets[i].center.position, Quaternion.identity);
                        targets[i].Hurt(Mathf.Max(targets[i].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[i].battleInstance.def), structure.player, structure.skillData);
                    }
                }

                yield return waitTime;
            }

            //반격기확인
            if (targets[i] != null && targets[i].isDead == false && targets[i].passiveFlag == true)
            {
                if (targets[i].currentPassiveName == PassiveName.ThunderMan)
                    elecFlag = true;
                else if (targets[i].currentPassiveName == PassiveName.PoisonBarrior)
                    poisonFlag = true;
                else if (targets[i].currentPassiveName == PassiveName.FireBraces)
                    burnFlag = true;

            }
        }

        Transform obj2 = effect.transform;
        Vector2 startPosition2 = obj2.localPosition;
        Vector2 endPosition2 = structure.player.transform.localPosition;

        float lerpSpeed2 = 4f;
        float currentTime2 = 0f;
        float lerpTime2 = 1f;

        while (currentTime2 < lerpTime2)
        {
            currentTime2 += Time.deltaTime * lerpSpeed2;

            float currentSpeed = currentTime2 / lerpTime2;
            obj2.localPosition = Vector3.Lerp(startPosition2, endPosition2, currentSpeed);
            yield return null;
        }

        Destroy(effect.gameObject);
        structure.player.gameObject.SetActive(true);

        yield return waitTime;

        if (suctionDmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //반격기확인
        if (elecFlag == true)
        {
            float value = Random.Range(0f, 1f);
            if (value <= thunderMan.statusRatio)
            {
                var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                yield return new WaitUntil(() => clone == null);
                yield return new WaitForSeconds(0.25f);
            }
        }
        if (poisonFlag == true)
        {
            float value = Random.Range(0f, 1f);
            if (value <= poisonBarriorData.statusRatio)
            {
                var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                if (clone != null)
                {
                    yield return new WaitUntil(() => clone == null);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        if (burnFlag == true)
        {
            float value = Random.Range(0f, 1f);
            if (value <= fireBracesData.statusRatio)
            {
                var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                if (clone != null)
                {
                    yield return new WaitUntil(() => clone == null);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        //필수
        isBlock = false;
        stopStructure.isBlock = false;
    }
    private IEnumerator TornadoBarrior(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //타겟설정
        int index = CombatManager.Instance.GetTrapTarget(structure.player);
        Wall outWall = null;
        Animator effect = null;
        CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
        if (outWall == null)
        {
            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(tornadoBarrior, randomPosition - new Vector2(0f, 0.25f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic.Add(index, wall);
        }
        else
        {
            Destroy(outWall.gameObject);

            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(tornadoBarrior, randomPosition - new Vector2(0f, 0.25f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic[index] = wall;
        }
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator WindClone(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        var positions = CombatManager.Instance.GetEmptyPositions(CombatManager.Instance.homegroundMonsters.Contains(structure.player));
        GameObject effect = null;
        if (positions.Count > 0)
        {
            int count = (positions.Count < 2) ? 1 : 2;
            for (int i = 0; i < count; ++i)
            {
                Vector2 heightDisOffset = new Vector2(CombatManager.Instance.heightDistance * (positions[i].height + 1), 0);
                var monster = EntityMonster.CreateMonster(structure.player.originInstance, positions[i].worldPosition - heightDisOffset, positions[i].width, positions[i].height, CombatManager.Instance.homegroundMonsters.Contains(structure.player));
                effect = Instantiate(windClone, monster.transform.position, Quaternion.identity);
                monster.battleInstance.battleAIType = monster.originInstance.battleAIType = SelectSkillAIType.Balance;
                monster.battleInstance.skillDatas.Clear();
                monster.battleInstance.percentSkillDatas.Clear();
                monster.battleInstance.triggerSkillDatas.Clear();
                monster.battleInstance.skillDatas.Add(headBut);
                monster.battleInstance.abilities.Clear();
                monster.mainBody = structure.player;
                monster.isClone = true;
            }
        }
        else
            yield return StartCoroutine(NoManaRoutine(structure));

        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator SharpWinds(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        SoundManager.Instance.PlayEffect(62, 1f);
        var effect = Instantiate(fairWindsEffect, Vector2.zero, Quaternion.identity);
        effect.transform.localScale = new Vector3(CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale.x : -1 * effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z);

        var effectChild = effect.transform.GetChild(0).GetComponent<Wind>();

        yield return new WaitUntil(() => effectChild.isEnd == true);
        SoundManager.Instance.StopEffect(62);
        Destroy(effect.gameObject);
        yield return waitTime;

        for (int a = 0; a < 3; ++a)
        {
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), a);
            if (targets.Count > 0)
            {
                for (int i = 0; i < targets.Count; ++i)
                {
                    effect = Instantiate(sharpWindHit, targets[i].transform.position, Quaternion.identity);
                }

                yield return new WaitUntil(() => effect == null);
                yield return new WaitForSeconds(0.5f);

                for (int i = 0; i < targets.Count; ++i)
                {
                    bool check = targets[i].DmgCheck(structure.player, structure.skillData);
                    if (check)
                    {
                        targets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                    }
                }

                yield return waitTime;
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator PoisonFog(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟설정
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        Vector2 position = CombatManager.Instance.GetPosition(!CombatManager.Instance.homegroundMonsters.Contains(structure.player), 1, 1);
        var effect = Instantiate(poisonFog, position, Quaternion.identity).GetComponent<ParticleSystem>();
        SoundManager.Instance.PlayEffect(73, 1f, true);

        yield return new WaitUntil(() => effect.isPlaying == false);
        SoundManager.Instance.StopEffect(73);
        Destroy(effect.gameObject);
        GameObject effect2 = null;
        for (int i = 0; i < targets.Count; ++i)
        {
            effect2 = Instantiate(poisonHit, targets[i].center.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => effect2 == null);
        yield return waitTime;

        float passiveValue = structure.player.CheckPoisonSkill();
        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].Hurt(structure.skillData.atk * passiveValue);
        }

        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;

            float value = Random.Range(0f, 1f);
            if (value <= structure.skillData.statusRatio * passiveValue)
                effect2 = targets[i].dogDmgStatus.AddPoisenFlag(targets[i], poisenEffect);
        }

        if (effect2 != null)
        {
            yield return new WaitUntil(() => effect2 == null);
            yield return waitTime;
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator PoisonNeedle(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
        int count = 0;
        var dialogTargets = CombatManager.Instance.GetNeighbourTarget(target, out count);

        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(poisonNeedle, target.center.transform.position, Quaternion.identity);
        effect.transform.localScale = new Vector3(CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale.x : -1 * Mathf.Abs(effect.transform.localScale.x), effect.transform.localScale.y, effect.transform.localScale.z);
        var anim = effect.GetComponent<Animator>();
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        yield return waitTime;

        var check = target.DmgCheck(structure.player, structure.skillData);
        if (check)
        {
            anim.Play("exit");
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            float passiveValue = structure.player.CheckPoisonSkill();
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def) * passiveValue, structure.player, structure.skillData);
            if (target != null && target.isDead == false)
            {
                yield return waitTime;

                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio * passiveValue)
                {
                    effect = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);
                    if (effect != null)
                    {
                        yield return new WaitUntil(() => effect == null);
                        yield return waitTime;
                    }
                }
            }
        }
        else
            Destroy(effect);
        yield return waitTime;

        if (check && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator NetBall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(netBall, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }

        float bulletSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);

                if (target != null && target.isDead == false)
                {
                    var effect2 = Instantiate(debuffSpeedEffect, target.center.position, Quaternion.identity);
                    yield return new WaitUntil(() => effect2 == null);
                    yield return new WaitForSeconds(0.25f);

                    target.battleInstance.maxDex = Mathf.Min(9, target.battleInstance.maxDex + (target.battleInstance.maxDex * structure.skillData.buffValue));
                    target.battleInstance.dex = Mathf.Min(target.battleInstance.maxDex, target.battleInstance.dex + (target.battleInstance.dex * structure.skillData.buffValue));
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator Cocktail(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(cocktailEffect, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        bullet.StartPalabola(target.transform);

        yield return new WaitUntil(() => bullet.IsEnd == true);

        //필수(데미지계산)
        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            bullet.transform.rotation = Quaternion.identity;
            anim.Play("exit");
            yield return new WaitUntil(() => anim == null);
            yield return new WaitForSeconds(0.25f);

            int random = Random.Range(0, 10);

            if (random >= 0 && random <= 5)
            {
                yield return StartCoroutine(target.DexRoutine(target.battleInstance.dex, 0f, 1f));
                CombatManager.Instance.battleQueue.RemoveAll(x => x == target);
            }
            else if (random > 5 && random <= 8)
            {
                yield return StartCoroutine(target.RecoverRoutine(target.battleInstance.mp, 0f, 1f, false));
            }
            else if (random > 8 && random <= 9)
            {
                yield return StartCoroutine(target.RecoverRoutine(target.battleInstance.hp, 1f, 1f, true));
            }

            yield return new WaitForSeconds(0.25f);
        }
        else
            Destroy(bullet.gameObject);

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator Cocktail_2(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(cocktailEffect_2, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        bullet.StartPalabola(target.transform);

        yield return new WaitUntil(() => bullet.IsEnd == true);

        //필수(데미지계산)
        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            bullet.transform.rotation = Quaternion.identity;
            anim.Play("exit");
            yield return new WaitUntil(() => anim == null);
            yield return waitTime;
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);

            if (target != null && target.isDead == false)
            {
                yield return waitTime;

                GameObject effect = null;
                List<int> rands = new List<int>() { 0, 1, 2 };
                int randomCount = rands[Random.Range(0, rands.Count)];
                if (randomCount == 0)
                    effect = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                else if (randomCount == 1)
                    effect = target.dogDmgStatus.AddElecFlag(target, palalysisEffect);
                else if (randomCount == 2)
                    effect = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);

                if (effect != null)
                {
                    yield return new WaitUntil(() => effect == null);
                    yield return waitTime;
                }


                rands.Remove(randomCount);
                randomCount = rands[Random.Range(0, rands.Count)];
                if (randomCount == 0)
                    effect = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                else if (randomCount == 1)
                    effect = target.dogDmgStatus.AddElecFlag(target, palalysisEffect);
                else if (randomCount == 2)
                    effect = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);

                if (effect != null)
                {
                    yield return new WaitUntil(() => effect == null);
                    yield return waitTime;
                }
            }
        }
        else
            Destroy(bullet.gameObject);

        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator DeadlyPoison(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟설정
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        var effect = Instantiate(deadPoisonHit, target.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        //맹독상태
        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            effect = target.dogDmgStatus.AddDeadlyPoisonFlag(target, deadlyPoisonEffect);
            yield return new WaitUntil(() => effect == null);
            yield return new WaitForSeconds(0.25f);
        }
        else
            yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator TailAttack(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            var hitEffects = Instantiate(tailAttackEffects, target.center.position, Quaternion.identity);
            var effects = hitEffects.GetComponentsInChildren<Animator>();

            int sumDmg = 0;
            float sumRatio = 0f;
            Animator effect = null;

            int count = Random.Range(2, effects.Length);
            WaitForSeconds waitTime2 = new WaitForSeconds(0.15f);

            for (int i = 0; i < count; ++i)
            {
                effect = effects[i];

                if (target == null)
                {
                    Destroy(hitEffects);
                    break;
                }
                if (target.isDead == true)
                {
                    Destroy(hitEffects);
                    break;
                }
                effects[i].Play("exit");
                sumDmg += (int)structure.skillData.atk;
                sumRatio += structure.skillData.statusRatio;
                yield return waitTime2;
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            if (hitEffects != null)
                Destroy(hitEffects);

            float passiveValue = structure.player.CheckPoisonSkill();
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + sumDmg) - target.battleInstance.def) * passiveValue);

            if (target != null && target.isDead == false)
            {
                float ratio = Random.Range(0f, 1f);
                if (ratio <= sumRatio * passiveValue)
                {
                    yield return new WaitForSeconds(0.5f);
                    var effect2 = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);
                    if (effect2 != null)
                    {
                        yield return new WaitUntil(() => effect2 == null);
                        yield return waitTime;
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (dmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator AcidBall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(acidBallEffect, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        float bulletSpeed = 2.5f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");

                float passiveValue = structure.player.CheckPoisonSkill();
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def) * passiveValue, structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);

                //방어력디버프
                if (target != null && target.isDead == false)
                {
                    var effect2 = Instantiate(debuffShiledEffect, target.center.position, Quaternion.identity);

                    target.battleInstance.def = Mathf.Max(0, target.battleInstance.def - Mathf.Round(target.battleInstance.def * structure.skillData.buffValue));

                    yield return new WaitUntil(() => effect2 == null);
                    yield return new WaitForSeconds(0.25f);
                }

                //독상태
                if (target != null && target.isDead == false)
                {
                    float ratio = Random.Range(0f, 1f);
                    if (ratio <= structure.skillData.statusRatio * passiveValue)
                    {
                        yield return new WaitForSeconds(0.25f);
                        var effect2 = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);
                        if (effect2 != null)
                        {
                            yield return new WaitUntil(() => effect2 == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator PoisonPool(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //타겟설정
        int index = CombatManager.Instance.GetTrapTarget(structure.player);
        Wall outWall = null;
        Animator effect = null;
        CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
        if (outWall == null)
        {
            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(poisonPool, randomPosition - new Vector2(0f, 0.12f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic.Add(index, wall);
        }
        else
        {
            Destroy(outWall.gameObject);

            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(poisonPool, randomPosition - new Vector2(0f, 0.12f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic[index] = wall;
        }
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator PoisonSwirl(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(poisonWindBlast, target.transform.position, Quaternion.identity).GetComponent<Animator>();
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        yield return waitTime;

        int randomCount = Random.Range(2, 7);
        float passiveValue = structure.player.CheckPoisonSkill();
        float sumValue = 0f;
        WaitForSeconds waitTime2 = new WaitForSeconds(0.5f);
        for (int i = 0; i < randomCount; ++i)
        {
            if (target == null)
                break;
            if (target.isDead == true)
                break;

            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            GameObject hitEffect = null;
            if (dmgCheck == true)
            {
                int randomValue = Random.Range(0, 6);
                float value = 1f;
                if (randomValue == 0)
                    value = 0.1f;
                else if (randomValue == 1)
                    value = 0.25f;
                else if (randomValue == 2)
                    value = 0.5f;
                else if (randomValue == 3)
                    value = 0.75f;
                else if (randomValue == 4)
                    value = 1f;
                else if (randomValue == 5)
                    value = 1.5f;

                hitEffect = Instantiate(nearDistanceHitEffect, target.center.position, Quaternion.identity);
                target.Hurt(structure.skillData.atk * passiveValue);

                value = value * (structure.player.battleInstance.atk + structure.skillData.atk);
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), value - target.battleInstance.def), structure.player, structure.skillData);
            }

            sumValue += structure.skillData.statusRatio;
            yield return new WaitUntil(() => hitEffect == null);
            yield return waitTime2;
        }

        effect.Play("exit");
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        if (target != null && target.isDead == false)
        {
            float value = Random.Range(0f, 1f);

            if (value <= sumValue)
            {
                var clone = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);
                if (clone != null)
                {
                    yield return new WaitUntil(() => effect == null);
                    yield return waitTime;
                }
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator PoisonWind(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        SoundManager.Instance.PlayEffect(62, 1f);
        var effect = Instantiate(poisonWind, Vector2.zero, Quaternion.identity);
        effect.transform.localScale = new Vector3(CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale.x : -1 * effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z);

        var effectChild = effect.transform.GetChild(0).GetComponent<Wind>();

        yield return new WaitUntil(() => effectChild.isEnd == true);
        SoundManager.Instance.StopEffect(62);
        Destroy(effect.gameObject);
        yield return waitTime;

        var poisonTargets = new HashSet<EntityMonster>();
        for (int a = 0; a < 3; ++a)
        {
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), a);
            if (targets.Count > 0)
            {
                //데미지
                var dmgTargets = new List<EntityMonster>();
                for (int i = 0; i < targets.Count; ++i)
                {
                    bool check = targets[i].DmgCheck(structure.player, structure.skillData);
                    if (check)
                        dmgTargets.Add(targets[i]);
                }

                for (int i = 0; i < dmgTargets.Count; ++i)
                {
                    if (dmgTargets[i].passiveFlag == true && dmgTargets[i].currentPassiveName == PassiveName.SandHide)
                        effect = Instantiate(airHit, dmgTargets[i].center.position, Quaternion.identity);
                    else
                        effect = Instantiate(airHit, dmgTargets[i].transform.position, Quaternion.identity);
                }

                yield return new WaitUntil(() => effect == null);
                yield return waitTime;

                for (int i = 0; i < dmgTargets.Count; ++i)
                {
                    if (dmgTargets[i].width < 2)
                        dmgTargets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                    else
                    {
                        //즉사
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                            dmgTargets[i].FlyingDead();
                        else
                        {
                            dmgTargets[i].Hurt(structure.skillData.atk, structure.player, structure.skillData);
                        }
                    }
                }
                poisonTargets.UnionWith(dmgTargets);
                yield return waitTime;

                //날리기
                if (a < 2)
                {
                    EntityMonster checkTarget = null;
                    for (int i = 0; i < dmgTargets.Count; ++i)
                    {
                        if (dmgTargets[i] == null)
                            continue;
                        if (dmgTargets[i].isDead == true)
                            continue;
                        if (dmgTargets[i].width >= 2)
                            continue;
                        if (dmgTargets[i].passiveFlag == true && dmgTargets[i].currentPassiveName == PassiveName.SandHide)
                            continue;

                        var target2 = CombatManager.Instance.homegroundMonsters.Contains(dmgTargets[i]) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == dmgTargets[i].width + 1) && (x.height == dmgTargets[i].height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == dmgTargets[i].width + 1) && (x.height == dmgTargets[i].height));
                        if (target2 != null)
                            StartCoroutine(FlyRoutine(dmgTargets[i], target2));
                        else
                            StartCoroutine(FlyRoutine2(dmgTargets[i]));
                        checkTarget = dmgTargets[i];
                        if (dmgTargets[i] != null && dmgTargets[i].isDead == false)
                            checkTarget = dmgTargets[i];
                    }

                    if (checkTarget != null)
                    {
                        yield return new WaitUntil(() => checkTarget == null || (checkTarget != null && checkTarget.checkSwapPosition == false) || (checkTarget != null && checkTarget.isDead == true));
                        yield return waitTime;
                    }
                }
            }
        }

        foreach (var target in poisonTargets)
        {
            if (target == null)
                continue;
            if (target.isDead == true)
                continue;
            effect = target.dogDmgStatus.AddPoisenFlag(target, poisenEffect);
        }

        if (effect != null)
        {
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator AcidRumble(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        //타겟설정
        List<EntityMonster> targets = null;

        if (isHomeground)
            targets = CombatManager.Instance.awayMonsters.ToList();
        else
            targets = CombatManager.Instance.homegroundMonsters.ToList();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var clone = Instantiate(acidRumble, Vector2.zero, Quaternion.identity);
        clone.transform.localScale = new Vector3(isHomeground ? clone.transform.localScale.x : -1 * clone.transform.localScale.x, clone.transform.localScale.y, clone.transform.localScale.z);
        yield return StartCoroutine(clone.AcidRumbleRoutine(targets, structure, waitTime, isHomeground));
        Destroy(clone.gameObject);
        yield return waitTime;

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator Suction(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        int count = 5;
        GameObject effect = null;
        var targetRenderer = target.GetComponent<SpriteRenderer>();
        var originMat = targetRenderer.material;
        targetRenderer.material = suctionMaterial;
        for (int i = 0; i < count; ++i)
        {
            int rand = Random.Range(2, 5);
            for (int j = 0; j < rand; ++j)
            {
                Vector2 offset = Random.insideUnitCircle * 0.75f;
                effect = Instantiate(suctionEffect, (Vector2)target.center.position + offset, Quaternion.identity);
                var effectCom = effect.GetComponent<SuctionEffect>();

                effectCom.Play(structure.player.center.position);
            }

            yield return waitTime;
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        targetRenderer.material = originMat;

        yield return waitTime;

        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            float value = structure.player.battleInstance.maxHp * (structure.skillData.buffValue + structure.player.battleInstance.hpRecoveryRatio);

            target.Hurt(value);

            yield return new WaitForSeconds(0.5f);

            effect = Instantiate(recoveryEffect, structure.player.transform.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            StartCoroutine(structure.player.RecoverRoutine(structure.player.battleInstance.hp, Mathf.Min(structure.player.battleInstance.maxHp, structure.player.battleInstance.hp + value)));
            yield return new WaitUntil(() => structure.player.startRecovery == false);
            yield return waitTime;
        }
        else
            yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;

    }
    private IEnumerator MpSuction(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        GameObject effect = Instantiate(mpSuctionEffect, (Vector2)target.center.position, Quaternion.identity);
        var pala = effect.GetComponent<Palabola>();

        SoundManager.Instance.PlayEffect(85, 1f);
        pala.StartPalabola(structure.player.center, () => { Destroy(effect); });
        var targetRenderer = target.GetComponent<SpriteRenderer>();
        var originMat = targetRenderer.material;
        targetRenderer.material = suctionMaterial;

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        targetRenderer.material = originMat;

        yield return waitTime;

        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            effect = Instantiate(recoveryEffect, structure.player.transform.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            float value = structure.player.battleInstance.maxMp * (structure.skillData.buffValue + structure.player.battleInstance.manaRecoveryRatio);

            //    structure.player.battleInstance.mp = Mathf.Min(structure.player.battleInstance.maxMp, structure.player.battleInstance.mp + value);
            //    target.battleInstance.mp = Mathf.Max(0, target.battleInstance.mp - structure.skillData.buffValue);

            StartCoroutine(structure.player.RecoverRoutine(structure.player.battleInstance.mp, Mathf.Min(structure.player.battleInstance.maxMp, structure.player.battleInstance.mp + value), 1f, false));
            StartCoroutine(target.RecoverRoutine(target.battleInstance.mp, Mathf.Max(0, target.battleInstance.mp - value), 1f, false));
            yield return new WaitUntil(() => structure.player.startRecovery == false);
            yield return new WaitUntil(() => target.startRecovery == false);
            yield return waitTime;
        }
        else
            yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator HitPlant(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟설정
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(hitPlant, target.center.position, Quaternion.identity);

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck)
        {
            target.Hurt(structure.skillData.atk);

            float endValue = Mathf.Max(0, target.battleInstance.mp - structure.skillData.atk);
            StartCoroutine(target.RecoverRoutine(target.battleInstance.mp, endValue, 1f, false));
            yield return new WaitUntil(() => target.startRecovery == false);
            yield return waitTime;
        }
        else
            yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator HitStamp(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(hitStamp, target.transform.position, Quaternion.identity);

        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.5f);

        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck)
        {
            float dmg = Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def);
            var weight = target.battleInstance.monsterWeight;
            if (weight == MonsterWeight.Big)
                dmg = Mathf.Max(1, dmg - structure.skillData.buffValue);
            else if (weight == MonsterWeight.Normal)
                dmg = dmg + structure.skillData.buffValue;
            else if (weight == MonsterWeight.Small)
                dmg = dmg + (structure.skillData.buffValue * 2f);

            target.Hurt(dmg, structure.player, structure.skillData);
            yield return waitTime;
        }
        else
            yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator GrassKnot(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(grassKnot, target.transform.position, Quaternion.identity);
        var anim = effect.GetComponent<Animator>();

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        yield return new WaitForSeconds(1f);
        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck)
        {
            float dmg = Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def);
            var weight = target.battleInstance.monsterWeight;
            if (weight == MonsterWeight.Big)
                dmg = dmg + (structure.skillData.buffValue * 2f);
            else if (weight == MonsterWeight.Middle)
                dmg = dmg + structure.skillData.buffValue;
            else if (weight == MonsterWeight.Small)
                dmg = Mathf.Max(1, dmg - structure.skillData.buffValue);

            SoundManager.Instance.PlayEffect(79, 1f);
            target.Hurt(dmg, structure.player, structure.skillData);
            yield return waitTime;
        }
        else
            yield return waitTime;

        anim.Play("exit");
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator Synthesis(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var renderer = structure.player.GetComponent<SpriteRenderer>();
        var originMaterial = renderer.material;
        renderer.material = suctionMaterial;

        SoundManager.Instance.PlayEffect(90, 1f);
        var effectParent = Instantiate(synthesis, structure.player.transform.position, Quaternion.identity);
        int childCount = effectParent.transform.childCount;

        for (int i = 0; i < childCount; ++i)
        {
            var child = effectParent.transform.GetChild(i).GetComponent<SuctionEffect>();
            child.gameObject.SetActive(true);
            child.Play(structure.player.center.position, false);
            yield return waitTime;
        }
        SoundManager.Instance.StopEffect(90);
        Destroy(effectParent);
        renderer.material = originMaterial;

        yield return waitTime;

        var effect = Instantiate(recoveryEffect, structure.player.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        structure.player.StartCoroutine(structure.player.RecoverRoutine(structure.player.battleInstance.hp, Mathf.Min(structure.player.battleInstance.maxHp, structure.player.battleInstance.hp + structure.player.battleInstance.maxHp * (structure.skillData.buffValue + structure.player.battleInstance.hpRecoveryRatio))));
        yield return new WaitUntil(() => structure.player.startRecovery == false);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator LeafBlade(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        var leafEffect = Instantiate(leafBlade, target.transform.position, Quaternion.identity).GetComponent<Animator>();
        yield return new WaitUntil(() => leafEffect.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        yield return new WaitForSeconds(0.75f);

        Destroy(leafEffect.gameObject);
        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            if (target != null && target.isDead == false)
                target.sternStatus.Start(target, woodStern, target.transform, 0f);
            yield return waitTime;
        }
        else
            yield return waitTime;

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator WoodBullet(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(woodBullet, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }
        yield return new WaitForSeconds(1f);

        float bulletSpeed = 3f;
        float addSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                int aValue, bValue;
                var weight = structure.player.battleInstance.monsterWeight;
                if (weight == MonsterWeight.Big)
                    aValue = 4;
                else if (weight == MonsterWeight.Middle)
                    aValue = 3;
                else if (weight == MonsterWeight.Normal)
                    aValue = 2;
                else
                    aValue = 1;

                weight = target.battleInstance.monsterWeight;
                if (weight == MonsterWeight.Big)
                    bValue = 4;
                else if (weight == MonsterWeight.Middle)
                    bValue = 3;
                else if (weight == MonsterWeight.Normal)
                    bValue = 2;
                else
                    bValue = 1;

                float value = aValue - bValue;
                float dmg = (value < 0) ? Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def) / (Mathf.Abs(value) + 1) : Mathf.Max(1, (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def) * (value + 1);

                anim.transform.rotation = Quaternion.identity;
                anim.Play("exit");
                target.Hurt(dmg, structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                Destroy(bullet);
                yield return new WaitForSeconds(0.25f);
            }
        }
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator HeavyWeight(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(changeWeight, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        var weight = structure.player.battleInstance.monsterWeight;

        if (weight != MonsterWeight.Big)
        {
            effect = Instantiate(buffShiledEffect, structure.player.center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            effect = Instantiate(debuffSpeedEffect, structure.player.center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            if (weight == MonsterWeight.Small)
            {
                structure.player.battleInstance.def += structure.skillData.buffValue;
                structure.player.battleInstance.maxDex = Mathf.Min(9f, structure.player.battleInstance.maxDex + 0.5f);
                structure.player.battleInstance.monsterWeight = MonsterWeight.Normal;
            }
            else if (weight == MonsterWeight.Normal)
            {
                structure.player.battleInstance.def += structure.skillData.buffValue;
                structure.player.battleInstance.maxDex = Mathf.Min(9f, structure.player.battleInstance.maxDex + 0.5f);
                structure.player.battleInstance.monsterWeight = MonsterWeight.Middle;
            }
            else if (weight == MonsterWeight.Middle)
            {
                structure.player.battleInstance.def += structure.skillData.buffValue;
                structure.player.battleInstance.maxDex = Mathf.Min(9f, structure.player.battleInstance.maxDex + 0.5f);
                structure.player.battleInstance.monsterWeight = MonsterWeight.Big;
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator LightWeight(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var weight = structure.player.battleInstance.monsterWeight;

        var effect = Instantiate(changeWeight, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        if (weight != MonsterWeight.Small)
        {
            effect = Instantiate(buffSpeedEffect, structure.player.center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            effect = Instantiate(debuffShiledEffect, structure.player.center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            if (weight == MonsterWeight.Normal)
            {
                structure.player.battleInstance.def -= structure.skillData.buffValue;
                structure.player.battleInstance.maxDex = Mathf.Max(0.1f, structure.player.battleInstance.maxDex - 0.5f);
                //   structure.player.battleInstance.dex = Mathf.Min(structure.player.battleInstance.maxDex, structure.player.battleInstance.dex);
                structure.player.battleInstance.monsterWeight = MonsterWeight.Small;
            }
            else if (weight == MonsterWeight.Middle)
            {
                structure.player.battleInstance.def -= structure.skillData.buffValue;
                structure.player.battleInstance.maxDex = Mathf.Max(0.1f, structure.player.battleInstance.maxDex - 0.5f);
                //    structure.player.battleInstance.dex = Mathf.Min(structure.player.battleInstance.maxDex, structure.player.battleInstance.dex);
                structure.player.battleInstance.monsterWeight = MonsterWeight.Normal;
            }
            else if (weight == MonsterWeight.Big)
            {
                structure.player.battleInstance.def -= structure.skillData.buffValue;
                structure.player.battleInstance.maxDex = Mathf.Max(0.1f, structure.player.battleInstance.maxDex - 0.5f);
                //    structure.player.battleInstance.dex = Mathf.Min(structure.player.battleInstance.maxDex, structure.player.battleInstance.dex);
                structure.player.battleInstance.monsterWeight = MonsterWeight.Middle;
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator FormChange3(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        //민첩초기화
        structure.player.battleInstance.dex = 0;



        //연출
        Animator backgroundAnim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        backgroundAnim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => backgroundAnim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var clone = structure.player.SwapMonster(forestGod, structure.skillData.consumMpAmount);
        float buffValue = Mathf.RoundToInt(structure.player.originInstance.maxHp * 0.25f);
        clone.battleInstance.maxHp += buffValue;
        clone.battleInstance.hp += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.maxMp * 0.25f);
        clone.battleInstance.maxMp += buffValue;
        clone.battleInstance.mp += buffValue;


        buffValue = Mathf.RoundToInt(structure.player.originInstance.atk * 0.25f);
        clone.battleInstance.atk += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.def * 0.25f);
        clone.battleInstance.def += buffValue;
        structure.player = clone;

        var anim = clone.GetComponent<Animator>();
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        yield return new WaitForSeconds(0.5f);


        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].sternStatus.Start(targets[i], woodCrash, targets[i].transform, 7f, !CombatManager.Instance.homegroundMonsters.Contains(structure.player));
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].Hurt(structure.skillData.atk);
        }

        yield return waitTime;

        backgroundAnim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => backgroundAnim == null);
        yield return waitTime;

        //필수
        isBlock = false;
    }
    private IEnumerator NaturePower(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟설정
        var targets = CombatManager.Instance.dotDmgObjList.FindAll(x => x.dogDmgStatus.CheckBadStatus() == true);
        bool isHomground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        targets.RemoveAll(x => isHomground != CombatManager.Instance.homegroundMonsters.Contains(x));

        //연출
        if (targets.Count > 0)
        {
            var target = CombatManager.Instance.GetDetailTarget(SelectDetailTargetType.Dot_Damage, structure.skillData, targets);
            var renderer = target.GetComponent<SpriteRenderer>();
            var originMat = renderer.material;
            renderer.material = suctionMaterial;
            var effect = Instantiate(naturePower, target.center.position, Quaternion.identity);
            SoundManager.Instance.PlayEffect(90, 1f);
            yield return new WaitUntil(() => effect == null);
            SoundManager.Instance.StopEffect(90);
            yield return waitTime;

            target.dogDmgStatus.ClearBadStatus(target);
            renderer.material = originMat;

            yield return waitTime;

            //반격기확인
            if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
            {
                //타겟가져오기
                targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
                if (target != null)
                    structure.player.CheckFireBaracesSkill(target);
            }
            //필수
            isBlock = false;
        }
        else
            yield return StartCoroutine(NoManaRoutine(structure));
    }
    private IEnumerator GivingTree(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟설정
        bool isHomground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        var targets = isHomground ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        targets = targets.ToList();
        targets.Remove(structure.player);

        //연출
        if (targets.Count > 0)
        {
            var effect = Instantiate(givingTree, structure.player.center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            structure.player.Hurt(structure.skillData.buffValue);

            yield return waitTime;

            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(recoveryEffect, targets[i].transform.position, Quaternion.identity);
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            for (int i = 0; i < targets.Count; ++i)
            {
                StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.hp, Mathf.Min(targets[i].battleInstance.maxHp, targets[i].battleInstance.hp + structure.skillData.buffValue)));
            }
            yield return new WaitUntil(() => targets[targets.Count - 1].startRecovery == false);
            yield return waitTime;

            if (structure.player != null && structure.player.isDead == false)
            {
                //반격기확인
                if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
                {
                    //타겟가져오기
                    targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                    var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
                    if (target != null)
                        structure.player.CheckFireBaracesSkill(target);
                }
            }

            //필수
            isBlock = false;
        }
        else
            yield return StartCoroutine(NoManaRoutine(structure));
    }
    private IEnumerator ShadowBall(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(shadowBall, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        float bulletSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);
                //화상상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= structure.skillData.statusRatio)
                    {
                        var clone = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator DemonsEyes(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var effect = Instantiate(demonsEyes, structure.player.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        effect = Instantiate(demonsEyesPoint, target.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        target.ClearAbility();

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator Consume(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟가져오기
        var targets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        targets = targets.ToList();
        targets.Remove(structure.player);

        //연출
        if (targets.Count > 0)
        {
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            Vector3 startPosition = structure.player.transform.localPosition;
            Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
            Transform shaodw = structure.player.transform.GetChild(4);
            Transform cavas = structure.player.transform.GetChild(0);
            Vector3 shadowOriginPosition = shaodw.position;
            Vector3 cavasPosition = cavas.position;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);

            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            var effect = Instantiate(destinyBondEffect, target.transform.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return new WaitForSeconds(0.25f);

            target.Dead();
            effect = Instantiate(recoveryEffect, structure.player.transform.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return new WaitForSeconds(0.25f);

            float endHp = Mathf.Min(structure.player.battleInstance.maxHp, structure.player.battleInstance.hp + structure.skillData.buffValue);
            float endMp = Mathf.Min(structure.player.battleInstance.maxMp, structure.player.battleInstance.mp + structure.skillData.buffValue);
            float startHp = structure.player.battleInstance.hp;
            float startMp = structure.player.battleInstance.mp;

            lerpSpeed = 1f;
            currentTime = 0f;
            lerpTime = 1f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.battleInstance.hp = Mathf.Lerp(startHp, endHp, currentSpeed);
                structure.player.battleInstance.mp = Mathf.Lerp(startMp, endMp, currentSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);

            //반격기확인
            if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
            {
                //타겟가져오기
                targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
                if (target != null)
                    structure.player.CheckFireBaracesSkill(target);
            }
            //필수
            isBlock = false;
        }
        else
            yield return StartCoroutine(NoManaRoutine(structure));
    }
    private IEnumerator SoulSlash(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(soulSlashEffect, target.center.position, Quaternion.identity);
        effect.transform.localScale = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale : new Vector3(-1 * effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            target.Hurt(target.battleInstance.hp * 0.5f);
            yield return waitTime;

            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    var clone = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    yield return new WaitUntil(() => clone == null);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        else
            yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator SoulAttack(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;
        SpriteRenderer renderer = structure.player.GetComponent<SpriteRenderer>();

        float startA = structure.player.battleInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().color.a;
        float endA = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(startA, endA, currentSpeed));
            yield return null;
        }

        yield return waitTime;

        var bullet = Instantiate(soulAttackEffect, structure.player.center.position, Quaternion.identity);
        var anim = bullet.GetComponent<Animator>();

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        yield return waitTime;

        float bulletSpeed = 7f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        while (m > bulletSpeed * Time.deltaTime)
        {
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            anim.Play("exit");
            yield return new WaitUntil(() => anim == null);
            yield return waitTime;

            target.Hurt(structure.skillData.atk, structure.player, structure.skillData);
            yield return waitTime;
            //화상상태
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    var clone = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    yield return new WaitUntil(() => clone == null);
                    yield return waitTime;
                }
            }
        }
        else
        {
            Destroy(bullet);
            yield return waitTime;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(endA, startA, currentSpeed));
            yield return null;
        }

        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator EvilSoul(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);
        targets.Reverse();

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        Vector2 effectPosition = Vector2.zero;
        float reverseX = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? 2.28f : -5.48f;
        if (index == 0)
            effectPosition = new Vector2(reverseX, -1.74f);
        else if (index == 1)
            effectPosition = new Vector2(reverseX, 0f);
        else if (index == 2)
            effectPosition = new Vector2(reverseX, 1.74f);

        SoundManager.Instance.PlayEffect(132, 1f);
        var effect = Instantiate(evilSoulParent, effectPosition, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        SoundManager.Instance.StopEffect(132);
        Destroy(effect);

        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].Hurt(structure.skillData.atk * deadCount + 1);
        }

        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;
            float ratio = Random.Range(0f, 1f);
            if (ratio <= structure.skillData.statusRatio)
            {
                effect = targets[i].dogDmgStatus.AddUnableFlag(targets[i], unableEffect);
                yield return new WaitUntil(() => effect == null);
                yield return waitTime;
            }
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator MindControl(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출

        var newPositions = CombatManager.Instance.GetEmptyPositions(!CombatManager.Instance.homegroundMonsters.Contains(target));
        if (newPositions.Count > 0)
        {
            Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
            anim.Play("SpecialAttack_Background");

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
            yield return waitTime;

            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            Vector3 startPosition = structure.player.transform.localPosition;
            Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
            Transform shaodw = structure.player.transform.GetChild(4);
            Transform cavas = structure.player.transform.GetChild(0);
            Vector3 shadowOriginPosition = shaodw.position;
            Vector3 cavasPosition = cavas.position;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            yield return waitTime;


            var effect = Instantiate(mindControlEffect, target.center.position, Quaternion.identity);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                target.checkMindControl = true;
                effect = Instantiate(mindControlSucessEffect, target.center.position, Quaternion.identity);
                yield return new WaitUntil(() => effect == null);
                yield return waitTime;

                target.dogDmgStatus.ClearBadStatus(target);
                target.sternStatus.Stop(target);
                target.ResetPassiveSkill();

                bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(target);
                if (isHomeground == true)
                {
                    CombatManager.Instance.homegroundMonsters.Remove(target);
                    CombatManager.Instance.awayMonsters.Add(target);
                }
                else
                {
                    CombatManager.Instance.awayMonsters.Remove(target);
                    CombatManager.Instance.homegroundMonsters.Add(target);
                }
                target.transform.localScale = new Vector3(-1 * target.transform.localScale.x, target.transform.localScale.y, target.transform.localScale.z);
                target.transform.GetChild(0).GetComponent<StatusUI>().Init();
                target.ResetPassiveSkill();

                var newPosition = newPositions[Random.Range(0, newPositions.Count)];
                target.width = newPosition.width;
                target.height = newPosition.height;

                startPosition = target.transform.position;
                endPosition = newPosition.worldPosition + new Vector2(0, target.originInstance.monsterData.monsterPrefab.transform.position.y);

                lerpSpeed = 4f;
                currentTime = 0f;
                lerpTime = 1f;

                while (currentTime < lerpTime)
                {
                    currentTime += Time.deltaTime * lerpSpeed;

                    float currentSpeed = currentTime / lerpTime;
                    target.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                    yield return null;
                }

                lerpSpeed = 2f;
                currentTime = 0f;
                lerpTime = 1f;
                var renderer = target.GetComponent<SpriteRenderer>();

                float startColor = renderer.color.r;
                float endColor = 0.4f;

                while (currentTime < lerpTime)
                {
                    currentTime += Time.deltaTime * lerpSpeed;

                    float currentSpeed = currentTime / lerpTime;
                    float color = Mathf.Lerp(startColor, endColor, currentSpeed);
                    renderer.color = new Color(color, color, color, renderer.color.a);
                    yield return null;
                }

                yield return waitTime;

            }
            else
                yield return waitTime;

            anim.Play("SpecialAttack_Background2");
            yield return new WaitUntil(() => anim == null);
            yield return waitTime;


            //반격기확인
            if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
            {
                //타겟가져오기
                targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
                if (target != null)
                    structure.player.CheckFireBaracesSkill(target);
            }
            //필수
            isBlock = false;
            CombatManager.Instance.SetFormationBuffDebuffs();
        }
        else
            yield return StartCoroutine(NoManaRoutine(structure));
    }
    private IEnumerator TrickRoom(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        CombatManager.Instance.PauseGame();
        var effect = Instantiate(trickRoomEffect, Vector3.zero, Quaternion.identity).GetComponent<Animator>();
        effect.speed = CombatManager.Instance.timeScale;
        yield return new WaitUntil(() => effect == null);
        CombatManager.Instance.RestartGame();
        yield return new WaitForSeconds(0.25f);

        var homegroundMonsters = CombatManager.Instance.homegroundMonsters;
        var awayMonsters = CombatManager.Instance.awayMonsters;

        for (int i = 0; i < homegroundMonsters.Count; ++i)
        {
            StartCoroutine(homegroundMonsters[i].DexRoutine(homegroundMonsters[i].battleInstance.dex, 0, 1f));
            CombatManager.Instance.battleQueue.RemoveAll(x => x == homegroundMonsters[i]);
            homegroundMonsters[i].battleInstance.maxDex = Mathf.Clamp(9f - homegroundMonsters[i].battleInstance.maxDex, 0.1f, 9f);
        }
        for (int i = 0; i < awayMonsters.Count; ++i)
        {
            StartCoroutine(awayMonsters[i].DexRoutine(awayMonsters[i].battleInstance.dex, 0, 1f));
            CombatManager.Instance.battleQueue.RemoveAll(x => x == awayMonsters[i]);
            awayMonsters[i].battleInstance.maxDex = Mathf.Clamp(9f - awayMonsters[i].battleInstance.maxDex, 0.1f, 9f);
        }

        yield return new WaitUntil(() => awayMonsters[0].startRecovery == false);
        yield return new WaitForSeconds(0.25f);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator SkullDice(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        bool active = target.shieldCreatePoint.gameObject.activeInHierarchy;
        EntityMonster shildTarget = null;
        if (active == true)
        {
            shildTarget = target;
            shildTarget.shieldCreatePoint.gameObject.SetActive(false);
        }

        var dice = Instantiate(skullDice, target.shieldCreatePoint.transform.position, Quaternion.identity);
        dice.Roll();
        yield return new WaitUntil(() => dice.startRolled == false);
        yield return waitTime;

        var selectNumber = dice.pickUpContent.number;

        Destroy(dice.gameObject);

        target = (selectNumber != 0) ? target : structure.player;
        if (selectNumber == 0)
            selectNumber = 6;
        var effect = Instantiate(debuffAttackEffect, target.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        effect = Instantiate(debuffShiledEffect, target.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        target.battleInstance.atk = Mathf.Max(0, target.battleInstance.atk - selectNumber);
        target.battleInstance.def = Mathf.Max(0, target.battleInstance.def - selectNumber);

        if (shildTarget != null)
        {
            shildTarget.shieldCreatePoint.gameObject.SetActive(true);
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator Wedge(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        EntityMonster target_0 = null, target_1 = null;
        CombatManager.Instance.GetRandomDoubleTarget(structure.player, structure.skillData, out target_0, out target_1);

        if (target_0 != null && target_1 != null)
        {
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            Vector3 startPosition = structure.player.transform.localPosition;
            Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
            Transform shaodw = structure.player.transform.GetChild(4);
            Transform cavas = structure.player.transform.GetChild(0);
            Vector3 shadowOriginPosition = shaodw.position;
            Vector3 cavasPosition = cavas.position;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            yield return waitTime;

            Instantiate(mindControlSucessEffect, target_0.center.position, Quaternion.identity);
            var effect = Instantiate(mindControlSucessEffect, target_1.center.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            Instantiate(nearDistanceHitEffect, target_0.center.position, Quaternion.identity);
            effect = Instantiate(nearDistanceHitEffect, target_1.center.position, Quaternion.identity);

            target_0.Hurt(Mathf.Max(target_0.GetDefenseDeal(), target_1.battleInstance.atk - target_0.battleInstance.def), structure.player, structure.skillData);
            target_1.Hurt(Mathf.Max(target_1.GetDefenseDeal(), target_0.battleInstance.atk - target_1.battleInstance.def), structure.player, structure.skillData);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            //반격기확인
            if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
            {
                //타겟가져오기
                var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
                if (target != null)
                    structure.player.CheckFireBaracesSkill(target);
            }
            //필수
            isBlock = false;
        }
        else
        {
            yield return StartCoroutine(NoManaRoutine(structure));
        }
    }
    private IEnumerator SummonsMagic(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var emptyPositions = CombatManager.Instance.GetEmptyPositions(CombatManager.Instance.homegroundMonsters.Contains(structure.player));
        if (emptyPositions.Count > 0)
        {
            var emptyPosition = emptyPositions[Random.Range(0, emptyPositions.Count)];

            var effect = Instantiate(summonsBook, structure.player.center.position, Quaternion.identity);
            SoundManager.Instance.PlayEffect(118, 1f);
            SoundManager.Instance.PlayEffect(117, 0.5f);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            Vector2 heightDisOffset = new Vector2(CombatManager.Instance.heightDistance * (emptyPosition.height + 1), 0);
            effect = Instantiate(magiceCicle, emptyPosition.worldPosition, magiceCicle.transform.rotation);
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            var data = structure.skillData.summonsMonsterDatas[Random.Range(0, structure.skillData.summonsMonsterDatas.Length)];
            var monsterInstance = MonsterInstance.Instance(data);

            EntityMonster.CreateMonster(monsterInstance, emptyPosition.worldPosition - heightDisOffset, emptyPosition.width, emptyPosition.height, CombatManager.Instance.homegroundMonsters.Contains(structure.player));
            yield return waitTime;

            //반격기확인
            if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
            {
                //타겟가져오기
                var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
                if (target != null)
                    structure.player.CheckFireBaracesSkill(target);
            }
            //필수
            isBlock = false;
        }
        else
        {
            yield return StartCoroutine(NoManaRoutine(structure));
        }
    }
    private IEnumerator Paradox(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var effect = Instantiate(paradox, structure.player.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        yield return StartCoroutine(structure.player.RecoverRoutine(structure.player.battleInstance.mp, Mathf.Min(structure.player.battleInstance.maxMp, structure.player.battleInstance.mp + Mathf.RoundToInt((structure.player.battleInstance.maxMp * structure.skillData.buffValue))), 1f, false));
        yield return waitTime;

        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);

        if (dmgCheck == true)
        {
            effect = Instantiate(surpriseAttackEffect);
            effect.transform.position = target.center.position;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
            yield return waitTime;
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value < structure.skillData.statusRatio)
                {
                    effect = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    yield return new WaitUntil(() => effect == null);
                    yield return waitTime;
                }
            }
        }
        else
            yield return waitTime;

        if (dmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return waitTime;
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return waitTime;
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator Doppelganger(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //타겟가져오기
        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        var targets = CombatManager.Instance.GetSortTarget(isHomeground, structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(mindControlSucessEffect, structure.player.center.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        var entityMonster = EntityMonster.CreateMonster(target.originInstance, structure.player.transform.position, structure.player.width, structure.player.height, isHomeground, false);
        entityMonster.battleInstance.skillDatas.Clear();
        entityMonster.battleInstance.percentSkillDatas.Clear();
        entityMonster.battleInstance.triggerSkillDatas.Clear();
        entityMonster.battleInstance.skillDatas.AddRange(structure.player.battleInstance.skillDatas);
        entityMonster.battleInstance.percentSkillDatas.AddRange(structure.player.battleInstance.percentSkillDatas);
        entityMonster.battleInstance.triggerSkillDatas.AddRange(structure.player.battleInstance.triggerSkillDatas);
        structure.player.Remove();

        entityMonster.battleInstance.skillDatas.RemoveAll(x => x == structure.skillData);
        entityMonster.battleInstance.percentSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        entityMonster.battleInstance.triggerSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        if (entityMonster.battleInstance.skillDatas.Count <= 0)
            entityMonster.battleInstance.skillDatas.Add(headBut);

        Destroy(structure.player.gameObject);

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator Pandora(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        List<EntityMonster> targets = null;
        int random = Random.Range(0, 2);
        if (random == 0)
            targets = isHomeground ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        else
            targets = isHomeground ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        //연출
        var effect = Instantiate(pandora, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        random = Random.Range(0, 5);

        if (random == 1)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(buffAttackEffect, targets[i].center.position, Quaternion.identity);
                targets[i].battleInstance.atk += Mathf.RoundToInt((targets[i].battleInstance.atk * structure.skillData.buffValue));
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 2)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(buffShiledEffect, targets[i].center.position, Quaternion.identity);
                targets[i].battleInstance.def += Mathf.RoundToInt((Mathf.Max(1, targets[i].battleInstance.def) * structure.skillData.buffValue));
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 3)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(buffSpeedEffect, targets[i].center.position, Quaternion.identity);
                targets[i].battleInstance.maxDex = Mathf.Max(0.1f, targets[i].battleInstance.maxDex - (targets[i].battleInstance.maxDex * structure.skillData.buffValue));
                targets[i].battleInstance.dex = Mathf.Max(0f, targets[i].battleInstance.dex - (targets[i].battleInstance.dex * structure.skillData.buffValue));
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 4)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(debuffAttackEffect, targets[i].center.position, Quaternion.identity);
                targets[i].battleInstance.atk = Mathf.RoundToInt(Mathf.Max(0, targets[i].battleInstance.atk - (targets[i].battleInstance.atk * structure.skillData.buffValue)));
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 5)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(debuffShiledEffect, targets[i].center.position, Quaternion.identity);
                targets[i].battleInstance.def = Mathf.RoundToInt(Mathf.Max(0, targets[i].battleInstance.def - (targets[i].battleInstance.def * structure.skillData.buffValue)));
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 6)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(debuffSpeedEffect, targets[i].center.position, Quaternion.identity);
                targets[i].battleInstance.maxDex = Mathf.Min(9f, targets[i].battleInstance.maxDex + (targets[i].battleInstance.maxDex * structure.skillData.buffValue));
                targets[i].battleInstance.dex = Mathf.Min(9f, targets[i].battleInstance.dex + (targets[i].battleInstance.dex * structure.skillData.buffValue));
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 7)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = Instantiate(mindControlSucessEffect, targets[i].center.position, Quaternion.identity);

            }
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            for (int i = 0; i < targets.Count; ++i)
            {
                targets[i].battleInstance.dex = 0f;

            }
        }
        else if (random == 8)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = targets[i].dogDmgStatus.AddUnableFlag(targets[i], unableEffect);

            }
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 9)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = targets[i].dogDmgStatus.AddPoisenFlag(targets[i], poisenEffect);

            }
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 10)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                effect = targets[i].dogDmgStatus.AddElecFlag(targets[i], palalysisEffect);

            }
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
        }
        else if (random == 11)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.hp, 1f));

            }
            yield return new WaitUntil(() => targets[targets.Count - 1].startRecovery == false);
            yield return waitTime;
        }
        else if (random == 12)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                float hp = targets[i].battleInstance.maxHp;
                StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.hp, hp));

            }
            yield return new WaitUntil(() => targets[targets.Count - 1].startRecovery == false);
            yield return waitTime;
        }
        else if (random == 13)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.mp, 0f, 1f, false));

            }
            yield return new WaitUntil(() => targets[targets.Count - 1].startRecovery == false);
            yield return waitTime;
        }
        else if (random == 14)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                float mp = targets[i].battleInstance.maxMp;
                StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.mp, mp, 1f, false));

            }
            yield return new WaitUntil(() => targets[targets.Count - 1].startRecovery == false);
            yield return waitTime;
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator DarkHole(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        //타겟설정
        int index = CombatManager.Instance.GetTrapTarget(structure.player);
        Wall outWall = null;
        Animator effect = null;
        CombatManager.Instance.trapDic.TryGetValue(index, out outWall);
        if (outWall == null)
        {
            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(darkHole, randomPosition - new Vector2(0f, 0.33f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic.Add(index, wall);
        }
        else
        {
            Destroy(outWall.gameObject);

            var randomPosition = CombatManager.Instance.trapPositionDic[index];
            effect = Instantiate(darkHole, randomPosition - new Vector2(0f, 0.33f), Quaternion.identity).GetComponent<Animator>();

            var wall = effect.GetComponent<Wall>();
            wall.index = index;
            wall.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
            CombatManager.Instance.trapDic[index] = wall;
        }
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator Timeleap(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var trapList = CombatManager.Instance.trapDic.Values.ToList();
        for (int i = 0; i < trapList.Count; ++i)
            trapList[i].Remove();

        var cloneTargets = CombatManager.Instance.allMonsters.FindAll(x => x.isClone == true);
        for (int i = 0; i < cloneTargets.Count; ++i)
            cloneTargets[i].CloneDead();

        var effect = Instantiate(timeleap, structure.player.center.position, Quaternion.identity);

        yield return new WaitForSeconds(1.2f);
        

        var targets = CombatManager.Instance.allMonsters;
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;

            float hp = targets[i].originInstance.hp;
            if (targets[i].originInstance.heathState == MonsterHeathState.Crippled)
                hp = hp * 0.5f;
            else if (targets[i].originInstance.heathState == MonsterHeathState.CrippedStrong)
                hp = hp * 0.25f;
            float mp = targets[i].originInstance.mp;
            if (targets[i].originInstance.heathState == MonsterHeathState.Crippled)
                mp = mp * 0.5f;
            else if (targets[i].originInstance.heathState == MonsterHeathState.CrippedStrong)
                mp = mp * 0.25f;

            float dex = targets[i].originInstance.abilities.Contains(AbilityType.Spurt) ? (targets[i].originInstance.maxDex * 0.33f) : 0f;
            if (targets[i].originInstance.heathState == MonsterHeathState.Crippled)
                dex = dex * 0.5f;
            else if (targets[i].originInstance.heathState == MonsterHeathState.CrippedStrong)
                dex = dex * 0.25f;


            targets[i].battleInstance.atk = targets[i].originInstance.atk;
            if (targets[i].originInstance.heathState == MonsterHeathState.FullCondition)
                targets[i].battleInstance.atk = targets[i].battleInstance.atk * 2f;
            targets[i].battleInstance.def = targets[i].originInstance.def;
            if (targets[i].originInstance.heathState == MonsterHeathState.FullCondition)
                targets[i].battleInstance.def = targets[i].battleInstance.def * 2f;
            targets[i].battleInstance.maxDex = targets[i].originInstance.maxDex;
            if (targets[i].originInstance.heathState == MonsterHeathState.FullCondition)
                targets[i].battleInstance.maxDex = targets[i].battleInstance.maxDex * 0.5f;

            targets[i].battleInstance.hpRecoveryRatio = targets[i].originInstance.hpRecoveryRatio;
            targets[i].battleInstance.manaRecoveryRatio = targets[i].originInstance.manaRecoveryRatio;
            targets[i].battleInstance.creaticalRatio = targets[i].originInstance.creaticalRatio;
            targets[i].battleInstance.repeatRatio = targets[i].originInstance.repeatRatio;

            targets[i].battleInstance.skillDatas.Clear();
            targets[i].battleInstance.skillDatas.AddRange(targets[i].originInstance.skillDatas);

            targets[i].battleInstance.percentSkillDatas.Clear();
            targets[i].battleInstance.percentSkillDatas.AddRange(targets[i].originInstance.percentSkillDatas);

            targets[i].battleInstance.triggerSkillDatas.Clear();
            targets[i].battleInstance.triggerSkillDatas.AddRange(targets[i].originInstance.triggerSkillDatas);

            targets[i].battleInstance.abilities.Clear();
            targets[i].battleInstance.abilities.AddRange(targets[i].originInstance.abilities);

            targets[i].battleInstance.currentConfirmSkillPriority = targets[i].originInstance.currentConfirmSkillPriority;
            targets[i].battleInstance.currentSelectDetailTargetType = targets[i].originInstance.currentSelectDetailTargetType;

            targets[i].checkFirstTurn = false;
            targets[i].unbreakableTarget = null;
            targets[i].confirmSkillIndex = 0;
            targets[i].fireVowStack = 3;
            targets[i].addHolyStatus = 0f;

            targets[i].sternStatus.Stop(targets[i]);
            targets[i].dogDmgStatus.Clear(targets[i]);

            targets[i].startLivingDeadFlag = false;
            targets[i].checkLivingDeadHolyHurt = false;
            CombatManager.Instance.livingDeadAllDeadFlag = false;

            for (int j = 0; j < rockClones.Count; ++j)
            {
                Destroy(rockClones[j].gameObject);
            }

            rockClones.Clear();
            CombatManager.Instance.rockBlasterTimer = 0f;

            homegroundDawnCount = 0;
            awayDawnCount = 0;
            dawnTimer = 0f;

            darkHoleList.Clear();

            sheerColdCount = 0;

            StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.hp, hp, 1f, true));
            StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.mp, mp, 1f, false));
            StartCoroutine(targets[i].DexRoutine(targets[i].battleInstance.dex, dex, 1f));
            targets[i].ResetPassiveSkill();
        }
        float start = CombatManager.Instance.battleTimer;
        float end = CombatManager.Instance.battleMaxTimer - 1;

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            CombatManager.Instance.battleTimer = Mathf.Lerp(start, end, currentSpeed);
            CombatUI.Instance.RenderBattleTimer();
            yield return null;
        }

        yield return new WaitUntil(() => targets[targets.Count - 1].startRecovery == false);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        targets = CombatManager.Instance.allMonsters;
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].isClone == false && targets[i].checkMindControl == true)
            {
                StartCoroutine(MindControlReturn(targets[i]));
            }
        }

        yield return null;

        if (mindControlReturnTarget != null)
        {
            yield return new WaitUntil(() => mindControlReturnTarget.checkMindControl == false);
            yield return new WaitForSeconds(0.25f);
            mindControlReturnTarget = null;
        }

        structure.player.battleInstance.skillDatas.RemoveAll(x => x == structure.skillData);
        structure.player.battleInstance.percentSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        structure.player.battleInstance.triggerSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        if (structure.player.battleInstance.skillDatas.Count <= 0)
            structure.player.battleInstance.skillDatas.Add(headBut);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator DarkLoad(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        WaitForSeconds waitTime2 = new WaitForSeconds(0.1f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        SoundManager.Instance.PlayEffect(118, 1f);
        SoundManager.Instance.PlayEffect(117, 0.5f);
        var effect = Instantiate(summonsBook, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        Vector2 offsetPosition = isHomeground ? new Vector2(3.5f, 2f) : new Vector2(-3.5f, 2f);
        var targets = isHomeground ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        targets = targets.ToList();
        effect = Instantiate(darkLoad, offsetPosition, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < targets.Count; ++j)
            {
                if (targets[j] == null)
                    continue;
                if (targets[j].isDead == true)
                    continue;
                var dmgCheck = targets[j].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck == true)
                {
                    int dmgPerValue = Random.Range(0, 4);
                    float value = 0f;
                    if (dmgPerValue == 0)
                        value = 0.25f;
                    else if (dmgPerValue == 1)
                        value = 0.5f;
                    else if (dmgPerValue == 2)
                        value = 0.75f;
                    else if (dmgPerValue == 3)
                        value = 1f;

                    var effect2 = Instantiate(nearDistanceHitEffect, targets[j].center.position, Quaternion.identity);
                    targets[j].Hurt(Mathf.Max(targets[j].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[j].battleInstance.def) * value, structure.player, structure.skillData);
                    targets[j].sumStatusRatio += structure.skillData.statusRatio;
                }
            }
            yield return waitTime;
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        var statusTargets = targets.FindAll(x => (x != null && x.isDead == false) && x.sumStatusRatio > 0f);
        GameObject clone = null;

        for (int i = 0; i < statusTargets.Count; ++i)
        {
            float value = Random.Range(0f, 1f);
            if (value <= (statusTargets[i].sumStatusRatio + structure.player.addHolyStatus))
            {
                clone = statusTargets[i].dogDmgStatus.AddBurnFlag(statusTargets[i], burnEffect);
                statusTargets[i].sumStatusRatio = -1f;

            }
            else
                statusTargets[i].sumStatusRatio = 0f;
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        statusTargets = targets.FindAll(x => (x != null && x.isDead == false) && x.sumStatusRatio == -1f);
        for (int i = 0; i < statusTargets.Count; ++i)
        {
            clone = statusTargets[i].dogDmgStatus.AddUnableFlag(statusTargets[i], unableEffect);
            statusTargets[i].sumStatusRatio = 0f;
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;
            targets[i].battleInstance.currentConfirmSkillPriority = ConfirmSkillPriority.Random;
            targets[i].battleInstance.currentSelectDetailTargetType = SelectDetailTargetType.Random;
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator MythOfSpear(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        WaitForSeconds waitTime2 = new WaitForSeconds(0.1f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(summonsBook, structure.player.center.position, Quaternion.identity);
        SoundManager.Instance.PlayEffect(118, 1f);
        SoundManager.Instance.PlayEffect(117, 0.5f);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        effect = Instantiate(mythofSpear, target.transform.position - new Vector3(0f, 0.2f, 0f), Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def, structure.player, structure.skillData);

            target.ResetPassiveSkill();
            yield return waitTime;

            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    effect = Instantiate(lockSkillEffect, target.transform.position, Quaternion.identity);
                    yield return new WaitUntil(() => effect == null);
                    yield return waitTime;

                    target.battleInstance.triggerSkillDatas.Clear();
                    target.battleInstance.percentSkillDatas.Clear();
                    target.battleInstance.skillDatas.Clear();
                    target.battleInstance.skillDatas.Add(headBut);
                }
            }
        }


        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator DeathRoll(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        WaitForSeconds waitTime2 = new WaitForSeconds(0.4f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var effect = Instantiate(summonsBook, structure.player.center.position, Quaternion.identity);
        SoundManager.Instance.PlayEffect(118, 1f);
        SoundManager.Instance.PlayEffect(117, 0.5f);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        var clone = Instantiate(deathRollEffect, target.transform.position, Quaternion.identity).GetComponent<Animator>();
        yield return new WaitUntil(() => clone.GetCurrentAnimatorStateInfo(0).IsName("stay"));

        int missCount = 10;
        while (true)
        {
            yield return waitTime2;

            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                missCount = 10;
                target.Hurt(structure.skillData.atk, structure.player, structure.skillData);
                Instantiate(nearDistanceHitEffect, target.center.position, Quaternion.identity);
                if (target == null || (target != null && target.isDead == true))
                {
                    yield return waitTime;
                    break;
                }
                float value = Random.Range(0, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    effect = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    yield return new WaitUntil(() => effect == null);
                    yield return waitTime;
                    break;
                }
                if (target != null && target.battleInstance.hp <= 0)
                {
                    yield return waitTime;
                    break;
                }
            }
            else
            {
                missCount--;
                if (missCount < 0)
                {
                    yield return waitTime;
                    break;
                }
            }
        }
        clone.Play("exit");
        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator DeathMatch(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //타겟
        var teams = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        var targets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        targets = targets.ToList();
        targets.Sort((a, b) =>
        {
            float aValue = Mathf.Max(0, a.battleInstance.atk + a.battleInstance.def + a.battleInstance.hp + a.battleInstance.maxHp + a.battleInstance.maxMp - a.battleInstance.maxDex);
            float bValue = Mathf.Max(0, b.battleInstance.atk + b.battleInstance.def + b.battleInstance.hp + b.battleInstance.maxHp + b.battleInstance.maxMp - b.battleInstance.maxDex);
            return bValue.CompareTo(aValue);
        });

        var target = targets[0];

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(deathMatchTimeEffect, structure.player.transform.position + new Vector3(0f, 0.4f, 0f), Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        yield return StartCoroutine(VictoryUI.Instance.DeathMatchRoutine());
        yield return waitTime;

        targets.AddRange(teams);
        targets.Remove(target);
        targets.Remove(structure.player);

        for (int i = 0; i < targets.Count; ++i)
        {
            effect = Instantiate(destinyBondEffect, targets[i].transform.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        if (targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                targets[i].DeathMatchDead();
            }

            yield return new WaitForSeconds(3.25f);
        }


        target.ResetPassiveSkill();
        StartCoroutine(target.RecoverRoutine(target.battleInstance.hp, target.battleInstance.maxHp, 1f, true));
        StartCoroutine(target.RecoverRoutine(target.battleInstance.mp, target.battleInstance.maxMp, 1f, false));
        StartCoroutine(target.DexRoutine(target.battleInstance.dex, 0, 1f));

        structure.player.ResetPassiveSkill();
        StartCoroutine(structure.player.RecoverRoutine(structure.player.battleInstance.hp, structure.player.battleInstance.maxHp, 1f, true));
        StartCoroutine(structure.player.RecoverRoutine(structure.player.battleInstance.mp, structure.player.battleInstance.maxMp, 1f, false));
        StartCoroutine(structure.player.DexRoutine(structure.player.battleInstance.dex, 0, 1f));

        float start = CombatManager.Instance.battleTimer;
        float end = Mathf.Min(CombatManager.Instance.battleTimer + 10, CombatManager.Instance.battleMaxTimer - 1);

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            CombatManager.Instance.battleTimer = Mathf.Lerp(start, end, currentSpeed);
            CombatUI.Instance.RenderBattleTimer();
            yield return null;
        }

        yield return new WaitUntil(() => structure.player.startRecovery == false);
        yield return waitTime;

        structure.player.battleInstance.skillDatas.RemoveAll(x => x == structure.skillData);
        structure.player.battleInstance.percentSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        structure.player.battleInstance.triggerSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        if (structure.player.battleInstance.skillDatas.Count <= 0)
            structure.player.battleInstance.skillDatas.Add(headBut);

        CombatManager.Instance.deadMonsters.Clear();
        easterList.Clear();

        for (int i = 0; i < darkHoleList.Count; ++i)
            Destroy(darkHoleList[i].obj.gameObject);
        darkHoleList.Clear();

        homegroundDawnCount = 0;
        awayDawnCount = 0;

        //필수
        isBlock = false;
    }
    private IEnumerator SoulBlade(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        var effect = Instantiate(soulBlade, target.transform.position, Quaternion.identity);
        effect.transform.localScale = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? effect.transform.localScale : new Vector3(effect.transform.localScale.x * -1f, effect.transform.localScale.y, effect.transform.localScale.z);

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return new WaitUntil(() => effect == null);

        //필수(데미지계산)
        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            //화상상태
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    yield return new WaitForSeconds(0.25f);

                    var clone = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    if (clone != null)
                    {
                        yield return new WaitUntil(() => clone == null);
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.25f);

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator ShadowPunch(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
        int count = 0;
        var dialogTargets = CombatManager.Instance.GetNeighbourTarget(target, out count);

        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);

        if (dmgCheck == true)
        {
            var effectClone = Instantiate(shadowPunch);
            effectClone.transform.position = target.center.position;
            yield return new WaitUntil(() => effectClone == null);
            yield return waitTime;
        }
        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
            //마비상태
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                {
                    var clone = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    yield return new WaitUntil(() => clone == null);
                    yield return waitTime;
                }
            }
        }

        if (dmgCheck == true)
        {
            if (count != 0)
            {
                yield return waitTime;

                List<GameObject> effects = new List<GameObject>();
                for (int i = 0; i < dialogTargets.Length; ++i)
                {
                    if (dialogTargets[i] == null)
                        continue;
                    if (dialogTargets[i].isDead == true)
                        continue;

                    var effect = Instantiate(shadowPunchSupport, dialogTargets[i].transform.position, Quaternion.identity);
                    effects.Add(effect);
                }

                for (int i = 0; i < effects.Count; ++i)
                {
                    yield return new WaitUntil(() => effects[i] == null);
                }

                yield return waitTime;

                for (int i = 0; i < dialogTargets.Length; ++i)
                {
                    if (dialogTargets[i] == null)
                        continue;
                    if (dialogTargets[i].isDead == true)
                        continue;

                    dialogTargets[i].Hurt(Mathf.Max(target.GetDefenseDeal(), ((structure.player.battleInstance.atk + structure.skillData.atk) * structure.skillData.buffValue) - dialogTargets[i].battleInstance.def), structure.player, structure.skillData);
                    //마비상태
                    if (target.isDead == false)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                        {
                            var clone = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                            yield return new WaitUntil(() => clone == null);
                            yield return waitTime;
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (dmgCheck && structure.player.battleInstance.abilities.Contains(AbilityType.Suction) && structure.player.battleInstance.hp < structure.player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(structure.player));

        //반격기 확인
        target.CheckDrivenSkill(structure.player);
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddElecFlag(structure.player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddPoisenFlag(structure.player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = structure.player.dogDmgStatus.AddBurnFlag(structure.player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }

    private IEnumerator FormChange5(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟설정
        List<EntityMonster> targets = null;

        if (CombatManager.Instance.homegroundMonsters.Contains(structure.player))
            targets = CombatManager.Instance.awayMonsters.ToList();
        else
            targets = CombatManager.Instance.homegroundMonsters.ToList();

        var roationTable = new float[6] { 0f, 45f, 60f, 90f, -45f, -60f };

        //연출
        Animator backgroundAnim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        backgroundAnim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => backgroundAnim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var clone = structure.player.SwapMonster(behimosformchange, structure.skillData.consumMpAmount);
        float buffValue = Mathf.RoundToInt(structure.player.originInstance.maxHp * 0.25f);
        clone.battleInstance.maxHp += buffValue;
        clone.battleInstance.hp += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.maxMp * 0.25f);
        clone.battleInstance.maxMp += buffValue;
        clone.battleInstance.mp += buffValue;


        buffValue = Mathf.RoundToInt(structure.player.originInstance.atk * 0.25f);
        clone.battleInstance.atk += buffValue;

        buffValue = Mathf.RoundToInt(structure.player.originInstance.def * 0.25f);
        clone.battleInstance.def += buffValue;
        structure.player = clone;

        var anim = clone.GetComponent<Animator>();
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        yield return new WaitForSeconds(0.5f);

        //여기에 스킬
        Queue<int> roationPreviousQueue = new Queue<int>();
        GameObject effect = null;

        for(int a = 0; a < 5; ++a)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                if (targets[i] == null)
                    continue;
                if (targets[i].isDead == true)
                    continue;

                var rotationIndex = Random.Range(0, roationTable.Length);

                if (roationPreviousQueue.Count > 0)
                {
                    while (roationPreviousQueue.Peek() == rotationIndex)
                        rotationIndex = Random.Range(0, roationTable.Length);
                    roationPreviousQueue.Dequeue();
                }

                effect = Instantiate(formchange5_hit, targets[i].center.position, Quaternion.Euler(0f, 0f, roationTable[rotationIndex]));

                bool dmgCheck = targets[i].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck)
                    targets[i].Hurt(structure.skillData.atk);

                if (targets[i] != null && targets[i].isDead == false)
                    roationPreviousQueue.Enqueue(rotationIndex);
            }
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            var emptyIndex = targets.FindIndex(x => x != null && x.isDead == false);
            if (emptyIndex == -1)
                goto jump;
        }

        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;

            effect = Instantiate(lockSkillEffect, targets[i].transform.position, Quaternion.identity);
            targets[i].battleInstance.triggerSkillDatas.Clear();
            targets[i].battleInstance.percentSkillDatas.Clear();
            targets[i].battleInstance.skillDatas.Clear();
            targets[i].battleInstance.skillDatas.Add(headBut);
        }
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

    jump:
        yield return waitTime;

        backgroundAnim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => backgroundAnim == null);
        yield return waitTime;

        //필수
        isBlock = false;
    }


    //빛속성 범위기는 공격받았을때 꼭 EntityMonster의 checkLivingDeadHolyHurt 체크해주기
    private IEnumerator HolyArrow(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(holyArrow, structure.player.bulletCreatePoint.position, Quaternion.identity);

        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)), bullet.transform.localScale.y, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }

        float bulletSpeed = 6f;
        float addSpeed = 4f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return waitTime;
                //신성상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= (structure.skillData.statusRatio + structure.player.addHolyStatus))
                    {
                        var clone = Instantiate(holyEffect, target.center.position, Quaternion.identity);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);

                        if (target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
                        {
                            target.DissolveDead();
                            yield return new WaitForSeconds(3f);

                        }
                        else
                        {
                            target.battleInstance.hpRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                            target.battleInstance.manaRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                        }
                    }
                }
            }
            else
                Destroy(bullet);

            yield return waitTime;

            if (target != null && target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
            {
                target.DissolveDead();
                yield return new WaitForSeconds(3.25f);
            }
        }


        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator HolySword(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(holySword, structure.player.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        effect = Instantiate(buffAttackEffect, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        structure.player.battleInstance.atk += structure.skillData.buffValue;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator HolyDefense(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(holyDefense, structure.player.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        effect = Instantiate(buffShiledEffect, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        structure.player.battleInstance.atk += structure.skillData.buffValue;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator HolyLight(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(holyLight, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        if (anim != null)
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        else
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }
        //   yield return new WaitForSeconds(0.25f);

        float bulletSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                if (anim != null)
                {
                    anim.Play("exit");
                }
                else
                {
                    isBlock = false;
                    stopStructure.dontMove = false;
                    yield break;
                }
                target.Hurt(structure.skillData.atk, structure.player, structure.skillData);
                target.SetOrigin();
                target.ResetPassiveSkill();
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);

                //신성상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= (structure.skillData.statusRatio + structure.player.addHolyStatus))
                    {
                        var clone = Instantiate(holyEffect, target.center.position, Quaternion.identity);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);

                        if (target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
                        {
                            target.DissolveDead();
                            yield return new WaitForSeconds(3f);

                        }
                        else
                        {
                            target.battleInstance.hpRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                            target.battleInstance.manaRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                        }
                    }
                }
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        if (target != null && target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
        {
            target.DissolveDead();
            yield return new WaitForSeconds(3.25f);
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator LazerBeam(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = structure.player.transform.position;
        stopStructure.stopObj = structure.player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        //타겟가져오기
        int aCount = 0, bCount = 0, cCount = 0;
        var targets = (CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i].height == 0)
                aCount++;
            else if (targets[i].height == 1)
                bCount++;
            else if (targets[i].height == 2)
                cCount++;
        }

        int bestCount = aCount;
        int index = 0;
        if (aCount < bCount)
            bestCount = bCount;
        if (bestCount < cCount)
            bestCount = cCount;

        if (bestCount == aCount)
            index = 0;
        else if (bestCount == bCount)
            index = 1;
        else if (bestCount == cCount)
            index = 2;

        targets = targets.FindAll(x => x.height == index);

        //연출
        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = CombatManager.Instance.trapPositionDic[index];

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        yield return waitTime;

        List<Animator> anims = new List<Animator>();
        Vector2 endPosition2 = endPosition + new Vector3(0f, 0.2f);

        targets.Reverse();
        SoundManager.Instance.PlayEffect(105, 1f);
        var effect = Instantiate(lazerBeam, structure.player.bulletCreatePoint.position, lazerBeam.transform.rotation);
        effect.transform.localScale = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? new Vector3(effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z) : new Vector3(effect.transform.localScale.x, -1 * effect.transform.localScale.y, effect.transform.localScale.z);
        lerpSpeed = 2f;
        currentTime = 0f;
        lerpTime = 1f;
        float startScale = 1f;
        float endScale = 9f;
        Transform child = effect.transform.GetChild(1);

        float xScale = 1f;
        var weight = structure.player.battleInstance.monsterWeight;
        if (weight == MonsterWeight.Normal)
            xScale = 1.5f;
        else if (weight == MonsterWeight.Middle)
            xScale = 1.75f;
        else if (weight == MonsterWeight.Big)
            xScale = 2f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(startScale, endScale, currentSpeed);
            child.transform.localScale = new Vector3(xScale, value, child.transform.localScale.z);
            yield return null;
        }


        GameObject effect2 = null;

        WaitForSeconds waitTime2 = new WaitForSeconds(0.5f);
        WaitUntil waitUntil = new WaitUntil(() => effect2 == null);

        int random = Random.Range(5, 10);

        for (int i = 0; i < random; ++i)
        {
            if (targets.Count <= 0)
                break;
            for (int j = 0; j < targets.Count; ++j)
            {
                if (targets[j] == null)
                    continue;
                if (targets[j].isDead == true)
                    continue;

                var dmgCheck = targets[j].DmgCheck(structure.player, structure.skillData);
                if (dmgCheck == true)
                {
                    int dmgPerValue = Random.Range(0, 4);
                    float value = 0f;
                    if (dmgPerValue == 0)
                        value = 0.25f;
                    else if (dmgPerValue == 1)
                        value = 0.5f;
                    else if (dmgPerValue == 2)
                        value = 0.75f;
                    else if (dmgPerValue == 3)
                        value = 1f;

                    effect2 = Instantiate(nearDistanceHitEffect, targets[j].center.position, Quaternion.identity);
                    targets[j].Hurt(Mathf.Max(targets[j].GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - targets[j].battleInstance.def) * value, structure.player, structure.skillData);
                    targets[j].sumStatusRatio += structure.skillData.statusRatio;
                    targets[j].checkLivingDeadHolyHurt = true;
                }
            }
            targets.RemoveAll(x => x == null || (x != null && x.isDead == true));
            yield return waitTime2;
        }

        Destroy(effect);
        yield return waitTime;

        var statusTargets = targets.FindAll(x => (x != null && x.isDead == false) && x.sumStatusRatio > 0f);
        GameObject clone = null;

        List<EntityMonster> deadTargets = new List<EntityMonster>();
        for (int i = 0; i < statusTargets.Count; ++i)
        {
            float value = Random.Range(0f, 1f);
            if (value <= (statusTargets[i].sumStatusRatio + structure.player.addHolyStatus))
            {
                clone = Instantiate(holyEffect, statusTargets[i].center.position, Quaternion.identity);
                statusTargets[i].battleInstance.hpRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                statusTargets[i].battleInstance.manaRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                //if (statusTargets[i].battleInstance.abilities.Contains(AbilityType.LivingDead) && statusTargets[i].battleInstance.hp <= 0)
                //{
                //    deadTargets.Add(statusTargets[i]);
                //}
                //else
                //{
                //    statusTargets[i].battleInstance.hpRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                //    statusTargets[i].battleInstance.manaRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                //}
            }

            statusTargets[i].sumStatusRatio = 0f;
        }

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i].isDead == true)
                continue;
            if ((targets[i].checkLivingDeadHolyHurt == true) && (targets[i].battleInstance.abilities.Contains(AbilityType.LivingDead)) && (targets[i].battleInstance.hp <= 0))
                deadTargets.Add(targets[i]);

            targets[i].checkLivingDeadHolyHurt = false;
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < deadTargets.Count; ++i)
        {
            deadTargets[i].DissolveDead();
        }

        if (deadTargets.Count > 0)
            yield return new WaitForSeconds(3.25f);

        lerpSpeed = 4f;
        currentTime = 0f;
        lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator LiberationKnell(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(liberationKnell, structure.player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.5f);

        var targets = CombatManager.Instance.homegroundMonsters;
        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].sternStatus.Stop(targets[i]);
        }

        targets = CombatManager.Instance.awayMonsters;
        for (int i = 0; i < targets.Count; ++i)
        {
            targets[i].sternStatus.Stop(targets[i]);
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator HolyRecovery(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        targets = targets.ToList();
        targets.Remove(structure.player);

        //연출
        EntityMonster target = null;
        if (targets.Count > 0)
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
        else
            target = structure.player;
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var renderer = target.GetComponent<SpriteRenderer>();
        var originMaterial = renderer.material;
        renderer.material = holyMaterial;

        SoundManager.Instance.PlayEffect(90, 1f);
        var effectParent = Instantiate(holyRecoveryEffect, target.transform.position, Quaternion.identity);
        int childCount = effectParent.transform.childCount;

        for (int i = 0; i < childCount; ++i)
        {
            var child = effectParent.transform.GetChild(i).GetComponent<SuctionEffect>();
            child.gameObject.SetActive(true);
            child.Play(target.center.position, false);
            yield return waitTime;
        }
        SoundManager.Instance.StopEffect(90);
        Destroy(effectParent);
        lerpSpeed = 1f;
        currentTime = 0f;
        lerpTime = 1f;
        string materialChar = "_FlashAmount";
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;
            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(1f, 0f, currentSpeed);
            holyMaterial.SetFloat(materialChar, value);
            yield return null;
        }

        renderer.material = originMaterial;
        holyMaterial.SetFloat(materialChar, 1f);

        yield return waitTime;

        var effect = Instantiate(recoveryEffect, target.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        target.StartCoroutine(target.RecoverRoutine(target.battleInstance.hp, Mathf.Min(target.battleInstance.maxHp, target.battleInstance.hp + target.battleInstance.maxHp * (target.battleInstance.hpRecoveryRatio + structure.skillData.buffValue))));
        yield return new WaitUntil(() => target.startRecovery == false);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator BlastRain(BattleStructure structure)
    {
        //필수
        isBlock = true;
        arrowSoundBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        var effect = Instantiate(blastRain);
        var originTargets = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        yield return StartCoroutine(effect.SpawnMeteoRoutine(!CombatManager.Instance.homegroundMonsters.Contains(structure.player), originTargets, structure.player, structure.skillData));
        yield return waitTime;

        Destroy(effect.gameObject);


        var statusTargets = originTargets.FindAll(x => (x != null && x.isDead == false) && x.sumStatusRatio > 0f);
        GameObject clone = null;

        for (int i = 0; i < statusTargets.Count; ++i)
        {
            float value = Random.Range(0f, 1f);
            if (value <= (statusTargets[i].sumStatusRatio + structure.player.addHolyStatus))
            {
                clone = Instantiate(holyEffect, statusTargets[i].center.position, Quaternion.identity);
                statusTargets[i].battleInstance.hpRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                statusTargets[i].battleInstance.manaRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                //if (statusTargets[i].battleInstance.abilities.Contains(AbilityType.LivingDead) && statusTargets[i].battleInstance.hp <= 0)
                //{
                //    deadTargets.Add(statusTargets[i]);
                //}
                //else
                //{
                //    statusTargets[i].battleInstance.hpRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                //    statusTargets[i].battleInstance.manaRecoveryRatio = Mathf.Max(0, statusTargets[i].battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                //}
            }

            statusTargets[i].sumStatusRatio = 0f;
        }

        List<EntityMonster> deadTargets = new List<EntityMonster>();

        for (int i = 0; i < originTargets.Count; ++i)
        {
            if (originTargets[i] == null)
                continue;
            if (originTargets[i].isDead == true)
                continue;
            if ((originTargets[i].checkLivingDeadHolyHurt == true) && (originTargets[i].battleInstance.abilities.Contains(AbilityType.LivingDead)) && (originTargets[i].battleInstance.hp <= 0))
                deadTargets.Add(originTargets[i]);

            originTargets[i].checkLivingDeadHolyHurt = false;
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < deadTargets.Count; ++i)
        {
            deadTargets[i].DissolveDead();
        }

        if (deadTargets.Count > 0)
            yield return new WaitForSeconds(3.25f);


        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        arrowSoundBlock = false;
        isBlock = false;
    }
    private IEnumerator SealedSword(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return waitTime;

        target.sternStatus.Start(target, sealedSword, target.transform, target.HolyRoutine(), structure.skillData.statusRatio, false, true);

        Animator effect = target.sternStatus.effect.GetComponent<Animator>();
        yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay"));
        yield return waitTime;

        target.Hurt(structure.skillData.atk);
        yield return waitTime;

        if (target != null && target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
        {
            target.DissolveDead();
            yield return new WaitForSeconds(3.25f);
        }

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator Easter(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        bool check = false;
        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        var targets = CombatManager.Instance.deadMonsters.FindAll(x => x.Item1 == isHomeground);
        EmptyPositionStructure emptyPosition = default;
        MonsterInstance data = null;
        if (targets.Count > 0)
        {
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, targets);
            emptyPosition = target.Item2;
            data = target.Item3;

            var originTargets = isHomeground ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
            for (int i = 0; i < originTargets.Count; ++i)
            {
                if ((originTargets[i].width == emptyPosition.width) && (originTargets[i].height == emptyPosition.height))
                {
                    var emptys = CombatManager.Instance.GetEmptyPositions(isHomeground);
                    if (emptys.Count > 0)
                    {
                        emptyPosition = emptys[Random.Range(0, emptys.Count)];
                        break;
                    }
                    else
                        goto jump;
                }
            }

            check = true;
            CombatManager.Instance.deadMonsters.Remove(target);
        }

    jump:
        if (check == true)
        {
            //연출
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            Vector3 startPosition = structure.player.transform.localPosition;
            Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);
            Transform shaodw = structure.player.transform.GetChild(4);
            Transform cavas = structure.player.transform.GetChild(0);
            Vector3 shadowOriginPosition = shaodw.position;
            Vector3 cavasPosition = cavas.position;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            currentTime = 0f;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
                shaodw.transform.position = shadowOriginPosition;
                cavas.transform.position = cavasPosition;
                yield return null;
            }

            yield return waitTime;

            Vector2 heightDisOffset = new Vector2(CombatManager.Instance.heightDistance * (emptyPosition.height + 1), 0);
            var clone = EntityMonster.CreateMonster(data, emptyPosition.worldPosition - heightDisOffset - (Vector2)data.monsterData.monsterPrefab.transform.position, emptyPosition.width, emptyPosition.height, isHomeground);
            var effect = Instantiate(easter, clone.transform.position, Quaternion.identity);
            clone.battleInstance.hp = clone.battleInstance.maxHp * clone.battleInstance.hpRecoveryRatio;

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            //필수
            //반격기확인
            if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
            {
                //타겟가져오기
                var targets2 = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
                var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets2);
                if (target != null)
                    structure.player.CheckFireBaracesSkill(target);
            }

            isBlock = false;
        }
        else
            yield return NoManaRoutine(structure);
    }
    private IEnumerator Collaboration(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        Animator anim = Instantiate(spicalAttackBackground2, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        //팀원들의 빛이 이동
        var combinations = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        List<GameObject> effects = new List<GameObject>();
        for (int i = 0; i < combinations.Count; ++i)
        {
            var holyEffect = Instantiate(holyRecoveryParent, combinations[i].center.position, Quaternion.identity);
            effects.Add(holyEffect);
            Vector2 endPosition = (Vector2)target.center.position + (Random.insideUnitCircle * 0.25f);
            float m = (endPosition - (Vector2)holyEffect.transform.position).magnitude;
            float moveSpeed = 6f;
            while (m > moveSpeed * Time.deltaTime)
            {
                m = (endPosition - (Vector2)holyEffect.transform.position).magnitude;
                Vector3 dir = (endPosition - (Vector2)combinations[i].center.position).normalized;
                holyEffect.transform.position += dir * moveSpeed * Time.deltaTime;
                yield return null;
            }

            yield return waitTime;
        }

        //빛이 모여 적을 하얗게만든다
        var targetRenderer = target.GetComponent<SpriteRenderer>();
        var oriMaterial = targetRenderer.material;
        targetRenderer.material = holyMaterial;
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 1f;
        string materialChar = "_FlashAmount";
        holyMaterial.SetFloat(materialChar, 0f);
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;
            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(0f, 1f, currentSpeed);
            holyMaterial.SetFloat(materialChar, value);
            yield return null;
        }

        // targetRenderer.material = oriMaterial;
        for (int i = 0; i < effects.Count; ++i)
            Destroy(effects[i]);

        yield return new WaitForSeconds(0.5f);

        //거대한 빛이펙트가 하늘에서 내려온다
        var effect = Instantiate(collaboration, target.transform.position, Quaternion.identity);
        var effectRenderer = effect.GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(0.8f);

        effectRenderer.sortingLayerName = "Object";
        effectRenderer.sortingOrder = -1;

        targetRenderer.material = shaodwMaterial;

        bool isActive = target.transform.GetChild(3).gameObject.activeInHierarchy;
        target.transform.GetChild(0).gameObject.SetActive(false);
        target.transform.GetChild(3).gameObject.SetActive(false);
        target.transform.GetChild(4).gameObject.SetActive(false);

        yield return new WaitForSeconds(0.4f);

        effectRenderer.sortingLayerName = "EB";
        effectRenderer.sortingOrder = 0;

        targetRenderer.material = oriMaterial;
        holyMaterial.SetFloat(materialChar, 1f);
        target.transform.GetChild(0).gameObject.SetActive(true);

        if (isActive)
            target.transform.GetChild(3).gameObject.SetActive(true);

        target.transform.GetChild(4).gameObject.SetActive(true);

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        //데미지계산
        float sumAtk = 0f;
        for (int i = 0; i < combinations.Count; ++i)
            sumAtk += combinations[i].battleInstance.atk;

        var dmgCheck = target.DmgCheck(structure.player, structure.skillData);
        if (dmgCheck)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), sumAtk - target.battleInstance.def), structure.player, structure.skillData);
            yield return waitTime;
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= (structure.skillData.statusRatio + structure.player.addHolyStatus))
                {
                    var clone = Instantiate(holyEffect, target.center.position, Quaternion.identity);
                    yield return new WaitUntil(() => clone == null);
                    yield return waitTime;

                    target.battleInstance.hpRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    target.battleInstance.manaRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));

                    //if (target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
                    //{
                    //    target.DissolveDead();
                    //    yield return new WaitForSeconds(3f);

                    //}
                    //else
                    //{
                    //    target.battleInstance.hpRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    //    target.battleInstance.manaRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    //}
                }
            }
        }
        else
            yield return waitTime;

        if (target != null && target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
        {
            target.DissolveDead();
            yield return new WaitForSeconds(3.25f);
        }


        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator Dawn(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟가져오기
        bool isHomeground = (CombatManager.Instance.homegroundMonsters.Contains(structure.player));
        var targets = isHomeground ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        var renderer = structure.player.GetComponent<SpriteRenderer>();
        var originMaterial = renderer.material;

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        renderer.material = holyMaterial;
        renderer.material.SetFloat("_FlashAmount", 1f);

        var effect = Instantiate(dawnEffect, Vector2.zero, Quaternion.identity);
        effect.PlayActive();

        yield return new WaitUntil(() => effect.isRunning == false);
        yield return waitTime;

        effect.PlayDontActive();
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 1f;
        string materialChar = "_FlashAmount";
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;
            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(1f, 0f, currentSpeed);
            renderer.material.SetFloat(materialChar, value);
            yield return null;
        }

        renderer.material = originMaterial;
        holyMaterial.SetFloat("_FlashAmount", 1f);

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        GameObject clone = null;
        for (int i = 0; i < targets.Count; ++i)
        {
            clone = Instantiate(buffAttackEffect, targets[i].center.position, Quaternion.identity);
            targets[i].battleInstance.atk += Mathf.RoundToInt(targets[i].battleInstance.atk * structure.skillData.buffValue);
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            clone = Instantiate(buffShiledEffect, targets[i].center.position, Quaternion.identity);
            targets[i].battleInstance.def += Mathf.RoundToInt(Mathf.Max(1, targets[i].battleInstance.def) * structure.skillData.buffValue);
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            clone = Instantiate(buffSpeedEffect, targets[i].center.position, Quaternion.identity);
            targets[i].battleInstance.maxDex = Mathf.Max(0.1f, targets[i].battleInstance.maxDex * structure.skillData.buffValue);
            targets[i].battleInstance.dex = Mathf.Max(0f, targets[i].battleInstance.dex * structure.skillData.buffValue);
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            float value = targets[i].battleInstance.maxHp + (targets[i].battleInstance.maxHp * structure.skillData.buffValue);
            StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.hp, value, 1f, true));
            StartCoroutine(targets[i].RecoverRoutine(targets[i].battleInstance.hp, value, 1f, false));
            targets[i].battleInstance.dex = Mathf.Max(0f, targets[i].battleInstance.dex * structure.skillData.buffValue);
        }

        yield return new WaitUntil(() => targets[targets.Count - 1].startRecovery == false);
        yield return waitTime;

        if (isHomeground)
        {
            homegroundDawnCount = 1;
            awayDawnCount = 0;
        }
        else
        {
            homegroundDawnCount = 0;
            awayDawnCount = 1;
        }

        structure.player.battleInstance.skillDatas.RemoveAll(x => x == structure.skillData);
        structure.player.battleInstance.percentSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        structure.player.battleInstance.triggerSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        if (structure.player.battleInstance.skillDatas.Count <= 0)
            structure.player.battleInstance.skillDatas.Add(headBut);

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
    }
    private IEnumerator DivinePower(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        SoundManager.Instance.PlayEffect(111, 1f, true);
        var effect = Instantiate(divinePowerEffect, structure.player.center);
        effect.transform.localPosition = Vector3.zero;

        var renderer = structure.player.GetComponent<SpriteRenderer>();
        var originMaterial = renderer.material;
        renderer.material = holyMaterial;

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 2f;
        string materialChar = "_FlashAmount";
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;
            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(0f, 1f, currentSpeed);
            holyMaterial.SetFloat(materialChar, value);
            yield return null;
        }

        holyMaterial.SetFloat(materialChar, 1f);

        yield return new WaitForSeconds(0.4f);

        //연출
        Transform obj = structure.player.transform;
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        lerpSpeed = 4f;
        currentTime = 0f;
        lerpTime = 1f;

        BoxCollider2D boxCol = structure.player.GetComponent<BoxCollider2D>();
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);

            var cols = Physics2D.OverlapBoxAll((Vector2)structure.player.transform.position + boxCol.offset, boxCol.size, 0f);
            for (int i = 0; i < cols.Length; ++i)
            {
                var wall = cols[i].GetComponent<Wall>();
                if (wall != null)
                    wall.Remove();
            }

            yield return null;
        }

        renderer.material = originMaterial;

        bool dmgCheck = target.DmgCheck(structure.player, structure.skillData, true);

        if (dmgCheck == true)
        {
            effect.transform.parent = null;
            effect.transform.position = target.center.position;
            effect.GetComponent<Animator>().Play("exit");
        }
        else
            Destroy(effect);

        SoundManager.Instance.StopEffect(111);
        SoundManager.Instance.PlayEffect(110, 1f);
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        float dmg = Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def);
        structure.player.DissolveDead();

        bool check = (dmgCheck == true) || (target != null && target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0);
        if (check)
        {
            target.DissolveDead();
        }
        else
        {
            target.Hurt(dmg, structure.player, structure.skillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
            //신성상태
            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= (structure.skillData.statusRatio + structure.player.addHolyStatus))
                {
                    var clone = Instantiate(holyEffect, target.center.position, Quaternion.identity);
                    yield return new WaitUntil(() => clone == null);
                    yield return new WaitForSeconds(0.25f);

                    target.battleInstance.hpRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    target.battleInstance.manaRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    //if (target.battleInstance.abilities.Contains(AbilityType.LivingDead) && target.battleInstance.hp <= 0)
                    //{
                    //    target.DissolveDead();
                    //    yield return new WaitForSeconds(3f);

                    //}
                    //else
                    //{
                    //    target.battleInstance.hpRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    //    target.battleInstance.manaRecoveryRatio = Mathf.Max(0, target.battleInstance.hpRecoveryRatio - (0.1f + structure.player.addHolyStatus));
                    //}
                }
            }

            yield return new WaitForSeconds(3.25f);
        }

        //필수
        isBlock = false;
    }
    private IEnumerator PowerBalance(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.allMonsters.ToList();
        targets.Remove(structure.player);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        Instantiate(powerBalance, structure.player.center.position, Quaternion.identity);
        var effect = Instantiate(powerBalance, target.center.position, Quaternion.identity);

        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        if (target.battleInstance.atk > structure.player.battleInstance.atk)
        {
            effect = Instantiate(debuffAttackEffect, target.center.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            effect = Instantiate(buffAttackEffect, structure.player.center.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            target.battleInstance.atk = Mathf.Max(0, target.battleInstance.atk - structure.skillData.buffValue);
            structure.player.battleInstance.atk += structure.skillData.buffValue;
        }
        else if (target.battleInstance.atk < structure.player.battleInstance.atk)
        {
            effect = Instantiate(buffAttackEffect, target.center.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            effect = Instantiate(debuffAttackEffect, structure.player.center.position, Quaternion.identity);

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            structure.player.battleInstance.atk = Mathf.Max(0, structure.player.battleInstance.atk - structure.skillData.buffValue);
            target.battleInstance.atk += structure.skillData.buffValue;
        }
        else
            yield return waitTime;

        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator LentPower(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        var effect = Instantiate(lentPower, structure.player.transform.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return waitTime;

        if (!structure.player.battleInstance.skillDatas.Contains(formChange4SkillData))
            structure.player.battleInstance.skillDatas.Add(formChange4SkillData);
        if (!structure.player.battleInstance.skillDatas.Contains(holyRecoverySkillData))
            structure.player.battleInstance.skillDatas.Add(holyRecoverySkillData);
        if (!structure.player.battleInstance.skillDatas.Contains(divinePowerSkillData))
            structure.player.battleInstance.skillDatas.Add(divinePowerSkillData);

        structure.player.addHolyStatus = Mathf.Min(1f, structure.player.addHolyStatus + structure.skillData.buffValue);

        structure.player.battleInstance.skillDatas.RemoveAll(x => x == structure.skillData);
        structure.player.battleInstance.percentSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        structure.player.battleInstance.triggerSkillDatas.RemoveAll(x => x.Item2 == structure.skillData);
        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator PotentialPower(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //연출
        var effect = Instantiate(summonsBook, structure.player.center.position, Quaternion.identity);
        SoundManager.Instance.PlayEffect(118, 1f);
        SoundManager.Instance.PlayEffect(117, 0.5f);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        var skillData = MonsterDataBase.Instance.randomSkills[Random.Range(0, MonsterDataBase.Instance.randomSkills.Count)];
        structure.skillData = skillData;
        yield return StartCoroutine(skillData.skillName.ToString(), structure);

        //필수
        isBlock = false;
    }
    private IEnumerator Driven(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = counterAttackSprite;
        structure.player.shieldCreatePoint.gameObject.SetActive(true);

        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        //필수
        isBlock = false;
    }
    private IEnumerator Driven(EntityMonster player)
    {
        //타겟 초기화
        if (player.shieldTarget == null)
            yield break;

        var target = player.shieldTarget;
        player.shieldTarget = null;

        //필수
        isBlock = true;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = player.transform.position;
        stopStructure.stopObj = player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(player);

        //연출
        Transform obj = player.transform;
        if (player == null)
        {
            isBlock = false;
            yield break;
        }
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(player, drivenSkillData);

        if (dmgCheck == true)
        {
            var effectClone = Instantiate(nearDistanceHitEffect);
            effectClone.transform.position = target.center.position;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (player.battleInstance.atk + drivenSkillData.atk) - target.battleInstance.def), player, drivenSkillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
        }

        yield return new WaitForSeconds(0.25f);

        if (dmgCheck && player.battleInstance.abilities.Contains(AbilityType.Suction) && player.battleInstance.hp < player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(player));

        //반격기확인
        target.CheckDrivenSkill(player);
        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddElecFlag(player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddPoisenFlag(player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddBurnFlag(player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }

        isBlock = false;
    }
    private IEnumerator LockOn(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = lockonSprite;
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        //필수
        isBlock = false;
    }
    private IEnumerator DestinyBond(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = destinyBodSprite;
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        //필수
        isBlock = false;
    }
    private IEnumerator DestinyBond(EntityMonster player)
    {
        //타겟 초기화
        if (player.shieldTarget == null)
            yield break;

        var target = player.shieldTarget;
        player.shieldTarget = null;

        //필수
        isBlock = true;

        //연출

        Instantiate(destinyBondEffect, player.transform.position, Quaternion.identity);
        var effect = Instantiate(destinyBondEffect, target.transform.position, Quaternion.identity);

        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        player.Dead();
        target.Dead();

        yield return new WaitForSeconds(0.5f);
        isBlock = false;
    }
    private IEnumerator DestinyOfTheTeam(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();
        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = destinyOfTheTeamSprite;
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        //필수
        isBlock = false;
    }
    private IEnumerator NoblesseOblige(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();
        var teams = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? CombatManager.Instance.homegroundMonsters : CombatManager.Instance.awayMonsters;
        for (int i = 0; i < teams.Count; ++i)
        {
            if (teams[i].passiveFlag == true && teams[i].currentPassiveName == PassiveName.NoblesseOblige)
                teams[i].ResetPassiveSkill();
        }

        if (CombatManager.Instance.homegroundMonsters.Contains(structure.player))
            homegrounsNoblesseOblige = structure.player;
        else
            awayNoblessOblige = structure.player;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = noblesseObligeSprite;
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        //필수
        isBlock = false;
    }
    private IEnumerator SurpriseAttack(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        structure.player.shieldCreatePoint.gameObject.SetActive(true);

        StartCoroutine(OnShieldRoutine(structure.player));


        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = surpriseAttackSprite;

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        supriseAttackList.Add(structure.player);
        //필수
        isBlock = false;
    }
    private IEnumerator Surprise_Attack(EntityMonster player)
    {
        //타겟 초기화
        if (player.shieldTarget == null)
            yield break;

        var target = player.shieldTarget;
        player.shieldTarget = null;

        //필수
        isBlock = true;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = player.transform.position;
        stopStructure.stopObj = player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(player);

        //연출
        Transform obj = player.transform;
        if (player == null)
        {
            isBlock = false;
            yield break;
        }
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        GameObject effectClone = null;
        effectClone = Instantiate(surpriseAttackEffect, target.center.position, Quaternion.identity);

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitUntil(() => effectClone == null);
        yield return new WaitForSeconds(0.25f);

        //필수(데미지계산)
        bool dmgCheck = target.DmgCheck(player, surpriseAttackSkillData);
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (player.battleInstance.atk + surpriseAttackSkillData.atk) - target.battleInstance.def), player, surpriseAttackSkillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);

            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value < surpriseAttackSkillData.statusRatio)
                {
                    yield return new WaitForSeconds(0.25f);
                    var effect = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    yield return new WaitUntil(() => effect == null);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }

        yield return new WaitForSeconds(0.25f);

        if (dmgCheck && player.battleInstance.abilities.Contains(AbilityType.Suction) && player.battleInstance.hp < player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(player));

        //반격기확인
        target.CheckDrivenSkill(player);
        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddElecFlag(player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddPoisenFlag(player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddBurnFlag(player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        isBlock = false;
    }
    private IEnumerator ShadowCrew(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;
        SpriteRenderer renderer = structure.player.GetComponent<SpriteRenderer>();

        float startA = renderer.color.a;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(startA, 0f, currentSpeed));
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        shadowCrewList.Add(structure.player);
        structure.player.GetComponent<BoxCollider2D>().enabled = false;
        //필수
        isBlock = false;
    }
    private IEnumerator ShadowCrew(EntityMonster player)
    {
        //타겟 초기화
        if (player.shieldTarget == null)
            yield break;

        var target = player.shieldTarget;
        player.shieldTarget = null;
        player.GetComponent<BoxCollider2D>().enabled = true;

        //필수
        isBlock = true;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = player.transform.position;
        stopStructure.stopObj = player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(player);


        //연출

        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;
        SpriteRenderer renderer = player.GetComponent<SpriteRenderer>();

        SoundManager.Instance.PlayEffect(150, 1f);
        float endA = player.battleInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().color.a;
        float startA = renderer.color.a;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(startA, endA, currentSpeed));
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Transform obj = player.transform;
        if (player == null)
        {
            isBlock = false;
            yield break;
        }
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        lerpSpeed = 4f;
        currentTime = 0f;
        lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(player, shadowCrewSkillData);

        GameObject effectClone = null;
        if (dmgCheck == true)
        {
            effectClone = Instantiate(surpriseAttackEffect, target.center.position, Quaternion.identity);
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        yield return new WaitUntil(() => effectClone == null);
        yield return new WaitForSeconds(0.25f);

        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (player.battleInstance.atk + shadowCrewSkillData.atk) - target.battleInstance.def), player, shadowCrewSkillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);

            if (target != null && target.isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value < shadowCrewSkillData.statusRatio)
                {
                    yield return new WaitForSeconds(0.25f);
                    var effect = target.dogDmgStatus.AddUnableFlag(target, unableEffect);
                    yield return new WaitUntil(() => effect == null);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }

        yield return new WaitForSeconds(0.25f);

        if (dmgCheck && player.battleInstance.abilities.Contains(AbilityType.Suction) && player.battleInstance.hp < player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(player));

        //반격기확인
        target.CheckDrivenSkill(player);
        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddElecFlag(player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddPoisenFlag(player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddBurnFlag(player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        isBlock = false;
    }
    private IEnumerator SandHide(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Transform maskTransform = structure.player.transform.GetChild(5);

        float maskXScale = spriteMask.transform.localScale.x;
        if (structure.player.originInstance.monsterWeight == MonsterWeight.Middle)
            maskXScale = 1.25f;
        else if (structure.player.originInstance.monsterWeight == MonsterWeight.Big)
            maskXScale = 2f;

        var mask = Instantiate(spriteMask, structure.player.transform.position, Quaternion.identity);
        mask.transform.localScale = new Vector3(maskXScale, maskTransform.localScale.y, mask.transform.localScale.z);
        structure.player.mask = mask;

        shaodw.gameObject.SetActive(false);
        cavas.gameObject.SetActive(false);

        structure.player.bulletCreatePoint.localPosition = new Vector3(structure.player.bulletCreatePoint.localPosition.x, maskTransform.localRotation.eulerAngles.y, structure.player.bulletCreatePoint.localPosition.z);

        SpriteRenderer renderer = structure.player.GetComponent<SpriteRenderer>();
        renderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 1f;

        var effect = Instantiate(sandHideEffect, structure.player.transform.position, Quaternion.identity);

        var obj = structure.player.transform;
        Vector2 startPosition = obj.position;
        Vector2 endPosition = startPosition + new Vector2(0f, maskTransform.localPosition.y);
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        Destroy(effect);

        yield return new WaitForSeconds(0.25f);
        cavas.gameObject.SetActive(true);

        var rec = cavas.GetComponent<RectTransform>();
        rec.anchoredPosition = new Vector2(rec.anchoredPosition.x, maskTransform.localPosition.z);


        structure.player.battleInstance.repeatRatio = Mathf.Min(1, structure.player.battleInstance.repeatRatio + structure.skillData.buffValue);

        structure.player.battleInstance.skillDatas.RemoveAll(x => x.type != SkillType.Supporter);
        structure.player.battleInstance.triggerSkillDatas.RemoveAll(x => x.Item2.type != SkillType.Supporter);
        structure.player.battleInstance.percentSkillDatas.RemoveAll(x => x.Item2.type != SkillType.Supporter);
        structure.player.battleInstance.skillDatas.Add(mudCast);

        structure.player.confirmSkillIndex = (int)structure.player.battleInstance.currentConfirmSkillPriority;
        structure.player.battleInstance.currentConfirmSkillPriority = ConfirmSkillPriority.Surppoting;

        structure.player.AddPassiveSkill(structure.skillData);

        //필수
        isBlock = false;
    }
    private IEnumerator SandHide(EntityMonster player)
    {
        //필수
        isBlock = true;
        player.shieldTarget = null;

        //연출
        Transform cavas = player.transform.GetChild(0);
        Transform shaodw = player.transform.GetChild(4);
        Transform maskTransform = player.transform.GetChild(5);

        cavas.gameObject.SetActive(false);

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 1f;

        var obj = player.transform;
        Vector2 startPosition = obj.position;
        Vector2 endPosition = startPosition - new Vector2(0f, maskTransform.localPosition.y);
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);

        player.ResetHide(shaodw, cavas, maskTransform);
        //필수
        isBlock = false;
    }
    private IEnumerator Madness(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = madnessSprite;
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        //필수
        isBlock = false;
    }
    private IEnumerator Madness(EntityMonster player)
    {
        //타겟 초기화
        if (player.shieldTarget == null)
            yield break;

        var target = player.shieldTarget;
        player.shieldTarget = null;

        //필수
        isBlock = true;

        //멈추기데이터 초기화
        stopStructure.bullet = null;
        stopStructure.originPosition = player.transform.position;
        stopStructure.stopObj = player;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(player);

        //연출

        var effect = Instantiate(madessBurn, player.center.position, Quaternion.identity);
        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        Transform obj = player.transform;
        if (player == null)
        {
            isBlock = false;
            yield break;
        }
        Vector2 startPosition = obj.localPosition;
        Vector2 endPosition = target.transform.localPosition;

        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        bool dmgCheck = target.DmgCheck(player, madnessSkillData);
        if (dmgCheck)
        {
            var effectClone = Instantiate(nearDistanceHitEffect);
            effectClone.transform.position = target.center.position;
        }
        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            obj.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            yield return null;
        }

        //필수(데미지계산)
        if (dmgCheck == true)
        {
            target.Hurt(Mathf.Max(target.GetDefenseDeal(), (player.battleInstance.atk + madnessSkillData.atk) * madnessSkillData.buffValue) - target.battleInstance.def, player, madnessSkillData);
            if (target != null && target.isDead == false)
                target.CheckSandHideSkill(target);
        }

        yield return new WaitForSeconds(0.25f);

        if (dmgCheck && player.battleInstance.abilities.Contains(AbilityType.Suction) && player.battleInstance.hp < player.battleInstance.maxHp)
            yield return StartCoroutine(SuctionRoutine(player));

        //반격기확인
        target.CheckDrivenSkill(player);
        //반격기확인
        if (target != null && target.isDead == false)
        {
            if (target.passiveFlag == true)
            {
                if (target.currentPassiveName == PassiveName.ThunderMan)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= thunderMan.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddElecFlag(player, palalysisEffect);
                        yield return new WaitUntil(() => clone == null);
                        yield return new WaitForSeconds(0.25f);
                    }
                }
                else if (target.currentPassiveName == PassiveName.PoisonBarrior)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= poisonBarriorData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddPoisenFlag(player, poisenEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
                else if (target.currentPassiveName == PassiveName.FireBraces)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= fireBracesData.statusRatio)
                    {
                        var clone = player.dogDmgStatus.AddBurnFlag(player, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
        }
        isBlock = false;
    }
    private IEnumerator StrongDefense(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = strongDefenseSprite;
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        structure.player.battleInstance.def += strongDefenseSkillData.buffValue;
        //필수
        isBlock = false;
    }
    private IEnumerator StrongDefense(EntityMonster player)
    {
        Instantiate(strongDefenseEffect, player.transform.position, Quaternion.identity);
        yield break;
    }
    private IEnumerator ElementalKingsArmor(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }
        structure.player.shieldCreatePoint.gameObject.SetActive(true);

        StartCoroutine(OnShieldRoutine(structure.player));

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = elementalKingsSprite;

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);

        var effect2 = Instantiate(elementalBuff, structure.player.center);
        effect2.transform.position = structure.player.transform.position;
        structure.player.thunderManAura = effect2.transform;
        //필수
        isBlock = false;
    }
    private IEnumerator FireBraces(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        var clone = Instantiate(fireBraces, structure.player.center.position, Quaternion.identity);
        clone.transform.parent = structure.player.transform;

        yield return StartCoroutine(clone.Play());
        yield return new WaitForSeconds(0.5f);


        structure.player.AddPassiveSkill(structure.skillData);

        //필수
        isBlock = false;
    }
    private IEnumerator FireBraces(EntityMonster player)
    {
        //타겟 초기화
        if (player.shieldTarget == null)
            yield break;

        var target = player.shieldTarget;
        player.shieldTarget = null;

        //필수
        isBlock = true;

        //연출
        var fireBraces = player.GetComponentInChildren<FireBraces>();

        //멈추기데이터 초기화
        var bullet = fireBraces.transform.GetChild(0).GetComponent<Animator>();
        stopStructure.bullet = bullet;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(player);

        yield return StartCoroutine(fireBraces.Fire(target.center.position));
        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(player, fireBracesSkillData);
            if (dmgCheck == true)
            {
                bullet.Play("exit");
                target.Hurt(fireBracesSkillData.atk, player, fireBracesSkillData);
                yield return new WaitUntil(() => bullet == null);
                yield return new WaitForSeconds(0.25f);
                //화상상태
                if (target != null && target.isDead == false)
                {
                    float value = Random.Range(0f, 1f);
                    if (value <= 0.1f)
                    {
                        var clone = target.dogDmgStatus.AddBurnFlag(target, burnEffect);
                        if (clone != null)
                        {
                            yield return new WaitUntil(() => clone == null);
                            yield return new WaitForSeconds(0.25f);
                        }
                    }
                }
            }
            else
                Destroy(bullet.gameObject);

            yield return new WaitForSeconds(0.25f);
        }

        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator ThunderMan(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = thunderManSprite;
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        var effect2 = Instantiate(thunderManAura, structure.player.center);
        effect2.transform.localPosition = Vector3.zero;

        structure.player.AddPassiveSkill(structure.skillData);
        structure.player.thunderManAura = effect2.transform;
        //필수
        isBlock = false;
    }
    private IEnumerator PoisonBarrior(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //패시브리셋
        structure.player.ResetPassiveSkill();

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //연출
        float lerpSpeed = 4f;
        float currentTime = 0f;
        float lerpTime = 1f;

        Vector3 startPosition = structure.player.transform.localPosition;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.15f);

        Transform shaodw = structure.player.transform.GetChild(4);
        Transform cavas = structure.player.transform.GetChild(0);
        Vector3 shadowOriginPosition = shaodw.position;
        Vector3 cavasPosition = cavas.position;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }

        structure.player.shieldCreatePoint.GetComponent<SpriteRenderer>().sprite = poisonBarriorSprite;
        structure.player.shieldCreatePoint.gameObject.SetActive(true);
        StartCoroutine(OnShieldRoutine(structure.player));

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            structure.player.transform.localPosition = Vector3.Lerp(endPosition, startPosition, currentSpeed);
            shaodw.transform.position = shadowOriginPosition;
            cavas.transform.position = cavasPosition;
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        structure.player.AddPassiveSkill(structure.skillData);
        //필수
        isBlock = false;
    }
    private IEnumerator Explosion(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        var explosionList = CombatManager.Instance.GetWithinPosition(CombatManager.Instance.homegroundMonsters.Contains(structure.player), true);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);
        var clone = Instantiate(self_destruction, structure.player.center.position, Quaternion.identity);
        structure.player.Hurt(structure.player.battleInstance.hp);

        yield return new WaitUntil(() => clone == null);

        for (int i = 0; i < 50; ++i)
        {
            var index = Random.Range(0, explosionList.Count);
            var index2 = Random.Range(0, explosionList.Count);
            while (index == index2)
                index2 = Random.Range(0, explosionList.Count);
            var tmp = explosionList[index];
            explosionList[index] = explosionList[index2];
            explosionList[index2] = tmp;
        }

        var targets = (isHomeground == true) ? CombatManager.Instance.awayMonsters.ToList() : CombatManager.Instance.homegroundMonsters.ToList();
        for (int i = 0; i < explosionList.Count; ++i)
        {
            Instantiate(explosition, explosionList[i], Quaternion.identity);

            if (i == explosionList.Count - 1)
            {
                yield return waitTime;
                for (int j = 0; j < targets.Count; j++)
                {
                    var mon = targets[j];

                    //데미지계산
                    mon.Hurt(Mathf.Max(mon.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - mon.battleInstance.def), structure.player, structure.skillData);

                    if (mon != null && mon.isDead == false)
                    {
                        float value = Random.Range(0f, 1f);
                        if (value <= structure.skillData.statusRatio)
                        {
                            clone = mon.dogDmgStatus.AddBurnFlag(mon, burnEffect);
                            if (clone != null)
                            {
                                yield return new WaitUntil(() => clone == null);
                                yield return waitTime;
                            }
                        }
                    }
                }
            }
            yield return waitTime;
        }

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        isBlock = false;
    }
    private IEnumerator NoManaRoutine(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        var parent = Instantiate(meditation).transform;
        SpriteRenderer renderer = parent.GetChild(0).GetComponent<SpriteRenderer>();
        parent.position = parent.localPosition + structure.player.center.position;
        parent.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player)) ? parent.localScale.x : -1 * parent.localScale.x, parent.localScale.y, parent.localScale.z);
        renderer.sprite = structure.player.GetComponent<SpriteRenderer>().sprite;

        yield return new WaitUntil(() => renderer == null);

        structure.player.battleInstance.mp = Mathf.Min(structure.player.battleInstance.maxMp, structure.player.battleInstance.mp + (structure.player.battleInstance.maxMp * structure.player.battleInstance.manaRecoveryRatio));
        Destroy(parent.gameObject);

        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        isBlock = false;
    }
    private IEnumerator WorldItemRoutine()
    {
        //필수
        isBlock = true;
        var objs = CombatManager.Instance.worldItemList;
        for (int i = 0; i < objs.Count; ++i)
            objs[i].EarnItem();

        yield return new WaitUntil(() => objs.Count <= 0);
        yield return new WaitForSeconds(0.25f);

        isBlock = false;
    }
    private IEnumerator OneBreak()
    {
        if (stopStructure.stopObj != null)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            isBlock = true;
            Vector2 startPosition = stopStructure.stopObj.transform.localPosition;
            Vector2 endPosition = stopStructure.originPosition;
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            while (currentTime < lerpTime)
            {
                isBlock = true;
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                stopStructure.stopObj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);

            if (stopStructure.stopObj.shieldCreatePoint.gameObject.activeInHierarchy)
                stopStructure.stopObj.ResetPassiveSkill();

            if (stopStructure.speicalBackground != null)
            {
                stopStructure.speicalBackground.Play("SpecialAttack_Background2");
                yield return new WaitUntil(() => stopStructure.speicalBackground == null);
                yield return new WaitForSeconds(0.25f);
            }
            isBlock = false;
        }
        else
        {
            if (stopStructure.bullet != null)
            {
                stopStructure.bullet.Play("exit");
                stopStructure.dontMove = true;
            }
        }
    }
    private IEnumerator DakeHoleRemove()
    {
        if (stopStructure.stopObj != null && collisionDarkHole != null)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            isBlock = true;
            WaitForSeconds waitTime = new WaitForSeconds(0.25f);
            Vector2 startPosition = stopStructure.stopObj.transform.localPosition;
            Vector2 endPosition = stopStructure.originPosition;
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            while (currentTime < lerpTime)
            {
                isBlock = true;
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                stopStructure.stopObj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                yield return null;
            }

            yield return waitTime;
            if (stopStructure.stopObj.isClone == true)
            {
                stopStructure.stopObj.Dead();
                isBlock = false;
                yield break;
            }

            DarkHoleStructure darkHoleData = new DarkHoleStructure();
            darkHoleData.obj = stopStructure.stopObj;
            darkHoleData.emptyPosition = new EmptyPositionStructure();
            darkHoleData.emptyPosition.worldPosition = darkHoleData.obj.transform.position;
            darkHoleData.emptyPosition.width = darkHoleData.obj.width;
            darkHoleData.emptyPosition.height = darkHoleData.obj.height;
            darkHoleData.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(darkHoleData.obj);
            darkHoleList.Add(darkHoleData);

            stopStructure.stopObj.Remove();
            stopStructure.stopObj.ResetPassiveSkill();

            SoundManager.Instance.PlayEffect(141, 1f);
            var obj = Instantiate(darkHoleRemoveObj, stopStructure.stopObj.transform.position, Quaternion.identity);
            obj.transform.localScale = stopStructure.stopObj.transform.localScale;
            obj.GetComponent<SpriteRenderer>().sprite = stopStructure.stopObj.GetComponent<SpriteRenderer>().sprite;

            lerpSpeed = 1f;
            currentTime = 0f;
            lerpTime = 1f;
            startPosition = obj.transform.localPosition;
            endPosition = collisionDarkHole.transform.position;

            float startScale = obj.transform.localScale.x;
            while (currentTime < lerpTime)
            {
                isBlock = true;
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                obj.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);

                float scale = Mathf.Lerp(startScale, 0f, currentSpeed);
                obj.transform.localScale = new Vector3(scale, scale, 1f);
                yield return null;
            }

            Destroy(obj);
            collisionDarkHole.Remove();
            yield return new WaitUntil(() => collisionDarkHole == null);
            yield return waitTime;

            if (stopStructure.speicalBackground != null)
            {
                stopStructure.speicalBackground.Play("SpecialAttack_Background2");
                yield return new WaitUntil(() => stopStructure.speicalBackground == null);
                yield return waitTime;
            }

            isBlock = false;
        }
        else
        {
            if (stopStructure.bullet != null)
            {
                Destroy(stopStructure.bullet.gameObject);
                stopStructure.dontMove = true;
                SoundManager.Instance.PlayEffect(141, 1f);
            }
        }
    }
    private IEnumerator Bounce()
    {
        isBlock = true;
        isBounceBlock = true;
        if (stopStructure.stopObj != null)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;

                Vector2 startPosition = stopStructure.stopObj.transform.localPosition;
                Vector2 endPosition = stopStructure.originPosition;
                float lerpSpeed = 4f;
                float currentTime = 0f;
                float lerpTime = 1f;

                while (currentTime < lerpTime)
                {
                    currentTime += Time.deltaTime * lerpSpeed;

                    float currentSpeed = currentTime / lerpTime;
                    stopStructure.stopObj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                    yield return null;
                }

                if (stopStructure.stopObj.shieldCreatePoint.gameObject.activeInHierarchy)
                    stopStructure.stopObj.ResetPassiveSkill();

                yield return new WaitForSeconds(0.25f);

                var target = stopStructure.stopObj;

                GameObject effect = null;
                if (target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide)
                    effect = Instantiate(airHit, target.center.position, Quaternion.identity);
                else
                    effect = Instantiate(airHit, target.transform.position, Quaternion.identity);

                yield return new WaitUntil(() => effect == null);
                yield return new WaitForSeconds(0.25f);

                if (target.width < 2)
                    target.Hurt(5);
                else
                {
                    //즉사
                    float value = Random.Range(0f, 1f);
                    if (value <= 0.25f)
                        target.FlyingDead();
                    else
                        target.Hurt(5);
                }

                yield return new WaitForSeconds(0.25f);


                if (target != null && target.checkSwapPosition == false && target.isDead == false && target.width < 2 && !(target.passiveFlag == true && target.currentPassiveName == PassiveName.SandHide) && target.sternStatus.Check() == false)
                {
                    var target2 = CombatManager.Instance.homegroundMonsters.Contains(target) ? CombatManager.Instance.homegroundMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height)) : CombatManager.Instance.awayMonsters.Find(x => (x.width == target.width + 1) && (x.height == target.height));
                    if (target2 != null)
                        StartCoroutine(FlyRoutine(target, target2));
                    else
                        StartCoroutine(FlyRoutine2(target));

                    yield return new WaitForSeconds(0.25f);

                    CombatManager.Instance.SetFormationBuffDebuffs();
                }

                if (stopStructure.speicalBackground != null)
                {
                    stopStructure.speicalBackground.Play("SpecialAttack_Background2");
                    yield return new WaitUntil(() => stopStructure.speicalBackground == null);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        else
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }

            bounceDestroyObjs.Remove(stopStructure.bullet);
            for (int i = 0; i < bounceDestroyObjs.Count; ++i)
            {
                Destroy(bounceDestroyObjs[i].gameObject);
            }
            bounceDestroyObjs.Clear();

            var target = CombatManager.Instance.GetRandomTarget(!stopStructure.isHomeground);
            if (stopStructure.bullet == null)
                goto jump;
            var obj = stopStructure.bullet.transform;
            obj.localScale = new Vector3(-1 * obj.localScale.x, obj.localScale.y, obj.localScale.z);

            float bulletSpeed = 3f;
            float m = (target.center.position - obj.position).magnitude;
            var dir = (target.center.position - obj.position).normalized;

            while (m > bulletSpeed * Time.deltaTime)
            {
                m = (target.center.position - obj.position).magnitude;
                obj.position += dir * bulletSpeed * Time.deltaTime;
                yield return null;
            }

            stopStructure.bullet.Play("exit");
            target.Hurt(5);
            yield return new WaitForSeconds(0.25f);

            if (stopStructure.speicalBackground != null)
            {
                stopStructure.speicalBackground.Play("SpecialAttack_Background2");
                yield return new WaitUntil(() => stopStructure.speicalBackground == null);
                yield return new WaitForSeconds(0.25f);
            }
        }

    jump:

        isBounceBlock = false;
        isBlock = false;
    }
    private IEnumerator IceBite()
    {
        if (stopStructure.stopObj != null)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }

            isBlock = true;
            Vector2 startPosition = stopStructure.stopObj.transform.position;
            Vector2 endPosition = stopStructure.originPosition;
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                stopStructure.stopObj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);

            stopStructure.stopObj.sternStatus.Start(stopStructure.stopObj, icebite, stopStructure.stopObj.center, 5);
            var effect = stopStructure.stopObj.sternStatus.effect.GetComponent<Animator>();
            yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay"));
            yield return new WaitForSeconds(0.25f);
            stopStructure.stopObj.Hurt(5f);
            yield return new WaitForSeconds(0.25f);

            if (stopStructure.speicalBackground != null)
            {
                stopStructure.speicalBackground.Play("SpecialAttack_Background2");
                yield return new WaitUntil(() => stopStructure.speicalBackground == null);
                yield return new WaitForSeconds(0.25f);
            }

            isBlock = false;
        }
    }
    private IEnumerator ElectronicBite()
    {
        if (stopStructure.stopObj != null)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }

            isBlock = true;
            Vector2 startPosition = stopStructure.stopObj.transform.localPosition;
            Vector2 endPosition = stopStructure.originPosition;
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                stopStructure.stopObj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);


            stopStructure.stopObj.sternStatus.Start(stopStructure.stopObj, electronicNetShot, stopStructure.stopObj.center, 5);
            yield return new WaitForSeconds(0.6f);

            CombatManager.Instance.battleQueue.RemoveAll(x => x == stopStructure.stopObj);
            stopStructure.stopObj.battleInstance.maxDex = Mathf.Min(9f, stopStructure.stopObj.battleInstance.maxDex + (stopStructure.stopObj.battleInstance.maxDex * electronicNetData.buffValue));
            stopStructure.stopObj.battleInstance.dex = Mathf.Min(stopStructure.stopObj.battleInstance.maxDex, stopStructure.stopObj.battleInstance.dex + (stopStructure.stopObj.battleInstance.dex * electronicNetData.buffValue));

            if (stopStructure.speicalBackground != null)
            {
                stopStructure.speicalBackground.Play("SpecialAttack_Background2");
                yield return new WaitUntil(() => stopStructure.speicalBackground == null);
                yield return new WaitForSeconds(0.25f);
            }

            isBlock = false;
        }
    }
    private IEnumerator PoisonBite()
    {
        if (stopStructure.stopObj != null)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }

            isBlock = true;

            Vector2 startPosition = stopStructure.stopObj.transform.localPosition;
            Vector2 endPosition = stopStructure.originPosition;
            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                stopStructure.stopObj.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                isBlock = true;
                yield return null;
            }

            yield return new WaitForSeconds(0.25f);

            var clone = stopStructure.stopObj.dogDmgStatus.AddPoisenFlag(stopStructure.stopObj, poisenEffect);
            if (clone != null)
            {
                yield return new WaitUntil(() => clone == null);
                yield return new WaitForSeconds(0.25f);
            }

            clone = Instantiate(debuffSpeedEffect, stopStructure.stopObj.center.position, Quaternion.identity);
            stopStructure.stopObj.battleInstance.maxDex = Mathf.Min(9f, stopStructure.stopObj.battleInstance.maxDex + (stopStructure.stopObj.battleInstance.maxDex * poisonPoolData.buffValue));
            stopStructure.stopObj.battleInstance.dex = Mathf.Min(stopStructure.stopObj.battleInstance.maxDex, stopStructure.stopObj.battleInstance.dex + (stopStructure.stopObj.battleInstance.dex * poisonPoolData.buffValue));
            yield return new WaitUntil(() => clone == null);
            yield return new WaitForSeconds(0.25f);

            if (stopStructure.speicalBackground != null)
            {
                stopStructure.speicalBackground.Play("SpecialAttack_Background2");
                yield return new WaitUntil(() => stopStructure.speicalBackground == null);
                yield return new WaitForSeconds(0.25f);
            }

            isBlock = false;
        }
    }
    private IEnumerator DotDmgTurn()
    {
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        //화상
        var burnList = CombatManager.Instance.dotDmgObjList.FindAll(x => (x != null) && (x.dogDmgStatus.burnFlag == true));
        if (burnList.Count > 0)
        {
            Animator startBurnEffect = null;
            for (int i = 0; i < burnList.Count; ++i)
            {
                startBurnEffect = Instantiate(burnEffect, burnList[i].center.position, Quaternion.identity).GetComponent<Animator>();
                burnList[i].dogDmgStatus.DecreaseBurnCount(burnList[i]);
            }

            yield return new WaitUntil(() => startBurnEffect == null);
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < burnList.Count; ++i)
            {
                burnList[i].Hurt(5f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        //독
        var poisenList = CombatManager.Instance.dotDmgObjList.FindAll(x => (x != null) && (x.dogDmgStatus.poisenFlag == true));
        if (poisenList.Count > 0)
        {
            Animator startPoisenEffect = null;
            for (int i = 0; i < poisenList.Count; ++i)
            {
                startPoisenEffect = Instantiate(poisenEffect, poisenList[i].center.position, Quaternion.identity).GetComponent<Animator>();
                poisenList[i].dogDmgStatus.DecreasePosienCount(poisenList[i]);
            }

            yield return new WaitUntil(() => startPoisenEffect == null);
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < poisenList.Count; ++i)
            {
                poisenList[i].Hurt(poisenList[i].battleInstance.maxHp * 0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        //마비
        var palalysisList = CombatManager.Instance.dotDmgObjList.FindAll(x => (x != null) && (x.dogDmgStatus.elecFlag == true));
        if (palalysisList.Count > 0)
        {
            Animator startPalalysisEffect = null;
            for (int i = 0; i < palalysisList.Count; ++i)
            {
                startPalalysisEffect = Instantiate(palalysisEffect, palalysisList[i].center.position, Quaternion.identity).GetComponent<Animator>();
                palalysisList[i].dogDmgStatus.DecreaseElecCount(palalysisList[i]);
            }

            yield return new WaitUntil(() => startPalalysisEffect == null);
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < palalysisList.Count; ++i)
            {
                CombatManager.Instance.battleQueue.RemoveAll(x => x == palalysisList[i]);
                palalysisList[i].battleInstance.dex = Mathf.Max(0f, palalysisList[i].battleInstance.dex - (palalysisList[i].battleInstance.maxDex * 0.5f));
            }

            yield return new WaitForSeconds(0.5f);
        }

        //맹독
        var deadlyPoisonList = CombatManager.Instance.dotDmgObjList.FindAll(x => (x != null) && (x.dogDmgStatus.deadlyPoisonFlag == true));
        if (deadlyPoisonList.Count > 0)
        {
            Animator startDeadlyPoisonEffect = null;
            for (int i = 0; i < deadlyPoisonList.Count; ++i)
            {
                startDeadlyPoisonEffect = Instantiate(deadlyPoisonEffect, deadlyPoisonList[i].center.position, Quaternion.identity).GetComponent<Animator>();
                deadlyPoisonList[i].dogDmgStatus.DecreaseDeadlyPoisonCount(deadlyPoisonList[i]);
            }

            yield return new WaitUntil(() => startDeadlyPoisonEffect == null);
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < deadlyPoisonList.Count; ++i)
            {
                deadlyPoisonList[i].Hurt(deadlyPoisonList[i].battleInstance.maxHp * 0.25f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        //암
        var unableList = CombatManager.Instance.dotDmgObjList.FindAll(x => (x != null) && (x.dogDmgStatus.unableFlag == true));
        if (unableList.Count > 0)
        {
            Animator startUnblaeEffect = null;
            for (int i = 0; i < unableList.Count; ++i)
            {
                startUnblaeEffect = Instantiate(unableEffect, unableList[i].transform.position, Quaternion.identity).GetComponent<Animator>();
                unableList[i].dogDmgStatus.DecreaseUnableCount(unableList[i]);
            }

            yield return new WaitUntil(() => startUnblaeEffect == null);
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < unableList.Count; ++i)
            {
                unableList[i].battleInstance.atk = Mathf.Max(0f, unableList[i].battleInstance.atk - Mathf.Round(unableList[i].battleInstance.atk * 0.25f));
                unableList[i].battleInstance.def = Mathf.Max(0f, unableList[i].battleInstance.def - Mathf.Round(unableList[i].battleInstance.def * 0.25f));
            }

            yield return new WaitForSeconds(0.5f);
        }

        //재생
        var recoveryList = CombatManager.Instance.dotDmgObjList.FindAll(x => (x != null) && (x.dogDmgStatus.recoveryFlag == true));
        if (recoveryList.Count > 0)
        {
            Animator recoveryEffectAnim = null;
            for (int i = 0; i < recoveryList.Count; ++i)
            {
                recoveryEffectAnim = Instantiate(recoveryEffect, recoveryList[i].transform.position, Quaternion.identity).GetComponent<Animator>();
            }

            yield return new WaitUntil(() => recoveryEffectAnim == null);
            yield return new WaitForSeconds(0.25f);

            for (int i = 0; i < recoveryList.Count; ++i)
            {
                recoveryList[i].battleInstance.hp = Mathf.Min(recoveryList[i].battleInstance.maxHp, recoveryList[i].battleInstance.hp + (recoveryList[i].battleInstance.maxHp * 0.25f));
            }

            yield return new WaitForSeconds(0.5f);
        }

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        isBlock = false;
    }
    private IEnumerator SuctionRoutine(EntityMonster player)
    {
        var recoveryEffectAnim = Instantiate(recoveryEffect, player.transform.position, Quaternion.identity).GetComponent<Animator>();
        yield return new WaitUntil(() => recoveryEffectAnim == null);
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(player.RecoverRoutine(player.battleInstance.hp, Mathf.Min(player.battleInstance.maxHp, Mathf.RoundToInt(player.battleInstance.hp + (player.battleInstance.maxHp * player.battleInstance.hpRecoveryRatio))), 1f));
        yield return new WaitForSeconds(0.25f);
    }
    private IEnumerator RockBlaster_0(BattleStructure structure)
    {
        //필수
        isBlock = true;

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //마나감소
        structure.player.battleInstance.mp = Mathf.Max(0, structure.player.battleInstance.mp - structure.skillData.consumMpAmount);

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //연출
        var bullet = Instantiate(rockBlasterBullet, structure.player.bulletCreatePoint.position, Quaternion.identity);

        float monScale = structure.player.bulletCreatePoint.localScale.x;
        bullet.transform.localScale = new Vector3((CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? bullet.transform.localScale.x : -1 * Mathf.Abs(bullet.transform.localScale.x)) * monScale, bullet.transform.localScale.y * monScale, bullet.transform.localScale.z);
        var anim = bullet.GetComponent<Animator>();

        //멈추기데이터 초기화
        stopStructure.bullet = anim;
        stopStructure.stopObj = null;
        stopStructure.isHomeground = CombatManager.Instance.homegroundMonsters.Contains(structure.player);

        yield return new WaitUntil(() => (anim == null) || (anim != null && anim.GetCurrentAnimatorStateInfo(0).IsName("stay") == true));
        if (anim == null)
        {
            isBlock = false;
            stopStructure.dontMove = false;
            yield break;
        }

        float bulletSpeed = 3f;
        float addSpeed = 3f;
        float m = (target.center.position - bullet.transform.position).magnitude;
        var dir = (target.center.position - bullet.transform.position).normalized;

        float degreeAngle = CombatManager.Instance.homegroundMonsters.Contains(structure.player) ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg : (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 180f;
        bullet.transform.rotation = Quaternion.Euler(Vector3.forward * degreeAngle);
        while (m > bulletSpeed * Time.deltaTime)
        {
            if (stopStructure.dontMove == true)
                break;
            m = (target.center.position - bullet.transform.position).magnitude;
            bullet.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            yield return null;
        }

        //필수(데미지계산)
        if (stopStructure.dontMove == false)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                anim.Play("exit");
                anim.transform.rotation = Quaternion.identity;
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);
            }
            else
                Destroy(bullet);
        }

        yield return new WaitForSeconds(0.25f);

        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }

        //필수
        isBlock = false;
        stopStructure.dontMove = false;
    }
    private IEnumerator RockBlaster_1(BattleStructure structure)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //민첩초기화
        structure.player.battleInstance.dex = 0;

        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);

        //멈춤데이터초기화
        stopStructure.isBlock = true;

        //연출
        var rockParent = Instantiate(rockBlasterBulletParent, structure.player.bulletCreatePoint.position, Quaternion.identity).transform;
        var anim = rockParent.GetComponent<Animator>();
        List<Vector3> positionList = new List<Vector3>();

        int positionCount = rockClones.Count;
        Vector3 startPosition = structure.player.bulletCreatePoint.position;
        float distance = 0.4f;
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 localDir = Quaternion.Euler(0f, 0f, angle) * Vector3.right;
            Vector3 position = startPosition + (localDir * distance);
            positionList.Add(position);
        }

        int value = 0;
        for (int i = 0; i < positionCount; i++)
        {
            int value2 = value % 2;

            rockClones[i].GetComponent<SpriteRenderer>().sortingLayerName = "EB";
            rockClones[i].MoveTowards(rockParent, rockClones[i].transform.position, positionList[i]);
            SoundManager.Instance.PlayEffect(42, 1f);

            if (value2 == 0)
                yield return waitTime;
            else
                yield return null;

            value++;
        }

        for (int i = 0; i < positionCount; i++)
        {
            yield return new WaitUntil(() => rockClones[i].isEnd2 == true);
        }
        yield return waitTime;

        float bulletSpeed = 3f;
        float addSpeed = 3f;
        float m = (target.center.position - rockParent.position).magnitude;
        var dir = (target.center.position - rockParent.position).normalized;

        float soundTime = 0.5f;
        SoundManager.Instance.PlayEffect(32, 1f);

        CircleCollider2D circleCol = rockParent.GetComponent<CircleCollider2D>();
        while (m > bulletSpeed * Time.deltaTime)
        {
            m = (target.center.position - rockParent.position).magnitude;
            rockParent.transform.position += dir * bulletSpeed * Time.deltaTime;

            bulletSpeed += addSpeed * Time.deltaTime;
            rockParent.Rotate(Vector3.forward * -64f * Time.deltaTime);

            var cols = Physics2D.OverlapCircleAll((Vector2)rockParent.transform.position + circleCol.offset, circleCol.radius);
            for (int j = 0; j < cols.Length; ++j)
            {
                var wall = cols[j].GetComponent<Wall>();
                if (wall != null)
                    wall.Remove();
            }

            if (soundTime > 0f)
            {
                soundTime -= Time.deltaTime;
            }
            else
            {
                soundTime = 0.5f;
                SoundManager.Instance.PlayEffect(32, 1f);
            }
            yield return null;
        }

        //필수(데미지계산)
        if (rockParent != null)
        {
            bool dmgCheck = target.DmgCheck(structure.player, structure.skillData);
            if (dmgCheck == true)
            {
                int count = rockClones.Count;
                for (int i = 0; i < count; ++i)
                    rockClones[i].gameObject.SetActive(false);

                rockClones.Clear();
                anim.Play("exit");
                anim.transform.rotation = Quaternion.identity;
                target.Hurt(Mathf.Max(target.GetDefenseDeal(), (structure.player.battleInstance.atk + structure.skillData.atk + (count * 3)) - target.battleInstance.def), structure.player, structure.skillData);
                yield return new WaitUntil(() => anim == null);
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                rockClones.Clear();
                Destroy(rockParent.gameObject);
            }

        }
        yield return waitTime;

        //필수
        //반격기확인
        if (structure.player.passiveFlag == true && structure.player.currentPassiveName == PassiveName.FireBraces)
        {
            //타겟가져오기
            targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), fireBracesData.targetLayer);
            target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
            if (target != null)
                structure.player.CheckFireBaracesSkill(target);
        }
        //필수
        isBlock = false;
        stopStructure.isBlock = false;
    }
    private IEnumerator WillPower()
    {
        //필수
        isBlock = true;

        for (int i = 0; i < willPowerStructure.objs.Count; ++i)
            StartCoroutine(WillPowerRoutine(i));

        for (int i = 0; i < willPowerStructure.checkSkillEnds.Count; ++i)
            yield return new WaitUntil(() => willPowerStructure.checkSkillEnds[i] == true);

        willPowerStructure.objs.Clear();
        willPowerStructure.effects.Clear();
        willPowerStructure.checkSkillEnds.Clear();
        willPowerStructure.isHomegrounds.Clear();
        //필수
        isBlock = false;
        willPowerStructure.isStart = false;
    }
    private IEnumerator WillPowerRoutine(int index)
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        bool check = willPowerStructure.objs[index].checkDead2;
        int count = 5;
        bool isAlive = false;
        float percent = 0.1f;
        float offsetScale = willPowerStructure.isHomegrounds[index] ? -1f : 1f;

        Vector2 startPosition = Vector2.zero;
        if (check)
            startPosition = willPowerStructure.objs[index].center.position;
        else
        {
            Vector2 offset = willPowerStructure.objs[index].center.position - willPowerStructure.objs[index].transform.position;
        
            Vector3 fianlOffset = CombatManager.Instance.homegroundMonsters.Contains(willPowerStructure.objs[index]) ? new Vector3(-offset.y, offset.x) : new Vector3(offset.y, -offset.x);
            startPosition = willPowerStructure.objs[index].transform.position + fianlOffset;
        }
        for (int i = count; i > 0; i--)
        {
            var clone = Instantiate(countText, startPosition, Quaternion.identity);
            SoundManager.Instance.PlayEffect(157, 1f);
            clone.SetText(i.ToString());
            yield return new WaitUntil(() => clone == null);
            yield return waitTime;

            float value = Random.Range(0f, 1f);
            float percentValue = percent * (i / count);
            if (value <= 0f)
                continue;
            if (value <= percent)
            {
                isAlive = true;
                break;
            }
        }

        if (isAlive == true)
        {
            var weakupClone = Instantiate(weakUpEffect, startPosition, Quaternion.identity);
            yield return new WaitUntil(() => weakupClone == null);
            yield return new WaitForSeconds(0.75f);
            var anim2 = willPowerStructure.effects[index].GetComponent<Animator>();
            anim2.Play("weakUp");

            yield return new WaitUntil(() => anim2 == null);

            //초기화
            willPowerStructure.objs[index].deadBlock = false;

            if (willPowerStructure.isHomegrounds[index])
                CombatManager.Instance.homegroundMonsters.Add(willPowerStructure.objs[index]);
            else
                CombatManager.Instance.awayMonsters.Add(willPowerStructure.objs[index]);

            CombatManager.Instance.allMonsters.Add(willPowerStructure.objs[index]);
            willPowerStructure.objs[index].gameObject.SetActive(true);
            //  willPowerStructure.objs[index].battleInstance.dex = 0f;
            willPowerStructure.objs[index].battleInstance.hp = willPowerStructure.objs[index].battleInstance.maxHp * willPowerStructure.objs[index].battleInstance.hpRecoveryRatio;
            willPowerStructure.objs[index].GetComponent<HurtObject>().ResetHurt();
            willPowerStructure.objs[index].startRecovery = false;
            willPowerStructure.objs[index].sumStatusRatio = 0f;
            //
        }
        else
        {
            yield return waitTime;
            var anim2 = willPowerStructure.effects[index].GetComponent<Animator>();
            anim2.Play("knockDownDie");
            SoundManager.Instance.PlayEffect(147, 1f);
            Destroy(willPowerStructure.objs[index].gameObject);
        }

        yield return waitTime;
        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        willPowerStructure.checkSkillEnds[index] = true;
    }
    private IEnumerator EasterRoutine(List<EasterStructure> easters)
    {
        //필수
        isBlock = true;
        var emptyPositions = CombatManager.Instance.GetEmptyPositions(easters[0].isHomeground);
        if (emptyPositions.Count > 0)
        {
            WaitForSeconds waitTime = new WaitForSeconds(0.25f);

            var targets = easters.FindAll(x => x.complete == true);
            easters.RemoveAll(x => x.complete == true);

            //연출
            Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
            anim.Play("SpecialAttack_Background");

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
            yield return waitTime;

            GameObject effect = null;
            for (int i = 0; i < targets.Count; ++i)
            {
                bool check = false;
                var emptyPosition = CombatManager.Instance.GetEmptyPosition(targets[i].isHomeground, targets[i].emptyPosition, out check);
                if (check == true)
                {
                    Vector2 heightDisOffset = new Vector2(CombatManager.Instance.heightDistance * (emptyPosition.height + 1), 0);

                    targets[i].obj.hp = targets[i].obj.maxHp;
                    targets[i].obj.heathState = MonsterHeathState.FullCondition;
                    var clone = EntityMonster.CreateMonster(targets[i].obj, emptyPosition.worldPosition - heightDisOffset - (Vector2)targets[i].obj.monsterData.monsterPrefab.transform.position, emptyPosition.width, emptyPosition.height, targets[i].isHomeground);
                    effect = Instantiate(easter, clone.transform.position, Quaternion.identity);
                }
            }

            yield return new WaitUntil(() => effect == null);
            yield return waitTime;
            anim.Play("SpecialAttack_Background2");
            yield return new WaitUntil(() => anim == null);
            yield return waitTime;

            //필수
            isBlock = false;
        }
        else
        {
            isBlock = false;
            yield break;
        }
    }
    private IEnumerator LivingDeadRoutine(List<EntityMonster> livingDeadTargets)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        var targets = livingDeadTargets.ToList();
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].DissolveDead();
        }

        yield return new WaitForSeconds(3.25f);
        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        isBlock = false;
    }
    private IEnumerator DawnTurn(bool isHomeground)
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        var targets = (isHomeground) ? CombatManager.Instance.awayMonsters : CombatManager.Instance.homegroundMonsters;
        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        GameObject clone = null;
        for (int i = 0; i < targets.Count; ++i)
        {
            clone = Instantiate(dawnDeathCountEffect, targets[i].center.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;


        if (isHomeground)
        {
            if (homegroundDawnCount <= 3f)
                homegroundDawnCount++;
            else
            {
                targets = targets.ToList();
                for (int i = 0; i < targets.Count; ++i)
                {
                    targets[i].DissolveDead();
                }

                yield return new WaitForSeconds(3.25f);
            }
        }
        else
        {
            if (awayDawnCount <= 3f)
                awayDawnCount++;
            else
            {
                targets = targets.ToList();
                for (int i = 0; i < targets.Count; ++i)
                {
                    targets[i].DissolveDead();
                }

                yield return new WaitForSeconds(3.25f);
            }
        }

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        isBlock = false;
    }
    private IEnumerator DarkHoleTurn(DarkHoleStructure darkHoleStructure)
    {
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        bool check = false;
        var emptyPosition = CombatManager.Instance.GetEmptyPosition(darkHoleStructure.isHomeground, darkHoleStructure.emptyPosition, out check);
        if (check == true)
        {
            //연출
            Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
            anim.Play("SpecialAttack_Background");
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
            yield return waitTime;

            SoundManager.Instance.PlayEffect(137, 1f);
            var effect = Instantiate(darkHole, emptyPosition.worldPosition, Quaternion.identity).GetComponent<Animator>();
            effect.GetComponent<SpriteRenderer>().sortingOrder = -1;
            yield return new WaitUntil(() => effect.GetCurrentAnimatorStateInfo(0).IsName("stay"));
            yield return waitTime;

            darkHoleStructure.obj.Return(darkHoleStructure.isHomeground, darkHoleStructure.emptyPosition);
            darkHoleList.Remove(darkHoleStructure);

            yield return waitTime;

            effect.Play("exit");
            yield return new WaitUntil(() => effect == null);
            yield return waitTime;

            anim.Play("SpecialAttack_Background2");
            yield return new WaitUntil(() => anim == null);
            yield return waitTime;
        }

        isBlock = false;
    }
    private IEnumerator FlyRoutine(EntityMonster target, EntityMonster target2)
    {
        target.checkSwapPosition = true;
        Vector2 startPosition = target.transform.position;
        Vector2 endPosition = target2.transform.position;

        float lerpSpeed = 4f;
        float lerpTime = 1f;
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float t = currentTime / lerpTime;

            t = t * t * t * (t * (6f * t - 15f) + 10f);
            target.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float t = currentTime / lerpTime;

            t = t * t * t * (t * (6f * t - 15f) + 10f);
            target.transform.position = Vector2.Lerp(endPosition, startPosition, t);
            yield return null;
        }

        var effect = Instantiate(nearDistanceHitEffect, target.center.position, Quaternion.identity);
        effect = Instantiate(nearDistanceHitEffect, target2.center.position, Quaternion.identity);

        target.Hurt(Mathf.Max(target.GetDefenseDeal(), (5 - target.battleInstance.def)));
        target2.Hurt(Mathf.Max(target.GetDefenseDeal(), (5 - target.battleInstance.def)));

        yield return new WaitUntil(() => effect == null);
        yield return new WaitForSeconds(0.25f);

        target.checkSwapPosition = false;
    }
    private IEnumerator FlyRoutine2(EntityMonster target, int index = 1)
    {
        target.checkSwapPosition = true;
        Vector2 startPosition = target.transform.position;
        Vector2 endPosition = (Vector2)CombatManager.Instance.GetPosition(CombatManager.Instance.homegroundMonsters.Contains(target), target.width + index, target.height) + new Vector2(0, target.originInstance.monsterData.monsterPrefab.transform.position.y);

        float lerpSpeed = 8f;
        float lerpTime = 1f;
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float t = currentTime / lerpTime;

            t = t * t * t * (t * (6f * t - 15f) + 10f);
            target.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        target.width += index;
        yield return new WaitForSeconds(0.25f);
        target.checkSwapPosition = false;
    }
    private IEnumerator FlyRoutine3(EntityMonster target, EntityMonster target2)
    {
        target.checkSwapPosition = true;
        Vector2 startPosition = target.transform.position;
        Vector2 endPosition = target2.transform.position;

        float lerpSpeed = 4f;
        float lerpTime = 1f;
        float currentTime = 0f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float t = currentTime / lerpTime;

            t = t * t * t * (t * (6f * t - 15f) + 10f);
            target.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        target.checkSwapPosition = false;
    }
    private IEnumerator OnShieldRoutine(EntityMonster target)
    {
        SoundManager.Instance.PlayEffect(145, 1f);
        Transform shieldTarget = target.transform.GetChild(3);

        float start = 0f;
        //float end = Mathf.Abs(1 / target.transform.localScale.x);
        float end = 1f * Mathf.Abs(shieldTarget.localScale.x);
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 0.5f;

        //float flip = (isFlip == false) ? 1f : -1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(start, end, currentSpeed);
            shieldTarget.localScale = new Vector3(value, value, 1f);
            yield return null;
        }
    }
    private IEnumerator MindControlReturn(EntityMonster target)
    {
        bool isHomeground = CombatManager.Instance.homegroundMonsters.Contains(target);
        var newPositions = CombatManager.Instance.GetEmptyPositions(!isHomeground);
        if (newPositions.Count > 0)
        {
            mindControlReturnTarget = target;

            var renderer = target.GetComponent<SpriteRenderer>();
            renderer.color = Color.white;

            if (isHomeground == true)
            {
                CombatManager.Instance.homegroundMonsters.Remove(target);
                CombatManager.Instance.awayMonsters.Add(target);
            }
            else
            {
                CombatManager.Instance.awayMonsters.Remove(target);
                CombatManager.Instance.homegroundMonsters.Add(target);
            }
            target.transform.localScale = new Vector3(-1 * target.transform.localScale.x, target.transform.localScale.y, target.transform.localScale.z);
            target.transform.GetChild(0).GetComponent<StatusUI>().Init();

            var newPosition = newPositions[Random.Range(0, newPositions.Count)];
            target.width = newPosition.width;
            target.height = newPosition.height;

            Vector3 startPosition = target.transform.position;
            Vector3 endPosition = newPosition.worldPosition + new Vector2(0, target.originInstance.monsterData.monsterPrefab.transform.position.y);

            float lerpSpeed = 4f;
            float currentTime = 0f;
            float lerpTime = 1f;

            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                target.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                yield return null;
            }

            target.checkMindControl = false;
        }
        else
            yield break;
    }

    private IEnumerator SheerColdTurnRoutine()
    {
        //필수
        isBlock = true;
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        //타겟설정
        List<EntityMonster> targets = null;

        if (checkSheercoldHomeground == false)
            targets = CombatManager.Instance.awayMonsters.ToList();
        else
            targets = CombatManager.Instance.homegroundMonsters.ToList();

        //연출
        Animator anim = Instantiate(spicalAttackBackground, Vector2.zero, Quaternion.identity).GetComponent<Animator>();
        anim.Play("SpecialAttack_Background");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle") == true);
        yield return waitTime;

        GameObject clone = null;

        for(int i = 0; i < targets.Count; ++i)
        {
            clone = Instantiate(sheercoldRandomDeathEffect, targets[i].center.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => clone == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            var dmgCheck = targets[i].DmgCheck(sheercoldData.buffValue);
            if(dmgCheck == true)
            {
                targets[i].Dead();
            }
         
        }

        anim.Play("SpecialAttack_Background2");
        yield return new WaitUntil(() => anim == null);
        yield return waitTime;

        //필수
        sheerColdCount--;
        sheerColdTime = 12f;
        isBlock = false;
    }

    public IEnumerator ZoomInOut(float time, float end, bool isDamp)
    {
        var cam = Camera.main;
        var pixelPerfect = cam.GetComponent<PixelPerfectCamera>();
        pixelPerfect.enabled = false;

        float lerpSpeed = 1f;
        float lerpTime = time;
        float currentTime = 0f;

        float start = cam.orthographicSize;

        if (isDamp)
        {
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float t = currentTime / lerpTime;

                t = t * t * t * (t * (6f * t - 15f) + 10f);

                cam.orthographicSize = Mathf.Lerp(start, end, t);
                yield return null;
            }
        }
        else
        {
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;

                cam.orthographicSize = Mathf.Lerp(start, end, currentSpeed);
                yield return null;
            }
        }
       

        pixelPerfect.enabled = true;
    }
    public IEnumerator SetCamPosition(float time, Vector2 end)
    {
        float lerpSpeed = 1f;
        float lerpTime = time;
        float currentTime = 0f;

        Vector2 start = Camera.main.transform.position;

        float z = Camera.main.transform.position.z;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;

            Vector2 position = Vector2.Lerp(start, end, currentSpeed);
            Camera.main.transform.position = new Vector3(position.x, position.y, z);
            yield return null;
        }

    }
}
