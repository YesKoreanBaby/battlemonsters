using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public Image[] images;

    [System.NonSerialized]
    public bool isPopUp = false;

    private static ScoreUI instance = null;
    public static ScoreUI Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        Init();
    }
    public void PopUp()
    {
        if (isPopUp == false)
        {
            SoundManager.Instance.PlayEffect(164, 1f);

            isPopUp = true;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(true);
            }

            SettingDatas();

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false)
            //    AdmobManager.Instance.DestroyBannerAds();
        }
    }
    public void Closed()
    {
        if (isPopUp == true)
        {
            isPopUp = false;
            var childCount = this.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                var gameObject = this.transform.GetChild(i).gameObject;
                gameObject.SetActive(false);
            }

            //if (AdmobManager.Instance != null && AdmobManager.Instance.adData.adsBlock == false && FirebaseDataManager.Instance.loadServerData != null && FirebaseDataManager.Instance.loadServerData.access == false)
            //    AdmobManager.Instance.LoadBannerAds(GoogleMobileAds.Api.AdSize.Banner, GoogleMobileAds.Api.AdPosition.Bottom);
        }
    }

    public void Cancel()
    {
        Closed();
    }

    private void Init()
    {
        isPopUp = false;
        var childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(false);
        }
    }
    private void SettingDatas()
    {
        LeagueManager.Instance.CaculateScores();
        for(int i = 0; i < images.Length; ++i)
        {
            DontActive(images[i]);
        }

        var teams = LeagueManager.Instance.currentLeagTeam;
        for(int i = 0; i < teams.Count; ++i)
        {
            Active(images[i], teams[i]);
            ColorUpdate(images[i], i, teams.Count);
        }
    }

    private void Active(Image background, TeamData teamData)
    {
        var teamMark = background.transform.GetChild(0).GetComponent<Image>();
        teamMark.gameObject.SetActive(true);
        teamMark.sprite = teamData.teamIcon;

        var scoreText = background.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"<color=black><b><size=32>{teamData.win}<color=blue><b><size=28>win <color=black><b><size=32>{teamData.draw}<color=#808080><b><size=28>draw <color=black><b><size=32>{teamData.lose}<color=red><b><size=28>lose <color=black><b><size=32>{teamData.score}<color=#A32CC4><b><size=28>pt";

        var numberText = background.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        numberText.gameObject.SetActive(true);

        var checkMyTeam = background.transform.GetChild(3).gameObject;
        checkMyTeam.gameObject.SetActive(SelectTeamUI.Instance.selectTeam == teamData);

        background.color = Color.white;
    }

    private void DontActive(Image background)
    {
        int childCount = background.transform.childCount;
        background.color = new Color();
        for (int i = 0; i < childCount; ++i)
        {
            background.transform.GetChild(i).gameObject.SetActive(false);
        }

        background.color = new Color(0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f, 0.5882352941176471f);
    }

    private void ColorUpdate(Image background, int currentIndex, int teamCount)
    {
        int winIndex = (LeagueManager.Instance.GetCurrentLeagueNumber() == 1) ? 0 : 2;
        if (currentIndex <= winIndex)
        {
            background.color = new Color(0.392156862745098f, 1f, 1f, 1f);
        }
        if (currentIndex >= (teamCount - 3) && (currentIndex < teamCount))
        {
            background.color = new Color(0.392156862745098f, 0.392156862745098f, 1f, 1f);
        }
    }
}
