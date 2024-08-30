using UnityEngine;

[System.Serializable]
public class TextInfo
{
    [Multiline(20)]
    public string korean;

    [Space()]
    [Space()]
    [Multiline(20)]
    public string english;
}
public class TextManager : MonoBehaviour
{
    public SerializableDictionary<string, TextInfo> textInfos;

    [System.NonSerialized]
    public SystemLanguage language;

    private static TextManager instance = null;
    public static TextManager Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }
    public string GetString(string key)
    {
        TextInfo value;
        textInfos.TryGetValue(key, out value);

        if (value != null)
        {
            if(language == SystemLanguage.Korean)
            {
                return value.korean;
            }
            else
            {
                return value.english;
            }
        }
        else
            throw new System.NotImplementedException();
    }
}
