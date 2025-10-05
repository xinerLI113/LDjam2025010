using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossRoomManager : MonoBehaviour
{
    public Button enterBossRoom;
    [SerializeField] private string bossRoomSceneName = "BossRoom"; 
    
    // Start is called before the first frame update
    void Start()
    {
        
        if (enterBossRoom != null)
        {
            enterBossRoom.onClick.AddListener(Enter);
        }
       
    }

    void Enter()
    {
        
        SceneManager.LoadScene(bossRoomSceneName);
    }
}