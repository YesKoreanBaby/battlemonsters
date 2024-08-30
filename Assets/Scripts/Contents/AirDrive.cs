using System;
using UnityEngine;

public class AirDrive : MonoBehaviour
{
    public GameObject effectPrefab;
    public float maxTime = 0.1f;
    public Action endEvent;
    private float time = 0f;

    private void Start()
    {
        time = maxTime;
    }

    private void Update()
    {
        if(time > 0f)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = maxTime;
            Instantiate(effectPrefab, transform.position, Quaternion.identity);

            if (endEvent != null)
                endEvent.Invoke();
        }
    }
}
