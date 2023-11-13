using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    private static CubeManager _Instance;

    private List<float> _tickDurationList = new List<float>();
    private List<TickSample> _tickSampleList = new List<TickSample>();

    // Start is called before the first frame update
    void Start()
    {
        if (_Instance == null) _Instance = this;
        else if(_Instance != this) Destroy(this);

    }

    public static CubeManager GetInstance() 
    {
        if (_Instance == null)
        {
            GameObject myObject =  new GameObject();
            _Instance = myObject.AddComponent<CubeManager>();
            myObject.name = "Cube Manager";

        }
        return _Instance;
    }

    private void Update()
    {
    }

    public TickSample CreateCoroutine(float pTime, CubeMovement pCubeMovement) 
    {
        if (_tickDurationList.Contains(pTime)) 
        {
            int lIndex = _tickDurationList.IndexOf(pTime);
            _tickSampleList[lIndex].CubeTickList.Add(pCubeMovement);
            return _tickSampleList[lIndex];
        }
        else 
        {
            _tickDurationList.Add(pTime);
            TickSample newTickSample = new GameObject().AddComponent<TickSample>();
            newTickSample.name = "Tick Sample";
            newTickSample.Init(pTime);
            _tickSampleList.Add(newTickSample);
            _tickSampleList[_tickSampleList.Count - 1].CubeTickList.Add(pCubeMovement);
            return newTickSample;
        }
    }

    private void OnDestroy()
    {
        if(_Instance == this ) _Instance = null;
    }


}
