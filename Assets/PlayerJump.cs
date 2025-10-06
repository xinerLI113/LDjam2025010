using UnityEngine;

public class SimpleWaterJump : MonoBehaviour
{
    public float jumpForce = 10f;
    private bool isInWater = false;

    void Update()
    {
        // 检测跳跃输入
        if (Input.GetKeyDown(KeyCode.Space) && isInWater)
        {
            Jump();
        }
    }

    void Jump()
    {
        // 执行跳跃
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, jumpForce);
        Debug.Log("水中跳跃！");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 进入水标签的物体
        if (other.CompareTag("Water"))
        {
            isInWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 离开水标签的物体
        if (other.CompareTag("Water"))
        {
            isInWater = false;
        }
    }
}