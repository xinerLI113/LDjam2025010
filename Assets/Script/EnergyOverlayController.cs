using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyOverlayController : MonoBehaviour
{
    [SerializeField] private Image energyOverlay;

    public void SetEnergyOverlay(bool state)
    {
        energyOverlay.enabled = state;
    }

    public void SetColor(Color color)
    {
        energyOverlay.color = color;
    }

    public void SetAlpha(float alpha)
    {
        energyOverlay.color = new Color(energyOverlay.color.r,energyOverlay.color.g,energyOverlay.color.b,alpha);
    }
    
    /*
     * 用法:
     * EnergyOverlayController controller = FindObjectOfType<EnergyOverlayController>();
     * controller.SetEnergyOverlay(true); // or false
     *
     * 记得先把prefab拖进场景里
     */
}
