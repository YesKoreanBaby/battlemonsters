using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    float time = 0f;
    float maxTime = 0f;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        maxTime = Random.Range(3f, 7.1f);
    }

    private void Update()
    {
        if(time <= maxTime)
            time += Time.deltaTime;
        else
        {
            maxTime = Random.Range(3f, 7.1f);
            time = 0f;
            animator.Play("displaynoisze");
        }
    }
}
