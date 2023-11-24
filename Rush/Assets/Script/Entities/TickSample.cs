using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickSample : MonoBehaviour
{
    private float _Time;
    public float _ElapseTime = 0;
    public float _HalfElapseTime = 0;

    public float _Ratio { get; private set; }

    public UnityEvent TickEvent = new UnityEvent();


    public void Init(float pTime) 
    {
        _Time = pTime / 2f;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_HalfElapseTime >= _Time)
        {
            _HalfElapseTime -= _Time;
            TickEvent.Invoke();
        }
        _HalfElapseTime+= Time.deltaTime;
        _ElapseTime += Time.deltaTime;
    }
   


}
