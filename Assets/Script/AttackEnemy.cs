using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public float lifeLevel = 100;
    
    public float skillOfQValue = 10;
    public float swordHurts = 10;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (lifeLevel <= 0)
        {
            Destroy(gameObject);
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Skills"))
        {
            lifeLevel -= skillOfQValue;
            

        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        
    }
}
