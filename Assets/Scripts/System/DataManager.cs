using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class DataManager : MonoBehaviour
{
    private static DataManager instance = null;

    public static DataManager Instance { get { return instance; } }

    private string dataPath;
    private void Awake()
    {
        instance = this;
        dataPath = Application.persistentDataPath; // 윈도우 C:\Users\[user name]\AppData\LocalLow\[company name]\[product //AppData를 들어갈려면 숨긴항목을 체크해야한다
                                                   // 안드로이드: 내파일 / Android / Data / [company name]\[product / files/
    }
    public string GetAbsolutePath()
    {
        return dataPath;
    }
    public void Save<T>(string fileName, T saveData)
    {
        Save(dataPath, fileName, saveData);
    }

    public void Save<T>(string path, string fileName, T saveData)
    {
        string jsonText = JsonConvert.SerializeObject(saveData, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        //Json암호화
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonText);
        string code = System.Convert.ToBase64String(bytes);
        File.WriteAllText($"{path}/{fileName}", code);
    }
    public void SaveNoCode<T>(string path, string fileName, T saveData)
    {
        string jsonText = JsonConvert.SerializeObject(saveData, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

        //Json암호화
        File.WriteAllText($"{path}/{fileName}", jsonText);
    }
    public bool Load<T>(string fileName, out T data)
    {
        return Load(dataPath, fileName, out data);
    }
    public bool Load<T>(string path, string fileName, out T data)
    {
        try
        {
            if (File.Exists($"{path}/{fileName}"))
            {
                string code = File.ReadAllText($"{path}/{fileName}");

                //Json암호화
                byte[] bytes = System.Convert.FromBase64String(code);
                string jData = System.Text.Encoding.UTF8.GetString(bytes);

                data = JsonConvert.DeserializeObject<T>(jData, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return true;
            }
            else
            {
                data = default;
                return false;
            }
        }
        catch
        {
            data = default;
            return false;
        }
    }

    public void Remove(string fileName)
    {
        Remove(dataPath, fileName);
    }
    public void Remove(string path, string fileName)
    {
        if (File.Exists($"{path}/{fileName}"))
            File.Delete($"{path}/{fileName}");
    }

}
