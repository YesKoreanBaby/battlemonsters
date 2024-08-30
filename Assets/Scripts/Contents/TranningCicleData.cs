using UnityEngine;

public enum TranningType { Random, Balance, Genius, Late_Bloomer, Laziness, Atk, Def, Hp, Mp, Speed, Locked, End}
public enum BattleTranningType { None, Hp, Mp, Atk, Def, Balance, End}
[CreateAssetMenu(fileName = "TranningCicle", menuName = "Data/TranningCicleData")]
public class TranningCicleData : ScriptableObject
{
    public StatTranningData hpData;
    public StatTranningData mpData;
    public StatTranningData atkData;
    public StatTranningData dexData;
    public StatTranningData defData;
    public StatTranningData hpRecoveryData;
    public StatTranningData mpRecoveryData;
    public StatTranningData criticalData;
    public StatTranningData dodgeData;
}
