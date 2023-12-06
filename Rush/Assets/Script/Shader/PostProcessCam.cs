using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessCam : MonoBehaviour
{

    [SerializeField] private Material material;

    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }


}
