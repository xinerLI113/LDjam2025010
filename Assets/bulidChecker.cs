#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class BuildChecker : MonoBehaviour
{
    [MenuItem("Tools/强制显示构建错误")]
    static void ForceBuildErrorDisplay()
    {
        // 这会触发构建并显示所有错误
        BuildPipeline.BuildPlayer(new BuildPlayerOptions()
        {
            scenes = new string[] { "Assets/Scenes/SampleScene.unity" },
            locationPathName = "Build/TestBuild",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.ShowBuiltPlayer
        });
    }
}
#endif