using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class WebRequestHandler : MonoBehaviour
{
    public Text networkStatus;

    private AssetBundle assetBundle;
        
    private bool newAsset;

    void Start()
    {

    }

    public void RequestAssetBundle(string location)
    {
        newAsset = false;
        if(assetBundle)
        {
            assetBundle.Unload(true);
        }
        StartCoroutine(LoadAssetBundle(location));
    }

    IEnumerator LoadAssetBundle(string location)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(location);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            networkStatus.text = www.error;
            Debug.Log(www.error);
        }   
        else
        {
            assetBundle = DownloadHandlerAssetBundle.GetContent(www);
            if(assetBundle)
            {
                networkStatus.text = "Loaded from " + location;
                newAsset = true;
            }
            else
            {
                networkStatus.text = "Null from " + location;
            }
            newAsset = true;
        }
    }

    public AssetBundle GetAssetBundle()
    {
        newAsset = false;
        return assetBundle;
    }

    public bool GetNewAsset()
    {
        return newAsset;
    }
}