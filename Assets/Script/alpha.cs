using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeOpacity : MonoBehaviour
{
    public float opacity = 0.5f; //暂时调一下到时候根据美术资产决定
    
    void Start()
    {
        SetOpacity(opacity);
    }
    
    
    public void SetOpacity(float alpha)
    {
        
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            
            Material material = renderer.material;

            
            Color color = material.color;
            color.a = alpha; 
            material.color = color;
        }
    }
}
