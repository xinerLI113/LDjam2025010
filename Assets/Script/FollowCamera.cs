using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;        
    public Vector3 offset = new Vector3(0, 2, -5);  
    public float cameraSpeed = 5f; 
    
    void LateUpdate()
    {
        if (player == null) return;
        
        
        Vector3 desiredPosition = player.position + offset;
        transform.position = desiredPosition;
        transform.LookAt(player);
    }
}