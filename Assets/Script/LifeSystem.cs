using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
{
    public double playerLifeLevel = 3;
    public GameObject obstacle;
    public int obstacleHurts = 1;//障碍物伤害
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerLifeLevel -= obstacleHurts;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
       
       
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
