using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraShader : MonoBehaviour
{
    [SerializeField] private Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material)
        {
            Graphics.Blit(source, destination, material);
        } else
        {
            Graphics.Blit(source, destination);
        }
    }

    public void GetHit()
    {
        material.SetFloat("_Transition", 0.2f);
    }

    float velocity = 0.0f;
    private void Update()
    {

            float currentTransitionValue = material.GetFloat("_Transition");
            material.SetFloat("_Transition", Mathf.SmoothDamp(currentTransitionValue, -1.0f, ref velocity,1.0f));
        
    }
}
