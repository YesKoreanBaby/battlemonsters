using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FriendShipLevel { Begginer, Bronz, Silver, Gold, Platinum, Diamond,  Legendary, Ancient,  Myth}
public class LeagueManager : MonoBehaviour
{
    public int timecount;

    [System.NonSerialized]
    public int currentTimeCount;

    [System.NonSerialized]
    public bool blockGamebling = false;

    [System.NonSerialized]
    public bool blockResetItemForAds = false;

    [System.NonSerialized]
    public bool blockResetMonsterForAds = false;

    [System.NonSerialized]
    public int diamondToGold;

    [System.NonSerialized]
    public FriendShipLevel friendshipLevel;

    [System.NonSerialized]
    public Dictionary<int, List<Tuple<TeamData, TeamData>>> currentLeague;

    [System.NonSerialized]
    public List<TeamData> currentLeagTeam;

    [System.NonSerialized]
    public List<TeamData> league3TeamDatas;

    [System.NonSerialized]
    public List<TeamData> league2TeamDatas;

    [System.NonSerialized]
    public List<TeamData> league1TeamDatas;

    [System.NonSerialized]
    public List<TeamData> league0TeamDatas;

    [System.NonSerialized]
    public List<Tuple<TeamData, TeamData>> finalMatch;

    [System.NonSerialized]
    public List<string> battleReports;

    [System.NonSerialized]
    public TeamData defendingChampionData;
    public Dictionary<int, List<Tuple<TeamData, TeamData>>> league_3;
    public Dictionary<int, List<Tuple<TeamData, TeamData>>> league_2;
    public Dictionary<int, List<Tuple<TeamData, TeamData>>> league_1;
    public Dictionary<int, List<Tuple<TeamData, TeamData>>> league_0;

    private static LeagueManager instance = null;
    public static LeagueManager Instance { get { return instance; } }

    private void Awake()
    {
        instance = this; 
    }

    public void Clear()
    {
        currentTimeCount = timecount;

        var teams = SelectTeamUI.Instance.teamDatas;
        for (int i = 0; i < teams.Length; ++i)
        {
            teams[i].Init();
        }
    }
    public void CaculateScores()
    {
        for(int i = 0; i < currentLeagTeam.Count; ++i)
        {
            currentLeagTeam[i].CaculateScore();
        }

        currentLeagTeam.Sort((a, b) =>
        {
            return b.score.CompareTo(a.score);
        });
    }
    public int GetSwapIndex()
    {
        for (int i = 0; i < currentLeagTeam.Count; ++i)
        {
            currentLeagTeam[i].CaculateScore();
        }

        var lists = currentLeagTeam.ToList();
        lists.Sort((a, b) =>
        {
            return b.score.CompareTo(a.score);
        });

        var playerTeam = SelectTeamUI.Instance.selectTeam;
        return lists.FindIndex(x => x == playerTeam);
    }
    public bool CheckAllFaint()
    {
        var monsterDatas = TranningUI.Instance.playerInventory.monsterDatas;
        for(int i = 0; i < monsterDatas.Length; ++i)
        {
            if (monsterDatas[i] != null && (monsterDatas[i].heathState != MonsterHeathState.Faint))
            {
                return false;
            }
        }

        return true;
    }
    public void SetRandomAIScores(List<Tuple<TeamData, TeamData>> machingDatas)
    {
        var playerTeam = SelectTeamUI.Instance.selectTeam;
        for(int i = 0; i < machingDatas.Count; ++i)
        {
            if ((machingDatas[i].Item1 != playerTeam) && (machingDatas[i].Item2 != playerTeam))
                SetRandomAIScore(machingDatas[i]);
        }
    }
    public void SetLeagueTarget(Dictionary<int, List<Tuple<TeamData, TeamData>>> league)
    {
        currentLeague = league;
        if (league == league_3)
            currentLeagTeam = league3TeamDatas;
        else if (league == league_2)
            currentLeagTeam = league2TeamDatas;
        else if (league == league_1)
            currentLeagTeam = league1TeamDatas;
        else if (league == league_0)
            currentLeagTeam = league0TeamDatas;

        if(currentLeagTeam != league0TeamDatas)
            SwapDuplicatedTeams();
    }
    public void SetFinalMatch()
    {
        List<int> indies = currentLeague.Keys.ToList();
        indies.Sort((a, b) =>
        {
            return a.CompareTo(b);
        });

        finalMatch = currentLeague[indies[indies.Count - 1]];
    }
    public void StartLeague(TeamData playerTeamData, TeamData[] teamDatas)
    {
        //리그 초기화
        Clear();

        //플레이어 삭제
        List<TeamData> teamList = teamDatas.ToList();
        teamList.Remove(playerTeamData);

        //리포트 초기화
        battleReports = new List<string>();

        //디펜딩챔피언 결정
        defendingChampionData = teamList[Random.Range(0, teamList.Count)];
        teamList.Remove(defendingChampionData);

        //팀 셔플
        for (int i = 0; i < 100; ++i)
        {
            int aIndex = Random.Range(0, teamList.Count);
            int bIndex = Random.Range(0, teamList.Count);
            while(aIndex == bIndex)
                bIndex = Random.Range(0, teamList.Count);

            var tmp = teamList[aIndex];
            teamList[aIndex] = teamList[bIndex];
            teamList[bIndex] = tmp;
        }

        //1, 2, 3부리 리그 나누기
        league3TeamDatas = new List<TeamData>();
        for(int i = 0; i < 11; ++i)
        {
            league3TeamDatas.Add(teamList[i]);
        }
        league3TeamDatas.Add(playerTeamData);
        teamList.RemoveRange(0, 11);

        league2TeamDatas = new List<TeamData>();
        for (int i = 0; i < 8; ++i)
        {
            league2TeamDatas.Add(teamList[i]);
        }
        teamList.RemoveRange(0, 8);

        league1TeamDatas = new List<TeamData>();
        for (int i = 0; i < 6; ++i)
        {
            league1TeamDatas.Add(teamList[i]);
        }
        teamList.RemoveRange(0, 6);

        //배틀 스케줄 만들기
        List<List<Tuple<TeamData, TeamData>>>  league_3Tuple = GetBattleSchedules(league3TeamDatas);
        List<List<Tuple<TeamData, TeamData>>>  league_2Tuple = GetBattleSchedules(league2TeamDatas);
        List<List<Tuple<TeamData, TeamData>>>  league_1Tuple = GetBattleSchedules(league1TeamDatas);

        this.league_3 = ConvertBattleSchedule(league_3Tuple);
        this.league_2 = ConvertBattleSchedule(league_2Tuple);
        this.league_1 = ConvertBattleSchedule(league_1Tuple);

        diamondToGold = MonsterDataBase.Instance.diamondPayTable[Random.Range(0, MonsterDataBase.Instance.diamondPayTable.Count)];
        DialougeUI.Instance.SetMonsterDatas();
        DialougeUI.Instance.SetItemDatas();

        RulletUI.Instance.Init();
        RulletUI.Instance.Clear();
    }
    public void RestartLeague()
    {
        //승격
        Promotion();

        //리그 초기화
        Clear();

        //배틀 스케줄 만들기
        List<List<Tuple<TeamData, TeamData>>> league_3Tuple = GetBattleSchedules(league3TeamDatas);
        List<List<Tuple<TeamData, TeamData>>> league_2Tuple = GetBattleSchedules(league2TeamDatas);
        List<List<Tuple<TeamData, TeamData>>> league_1Tuple = GetBattleSchedules(league1TeamDatas);

        this.league_3 = ConvertBattleSchedule(league_3Tuple);
        this.league_2 = ConvertBattleSchedule(league_2Tuple);
        this.league_1 = ConvertBattleSchedule(league_1Tuple);

        diamondToGold = MonsterDataBase.Instance.diamondPayTable[Random.Range(0, MonsterDataBase.Instance.diamondPayTable.Count)];
        DialougeUI.Instance.SetMonsterDatas();
        DialougeUI.Instance.SetItemDatas();
        RulletUI.Instance.Clear();
    }
    public void RestartDefendingLeage()
    {
        //리그 초기화
        Clear();

        var playerTeam = SelectTeamUI.Instance.selectTeam;

        if(defendingChampionData == playerTeam)
        {
            league0TeamDatas = new List<TeamData>();

            int random = Random.Range(0, 3);
            var teamDatas = GetLeagueTeams(random + 1);
            league0TeamDatas.Add(teamDatas[Random.Range(0, league0TeamDatas.Count)]);
            league0TeamDatas.Add(playerTeam);

            this.league_0 = new Dictionary<int, List<Tuple<TeamData, TeamData>>>();
            league_0.Add(Random.Range(10, 29), new List<Tuple<TeamData, TeamData>>() { new Tuple<TeamData, TeamData>(defendingChampionData, league0TeamDatas[0]) });

            currentLeagTeam = league0TeamDatas;
            currentLeague = league_0;

            diamondToGold = MonsterDataBase.Instance.diamondPayTable[Random.Range(0, MonsterDataBase.Instance.diamondPayTable.Count)];
            DialougeUI.Instance.SetMonsterDatas();
            DialougeUI.Instance.SetItemDatas();
            RulletUI.Instance.Clear();
        }
        else
        {
            var loseTeam = defendingChampionData;
            defendingChampionData = playerTeam;
            league1TeamDatas.Remove(playerTeam);
            league1TeamDatas.Add(loseTeam);

            league0TeamDatas = new List<TeamData>();

            int random = Random.Range(0, 3);
            var teamDatas = GetLeagueTeams(random + 1);
            league0TeamDatas.Add(teamDatas[Random.Range(0, league0TeamDatas.Count)]);
            league0TeamDatas.Add(playerTeam);

            this.league_0 = new Dictionary<int, List<Tuple<TeamData, TeamData>>>();
            league_0.Add(Random.Range(10, 29), new List<Tuple<TeamData, TeamData>>() { new Tuple<TeamData, TeamData>(defendingChampionData, league0TeamDatas[0]) });

            currentLeagTeam = league0TeamDatas;
            currentLeague = league_0;

            diamondToGold = MonsterDataBase.Instance.diamondPayTable[Random.Range(0, MonsterDataBase.Instance.diamondPayTable.Count)];
            DialougeUI.Instance.SetMonsterDatas();
            DialougeUI.Instance.SetItemDatas();
            RulletUI.Instance.Clear();
        }
    }
    public void RelegationChampion()
    {
        league0TeamDatas = null;
        league_0 = null;

        var loseTeam = defendingChampionData;
        var winTeam = league1TeamDatas[0];
        defendingChampionData = winTeam;
        league1TeamDatas.Remove(winTeam);
        league1TeamDatas.Add(loseTeam);

        Clear();

        //배틀 스케줄 만들기
        List<List<Tuple<TeamData, TeamData>>> league_3Tuple = GetBattleSchedules(league3TeamDatas);
        List<List<Tuple<TeamData, TeamData>>> league_2Tuple = GetBattleSchedules(league2TeamDatas);
        List<List<Tuple<TeamData, TeamData>>> league_1Tuple = GetBattleSchedules(league1TeamDatas);

        this.league_3 = ConvertBattleSchedule(league_3Tuple);
        this.league_2 = ConvertBattleSchedule(league_2Tuple);
        this.league_1 = ConvertBattleSchedule(league_1Tuple);

        currentLeagTeam = league1TeamDatas;
        currentLeague = league_1;

        diamondToGold = MonsterDataBase.Instance.diamondPayTable[Random.Range(0, MonsterDataBase.Instance.diamondPayTable.Count)];
        DialougeUI.Instance.SetMonsterDatas();
        DialougeUI.Instance.SetItemDatas();
        RulletUI.Instance.Clear();
    }
    public void Promotion()
    {
        CaculateScores();

        //3부 리그 팀
        var team_1 = league3TeamDatas[0];
        var team_2 = league3TeamDatas[1];
        var team_3 = league3TeamDatas[2];

        //2부 리그 팀
        var team_4 = league2TeamDatas[0];
        var team_5 = league2TeamDatas[1];
        var team_6 = league2TeamDatas[2];

        var team_7 = league2TeamDatas[league2TeamDatas.Count - 1];
        var team_8 = league2TeamDatas[league2TeamDatas.Count - 2];
        var team_9 = league2TeamDatas[league2TeamDatas.Count - 3];

        //1부 리그 팀
        var team_10 = league1TeamDatas[league1TeamDatas.Count - 1];
        var team_11 = league1TeamDatas[league1TeamDatas.Count - 2];
        var team_12 = league1TeamDatas[league1TeamDatas.Count - 3];

        //3부 승격
        league2TeamDatas.Add(team_1);
        league2TeamDatas.Add(team_2);
        league2TeamDatas.Add(team_3);
        league3TeamDatas.Remove(team_1);
        league3TeamDatas.Remove(team_2);
        league3TeamDatas.Remove(team_3);

        //2부 승격 강등
        league1TeamDatas.Add(team_4);
        league1TeamDatas.Add(team_5);
        league1TeamDatas.Add(team_6);
        league2TeamDatas.Remove(team_4);
        league2TeamDatas.Remove(team_5);
        league2TeamDatas.Remove(team_6);

        league3TeamDatas.Add(team_7);
        league3TeamDatas.Add(team_8);
        league3TeamDatas.Add(team_9);
        league2TeamDatas.Remove(team_7);
        league2TeamDatas.Remove(team_8);
        league2TeamDatas.Remove(team_9);

        //1부 강등
        league2TeamDatas.Add(team_10);
        league2TeamDatas.Add(team_11);
        league2TeamDatas.Add(team_12);
        league1TeamDatas.Remove(team_10);
        league1TeamDatas.Remove(team_11);
        league1TeamDatas.Remove(team_12);
    }
    public void Report(int windAndLose)
    {
        List<Tuple<TeamData, TeamData>> machingDatas = null;
        currentLeague.TryGetValue(timecount - currentTimeCount, out machingDatas);
        TeamData machingTeam = FindPlayMatchTeam(machingDatas);
        string winAndLoseText = "";
        if (windAndLose == 1)
            winAndLoseText = (TextManager.Instance.language == SystemLanguage.Korean) ? "<color=blue>승리<color=white>" : "<color=blue>Win<color=white>";
        else if (windAndLose == -1)
            winAndLoseText = (TextManager.Instance.language == SystemLanguage.Korean) ? "<color=red>패배<color=white>" : "<color=red>Lose<color=white>";
        else if (windAndLose == 0)
            winAndLoseText = (TextManager.Instance.language == SystemLanguage.Korean) ? "무승부" : "Draw";

        string value = $"D-{currentTimeCount}\nTeam {machingTeam.teamName} {winAndLoseText}\n\n";

        battleReports.Add(value);
    }
    public string Write()
    {
        string value = "";


        string title = (TextManager.Instance.language == SystemLanguage.Korean) ? "<3부 리그>\n\n" : "<3rd League>\n\n";
        value += title;

        for(int i = 0; i < 11; ++i)
        {
            value += battleReports[i];
        }

        title = (TextManager.Instance.language == SystemLanguage.Korean) ? "<2부 리그>\n\n" : "<2nd League>\n\n";
        value += title;

        for (int i = 11; i < 18; ++i)
        {
            value += battleReports[i];
        }

        title = (TextManager.Instance.language == SystemLanguage.Korean) ? "<1부 리그>\n\n" : "<1st League>\n\n";
        value += title;

        for (int i = 18; i < 5; ++i)
        {
            value += battleReports[i];
        }

        title = (TextManager.Instance.language == SystemLanguage.Korean) ? "<챔피언>\n" : "<Champion>\n";
        value += title;
        value += battleReports[23];

        return value;
    }
    public TeamData FindPlayMatchTeam(List<Tuple<TeamData, TeamData>> schedule)
    {
        for(int i = 0; i < schedule.Count; ++i)
        {
            if (schedule[i].Item1 == SelectTeamUI.Instance.selectTeam)
                return schedule[i].Item2;
            if (schedule[i].Item2 == SelectTeamUI.Instance.selectTeam)
                return schedule[i].Item1;
        }

        return null;
    }
    public int GetCurrentLeagueNumber()
    {
        if (currentLeague == league_1)
        {
            if (CombatManager.Instance.battleDataObject == defendingChampionData.championData)
                return 0;
            else
                return 1;
        }
        else if (currentLeague == league_0)
            return -1;
        else if (currentLeague == league_2)
            return 2;
        else if (currentLeague == league_3)
            return 3;
        else
            return 0;
    }
    public List<TeamData> GetLeagueTeams(int number)
    {
        if (number == 1)
            return league1TeamDatas;
        else if (number == 2)
            return league2TeamDatas;
        else if (number == 3)
            return league3TeamDatas;
        else if (number == -1)
            return league0TeamDatas;
       
        return null;
    }

    public Dictionary<int, List<Tuple<TeamData, TeamData>>> GetLeague(int number)
    {
        if (number == 1)
            return league_1;
        else if (number == 2)
            return league_2;
        else if (number == 3)
            return league_3;
        else if (number == -1)
            return league_0;

        return null;
    }
    public Dictionary<int, List<Tuple<TeamData, TeamData>>> GetPlayerContainsLeague()
    {
        var player = SelectTeamUI.Instance.selectTeam;
        if (league3TeamDatas.Contains(player))
            return league_3;
        else if (league2TeamDatas.Contains(player))
            return league_2;
        else if (league1TeamDatas.Contains(player))
            return league_1;
        else if (league0TeamDatas.Contains(player))
            return league_0;

        return null;
    }
    public int CheckWindAndLose()
    {
        List<Tuple<TeamData, TeamData>> battleData = null;
        currentLeague.TryGetValue(timecount - currentTimeCount, out battleData);
        if (battleData == finalMatch)
        {
            CaculateScores();

            var playerTeam = SelectTeamUI.Instance.selectTeam;
            var currentIndex = currentLeagTeam.FindIndex(x => x == playerTeam);
           
            var teamCount = currentLeagTeam.Count;

            int winIndex = (GetCurrentLeagueNumber() == 1) ? 0 : 2;
            if (currentIndex <= winIndex)
                return 1;
            else if (currentIndex >= (teamCount - 3) && (currentIndex < teamCount))
                return -1;
            else
                return 0;
        }

        return -2;
    }
    public void PlayerWinAndLose(int winAndLose)
    {
        if(winAndLose == 1)
        {
            var playerTeam = SelectTeamUI.Instance.selectTeam;
            var enemyTeam = MainSystemUI.Instance.machingTeam;

            playerTeam.win += 1;
            enemyTeam.lose += 1;
        }
        else if(winAndLose == -1)
        {
            var playerTeam = SelectTeamUI.Instance.selectTeam;
            var enemyTeam = MainSystemUI.Instance.machingTeam;

            playerTeam.lose += 1;
            enemyTeam.win += 1;
        }
        else if (winAndLose == 0)
        {
            var playerTeam = SelectTeamUI.Instance.selectTeam;
            var enemyTeam = MainSystemUI.Instance.machingTeam;

            playerTeam.draw += 1;
            enemyTeam.draw += 1;
        }
    }
    private Dictionary<int, List<Tuple<TeamData, TeamData>>> ConvertBattleSchedule(List<List<Tuple<TeamData, TeamData>>> schedules)
    {
        Dictionary<int, List<Tuple<TeamData, TeamData>>> scheduleDic = new Dictionary<int, List<Tuple<TeamData, TeamData>>>();

        while(schedules.Count > 0)
        {
            int key = Random.Range(0, 30);
            while(scheduleDic.ContainsKey(key))
                key = Random.Range(0, 30);

            scheduleDic.Add(key, schedules[0]);
            schedules.RemoveAt(0);
        }
        return scheduleDic;
    }
    //무조건 짝수만
    private List<List<Tuple<TeamData, TeamData>>> GetBattleSchedules(List<TeamData> originDatas)
    {
        List<List<Tuple<TeamData, TeamData>>> schedules = new List<List<Tuple<TeamData, TeamData>>>();

        int count = originDatas.Count - 1;
        for (int i = 0; i < count; ++i)
        {
            var data = SplitDatas(originDatas);
            schedules.Add(data);
        }
        return schedules;
    }
    //무조건 짝수만
    private List<Tuple<TeamData, TeamData>> SplitDatas(List<TeamData> originDatas)
    {
        var datas = originDatas.ToList();
        var battleDatas = new List<Tuple<TeamData, TeamData>>();

        int aIndex = 0;
        int bIndex = 0;
        while (datas.Count > 0)
        {
            aIndex = Random.Range(0, datas.Count);
            bIndex = Random.Range(0, datas.Count);

            while (aIndex == bIndex)
                bIndex = Random.Range(0, datas.Count);

            TeamData a = datas[aIndex];
            TeamData b = datas[bIndex];

            Tuple<TeamData, TeamData> battleData = new Tuple<TeamData, TeamData>(a, b);
            battleDatas.Add(battleData);

            datas.Remove(a);
            datas.Remove(b);
        }

        return battleDatas;

    }

    private void SetRandomAIScore(Tuple<TeamData, TeamData> machingData)
    {
        int random = Random.Range(0, 3);

        if(random == 0)
        {
            machingData.Item1.win += 1;
            machingData.Item2.lose += 1;
        }
        else if(random == 1)
        {
            machingData.Item1.draw += 1;
            machingData.Item2.draw += 1;
        }
        else
        {
            machingData.Item1.lose += 1;
            machingData.Item2.win += 1;
        }
    }

    private void SwapDuplicatedTeams()
    {
        var counts = GetDuplicatedCount();
        var zeroMatchingTeams = GetZeroMatchingTeams(counts);

        if (zeroMatchingTeams == null)
            return;
        else
        {
            foreach (var pair in currentLeague)
            {
                var matchingTeams = pair.Value;

                for (int i = 0; i < matchingTeams.Count; ++i)
                {
                    if (matchingTeams[i].Item1 == SelectTeamUI.Instance.selectTeam)
                    {
                        var awayTeam = matchingTeams[i].Item2;
                        if (counts.ContainsKey(awayTeam))
                        {
                            var duplicatedCount = counts[awayTeam];
                            if (duplicatedCount >= 2)
                            {
                                var zeroMatchingTeam = zeroMatchingTeams.First();

                                Tuple<TeamData, TeamData> currentMatchingTeam = matchingTeams[i];
                                Tuple<TeamData, TeamData> containsZeroMatching = matchingTeams.Find(x => x.Item1 == zeroMatchingTeam || x.Item2 == zeroMatchingTeam);
                                int containsZeroMatchingIndex = matchingTeams.FindIndex(x => x == containsZeroMatching);
                                TeamData zeroMatchingAwayTeam = (containsZeroMatching.Item1 == zeroMatchingTeam) ? containsZeroMatching.Item2 : containsZeroMatching.Item1;



                                Tuple <TeamData, TeamData> newCurrentMatching = new Tuple<TeamData, TeamData>(matchingTeams[i].Item1, zeroMatchingTeam);
                                Tuple<TeamData, TeamData> newContainsZeroMatching = new Tuple<TeamData, TeamData>(zeroMatchingAwayTeam, awayTeam);

                                matchingTeams[i] = newCurrentMatching;
                                matchingTeams[containsZeroMatchingIndex] = newContainsZeroMatching;

                                duplicatedCount--;
                                counts[awayTeam] = duplicatedCount;

                                zeroMatchingTeams.Remove(zeroMatchingTeam);

                                if (zeroMatchingTeams.Count <= 0)
                                    return;
                            }
                        }


                    }

                    if (matchingTeams[i].Item2 == SelectTeamUI.Instance.selectTeam)
                    {
                        var awayTeam = matchingTeams[i].Item1;
                        if (counts.ContainsKey(awayTeam))
                        {
                            var duplicatedCount = counts[awayTeam];
                            if (duplicatedCount >= 2)
                            {
                                var zeroMatchingTeam = zeroMatchingTeams.First();

                                Tuple<TeamData, TeamData> currentMatchingTeam = matchingTeams[i];
                                Tuple<TeamData, TeamData> containsZeroMatching = matchingTeams.Find(x => x.Item1 == zeroMatchingTeam || x.Item2 == zeroMatchingTeam);
                                int containsZeroMatchingIndex = matchingTeams.FindIndex(x => x == containsZeroMatching);
                                TeamData zeroMatchingAwayTeam = (containsZeroMatching.Item1 == zeroMatchingTeam) ? containsZeroMatching.Item2 : containsZeroMatching.Item1;


                                Tuple<TeamData, TeamData> newCurrentMatching = new Tuple<TeamData, TeamData>(matchingTeams[i].Item2, zeroMatchingTeam);
                                Tuple<TeamData, TeamData> newContainsZeroMatching = new Tuple<TeamData, TeamData>(zeroMatchingAwayTeam, awayTeam);

                                matchingTeams[i] = newCurrentMatching;
                                matchingTeams[containsZeroMatchingIndex] = newContainsZeroMatching;

                                duplicatedCount--;
                                counts[awayTeam] = duplicatedCount;

                                zeroMatchingTeams.Remove(zeroMatchingTeam);

                                if (zeroMatchingTeams.Count <= 0)
                                    return;
                            }
                        }


                    }
                }
            }
        }
    }
    private Dictionary<TeamData, int> GetDuplicatedCount()
    {
        Dictionary<TeamData, int> counts = new Dictionary<TeamData, int>();
        foreach(var pair in currentLeague)
        {
            var matchingTeams = pair.Value;

            for (int i = 0; i < matchingTeams.Count; ++i)
            {
                if (matchingTeams[i].Item1 == SelectTeamUI.Instance.selectTeam)
                {
                    int count;
                    bool check = counts.TryGetValue(matchingTeams[i].Item2, out count);
                    if(check == true)
                    {
                        count++;
                        counts[matchingTeams[i].Item2] = count;
                    }
                    else
                    {
                        count = 1;
                        counts.Add(matchingTeams[i].Item2, count);
                    }
                }
                else if(matchingTeams[i].Item2 == SelectTeamUI.Instance.selectTeam)
                {
                    int count;
                    bool check = counts.TryGetValue(matchingTeams[i].Item1, out count);
                    if (check == true)
                    {
                        count++;
                        counts[matchingTeams[i].Item1] = count;
                    }
                    else
                    {
                        count = 1;
                        counts.Add(matchingTeams[i].Item1, count);
                    }
                }
            }
        }

        return counts;
    }
    private HashSet<TeamData> GetZeroMatchingTeams(Dictionary<TeamData, int> duplicatedCounts)
    {
        HashSet<TeamData> teams = null;

        HashSet<TeamData> duplicatedTeams = duplicatedCounts.Keys.ToHashSet();

        var myTeam = SelectTeamUI.Instance.selectTeam;
        if(duplicatedTeams.Count > 0)
        {
            teams = currentLeagTeam.FindAll(x => (x != myTeam) && (duplicatedTeams.Contains(x) == false)).ToHashSet();
        }
        return teams;
    }
}
