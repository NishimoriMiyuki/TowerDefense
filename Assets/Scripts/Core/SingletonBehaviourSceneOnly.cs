using UnityEngine;

/// <summary>
/// シングルトン
/// シーンに配置されている必要があり、シーンを抜けると破棄する
/// </summary>
public class SingletonBehaviourSceneOnly<T> : MonoBehaviour where T : Component
{
    private static T m_ActiveInstance;

    public static T Instance
    {
        get
        {
            return m_ActiveInstance;
        }
    }

    protected virtual void Awake()
    {
        m_ActiveInstance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (this == m_ActiveInstance)
        {
            m_ActiveInstance = null;
        }
    }
}