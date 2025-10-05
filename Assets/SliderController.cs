using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider hpSolider;//玩家生命值
    public Slider waterSolider;//潜水值
    [SerializeField] private LifeSystem lifeSystem;
    // Start is called before the first frame update
    void Start()
    {
        waterSolider.maxValue = lifeSystem.playerWaterValue;
        hpSolider.maxValue = lifeSystem.playerLifeLevel;
    }

    // Update is called once per frame
    void Update()
    {
        hpSolider.value = lifeSystem.playerLifeLevel;
        waterSolider.value = lifeSystem.playerWaterValue;
    }
}
