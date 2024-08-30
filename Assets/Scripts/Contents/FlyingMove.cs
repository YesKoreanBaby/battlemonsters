using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMove : MonoBehaviour
{
    public void Move(bool isHomeground)
    {
        StartCoroutine(MoveRoutine(isHomeground));
    }
    private IEnumerator MoveRoutine(bool isHomeground)
    {
        Vector3 startPosition = transform.position;
        Vector3 endOffset = isHomeground ? new Vector3(-12, 7) : new Vector3(12, 7);
        Vector3 endPosition = startPosition + endOffset;
        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
