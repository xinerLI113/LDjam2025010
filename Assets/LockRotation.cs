using UnityEngine;

public class LockRotation2D : MonoBehaviour
{
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            
            rb.freezeRotation = true;
        }
    }
    
    void Update()
    {
        //锁定z轴不移动
        transform.rotation = Quaternion.identity;
    }
}