using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum FlowingTextType { Flowing, Rotation, Count}
public class FlowingText : MonoBehaviour
{
    public FlowingTextType textType;
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if(textType == FlowingTextType.Flowing)
            StartCoroutine(Flowing());
    }

    public void SetText(string text)
    {
        TextMeshProUGUI textUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textUGUI.text = text;

        textUGUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        textUGUI.text = text;
    }

    private IEnumerator Flowing()
    {
        float lerpTime = 1f;
        float lerpSpeed = 0.75f;
        float currentTime = 0f;
        float moveSpeed = 1f;

        int childCount = rectTransform.childCount;

        while (currentTime  < lerpTime)
        {
            for(int i = 0; i < childCount; i++)
            {
                currentTime += Time.deltaTime * lerpSpeed;
                float currentSpeed = currentTime / lerpTime;

                var text = rectTransform.GetChild(i).GetComponent<TextMeshProUGUI>();
                float a = Mathf.Lerp(1f, 0f, currentSpeed);
                text.color = new Color(text.color.r, text.color.g, text.color.b, a);
            }

            rectTransform.position += Vector3.up * moveSpeed * Time.deltaTime;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
