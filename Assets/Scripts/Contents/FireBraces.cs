using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireBraces : MonoBehaviour
{
    public float distance = 2f;
    public float rotationSpeed = 9f;
    public int count = 5;
    public GameObject childPrefab;

    private bool isStart = false;
    private void Update()
    {
        if(isStart == true)
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
            }
        }
    }

    public IEnumerator Fire(Vector2 endPosition)
    {
        Vector2 startPosition = (Vector2)transform.position + Vector2.right * (distance + 1f);
        Transform bullet = transform.GetChild(0);
        bullet.parent = null;
        bullet.rotation = Quaternion.identity;
        bullet.position = startPosition;
        bullet.GetComponent<BoxCollider2D>().enabled = true;
        if (transform.childCount > 0)
        {
            int count = transform.childCount;
            float besideAngle = 360 / count;
            for (int i = 0; i < count; i++)
            {
                float angle = i * besideAngle;
                Vector2 dir = Quaternion.Euler(0f, 0f, angle) * new Vector2(1, 0);
                Vector2 position = (Vector2)transform.position + dir * distance;
                transform.GetChild(i).position = position;
            }
        }

        SoundManager.Instance.PlayEffect(0, 1f);
        float lerpSpeed = 2f;
        float currentTime = 0f;
        float lerpTime = 1f;

        while (currentTime < lerpTime)
        {
            if (SkillManager.Instance.stopStructure.dontMove == true)
                break;
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            bullet.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }
    }

    public IEnumerator Play()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        float besideAngle = 360 / count;
        for (int i = 0; i < count; i++)
        {
            float angle = i * besideAngle;
            Vector2 dir = Quaternion.Euler(0f, 0f, angle) * new Vector2(1, 0);
            Vector2 position = (Vector2)transform.position + dir * distance;
            var clone = Instantiate(childPrefab, position, Quaternion.identity);
            clone.transform.parent = transform;
            clone.gameObject.name = i.ToString();
            SoundManager.Instance.PlayEffect(17, 1f);
            yield return waitTime;
        }
        yield return waitTime;
        SoundManager.Instance.PlayEffect(2, 1f);
        isStart = true;
    }
}
