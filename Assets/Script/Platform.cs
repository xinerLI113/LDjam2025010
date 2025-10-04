using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject platFormPrefab;
    GameObject platForm;
    public Vector2 gap = new Vector2(2, 2);
    public float makeTime = 20;
    private float gapTime;
    [SerializeField] private PlayerCollect playerCollect;
    // Start is called before the first frame update
    void Start()
    {
       playerCollect = FindFirstObjectByType<PlayerCollect>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P)&&playerCollect.materialCount>=10)//收集十个材料才能生成平台
        {
            platForm = Instantiate(platFormPrefab, gap, quaternion.identity);//生成平台在上面攻击敌人
            gapTime = Time.time + makeTime;
        }
        if (Time.time > gapTime)
        {
            Destroy(platForm);

        }
        
        
    }
}
