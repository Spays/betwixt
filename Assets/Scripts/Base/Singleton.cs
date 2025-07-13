using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    
    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("New instance: " + this);
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Debug.Log("uplicate instance: " + this);
            Destroy(gameObject);
        }
    }
}