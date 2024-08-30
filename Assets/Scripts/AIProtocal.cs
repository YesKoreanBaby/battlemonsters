using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
//Distributed: 하나의 width에 하나씩 배치된다 이때 height이 중복되면 안된다 마지막으로 랜덤한 곳에 하나를 놓는다
//Opportunity: 전방 후방에 사이드에 하나씩 놓는다
//Pressure: 전방에 세명 가운데 혹은 후방에 하나를 놓는다
//Evasion: 후방에 세명 가운데 혹은 전방에 하나를 놓는다
public enum AIFormationType { Random, Distributed, Opportunity, Pressure, Evasion, Diamond }
public enum AIRoll { OnlyTanker, OnlyDealer, OnceDealer, TankerDealer, SupporterHealer, SuppoerterDealer, SuppoerterTraper }

//T: OnlyTanker
//D: OnlyDealer
//OD: OnceDealer
//TD: TankerDealer
//SH: SuppoerterHealer
//SD: SupooerterDealer
//ST: SuppoerterTraper
//Strong
public enum AIStrategyType {
    T4, //Distributed, Opportunity
    D4, //Distributed, Opportunity
    OD4, //Distributed, Opportunity
    TD4, //Distributed, Opportunity
    T1_3D, //Evasion
    T1_3OD, //Evasion
    T1_3TD, //Evasion
    T3_D1, //Pressure
    T3_OD1, //Pressure
    T3_TD1, //Pressure
    SH3_D1, //Evasion
    SH3_OD1, //Evasion
    SH3_TD1, //Evasion
    SD3_D1, //Evasion
    SD3_OD1, //Evasion
    SD3_TD1, //Evasion
    ST3_D1, //Evasion
    ST3_OD1, //Evasion
    ST3_TD1, //Evasion
    SH1_SD1_ST1_OD1, //Distributed, Diamond
    SH1_SD1_T1_OD1, //Distributed, Diamond
    T1_SH1_D2,  //Diamond 
    T1_SD1_D2,  //Diamond 
    T1_ST1_D2,  //Diamond 
    T1_SH1_OD2, //Diamond 
    T1_SD1_OD2, //Diamond 
    T1_ST1_OD2, //Diamond 
    T1_SH1_TD2, //Diamond 
    T1_SD1_TD2, //Diamond 
    T1_ST1_TD2, //Diamond 
    END
}

[System.Serializable]
public class AIProtocalUserData
{
    public string userName;
    public string id;
    public FriendShipLevel level;
    public bool isBlock;
}
public class AIProtocal : MonoBehaviour
{

}
