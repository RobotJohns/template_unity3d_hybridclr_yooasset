using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;

public static class HybridAssets
{
    /// <summary>
    /// 生成热更新 dll 并且拷贝到目标文件夹
    /// </summary>
    [MenuItem("HybridCLR/Assets/BuildDllHotupdateAssembly")]
    public static void BuildDllHotupdateAssembly()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string dllPath = Application.dataPath + "/Bundles/Assembly/HotFix";
        CopyHotUpdateAssembliesToAssetPath(dllPath);

        CompileDllCommand.CompileDll(target);
        CopyHotUpdateAssembliesToAssetPath(dllPath);
        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 生成 AOT母包  dll 元数据 并且开拷贝到目标文件夹
    /// </summary>
    [MenuItem("HybridCLR/Assets/BuildDllAOTMetaDataAssembly")]
    public static void BuildDllAOTMetaDataAssembly()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string dllMetaPath = Application.dataPath + "/Bundles/Assembly/Metadata";
        StripAOTDllCommand.GenerateStripedAOTDlls();
        CopyAOTMetaDataAssembliesToAssetPath(dllMetaPath);
        AssetDatabase.Refresh();
    }

 
    /// <summary>
    /// 拷贝 元数据 dll
    /// </summary>
    public static void CopyAOTMetaDataAssembliesToAssetPath(string path)
    {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAsssembliesDstDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        Debug.Log("---aotAsssembliesDstDir:" + aotAsssembliesDstDir);
        //Debug.Log("---SettingsUtil.AOTAssemblyNames:" + SettingsUtil.AOTAssemblyNames.ToArray().Length);
        //Parallel.ForEach(SettingsUtil.AOTAssemblyNames, aotDll =>
        //{
        //    string dllPath = $"{aotAsssembliesDstDir}/{aotDll}.dll";
        //    string dllBytesPath = $"{path}/{aotDll}.dll.bytes";
        //    System.IO.File.Copy(dllPath, dllBytesPath, true);
        //    Debug.Log($"copy AOTMetadata dll {dllPath} TO {dllBytesPath}");
        //});
        if (Directory.Exists(aotAsssembliesDstDir))
        {
            // 遍历文件夹中的文件
            string[] files = Directory.GetFiles(aotAsssembliesDstDir);
            foreach (string file in files)
            {
                string destinationPath = Path.Combine(path, $"{Path.GetFileName(file)}.bytes");
                System.IO.File.Copy(file, destinationPath, true);
            }
        }

    }

    /// <summary>
    /// 拷贝 代码 dll
    /// </summary>
    /// <param name="assetsdllPth"></param>
    public static void CopyHotUpdateAssembliesToAssetPath(string path)
    {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string asssembliesDstDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        Parallel.ForEach(SettingsUtil.HotUpdateAssemblyNamesExcludePreserved,  aotDll => 
        {
            Debug.Log(aotDll);
            string dllPath = $"{asssembliesDstDir}/{aotDll}.dll";
            string dllBytesPath = $"{path}/{aotDll}.dll.bytes";
            System.IO.File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"copy hotfix dll {dllPath} TO {dllBytesPath}");
        });
 
    }
}