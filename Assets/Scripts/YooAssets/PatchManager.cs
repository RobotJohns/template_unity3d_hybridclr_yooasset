using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Event;
using YooAsset;

public class PatchManager
{
    private static PatchManager _instance;
    public static PatchManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PatchManager();
            return _instance;
        }
    }

    private readonly EventGroup _eventGroup = new EventGroup();

    /// <summary>
    /// 协程启动器
    /// </summary>
    public MonoBehaviour Behaviour;


    private PatchManager()
    {
        // 注册监听事件
        _eventGroup.AddListener<SceneEventDefine.ChangeToLobbyScene>(OnHandleEventMessage);
    }

    /// <summary>
    /// 开启一个协程
    /// </summary>
    public void StartCoroutine(IEnumerator enumerator)
    {
        Behaviour.StartCoroutine(enumerator);
    }

    /// <summary>
    /// 接收事件
    /// </summary>
    private void OnHandleEventMessage(IEventMessage message)
    {
        if (message is SceneEventDefine.ChangeToLobbyScene)
        {
            YooAssets.LoadSceneAsync("Assets/Bundles/Scenes/Lobby");
        }
    }
}