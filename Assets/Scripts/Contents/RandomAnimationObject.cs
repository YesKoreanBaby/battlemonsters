using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationObject : MonoBehaviour
{
    private bool isRunning = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(isRunning == false)
            StartCoroutine(RandomTimeRoutine());
    }

    private void OnEnable()
    {
        if(isRunning == true)
        {
            isRunning = false;
            StopAllCoroutines();
        }
    }
    private void OnDisable()
    {
        if (isRunning == true)
        {
            isRunning = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator RandomTimeRoutine()
    {
        isRunning = true;

        float randomTime = Random.Range(0.1f, 2f);

        yield return new WaitForSeconds(5f + randomTime);

        animator.Play("stay");
        yield return null;
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("idle"));

        isRunning = false;
    }
}
