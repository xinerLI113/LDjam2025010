using UnityEngine;

public class SwordShoot : MonoBehaviour
{
    public GameObject swordPre;  // 剑气预制体
    public float speed = 20f;  // 飞行速度
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // 在玩家前方2米生成剑气
            Vector3 pos = transform.position + transform.right*2;
            GameObject newBullet = Instantiate(swordPre, pos, transform.rotation);
            
            // 给剑气一个向前飞的速度
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;
            
            // 3秒后自动销毁
            Destroy(newBullet, 3f);
        }
    }
}