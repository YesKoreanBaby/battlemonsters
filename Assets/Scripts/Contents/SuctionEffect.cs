using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SuctionEffect : MonoBehaviour
{
    public void Play(Vector2 endPosition, bool isDead = true)
    {
        StartCoroutine(PlayRoutine(endPosition, isDead));
    }
    private IEnumerator PlayRoutine(Vector2 endPosition, bool isDead)
    {
        //ø¨√‚
        Vector2 startPosition = this.transform.position;
        float currentScale = this.transform.localScale.x;
    
        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);

            float scale = Mathf.Lerp(currentScale, 0f, currentSpeed);
            this.transform.localScale = new Vector3(scale, scale, this.transform.localScale.z);
            yield return null;
        }

        if(isDead == true)
            Destroy(this.gameObject);
    }
}
