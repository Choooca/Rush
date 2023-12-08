using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private static VFXManager _Instance;

    [SerializeField] private Animator DissolveAnimator;
    [SerializeField] private GameObject Dissolve;
    [SerializeField] private GameObject DestroyParticle;

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

    public void SpawnDestroyParticle(Vector3 pos, Color color) 
    {
        GameObject lObject = Instantiate(DestroyParticle, pos, Quaternion.identity);
        lObject.GetComponent<Renderer>().material.SetColor("_Color", color);
    }

    private void OnDestroy()
    {
        if(_Instance == this) _Instance = null;
    }
}
