using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TypingText : MonoBehaviour
{
    public bool isRunning { get; private set; }
    public void Play(string text, float time, Action evt = null)
    {
        if (isRunning == false)
            StartCoroutine(TypingRoutine(text, time, evt));
    }

    public void PlayLerp(string text, float time, Action evt = null)
    {
        if (isRunning == false)
            StartCoroutine(TypingLerpRoutine(text, time, evt));
    }
    public void ResetTyping()
    {
        StopAllCoroutines();
        isRunning = false;
    }
    private IEnumerator TypingRoutine(string text, float time, Action evt)
    {
        isRunning = true;

        var textUI = GetComponent<TextMeshProUGUI>();
        WaitForSeconds waitTime = new WaitForSeconds(time);

        for (int i = 0; i <= text.Length; i++)
        {
            textUI.text = text.Substring(0, i);
            SoundManager.Instance.PlayEffect(Random.Range(168, 171), 0.5f);
            yield return waitTime;

            time = Mathf.Max(0.0015f, time - 0.0005f);
            waitTime = new WaitForSeconds(time);
        }

        SoundManager.Instance.StopAllEffect(168);
        SoundManager.Instance.StopAllEffect(169);
        SoundManager.Instance.StopAllEffect(170);
        isRunning = false;
        if (evt != null)
            evt.Invoke();
    }

    private IEnumerator TypingLerpRoutine(string text, float duration, Action evt)
    {
        isRunning = true;

        float time = duration / text.Length;
        var textUI = GetComponent<TextMeshProUGUI>();

        WaitForSeconds waitTime = new WaitForSeconds(time);
        for (int i = 0; i <= text.Length; i++)
        {
            textUI.text = text.Substring(0, i);
            SoundManager.Instance.PlayEffect(Random.Range(168, 171), 0.5f);

            if(i % 2 == 0)
            {

            }
            else
                yield return waitTime;

        }

        SoundManager.Instance.StopAllEffect(168);
        SoundManager.Instance.StopAllEffect(169);
        SoundManager.Instance.StopAllEffect(170);
        isRunning = false;
        if (evt != null)
            evt.Invoke();
    }

}
