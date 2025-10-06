using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Button loadSceneButton;
    public string SampleScene; // 在Inspector中设置要加载的场景名称

    void Start()
    {
        // 绑定按钮点击事件
        loadSceneButton.onClick.AddListener(LoadTargetScene);
    }

    void LoadTargetScene()
    {
        // 加载场景
        SceneManager.LoadScene(SampleScene);
    }
}