using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DiceStructure
{
    public Sprite image;
    public int number;
}
public class Dice : MonoBehaviour
{
    public List<DiceStructure> contents;
    public float rollTime;
    public float coolTime;

    public DiceStructure pickUpContent;
    public bool startRolled { get; private set; }
    public void Roll()
    {
        if(startRolled == false)
            StartCoroutine(RollRoutine());
    }
    private IEnumerator RollRoutine()
    {
        startRolled = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        float currentTime = 0f;
        float currentTime2 = 0f;
        int currentIndex = 0;

        for(int i = 0; i < 40; ++i)
        {
            int a = Random.Range(0, contents.Count);
            int b = Random.Range(0, contents.Count);
            while(a == b)
                b = Random.Range(0, contents.Count);

            var tmp = contents[a];
            contents[a] = contents[b];
            contents[b] = tmp;
        }
        renderer.sprite = contents[currentIndex].image;
        while (currentTime < rollTime)
        {
            if (currentTime2 >= coolTime)
            {
                currentIndex++;
                if (currentIndex == contents.Count)
                    currentIndex = 0;

                renderer.sprite = contents[currentIndex].image;
                pickUpContent = contents[currentIndex];
                SoundManager.Instance.PlayEffect(134, 1f);
                currentTime2 = 0f;
            }
            else
                currentTime2 += Time.deltaTime;

            currentTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        float lerpSpeed = 2f;
        currentTime = 0f;
        float lerpTime = 1f;
    
        float startA = renderer.color.a;

        bool palseSound = false;
        for (int i = 0; i < 3; ++i)
        {
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(startA, 0f, currentSpeed));
                yield return null;
            }
            currentTime = 0f;
            palseSound = false;
            while (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime * lerpSpeed;

                float currentSpeed = currentTime / lerpTime;
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, Mathf.Lerp(0f, startA, currentSpeed));
                if (palseSound == false)
                {
                    SoundManager.Instance.PlayEffect(134, 1f);
                    palseSound = true;
                }

                yield return null;
            }
            yield return null;
        }
        startRolled = false;
    }
}
