using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgUI : MonoBehaviour 
{

	// Use this for initialization
	IEnumerator Start () 
	{
		//异步加载ab包
		AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(PathUtil.BuildOutPath + "/ui/prefabs/bgui.prefab.ab");
		yield return request;
		AssetBundleCreateRequest request1 = AssetBundle.LoadFromFileAsync(PathUtil.BuildOutPath + "/ui/resources/background.png.ab");
		yield return request1;
		AssetBundleCreateRequest request2 = AssetBundle.LoadFromFileAsync(PathUtil.BuildOutPath + "/ui/resources/button_150.png.ab");
		yield return request2;
		//加载文件
		AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync("Assets/BuildResources/UI/Prefabs/BgUI.prefab");
		yield return bundleRequest;
		//实例化组件
		GameObject go = Instantiate(bundleRequest.asset) as GameObject;
		go.transform.SetParent(this.transform);
		go.SetActive(true);
		go.transform.localPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
