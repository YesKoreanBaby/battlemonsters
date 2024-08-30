using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public float speed;
    public Transform destination;
    public Transform localObj;
    public Transform localDirObj;


    public bool isFalling { get; private set; }
    private Vector2 startPosition;
    public void Init()
    {
        startPosition = transform.position;
    }
    public void Play()
    {
        if (isFalling == false)
        {
            this.gameObject.SetActive(true);
            StartCoroutine(FallingRoutine());
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        isFalling = false;
        this.gameObject.SetActive(false);
    }
    private IEnumerator FallingRoutine()
    {
        isFalling = true;
        transform.position = startPosition;
        localObj.transform.localPosition = Vector2.zero;

        var routine = StartCoroutine(LerpRoutine());
        float m = (destination.position - transform.position).magnitude;
        Vector2 dir = (destination.position - transform.position).normalized;
        while(m >= speed * Time.unscaledDeltaTime)
        {
            m = (destination.position - transform.position).magnitude;
            transform.position += (Vector3)dir * speed * Time.unscaledDeltaTime;
            yield return null;
        }

        StopCoroutine(routine);
        yield return new WaitForSeconds(Random.Range(1f, 2.5f));

        isFalling = false;
        this.gameObject.SetActive(false);
    }

    private IEnumerator LerpRoutine()
    {
        float value = 1f;

        WaitForSeconds waitTime = new WaitForSeconds(0.5f);
        SpriteRenderer renderer = localObj.GetComponent<SpriteRenderer>();
        Vector2 dir = (localDirObj.transform.position - localObj.transform.position).normalized;
        dir = new Vector2(dir.x * value, dir.y);
        renderer.flipX = dir.x < 0f;
        while(true)
        {
            float length = (localDirObj.transform.position - localObj.transform.position).magnitude;
            float lerpTime = 1f;
            float lerpSpeed = 1f;
            float currentTime = 0f;

            Vector2 startPosition = localObj.localPosition;
            Vector2 endPosition = (Vector2)localObj.localPosition + (dir * length);
            while (currentTime < lerpTime)
            {
                currentTime += Time.unscaledDeltaTime * lerpSpeed;

                float t = currentTime / lerpTime;

                t = t * t * t * (t * (6f * t - 15f) + 10f);
                localObj.transform.localPosition = Vector2.Lerp(startPosition, endPosition, t);
                yield return null;
            }

            value *= -1f;
            yield return waitTime;

            dir = (localDirObj.transform.position - localObj.transform.position).normalized;
            dir = new Vector2(dir.x * value, dir.y);
        }
    }
}
