using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongClickManager : MonoBehaviour
{
    private static LongClickManager instance;
    public static LongClickManager Instance { get { return instance; } }

    public float requirHoldTime = 1f;

    public bool pointerDown { get; private set; }
    private float poineterDownTimer;
    private Action beginEvent;
    private Action endEvent;
    private bool isBlock = false;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if(pointerDown == true && isBlock == false)
        {
            poineterDownTimer += Time.deltaTime;
            if(poineterDownTimer >= requirHoldTime)
            {
                isBlock = true;
                if (beginEvent != null)
                    beginEvent.Invoke();

                Reset();
            }
        }
    }

    public void OnPointerDown(Action evt)
    {
        pointerDown = true;
        this.beginEvent = evt;
    }
    public void OnPointerUp(Action evt)
    {
        this.endEvent = evt;
        if(endEvent != null)
            endEvent.Invoke();
        Reset();
    }
    private void Reset()
    {
        pointerDown = false;
        isBlock = false;
        poineterDownTimer = 0f;
        beginEvent = null;
        endEvent = null;
    }
}
