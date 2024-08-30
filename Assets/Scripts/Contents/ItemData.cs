using UnityEngine;

public enum ItemEffectType { Earn5Gold, Earn10Gold, Earn15Gold, Earn25Gold, Earn50Gold, Earn100Gold, Earn500Gold, Earn1000Gold, EarnHp, EarnMp, Earndex, DamageHp, DamageMp, DamageDex, AddPoison, Earn1Diamond, EarnPoisonStone, EarnElecStone, EarnNormalStone, HeroBook, OldBook, MiracleNeck, ThorHammer, MagicalLeaf, EarnFireStone, EarnGunslinger }

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemImage;
    public ItemEffectType effectType;
    public bool isAlive;
}
