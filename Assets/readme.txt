项目说明：
	本项目 为热更新 模版框架
	unity3d 2022.3.50f1c1
	yooAssets  作为资源打包加载工具  yooAssets 版本 2.2.4-preview   
	HybridCLR  为 Dll 更新插件                 版本 7.1.0

	


目录说明
	Bundles      作为所有资源的文件夹，也就是 yooAssets 的资源文件夹
		Assembly 热更新代码dll 
			HotFix   为  热更新代码逻辑 dll 目录
			Metadata 为  补充 AOT元数据 dll 目录
		models   模型文件
		Prafabs  预制体
		Scenes   场景
		Shaders  着色器
		Texture  纹理 图片

	Scripts      为脚本目录
		UI       为热更新程序集 UI.dll
		其余目录 为AOT 母包相关，yooAssets 资源管理 加载, HybrudCRL 热更新加载加载等

   Resources     资源文件夹 AOT 母包所用，和 编辑器保存目录

其余目录三方插件生成

   NuGet       用于导入 C# 库    例如 Newtonsoft.Json


	