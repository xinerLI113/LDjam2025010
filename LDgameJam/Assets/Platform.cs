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
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            platForm = Instantiate(platFormPrefab, gap, quaternion.identity);
            gapTime = Time.time + makeTime;
        }
        if (Time.time > gapTime)
        {
            Destroy(platForm);

        }
        
        
    }
}
