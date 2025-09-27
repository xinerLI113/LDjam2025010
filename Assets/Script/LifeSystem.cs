using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public int playerLifeLevel = 3;
    public GameObject obstacle;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerLifeLevel -= 1;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerLifeLevel -= 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        if (playerLifeLevel <= 0)
        {
            Destroy(gameObject);

        }
        
    }
}
