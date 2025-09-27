using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    



    public Transform player;        
    public Vector3 offset = new Vector3(0, 2, -5);  
    public float cameraSpeed = 0.125f;  

    void LateUpdate()
    {
        if (player == null) return;
        
        
        Vector3 desiredPosition = player.position + offset;
        
        Vector3 cameraPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
        transform.position = cameraPosition;
        transform.LookAt(player);
    }
}

