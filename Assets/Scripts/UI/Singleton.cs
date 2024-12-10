using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 存储单例实例
    private static T instance;

    // 保证线程安全的锁
    private static readonly object lockObj = new object();

    // 单例属性，访问时返回唯一实例
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 线程安全的方式检查和创建实例
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        // 寻找场景中的现有实例
                        instance = FindObjectOfType<T>();

                        // 如果场景中没有现有实例，就创建一个新的
                        if (instance == null)
                        {
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            instance = singletonObject.AddComponent<T>();
                            DontDestroyOnLoad(singletonObject); // 确保单例不会被销毁
                        }
                    }
                }
            }
            return instance;
        }
    }

    // 确保在场景销毁时，正确地清理掉实例
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}

