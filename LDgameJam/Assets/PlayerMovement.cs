using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 7;
    private bool isGround = false;
    public float jumpForce = 3f;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
        
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        float InputHorizontal = Input.GetAxis("Horizontal");
        float InputVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(InputHorizontal, InputVertical,0.0f);
        rb.AddForce(playerSpeed * movement, ForceMode2D.Force);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

        }
        
    }
}
