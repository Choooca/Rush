using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private OrbitalCamera _camera;
    public void AnimFinish() 
    {
        _camera.FreeCam();
    }
}
