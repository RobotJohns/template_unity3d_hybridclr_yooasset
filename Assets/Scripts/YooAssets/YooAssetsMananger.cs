using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniFramework.Event;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using YooAsset;

public class YooAssetsMananger : MonoBehaviour
{
    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    void Awake()
    {
        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
    }
    IEnumerator Start()
    {

        // 游戏管理器
        PatchManager.Instance.Behaviour = this;

        // 初始化事件系统
        UniEvent.Initalize();

        // 初始化资源系统
        YooAssets.Initialize();

        // 加载更新页面
        GameObject go = Resources.Load<GameObject>("Prefabs/PatchWindow");
        GameObject.Instantiate(go);

        // 开始补丁更新流程
        PatchOperation operation = new PatchOperation("DefaultPackage", EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

        // 设置默认的资源包
        var gamePackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(gamePackage);

        // 资源下载完毕哦 执行热更 DLl 
        HybirdCLRManager hybirdCLRManager = FindAnyObjectByType<HybirdCLRManager>();
        yield return StartCoroutine(hybirdCLRManager.InitHybirdCLR(gamePackage, PlayMode));

        // 切换到主页面场景
        SceneEventDefine.ChangeToLobbyScene.SendEventMessage();
    }
}
