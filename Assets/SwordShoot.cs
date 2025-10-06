using UnityEngine;
using UnityEngine.UI;

public class SwordShoot : MonoBehaviour
{
    public GameObject swordPre;
    public float speed = 20f;
    public Button swordFar;
    private bool onSword;
    private float swordTimer;
    void Start()
    {
        if(swordPre != null)
            swordFar.onClick.AddListener(SwordEnable);
    }
    void Update()
    {
        swordTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F) && swordTimer>3)
        {


            Vector3 pos = transform.position + transform.right * 2;
            GameObject newBullet = Instantiate(swordPre, pos, transform.rotation);

            //剑气移动
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;
            swordTimer = 0;

            // 3秒后自动销毁
            Destroy(newBullet, 3f);
        }
    }
    void SwordEnable()
    {
        onSword = true;
        

    }
}