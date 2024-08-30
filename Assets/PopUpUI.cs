using UnityEngine;

public abstract class PopUpUI<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool isPopUp { get; protected set; }


    //½Ì±ÛÅæ º¯¼ö
    private static T instance = null;

    public static T Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            T obj = FindObjectOfType<T>();
            instance = obj;
            Closed();
        }
    }
    public abstract void SettingData();

    public virtual void PopUp()
    {
        isPopUp = true;
        this.gameObject.SetActive(true);
        SettingData();
    }


    public virtual void Closed()
    {
        isPopUp = false;
        this.gameObject.SetActive(false);
    }
}
