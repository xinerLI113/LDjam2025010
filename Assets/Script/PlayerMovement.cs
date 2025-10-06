using UnityEngine;

namespace Script
{
    public class PlayerMovement : MonoBehaviour
    {
        public float playerSpeed = 7;
        Rigidbody2D rb;
        public bool isGrounded;
        private float checkRadius = 0.2f;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }



        // Update is called once per frame
        void Update()
        {
            CheckGrounded();
            if (isGrounded)
            {
           
            }
            Movement();

        }
        void Movement()
        {
            float InputHorizontal = Input.GetAxis("Horizontal");
            float InputVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(InputHorizontal, InputVertical, 0.0f);
            rb.AddForce(playerSpeed * movement, ForceMode2D.Force);

        }
        void CheckGrounded()
        {
        
            Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        
        
            Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPosition, checkRadius);
            isGrounded = false;
        
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != gameObject && collider.CompareTag("Ground"))
                {
                    isGrounded = true;
                    break;
                }
            }
        
        
            if (!isGrounded)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
                if (hit.collider != null && hit.collider.CompareTag("Ground"))
                {
                    isGrounded = true;
                }
            }
        }
    }
}
