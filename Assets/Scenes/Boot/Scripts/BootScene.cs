using UnityEngine;

public class BootScene : SceneBase
{
    public static string NextSceneName = ConstSceneName.Title;

    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            NextSceneName = ConstSceneName.Title;
        }

        Application.targetFrameRate = 60;

        MainSystem.Instance.AppSceneManager.ChangeScene(NextSceneName);
    }
}
