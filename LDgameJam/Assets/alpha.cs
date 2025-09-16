using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeOpacity : MonoBehaviour
{
    public float opacity = 0.5f; // 透明度，0完全透明，1完全不透明
    
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
