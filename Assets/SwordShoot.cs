using UnityEngine;

public class SwordShoot : MonoBehaviour
{
    public GameObject swordPre;  
    public float speed = 20f;  
    [SerializeField] private Animator swordAnimator;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            swordAnimator.SetBool("isOnSkill",true);
            Vector3 pos = transform.position + transform.right*2;
            GameObject newBullet = Instantiate(swordPre, pos, transform.rotation);
            
           //剑气移动
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;
            
            // 3秒后自动销毁
            Destroy(newBullet, 3f);
        }
    }
}