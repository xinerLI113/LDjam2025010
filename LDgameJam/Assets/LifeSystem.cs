using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public float lifeLevel = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Skills"))
        {
            lifeLevel -= Skills.skillValue;

        }
    }
    void OerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Skills"))
        {
            lifeLevel -= Skills.skillValue;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeLevel <= 0)
        {
            Destroy(gameObject);
        }
        
    }
}
