using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class CloudHandler : MonoBehaviour, IObjectRecoEventHandler
{
    public Text message, message2, cloudStatus;

    public ImageTargetBehaviour ImageTargetTemplate;

    CloudRecoBehaviour cloudRecoBehaviour;
    ObjectTracker objectTracker;
    TargetFinder targetFinder;

    public WebRequestHandler webRequestHandler;

    // Start is called before the first frame update
    void Start()
    {
        cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if(cloudRecoBehaviour)
        {
            cloudRecoBehaviour.RegisterEventHandler(this);
        }
    }

    public void OnInitialized(TargetFinder targetFinder)
    {
        message.text = "Cloud Reco Initialized Success!";

        objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        this.targetFinder = targetFinder;
    }

    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnStateChanged(bool scanning)
    {
        message.text = "Scanning Status: " + scanning;
        if(scanning)
        {
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
        }
    }

    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        message.text = "Target Search Result: " + targetSearchResult.TargetName;
        TargetFinder.CloudRecoSearchResult cloudRecoResult = (TargetFinder.CloudRecoSearchResult)targetSearchResult;

        // Build augmentation based on target 
        if (ImageTargetTemplate)
        {
            // enable the new result with the same ImageTargetBehaviour: 
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
            tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
        }

        if (cloudRecoResult.MetaData == null)
        {
            message2.text = "No Meta Data";
        }
        else
        {
            message2.text = "MetaData: " + cloudRecoResult.MetaData +
                "\nTargetName: " + cloudRecoResult.TargetName;
            webRequestHandler.RequestAssetBundle(cloudRecoResult.MetaData); 
        }
    }
}
