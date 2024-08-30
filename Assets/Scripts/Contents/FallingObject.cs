using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [System.NonSerialized]
    public bool isEnd = false;

    [System.NonSerialized]
    public bool isEnd2 = false;
    public void Init(float xDir, float power)
    {
        StartCoroutine(LerpRoutine(power, xDir));
    }

    public void MoveTowards(Transform parent, Vector2 startPosition, Vector2 endPosition)
    {
        StartCoroutine(MoveTowardsRoutine(parent, startPosition, endPosition));
    }

    private IEnumerator MoveTowardsRoutine(Transform parent, Vector2 startPosition, Vector2 endPosition)
    {
        isEnd2 = false;
        float lerpTime = 1f;
        float lerpSpeed = 2f;
        float currentTime = 0f;

        float startScale = transform.localScale.x;
        float endScale = transform.localScale.x + 0.25f;
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);

            float value = Mathf.Lerp(startScale, endScale, currentSpeed);
            transform.localScale = new Vector3(value, value, 1f);
            yield return null;
        }

        this.transform.parent = parent;

        isEnd2 = true;
    }
    private IEnumerator LerpRoutine(float _jumpPower, float xDir)
    {
        isEnd = false;

        float _posY = transform.position.y;
        float _gravity = 9.8f;
        float _jumpTime = 0.0f;
        float height = (_jumpTime * _jumpTime * (-_gravity) / 2) + (_jumpTime * _jumpPower);

        float randomDownPosition = Random.Range(-0.5f, 0f);
        while (height >= randomDownPosition)
        {
            height = (_jumpTime * _jumpTime * (-_gravity) / 2) + (_jumpTime * _jumpPower);
            transform.position = new Vector3(transform.position.x + (xDir * 6f * Time.deltaTime), _posY + height, transform.position.z);
            //점프시간을 증가시킨다.
            _jumpTime += Time.deltaTime;
            yield return null;
        }

        _posY = transform.position.y;
        _gravity = 9.8f;
        _jumpPower = 1.2f;
        _jumpTime = 0.0f;
        height = (_jumpTime * _jumpTime * (-_gravity) / 2) + (_jumpTime * _jumpPower);

        while (height >= 0f)
        {
            height = (_jumpTime * _jumpTime * (-_gravity) / 2) + (_jumpTime * _jumpPower);
            transform.position = new Vector3(transform.position.x, _posY + height, transform.position.z);
            //점프시간을 증가시킨다.
            _jumpTime += Time.deltaTime;
            yield return null;
        }

        isEnd = true;
    }
}
