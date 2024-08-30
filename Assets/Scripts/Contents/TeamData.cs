using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamData", menuName = "Data/TeamData")]
public class TeamData : ScriptableObject
{
    public string teamName;

    public int commentViewIndex;

    public int teamLevel;

    public Sprite teamIcon;

    public List<MonsterData> startMonsters;
    public List<CombineData> startCombineDatas;
    public int startMoney;
    public int startDiamond;

    [Space]
    [Space]
    [Space]
    [Space]
    public int win;
    public int draw;
    public int lose;
    public int score;

    [Space]
    [Space]
    [Space]
    [Space]
    public BattleData league_3Data;
    public BattleData league_2Data;
    public BattleData league_1Data;
    public BattleData championData;

    public void Init()
    {
        win = draw = lose = score = 0;
    }
    public void CaculateScore()
    {
        score = (3 * win) + (1 * draw) + (-1 * lose);
    }
}
