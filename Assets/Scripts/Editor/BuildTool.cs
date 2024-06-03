using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class BuildTool : Editor
{
    [MenuItem("Tools/Build Windows Bundle")]
    static void BundleWindowsBuild()
    {
        Build(BuildTarget.StandaloneWindows);
    }
    [MenuItem("Tools/Build Android Bundle")]
    static void BundleAndroidBuild()
    {
        Build(BuildTarget.Android);
    }
    [MenuItem("Tools/Build iPhone Bundle")]
    static void BundleiPhoneBuild()
    {
        Build(BuildTarget.iOS);
    }

    static void Build(BuildTarget target)
    {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))//meta文件不需要打包
                continue;
            AssetBundleBuild assetBundle = new AssetBundleBuild();
            //文件路径标准化
            string filename = PathUtil.GetStandardPath(files[i]);
            Debug.Log("file:" + filename);
            //获取Unity的相对路径
            string assetName = PathUtil.GetUnityPath(filename);
            assetBundle.assetNames = new string[] { assetName };

            //获取需要打bundle的文件名 并设置为bundle文件
            string bundleName = filename.Replace(PathUtil.BuildResourcesPath, "").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            assetBundleBuilds.Add(assetBundle);
        }
        if (Directory.Exists(PathUtil.BuildOutPath))
            Directory.Delete(PathUtil.BuildOutPath, true);
        Directory.CreateDirectory(PathUtil.BuildOutPath);

        BuildPipeline.BuildAssetBundles(PathUtil.BuildOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
    }

}
