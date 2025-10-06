using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI面板")]
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;

    void Start()
    {
        // 绑定按钮事件
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        //quitButton.onClick.AddListener(QuitGame);
        
        // 初始隐藏暂停面板
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        // ESC键暂停/继续
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale > 0)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // 暂停游戏
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // 恢复游戏
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void QuitGame()
    {
        // 退出游戏
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void RestartGame()
    {
        // 重新开始游戏
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}