using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleHandler : MonoBehaviour
{
    //public DefaultTrackableEventHandler imageTracking;

    public AssetBundle assetBundle;
    /*
     * Receives Asset Bundle file from Web Request Handler and Instantiates it where necessary
     */
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AssetBundle GetBundle()
    {
        return assetBundle;
    }

    public void SetBundle(AssetBundle assetBundle)
    {
        this.assetBundle = assetBundle;
    }

    /*public void LoadBundle(AssetBundle assetBundle)
    {c
        //imageTracking.SetChild(tmp);
    }*/
}
