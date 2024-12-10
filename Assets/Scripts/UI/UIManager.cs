using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public Transform UIRoot;
    public Transform UITop;
    private readonly string basePath = "Assets/Bundles/Prafabs/UI";
    private ResourcePackage resourcePackage;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        resourcePackage = YooAssets.GetPackage("DefaultPackage");
        UIImages uIImages = ShowUI<UIImages>("UIImages");
        Debug.Log($"UIManager-- {uIImages.name}");
    }
    // �����ľ�̬��������ȡ����ʵ��
    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }


    public T ShowUI<T>(string path, System.Object args = null)  where T : MonoBehaviour
    {
        T uiBehaviour = null;
        AssetHandle handle =  this.resourcePackage.LoadAssetSync<GameObject>($"{this.basePath}/{path}");
        GameObject clone = handle.InstantiateSync(this.UIRoot);
        uiBehaviour = clone.GetComponent<T>();
        if (uiBehaviour == null)
        {
            uiBehaviour = clone.AddComponent<T>();
        }
        if (uiBehaviour is UIBehaviourData behaviour)
        {
            behaviour.InitData(args);
        }

        return uiBehaviour;
    }     
}
