using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YooAsset;

public class HybirdCLRManager : MonoBehaviour
{
    private Assembly hotAssembly;
    //private EPlayMode playMode ;
    private ResourcePackage resourcePackage;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    internal IEnumerator InitHybirdCLR(ResourcePackage package, EPlayMode playMode)
    {   
        resourcePackage = package;
        if (playMode == EPlayMode.EditorSimulateMode)
        {
            hotAssembly = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "UI");
        }
        else {
            yield return StartCoroutine(HotfixPipeline());
        }   
    }
    /// <summary>
    /// �ȸ��� Dll ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator HotfixPipeline() {
        yield return StartCoroutine(MetaDataAOTProces());
        yield return StartCoroutine(HotFixDllProces());
    }

    /// <summary>
    /// �� AOT dll ���� Ԫ����
    /// 
    /// Ϊ�� ��ֹ©���� Ԫ�������� һ���Լ������� AOT dll Ԫ���� �е�򵥴ֱ� 
    /// ����Ҳ�������� MetaDataConfig.MetaDataList
    /// </summary>
    /// <returns></returns>
    private IEnumerator MetaDataAOTProces() {
        List<TextAsset> aotMetaDataList = new List<UnityEngine.TextAsset>();
        //for (int i = 0; i < MetaDataConfig.MetaDataList.Count; i++)
        //{
        //    AssetHandle metadataHandle = resourcePackage.LoadAssetAsync<TextAsset>("Assets/Bundles/Assembly/Metadata/" + MetaDataConfig.MetaDataList[i]);
        //    yield return metadataHandle;
        //    TextAsset textAsset = metadataHandle.AssetObject as TextAsset;
        //    aotMetaDataList.Add(textAsset);
        //}

        ///����Ŀ¼���� ����
        ///���� ��Դ�� // ע�⣺locationֻ��Ҫ��д��Դ�����������Դ��ַ��
        AllAssetsHandle handle = resourcePackage.LoadAllAssetsAsync<TextAsset>("Assets/Bundles/Assembly/Metadata/mscorlib.dll");
        yield return handle;
        foreach (var assetObj in handle.AllAssetObjects)
        {
            aotMetaDataList.Add(assetObj as TextAsset);
        }

        foreach (var aotMetaData in aotMetaDataList)
        {  
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(aotMetaData.bytes, HomologousImageMode.SuperSet);
            if (err != LoadImageErrorCode.OK) {
                Debug.Log($"LoadMetadataForAOTAssembly:{aotMetaData.name}. ret:{err}");
            }
        }
    }

 

    /// <summary>
    /// ���� �ȸ��� Dll
    /// </summary>
    /// <returns></returns>
    private IEnumerator  HotFixDllProces()
    {
        AssetHandle handle = this.resourcePackage.LoadAssetAsync<TextAsset>("Assets/Bundles/Assembly/HotFix/UI.dll");
        yield return handle;
        TextAsset textAsset = handle.AssetObject as TextAsset;
        hotAssembly = Assembly.Load(textAsset.bytes);

        ///�������
        //AllAssetsHandle handle = resourcePackage.LoadAllAssetsAsync<TextAsset>("Assets/Bundles/Assembly/HotFix/UI.dll");
        //yield return handle;
        //foreach (var assetObj in handle.AllAssetObjects)
        //{
        //    Assembly.Load((assetObj as TextAsset).bytes);
        //}
    }
}
