using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private Animator anim;
    public bool isEnd { get; private set; }
    private void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(Move(4, 1.75f));
    }
    private IEnumerator Move(int count, float distance)
    {
        isEnd = false;
        for (int i = 0; i < count; i++)
        {
            anim.Play("start");

            Vector2 startPosition = transform.position;
            Vector2 endPosition = transform.position + new Vector3(distance, 0f);

            float lerpSpeed = 2f;
            float currentTime = 0f;
            float lerpTime = 1.03f;

            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentSpeed);
                yield return null;
            }

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("idle"));
            transform.position = transform.GetChild(0).position;
            transform.position = new Vector3(transform.parent.localScale.x * transform.position.x, transform.position.y);
        }

        isEnd = true;
    }
}
