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
    /// ��Դϵͳ����ģʽ
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
    void Awake()
    {
        Debug.Log($"��Դϵͳ����ģʽ��{PlayMode}");
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
    }
    IEnumerator Start()
    {

        // ��Ϸ������
        PatchManager.Instance.Behaviour = this;

        // ��ʼ���¼�ϵͳ
        UniEvent.Initalize();

        // ��ʼ����Դϵͳ
        YooAssets.Initialize();

        // ���ظ���ҳ��
        GameObject go = Resources.Load<GameObject>("Prefabs/PatchWindow");
        GameObject.Instantiate(go);

        // ��ʼ������������
        PatchOperation operation = new PatchOperation("DefaultPackage", EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

        // ����Ĭ�ϵ���Դ��
        var gamePackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(gamePackage);

        // ��Դ�������Ŷ ִ���ȸ� DLl 
        HybirdCLRManager hybirdCLRManager = FindAnyObjectByType<HybirdCLRManager>();
        yield return StartCoroutine(hybirdCLRManager.InitHybirdCLR(gamePackage, PlayMode));

        // �л�����ҳ�泡��
        SceneEventDefine.ChangeToLobbyScene.SendEventMessage();
    }
}
