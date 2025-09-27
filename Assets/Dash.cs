using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("位移设置")]
    public float dashDistance = 3f;    
    public float dashDuration = 2f;    
    
    private bool isInvincible = false;
    private float invincibleTimer = 0f;
    private bool canDash = true;     
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && canDash)
        {
            PerformDash();
        }
        
        
        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer >= dashDuration)
            {
                isInvincible = false;
                invincibleTimer = 0f;
            }
        }
    }
    
    void PerformDash()
    {
        canDash = false;
        isInvincible = true;
        float direction = transform.localScale.x >= 0 ? 1f : -1f;
        
        
        Vector3 newPosition = transform.position + new Vector3(direction * dashDistance, 0f, 0f);
        transform.position = newPosition;
        
        
        Invoke("ResetDash", 5f); //cd
        
        
    }
    
    void ResetDash()
    {
        canDash = true;
    }
    
    public bool IsInvincible()
    {
        return isInvincible;//在别的脚本中再调用还没写
    }
}