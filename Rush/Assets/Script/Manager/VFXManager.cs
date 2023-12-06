using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private static VFXManager _Instance;

    [SerializeField] private Animator DissolveAnimator;
    [SerializeField] private GameObject Dissolve;
    public float test;

    [SerializeField] private float timeToShow;
    private float count = 0;

    public bool show = false;
    
    public static VFXManager GetInstance() 
    {
        if( _Instance == null ) return new VFXManager();
        return _Instance;
    }

    void Start()
    {
        if (_Instance != null) Destroy(this);
        else _Instance = this;
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void ShowGame() 
    {
        Dissolve.GetComponent<Renderer>().material.SetFloat("_DissolveStep", 0);
        DissolveAnimator.SetTrigger("Dissolve");
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }
}
