using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlarmUI : MonoBehaviour
{
    public TextMeshProUGUI alamText;
    public Button button_0;
    public Button button_1;
    public Button cancel;
    public Animator alarmAnim;

    public bool isPopUp { get; private set; }

    public static AlarmUI Instance { get { return instance; } }
    private static AlarmUI instance = null;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Closed();
    }
    public void PopUp(string text, UnityAction evt_0, UnityAction evt_1 = null, string enterText = "")
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEffect(165, 1f);

        var childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(true);
        }

        alarmAnim.gameObject.SetActive(false);
        alamText.gameObject.SetActive(true);

        alamText.text = text;

        if (evt_0 == null)
        {
            button_0.gameObject.SetActive(false);
            button_1.gameObject.SetActive(false);
            cancel.gameObject.SetActive(true);

            cancel.onClick.RemoveAllListeners();
            if (evt_1 == null)
                cancel.onClick.AddListener(() => Closed());
            else
                cancel.onClick.AddListener(evt_1);
        }
        else
        {
            button_0.gameObject.SetActive(true);
            button_1.gameObject.SetActive(true);
            cancel.gameObject.SetActive(false);

            button_0.onClick.RemoveAllListeners();
            button_1.onClick.RemoveAllListeners();
            button_0.onClick.AddListener(evt_0);

            if (evt_1 == null)
                button_1.onClick.AddListener(() => Closed());
            else
                button_1.onClick.AddListener(evt_1);

            button_0.onClick.AddListener(ClickSound);
            button_1.onClick.AddListener(ClickSound);
        }


        if (enterText.Equals(""))
            button_0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Enter";
        else
            button_0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = enterText;
        isPopUp = true;
    }

    public void PopUpForKey(string key, UnityAction evt_0, UnityAction evt_1 = null, string enterText = "")
    {
        string text = TextManager.Instance.GetString(key);
        PopUp(text, evt_0, evt_1, enterText);
    }

    public void PopUpForMathcing(UnityAction evt_0, UnityAction evt_1, Sprite homegroundIcon, string homegroundText, Sprite awayIcon, string awayText)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayEffect(165, 1f);

        var childCount = this.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(true);
        }

        if (evt_0 == null)
        {
            button_0.gameObject.SetActive(false);
            button_1.gameObject.SetActive(false);
            cancel.gameObject.SetActive(true);

            cancel.onClick.RemoveAllListeners();
            if (evt_1 == null)
                cancel.onClick.AddListener(() => Closed());
            else
                cancel.onClick.AddListener(evt_1);
        }
        else
        {
            button_0.gameObject.SetActive(true);
            button_1.gameObject.SetActive(true);
            cancel.gameObject.SetActive(false);

            button_0.onClick.RemoveAllListeners();
            button_1.onClick.RemoveAllListeners();
            button_0.onClick.AddListener(evt_0);

            if (evt_1 == null)
                button_1.onClick.AddListener(() => Closed());
            else
                button_1.onClick.AddListener(evt_1);

            button_0.onClick.AddListener(ClickSound);
            button_1.onClick.AddListener(ClickSound);
        }

        button_0.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Enter";

        var hImage = alarmAnim.transform.GetChild(0).GetComponent<Image>();
        var hText = hImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        var aImage = alarmAnim.transform.GetChild(1).GetComponent<Image>();
        var aText = aImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        hImage.sprite = homegroundIcon;
        aImage.sprite = awayIcon;
        hText.text = homegroundText;
        aText.text = awayText; 
        StartCoroutine(PopUpForMatchingRoutine());

        isPopUp = true;
    }
    public void Closed()
    {
        var childCount = this.transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            var gameObject = this.transform.GetChild(i).gameObject;
            gameObject.SetActive(false);
        }

        isPopUp = false;
    }

    public void ClickSound()
    {
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlayEffect(166, 1f);
    }

    private IEnumerator PopUpForMatchingRoutine()
    {
        alarmAnim.gameObject.SetActive(true);
        alamText.gameObject.SetActive(false);
        button_0.gameObject.SetActive(false);
        button_1.gameObject.SetActive(false);
        cancel.gameObject.SetActive(false);
        yield return null;

        alarmAnim.Play("popup");
        yield return new WaitUntil(() => alarmAnim.GetCurrentAnimatorStateInfo(0).IsName("idle"));

        button_0.gameObject.SetActive(true);
        button_1.gameObject.SetActive(true);
        cancel.gameObject.SetActive(true);
    }
}
