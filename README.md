## template_unity3d_hybridclr_yooasset
__unity3d 使用 hybridclr 和  yooasset 实现热更新资源和代码， 模版工程。__
__工程特点：实现完整 资源更新 和 代码更新逻辑。可直接商用 考虑到方便移植 除过（hybridclr，yooasset）再无其他框架 。__

### 

___文章介绍基于 这两个插件安装成功的前提下: hybridclr 负责代码相关逻辑，以及生成可用于热更新的脚步dll.  yooasset 负责 将资源打包（hybridclr 生成的dll），下载更新，资源加载， 安装过程官网有详细介绍___

### unity3D     版本：2022.3.50f1c1

### hybridclr   版本：7.1.0   官网：https://hybridclr.doc.code-philosophy.com/docs/intro
- #### hybridclr 安装 官网有详细教程，<font color="red">（打开项目必须自行安装一遍，应为 hybridclr 对 unity3d 运行时做了修改，也就是修改了 Unity3d 工程相关源码，执行安装过程会自动修改或者替换）</font>
- #### <font color="blue">主要功能： 生成热更脚本的资源，和执行脚本相关</font>

### yooasset    版本：2.2.4-preview  官网：https://www.yooasset.com
- #### yooasset 安装 官网有详细教程自行安装，
- #### <font color="blue">主要功能： 游戏资源更新处理： 资源的打包， 资源的下载 更新 加载 等逻辑</font>

### 项目目录介绍：
    Assets :
    	Bundles     作为所有资源的文件夹，也就是 yooAssets 需要打包的资源文件夹
                    Assembly     热更新代码dll 
                        HotFix   为  热更新代码逻辑 dll 目录
                        Metadata 为  补充 AOT元数据 dll 目录
                    models       模型文件
                    Prafabs      预制体
                    Scenes       场景
                    Shaders      着色器
                    Texture      纹理 图片

	    Scripts      为脚本目录
		    UI       为热更新程序集 UI.dll
		    其余目录 为AOT 母包相关，yooAssets 资源管理 加载, HybrudCRL 热更新加载加载等

    ThreeTools：
		    hfs.exe 轻量级 本地http 资源服务器：可用于搭建 http 资源服务器 用于本地资源热更测试

            

## 项目入口介绍： 
__Entrance 为项目入口场景, 成功完成更新资源之后（或者检查无需要更新资源以及）就会进入 Lobby 场景__
#### 项目启动：
- yooasset 资源检查资源版本，匹配资源 ，更新资源  下载UI进度条等
- hybridclr 等待所有资源下载完毕之后， 加载 AOT元数据, 加载 Dll 资源
#### 入口场景：    
![入口场景](https://github.com/RobotJohns/Assets/blob/main/202412/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20241210134118.png?raw=true)
#### 启动脚本挂载：  
![场景挂载](https://github.com/RobotJohns/Assets/blob/main/202412/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20241210134405.png?raw=true)
##### YooAssetsMananger.cs
``` 
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

        // 游戏更新界面管理器
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
```

##### HybirdCLRManager.cs
``` 
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
    /// 热更新 Dll 流程
    /// </summary>
    /// <returns></returns>
    private IEnumerator HotfixPipeline() {
        yield return StartCoroutine(MetaDataAOTProces());
        yield return StartCoroutine(HotFixDllProces());
    }

    /// <summary>
    /// 给 AOT dll 补充 元数据
    /// 
    /// 为了 防止漏掉的 元数据这里 一次性加载所有 AOT dll 元数据 有点简单粗暴 
    /// 这里也可以配置 MetaDataConfig.MetaDataList
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

        ///按照目录进行 加载
        ///参数 资源组 // 注意：location只需要填写资源包里的任意资源地址。
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
    /// 加载 热更新 Dll
    /// </summary>
    /// <returns></returns>
    private IEnumerator  HotFixDllProces()
    {
        AssetHandle handle = this.resourcePackage.LoadAssetAsync<TextAsset>("Assets/Bundles/Assembly/HotFix/UI.dll");
        yield return handle;
        TextAsset textAsset = handle.AssetObject as TextAsset;
        hotAssembly = Assembly.Load(textAsset.bytes);

        ///多个加载
        //AllAssetsHandle handle = resourcePackage.LoadAllAssetsAsync<TextAsset>("Assets/Bundles/Assembly/HotFix/UI.dll");
        //yield return handle;
        //foreach (var assetObj in handle.AllAssetObjects)
        //{
        //    Assembly.Load((assetObj as TextAsset).bytes);
        //}
    }
}

```



## 编辑器介绍（大家可以直接看官网）： 
### hybridclr
### 使用 hybridclr 自带的按钮生成也是可以的，他是有自己的目录的，我们自己编写的脚步 可以帮助我们打包生成，修改文件后缀 并且自行拷贝到 目标文件夹
- #### 配置需要更新的程序集
 ![hybridclr](https://github.com/RobotJohns/Assets/blob/main/202412/png_03.png?raw=true)
这里是配置 HybridCRL 需要热更新的 程序集
<font color="blue">Hot Update Assembly Definitions</font> 等价 <font color="blue">Hot Update Assemblies</font> 配置一个就可以了
- #### 生成热更新资源
 ![hybridclr](https://github.com/RobotJohns/Assets/blob/main/202412/png_02.png?raw=true)
- 大多时候 我们代码的修改 只需要执行 Assets -> BuildDllHotupdateAssembly 就可了，
- 至于元数据补充 要看你代码是否使用 被裁剪的函数，就是 用户使用的母包AOT 中的函数被裁剪，热更新代码中使用到了。这个时候需要补充元数据


### yooasset
### 我这里预设了 6个文件夹，这里主要介绍下 Assembly，就是代码 组 ，执行完毕打包 我们需要拷贝资源到 对应的服务器目录
- ### 资源组的配置
 ![yooasset](https://github.com/RobotJohns/Assets/blob/main/202412/png_04.png?raw=true)
- ### 资源打包
 ![yooasset](https://github.com/RobotJohns/Assets/blob/main/202412/png_05.png?raw=true)
 
 

## hfs 是一个轻量级http 资源服务器，可以帮助我们快速在本地大家资源服务器，当然你 nginx 也是可以。选择 hfs是应为开关方便 添加文件目录方法，完全按钮操作就可以： 
- ### hfs 配置页面
 ![yooasset](https://github.com/RobotJohns/Assets/blob/main/202412/png_06.png?raw=true)
- ### hfs 映射本地路径
 ![yooasset](https://github.com/RobotJohns/Assets/blob/main/202412/png_07.png?raw=true)
- ### 资源服务器的路径要和代码中的路径对应
 ![yooasset](https://github.com/RobotJohns/Assets/blob/main/202412/png_08.png?raw=true)
``` HostServerConfig
public class HostServerConfig  
{
    public static string GetConfig() {

        //string hostServerIP = "http://192.168.1.9:8080"; //安卓模拟器地址
        //string hostServerIP = "http://192.168.1.9:8080";
        string hostServerIP = "http://192.168.1.9:8080";
        string appVersion = "1.0.0";

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
    }
}
``` 

## 上手跑起来-运行
1. `HybridCRl -> Generata -> All                          #生成HybridCRl 所需资源`
2. `HybridCRl -> Assets   -> BuildDllAOTMetaDataAssembly  #生成元数据    并执行拷贝到资源路径`
3. `HybridCRl -> Assets   -> BuildDllHotupdateAssembly    #生成代码      热更新 Dll 并执行拷贝`
4. `yooassets  生成资源`
5. `hfs 部署`
6. `运行`

<figure class="video_container">
    <iframe width="560" height="315" src="https://www.youtube.com/embed/s8Cs2zaQ8g8" frameborder="0" allowfullscreen></iframe>
</figure>



## 上手跑起来-热更新资源
1. `新加预制体 和 对应脚本`
2. `HybridCRl -> Assets   -> BuildDllHotupdateAssembly    #生成代码      热更新 Dll`
3. `yooassets  生成资源`
4. `hfs 部署`
5. `运行 查看效果`

<figure class="video_container">
    <iframe width="560" height="315" src="https://www.youtube.com/embed/Djama3DxfiY" frameborder="0" allowfullscreen></iframe>
</figure>
