using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    private static LoadingSceneManager instance = null;
    public static LoadingSceneManager Instance { get { return instance; } }

    public float value { get; private set; }

    public Action successEvent { get; private set; }
    private void Awake()
    {
        instance = this; 
    }
    
    public void Loading(string nextSceneName)
    {
        StartCoroutine(LoadingRoutine(nextSceneName));
    }

    public void OnSuccess(Action _sucessEvent)
    {
        successEvent += _sucessEvent;
    }
    private IEnumerator LoadingRoutine(string nextSceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        value = 0f;
        while (!op.isDone)
        {
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                value = Mathf.Lerp(value, op.progress, timer);
                if (value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                value = Mathf.Lerp(value, 1f, timer);
                if (value == 1.0f)
                {
                    op.allowSceneActivation = true;

                    if(successEvent != null)
                        successEvent.Invoke();
                    yield break;
                }
            }

            yield return null;
        }
    }


}
