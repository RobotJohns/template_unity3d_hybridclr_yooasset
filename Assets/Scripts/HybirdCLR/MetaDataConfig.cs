using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaDataConfig
{
    /// <summary>
    /// 说明：
    ///     为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    ///     一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    ///     
    ///     注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
    ///     热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
    /// 
    /// 补充元数据
    /// </summary>
    public static List<string> MetaDataList = new List<string>
    {
        "mscorlib.dll.bytes",
        "System.dll.bytes",
        "System.Core.dll.bytes", // 如果使用了Linq，需要这个
        "Newtonsoft.Json.dll.bytes",
        "protobuf-net.dll.bytes",
        "YooAsset.dll.bytes",
        "Unity.Model.dll.bytes",
        "UnityEngine.CoreModule.dll.bytes",
        "UnityEngine.JSONSerializeModule.dll.bytes",
    };
}
