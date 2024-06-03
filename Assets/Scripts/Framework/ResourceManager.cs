using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    internal class BundleInfo
    {
        public string AssetsName;
        public string BundleName;
        public List<string> Dependences;
    }
    //存放bundle信息的集合
    private Dictionary<string, BundleInfo> m_BundleInfos = new Dictionary<string, BundleInfo>();
    /// <summary>
    /// 解析版本文件
    /// </summary>
    private void ParseVersionFile()
    {
        //版本文件的路径
        string url = Path.Combine(PathUtil.BunleResouresPath, AppConst.FileListName);
        string[] data = File.ReadAllLines(url);
        //解析文件信息
        for(int i=0;i<data.Length;i++)
        {
            BundleInfo bundleInfo = new BundleInfo();
            string[] info = data[i].Split('|');
            bundleInfo.AssetsName = info[0];
            bundleInfo.BundleName = info[1];
            bundleInfo.Dependences = new List<string>(info.Length - 2);
            for(int j=2;j<info.Length;j++)
            {
                bundleInfo.Dependences.Add(info[j]);
            }
            m_BundleInfos.Add(bundleInfo.AssetsName, bundleInfo);
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetsName">资源名</param>
    /// <param name="action">完成回调</param>
    /// <returns></returns>
    IEnumerator LoadBundleAsync(string assetsName,Action<UnityEngine.Object> action = null)
    {
        string bundleName = m_BundleInfos[assetsName].BundleName;
        string bundlePath = Path.Combine(PathUtil.BunleResouresPath, bundleName);
        List<string> dependences = m_BundleInfos[assetsName].Dependences;
        if(dependences!=null&& dependences.Count>0)
        {
            for(int i=0;i<dependences.Count;i++)
            {
                yield return LoadBundleAsync(dependences[i]);
            }
        }
        //异步加载ab包
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return request;
        //加载文件
        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync(assetsName);
        yield return bundleRequest;

        if(action!=null&&bundleRequest!=null)
            action.Invoke(bundleRequest.asset);
    }

    public void LoadAssets(string assetsName,Action<UnityEngine.Object> action )
    {
        StartCoroutine(LoadBundleAsync(assetsName,action));
    }

    void Start()
    {
        ParseVersionFile();
        LoadAssets("Assets/BuildResources/UI/Prefabs/BgUI.prefab", OnComplete);
    }

    private void OnComplete(UnityEngine.Object obj)
    {
        GameObject go = Instantiate(obj) as GameObject;
        go.transform.SetParent(this.transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }
}
