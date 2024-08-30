using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public Image loadingImage;
    void Start()
    {
        LoadingSceneManager.Instance.Loading("New Scene");

    }

    // Update is called once per frame
    void Update()
    {
        loadingImage.fillAmount = LoadingSceneManager.Instance.value;
    }
}
