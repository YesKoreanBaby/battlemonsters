using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    public Scrollbar bgmBar;
    public Scrollbar effectBar;
    public Scrollbar battleSpeedBar;
    public RectTransform setUpView;
    public RectTransform basicView;
    public RectTransform helpView;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI battleSpeedText;
    public Button exitButton;
    public Button prevButton;
    public Button resetButton;
    public TextMeshProUGUI changeLanguageText;

    public RectTransform[] helpViewTexts;
    public Button[] helpViewButtons;
    public TextMeshProUGUI helpViewTitle;
    public TextMeshProUGUI[] helpViewLanguages;
    public TextMeshProUGUI[] startViewTexts;

    private static SystemUI instance = null;
    public static SystemUI Instance { get { return instance; } }

    [System.NonSerialized]
    public bool isPopUp = false;

    [System.NonSerialized]
    public float addBattleSpeed = 1f;


    [System.NonSerialized]
    public float startBgmValue;
    [System.NonSerialized]
    public float startEffectValue;
    [System.NonSerialized]
    public float startBattleSpeedValue;
    private bool palseSound = false;
    private bool blockSound = false;
    private const string methodName = "PalseSound";
    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
        SettingHelpViewTexts();
    }
    private void Update()
    {
        if(isPopUp == true)
        {
            if(Input.GetMouseButtonUp(0))
            {
                palseSound = false;
            }
            if(Input.GetMouseButton(0))
            {
                palseSound = true;

                if(blockSound == false)
                {
                    blockSound = true;
                    Invoke(methodName, 0.25f);
                }
            }
        }
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

            ChangeBgmVolume();
            ChangeEffectVolume();
            ChangeBattleSpeed();
            ChangeViews(0);
            SetChangeLangulage();
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
        }
    }

    public void ResetSetting()
    {
        SoundManager.Instance.PlayEffect(166, 1f);

        bgmBar.value = startBgmValue;
        effectBar.value = startEffectValue;

        battleSpeedBar.value = startBattleSpeedValue;

        ChangeBattleSpeed();
        ChangeBgmVolume();
        ChangeEffectVolume();

        TextManager.Instance.language = (Application.systemLanguage == SystemLanguage.Korean) ? SystemLanguage.Korean : SystemLanguage.English;
        SetChangeLangulage();
    }

    public void ChangeLanguage()
    {
        TextManager.Instance.language = (TextManager.Instance.language == SystemLanguage.Korean) ? SystemLanguage.English : SystemLanguage.Korean;
        SetChangeLangulage();

        MainSystemUI.Instance.SetTextFormats();
      
        SetTextFormats();
        SettingHelpViewTexts();
    }

    public void PopUpAnnouncement()
    {
        Closed();
      
    }

    public void ChangeBgmVolume()
    {
        if (palseSound == true && blockSound == false)
        {
            SoundManager.Instance.PlayEffect(166, 1f);
        }

        var audioMixer = SoundManager.Instance.audioMixer;

        float maxVolum = 20;

        float currentBgmVolume = bgmBar.value * maxVolum;

        audioMixer.SetFloat("bgm", currentBgmVolume - maxVolum);

        bgmText.text = (Mathf.Floor((2 * bgmBar.value) * 10f) / 10f).ToString();

        if(bgmBar.value <= 0f)
        {
            audioMixer.SetFloat("bgm", -80f);
        }
    }
    public void ChangeEffectVolume()
    {
        if(palseSound == true && blockSound == false)
        {
            SoundManager.Instance.PlayEffect(166, 1f);
        }
        var audioMixer = SoundManager.Instance.audioMixer;

        float maxVolum = 20;

        float currentEffectVolume = effectBar.value * maxVolum;

        audioMixer.SetFloat("effect", currentEffectVolume - maxVolum);

        effectText.text = (Mathf.Floor((2 * effectBar.value) * 10f) / 10f).ToString();

        if (effectBar.value <= 0f)
        {
            audioMixer.SetFloat("effect", -80f);
        }
    }
    public void ChangeBattleSpeed()
    {
        if (palseSound == true && blockSound == false)
        {
            SoundManager.Instance.PlayEffect(166, 1f);
        }

        addBattleSpeed = Mathf.Max(0.1f, battleSpeedBar.value * 2);
        battleSpeedText.text = (Mathf.Floor(addBattleSpeed * 10f) / 10f).ToString();
    }
    public void Prev()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(0);
    }
    public void ChangeSetUpView()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(1);
    }
    public void End()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void PopUpHelpView_0()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(2);
    }
    public void PopUpHelpView_1()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(3);
    }
    public void PopUpHelpView_2()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(4);
    }
    public void PopUpHelpView_3()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(5);
    }
    public void PopUpHelpView_4()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(6);
    }
    public void PopUpHelpView_5()
    {
        SoundManager.Instance.PlayEffect(166, 1f);
        ChangeViews(7);
    }

    public void ClearData()
    {
        string text = (TextManager.Instance.language == SystemLanguage.Korean) ? "구매한 상품을 제외한 \r\n\r\n모든 데이터를 삭제하고 \r\n\r\n게임을 재시작합니다" : "Erase all data except \r\n\r\npurchased items and \r\n\r\nrestart the game.";
        AlarmUI.Instance.PopUp(text, () =>
        {
            MainSystemUI.Instance.StopBgm();
            AlarmUI.Instance.Closed();
            Parser.Instance.RemoveData();

            //if(AdmobManager.Instance != null && AdmobManager.Instance.adData != null && AdmobManager.Instance.adData.adsBlock == false)
            //    AdmobManager.Instance.DestroyBannerAds();

            

         

            SelectTeamUI.Instance.UpdateTeamLockeds();

            SceneManager.LoadScene("LoadingScene");
        });
    }

    public void InitValues()
    {
        var audioMixer = SoundManager.Instance.audioMixer;
        float currentEffectVolum, currentBgmVolum;

        float maxVolum = 20;

        audioMixer.GetFloat("effect", out currentEffectVolum);
        audioMixer.GetFloat("bgm", out currentBgmVolum);

        currentBgmVolum = maxVolum - currentBgmVolum;
        currentEffectVolum = maxVolum - currentEffectVolum;

        bgmBar.value = maxVolum / currentBgmVolum;
        effectBar.value = maxVolum / currentEffectVolum;

        battleSpeedBar.value = addBattleSpeed / 2;

        ChangeBgmVolume();
        ChangeEffectVolume();
        ChangeBattleSpeed();

        startBgmValue = bgmBar.value;
        startEffectValue = effectBar.value;
        startBattleSpeedValue = battleSpeedBar.value;
    }

    public void SettingHelpViewTexts()
    {
        helpViewLanguages[0].text = TextManager.Instance.GetString("H0");
        helpViewLanguages[1].text = TextManager.Instance.GetString("H1");
        helpViewLanguages[2].text = TextManager.Instance.GetString("H2");
        helpViewLanguages[3].text = TextManager.Instance.GetString("H3");
        helpViewLanguages[4].text = TextManager.Instance.GetString("H4");
        helpViewLanguages[5].text = TextManager.Instance.GetString("H5");


        if (TextManager.Instance.language == SystemLanguage.Korean)
        {
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-673f, 486f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-673f, 333f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-666.75f, 127f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-663.63f, -39f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(-663.63f, -193f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(-663.63f, -354f);

            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-707f, 997f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-665f, 844f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-678.17f, 733f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-665f, 573f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(-665.31f, 457.38f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(-673f, 252f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(-665f, 99f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(-660.5001f, -62.5f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<RectTransform>().anchoredPosition = new Vector2(-660.5f, -185f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(9).GetComponent<RectTransform>().anchoredPosition = new Vector2(-678.17f, -297f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(10).GetComponent<RectTransform>().anchoredPosition = new Vector2(-690.17f, -477f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(11).GetComponent<RectTransform>().anchoredPosition = new Vector2(-658f, -578f);
        }
        else
        {
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-617f, 492.4f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-569f, 260.7f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-574f, 36f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-617f, -116f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(-649f, -312.71f);
            helpViewTexts[4].transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(-659.63f, -494f);

            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-620f, 987.8f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-465.78f, 804.4f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(-556f, 674f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(-665f, 479f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(-533f, 379f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(-637.63f, 113.33f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(-663f, -51f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(-608.28f, -147f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<RectTransform>().anchoredPosition = new Vector2(-572.73f, -281f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(9).GetComponent<RectTransform>().anchoredPosition = new Vector2(-681f, -432f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(10).GetComponent<RectTransform>().anchoredPosition = new Vector2(-663f, -595f);
            helpViewTexts[5].transform.GetChild(0).GetChild(0).GetChild(11).GetComponent<RectTransform>().anchoredPosition = new Vector2(-452.73f, -735f);
        }
    }

    public void SetTextFormats()
    {
        for (int i = 0; i < startViewTexts.Length; ++i)
        {
            TextMeshProUGUI text = startViewTexts[i];

            if (TextManager.Instance.language == SystemLanguage.English)
            {
                text.font = MonsterDataBase.Instance.ehFont;
                text.rectTransform.anchoredPosition = new Vector2(4.8f, 0f);
                text.fontStyle = FontStyles.Normal;
                text.fontSize = 42;

                if (i == 0)
                {
                    text.text = "Announcement";
                }
                else if (i == 1)
                {
                    text.text = "Setting";
                }
                else if (i == 2)
                {
                    text.text = "Help";
                }
                else if (i == 3)
                {
                    text.text = "Clear Data";
                }
                else if (i == 4)
                {
                    text.text = "End";
                }
            }
            else
            {
                text.font = MonsterDataBase.Instance.krFont;
                text.rectTransform.anchoredPosition = new Vector2(-0.00059795f, 6f);
                text.fontStyle = FontStyles.Bold;
                text.fontSize = 52;

                if (i == 0)
                {
                    text.text = "공 지 사 항";
                }
                else if (i == 1)
                {
                    text.text = "환 경 설 정";
                }
                else if (i == 2)
                {
                    text.text = "도움말";
                }
                else if (i == 3)
                {
                    text.text = "데이터 초기화";
                }
                else if (i == 4)
                {
                    text.text = "종료";
                }
            }
        }

    }
    private void ChangeViews(int index)
    {
        setUpView.gameObject.SetActive(false);
        basicView.gameObject.SetActive(false);
        helpView.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        changeLanguageText.transform.parent.gameObject.SetActive(false);

        for(int i = 0; i < helpViewTexts.Length; ++i)
        {
            helpViewTexts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < helpViewTexts.Length; ++i)
        {
            helpViewButtons[i].interactable = true;
        }
        if (index == 0)
        {
            exitButton.gameObject.SetActive(true);
            basicView.gameObject.SetActive(true);
        }
        else if(index == 1)
        {
            prevButton.gameObject.SetActive(true);
            setUpView.gameObject.SetActive(true);
            resetButton.gameObject.SetActive(true);
            changeLanguageText.transform.parent.gameObject.SetActive(true);
        }
        else if(index == 2)
        {
            prevButton.gameObject.SetActive(true);
            helpView.gameObject.SetActive(true);
            helpViewTexts[0].gameObject.SetActive(true);
            helpViewButtons[0].interactable = false;

            helpViewTitle.text = "AI TYPE";
        }
        else if (index == 3)
        {
            prevButton.gameObject.SetActive(true);
            helpView.gameObject.SetActive(true);
            helpViewTexts[1].gameObject.SetActive(true);
            helpViewButtons[1].interactable = false;

            helpViewTitle.text = "S T A T";

            var scrollView = helpViewTexts[1].GetComponent<ScrollRect>();
            scrollView.verticalScrollbar.value = 1f;
        }
        else if (index == 4)
        {
            prevButton.gameObject.SetActive(true);
            helpView.gameObject.SetActive(true);
            helpViewTexts[2].gameObject.SetActive(true);
            helpViewButtons[2].interactable = false;

            helpViewTitle.text = "S K I L L";
        }
        else if (index == 5)
        {
            prevButton.gameObject.SetActive(true);
            helpView.gameObject.SetActive(true);
            helpViewTexts[3].gameObject.SetActive(true);
            helpViewButtons[3].interactable = false;

            helpViewTitle.text = "R U L E";
        }
        else if (index == 6)
        {
            prevButton.gameObject.SetActive(true);
            helpView.gameObject.SetActive(true);
            helpViewTexts[4].gameObject.SetActive(true);
            helpViewButtons[4].interactable = false;

            helpViewTitle.text = "C O N D I T I O N";

            var scrollView = helpViewTexts[4].GetComponent<ScrollRect>();
            scrollView.verticalScrollbar.value = 1f;
        }
        else if (index == 7)
        {
            prevButton.gameObject.SetActive(true);
            helpView.gameObject.SetActive(true);
            helpViewTexts[5].gameObject.SetActive(true);
            helpViewButtons[5].interactable = false;

            helpViewTitle.text = "S T A T U S";

            var scrollView = helpViewTexts[5].GetComponent<ScrollRect>();
            scrollView.verticalScrollbar.value = 1f;
        }
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
    private void PalseSound()
    {
        blockSound = false;
    }

    private void SetChangeLangulage()
    {
        changeLanguageText.text = (TextManager.Instance.language == SystemLanguage.Korean) ? "K O R E A" : "E N G L I S H";
    }
}
