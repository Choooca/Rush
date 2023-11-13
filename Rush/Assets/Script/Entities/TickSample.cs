using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickSample : MonoBehaviour
{
    private float _Time;
    private float _ElapseTime = 0;
    public float _Ratio { get; private set; }

    public List<CubeMovement> CubeTickList = new List<CubeMovement>();

    public void Init(float pTime) 
    {
        _Time = pTime;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (_ElapseTime >= _Time)
        {
            foreach (CubeMovement lCubeTick in CubeTickList)
            {
                if (lCubeTick.TickAction.Method != null)
                {
                    lCubeTick.TickAction();
                }
            }
            _ElapseTime -= _Time;
        }    
        _ElapseTime += Time.deltaTime;
        _Ratio = _ElapseTime / _Time;
    }
   


}
