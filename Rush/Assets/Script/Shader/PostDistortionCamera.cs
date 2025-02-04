using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostDistortionCamera : MonoBehaviour
{

    public Shader postShader;
    Material postEffectMaterial;

    public Color screenTint;

    RenderTexture postRenderTexture;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(postEffectMaterial == null) postEffectMaterial = new Material(postShader);
        if(postRenderTexture == null) postRenderTexture = new RenderTexture(source.width, source.height, 0, source.format);

        postEffectMaterial.SetColor("_ScreenTint", screenTint);
        Graphics.Blit(source, postRenderTexture, postEffectMaterial, 0);

        Shader.SetGlobalTexture("_GlobalRenderTexture", postRenderTexture);
        Graphics.Blit(source, destination);
    }
}
