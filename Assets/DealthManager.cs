using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathGameManager : MonoBehaviour
{
    public GameObject gameOverUI; // 拖入一个UI对象
    
    void Update()
    {
        // 如果找不到玩家就显示游戏结束
        if (GameObject.FindGameObjectWithTag("Player") == null && gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f; // 暂停游戏
        }
    }
    
    // 重新开始
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // 退出游戏
    public void Quit()
    {
        Application.Quit();
    }
}