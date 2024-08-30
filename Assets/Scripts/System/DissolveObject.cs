using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveObject : MonoBehaviour
{
    
    private void Start()
    {
        StartCoroutine(StartDissolve(GetComponent<SpriteRenderer>()));
    }
    public IEnumerator StartDissolve(SpriteRenderer rendererObject)
    {
        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = rendererObject.sprite;
        transform.localScale = rendererObject.transform.localScale;

        string valueTex = "_DissolvePower";
    //    string valueTex2 = "_EmissionThickness";
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 3f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            float value = Mathf.Lerp(0.55f, 0f, currentSpeed);
       //     float value2 = Mathf.Lerp(0.55f, 0f, currentSpeed);
            renderer.material.SetFloat(valueTex, value);
      //      renderer.material.SetFloat(valueTex2, value2);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
