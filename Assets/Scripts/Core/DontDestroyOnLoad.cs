using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField]
    private bool _dontDestroyOnLoad = false;

    private void Awake()
    {
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}