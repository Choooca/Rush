using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] public int cubeType;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) child.GetComponent<MeshRenderer>().material.color = GameManager.GetInstance().colorType[cubeType % GameManager.GetInstance().colorType.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
