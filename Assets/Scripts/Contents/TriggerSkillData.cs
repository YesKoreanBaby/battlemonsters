using UnityEngine;

public enum SkillTrigger { CounterAttack, FirstTurn, HpLack, MpLack, TotalHpLack, OtherEnemy, OneEnemy, Alone, SternState, DotDmgState, NotSternState, NotDotDmgState, TeamDie, EnemyDie, SpeedLack, Disadvantage, HpLackAlways, MpLackAlways, Unbreakable, OtherTeam }

[CreateAssetMenu(fileName = "TriggerSkillData", menuName = "Data/TriggerSkillData")]
public class TriggerSkillData : ScriptableObject
{
    public SkillData skillData;
    public SkillTrigger skillTrigger;
}
