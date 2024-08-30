using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowEffect : MonoBehaviour
{
    private void Start()
    {
        int childCount = transform.childCount;
        for(int i = 0; i < childCount; i++)
        {
            int rand = Random.Range(1, 5);
            var anim = transform.GetChild(i).GetComponent<Animator>();
            anim.Play(rand.ToString());
        }
    }
}
