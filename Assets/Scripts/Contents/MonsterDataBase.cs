using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FieldType { Volcano, Snow, Desert, Forest, Cave, Plateau, Shine, Spooky, Fall, Desert2, Spooky2, Snow2, Ship, Circuit, End }
public enum BattleType { Story, Official, FriendShip, Gamebling, PvP}
public class MonsterDataBase : MonoBehaviour
{
    public Sprite[] purchaseGoldTable;
    public Sprite[] purchaseDiamondTable;
    public MonsterData[] deadTable;
    public MonsterData[] bugs;
    public MonsterData[] randomBoxies;
    public MonsterData[] dragons;
    public MonsterData[] randomBoxies2;
    public MonsterData[] productMonsters;
    public MonsterData darkKnightWisp;
    public MonsterData undeadKingWisp;
    public MonsterData undeadKing;
    public MonsterData darkKnight;
    public MonsterData pupleGhost;
    public MonsterData redGhost;
    public MonsterData greenGhost;
    public MonsterData livinglegendData;
    public MonsterData stage_1RewardMonster;
    public EntityMonster checkBasilisk;
    public EntityMonster checkDrake;
    public EntityMonster checkLivingLegend;
    public ItemElementalData potion;
    public ItemElementalData deadPotion;
    public ItemElementalData fullCondition;
    public ItemElementalData statusPotion;
    public ItemElementalData evolutionCandy;
    public ItemElementalData karmaCandy;
    public ItemElementalData[] tranningBooks;
    public ItemElementalData oldBook;
    public ItemElementalData heroBook;
    public ItemElementalData miracleNeck;
    public ItemElementalData magicalLeaf;
    public ItemElementalData thorHammer;
    public ItemElementalData fireStone;
    public ItemElementalData iceStone;
    public ItemElementalData normalStone;
    public ItemElementalData elecStone;
    public ItemElementalData poisonStone;
    public ItemElementalData easterstone;
    public ItemElementalData piggyBank;
    public ItemElementalData symbolofSwordMaster;
    public ItemElementalData symbolofGunslinger;
    public ItemData treagureInsteadData;
    public ConversationData shopKeeperData;
    public ConversationData bartenderData;
    public ConversationData repoterData;
    public ConversationData friendShipGradeWin;
    public ConversationData friendShipGradeLose;
    public ConversationData tutorialData;
    public ConversationData tutorialEndData;
    public List<int> lotteryTable;
    public List<int> diamondPayTable;
    public List<MonsterShopData> uncomonDatas;
    public List<MonsterShopData> comonDatas;
    public List<MonsterShopData> lairDatas;
    public List<ItemShopData> itemDatas;

    public Sprite tutorialIcon;
    public MonsterData infightTutorialData;
    public MonsterData outFightTutorialData;
    public MonsterData slergerTutorialData;
    public TMP_FontAsset buffFont;
    public TMP_FontAsset debuffFont;
    public TMP_FontAsset krFont;
    public TMP_FontAsset ehFont;

    public FieldType[] friendShipNormalFIelds;
    public FieldType[] friendShipHardFields;

    private static MonsterDataBase instance = null;
    public static MonsterDataBase Instance { get { return instance; } }

    //AbilityIcon
    public SerializableDictionary<AbilityType, AbilityDictionaryData> abilityDatas = new SerializableDictionary<AbilityType, AbilityDictionaryData>();
    public SerializableDictionary<FieldType, GameObject> fieldTypes = new SerializableDictionary<FieldType, GameObject>();
    public SerializableDictionary<TranningType, TranningCicleData> tranningDatas = new SerializableDictionary<TranningType, TranningCicleData>();
    public SerializableDictionary<MonsterHeathState, Sprite> heathIcons = new SerializableDictionary<MonsterHeathState, Sprite>();
    public List<SkillData> randomSkills;

    //Icon
    public Sprite oneIcon;
    public Sprite twoIcon;
    private void Awake()
    {
        instance = this;
    }
}
