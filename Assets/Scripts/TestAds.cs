using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAds : MonoBehaviour
{
    public Scrollbar scrollBar;
    public Scrollbar skillScrollBar;

    private void Start()
    {
        StartCoroutine(MoveRoutine());
    }
    private IEnumerator MoveRoutine()
    {
        yield return new WaitForSeconds(2f);
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 32f;
        
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            scrollBar.value = Mathf.Lerp(0f, 1f, currentSpeed);
            yield return null;
        }
    }
    private IEnumerator SkillMoveRoutine()
    {
        yield return new WaitForSeconds(2f);
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 16f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            skillScrollBar.value = Mathf.Lerp(1f, 0f, currentSpeed);
            yield return null;
        }
    }
}
