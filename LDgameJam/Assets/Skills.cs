using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public GameObject arcPrefab;
    GameObject arc;
    private float destroyTime;
    public float CDtime = 0;
    public bool CD;
    static public float skillValue = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime += Time.deltaTime;
        CDtime += Time.deltaTime;
        if (CDtime >= 9f)
        {
            CD = true;
            
        }

        if (Input.GetKeyDown(KeyCode.Q) && CD == true)
        {
            arc = Instantiate(arcPrefab, transform.position, transform.rotation);
            CDtime = 0;
            CD = false;
            destroyTime = 0;

        }
        if (arc != null)
        {
            arc.transform.position = transform.position;
        }
        
        if (arc != null && destroyTime>2f)
        {
            Destroy(arc);
            arc = null;
            destroyTime = 0;
            return; 
            
        }
    }
}
