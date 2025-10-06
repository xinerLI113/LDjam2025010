using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlatformManager : MonoBehaviour
{
    public Slider platformCollect;
    [SerializeField] private PlayerCollect playerCollect;
    public GameObject enterPre;
    private GameObject enter;
    public GameObject player;
    public bool enterEnable;
    
    // Start is called before the first frame update
    void Start()
    {
        platformCollect.maxValue = 7;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCollect.materialCount == 7)
        {
            enterEnable = true;
        }
        platformCollect.value = playerCollect.materialCount;
        if (enterEnable==true)
        {
            enter = Instantiate(enterPre, player.transform.position, quaternion.identity);
            
            //生成入口
        }

    }
    
    
    
}
