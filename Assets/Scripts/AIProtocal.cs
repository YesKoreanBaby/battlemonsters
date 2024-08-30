using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
//Distributed: �ϳ��� width�� �ϳ��� ��ġ�ȴ� �̶� height�� �ߺ��Ǹ� �ȵȴ� ���������� ������ ���� �ϳ��� ���´�
//Opportunity: ���� �Ĺ濡 ���̵忡 �ϳ��� ���´�
//Pressure: ���濡 ���� ��� Ȥ�� �Ĺ濡 �ϳ��� ���´�
//Evasion: �Ĺ濡 ���� ��� Ȥ�� ���濡 �ϳ��� ���´�
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
