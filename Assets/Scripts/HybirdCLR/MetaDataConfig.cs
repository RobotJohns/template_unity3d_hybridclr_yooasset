using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaDataConfig
{
    /// <summary>
    /// ˵����
    ///     Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
    ///     һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
    ///     
    ///     ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
    ///     �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
    /// 
    /// ����Ԫ����
    /// </summary>
    public static List<string> MetaDataList = new List<string>
    {
        "mscorlib.dll.bytes",
        "System.dll.bytes",
        "System.Core.dll.bytes", // ���ʹ����Linq����Ҫ���
        "Newtonsoft.Json.dll.bytes",
        "protobuf-net.dll.bytes",
        "YooAsset.dll.bytes",
        "Unity.Model.dll.bytes",
        "UnityEngine.CoreModule.dll.bytes",
        "UnityEngine.JSONSerializeModule.dll.bytes",
    };
}
