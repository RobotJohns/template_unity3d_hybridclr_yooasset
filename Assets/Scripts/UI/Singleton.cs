using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // �洢����ʵ��
    private static T instance;

    // ��֤�̰߳�ȫ����
    private static readonly object lockObj = new object();

    // �������ԣ�����ʱ����Ψһʵ��
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // �̰߳�ȫ�ķ�ʽ���ʹ���ʵ��
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        // Ѱ�ҳ����е�����ʵ��
                        instance = FindObjectOfType<T>();

                        // ���������û������ʵ�����ʹ���һ���µ�
                        if (instance == null)
                        {
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            instance = singletonObject.AddComponent<T>();
                            DontDestroyOnLoad(singletonObject); // ȷ���������ᱻ����
                        }
                    }
                }
            }
            return instance;
        }
    }

    // ȷ���ڳ�������ʱ����ȷ�������ʵ��
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}

