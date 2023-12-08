using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterColor : MonoBehaviour
{
    [SerializeField] Color _color;
    [SerializeField] List<MeshRenderer> _meshRenderers;

    private void Start()
    {
        foreach (MeshRenderer renderer in _meshRenderers) 
        {
            renderer.material.color = _color;
        }
    }
}
