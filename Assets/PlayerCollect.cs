using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollect : MonoBehaviour
{

    public int materialCount;
    private Rigidbody2D rb;
    public string sceneToLoad = "BossRoom";
    public bool platFormEnable = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Material"))
        {
            Destroy(collision.gameObject);
            materialCount++;
        }
        if (collision.gameObject.CompareTag("Enter"))
        {
            SceneManager.LoadScene("BossRoom");
        }
        if (materialCount >= 7)
        {
            platFormEnable = true;
        }

    }
    
    
}
