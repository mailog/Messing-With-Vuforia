using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CustomTrackingEventHandler : MonoBehaviour
{
    private enum TrackingStatusFilter
    {
        Tracked,
        Tracked_ExtendedTracked,
        Tracked_ExtendedTracked_Limited
    };
    private TrackingStatusFilter StatusFilter = TrackingStatusFilter.Tracked_ExtendedTracked_Limited;

    public WebRequestHandler webRequestHandler;
    public Text trackingStatus, childrenStatus;
    private bool callbackReceivedOnce, ready;

    private TrackableBehaviour trackableBehaviour;
    private TrackableBehaviour.Status prevStatus, currStatus;
    // Start is called before the first frame update
    void Start()
    {
        trackableBehaviour = GetComponent<TrackableBehaviour>();

        if (trackableBehaviour)
        {
            trackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(ready)
        {
            if(webRequestHandler.GetNewAsset())
            {
                LoadChildren(webRequestHandler.GetAssetBundle());
            }
        }
    }

    void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult statusChangeResult)
    {
        prevStatus = statusChangeResult.PreviousStatus;
        currStatus = statusChangeResult.NewStatus;

        HandleTrackableStatusChanged();
    }

    protected virtual void HandleTrackableStatusChanged()
    {
        //OnTrackingFound();

        if (!ShouldBeRendered(prevStatus) &&
            ShouldBeRendered(currStatus))
        {
            OnTrackingFound();
        }
        else if (ShouldBeRendered(prevStatus) &&
                 !ShouldBeRendered(currStatus))
        {
            OnTrackingLost();
        }
        else if(!ShouldBeRendered(currStatus))
        {
            if (!callbackReceivedOnce && !ShouldBeRendered(currStatus))
            {
                // This is the first time we are receiving this callback, and the target is not visible yet.
                // --> Hide the augmentation.
                OnTrackingLost();
            }
        }

        callbackReceivedOnce = true;
    }

    protected bool ShouldBeRendered(TrackableBehaviour.Status status)
    {
        if (status == TrackableBehaviour.Status.DETECTED ||
            status == TrackableBehaviour.Status.TRACKED)
        {
            // always render the augmentation when status is DETECTED or TRACKED, regardless of filter
            return true;
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                // also return true if the target is extended tracked
                return true;
            }
        }

        if (StatusFilter == TrackingStatusFilter.Tracked_ExtendedTracked_Limited)
        {
            if (status == TrackableBehaviour.Status.EXTENDED_TRACKED ||
                status == TrackableBehaviour.Status.LIMITED)
            {
                // in this mode, render the augmentation even if the target's tracking status is LIMITED.
                // this is mainly recommended for Anchors.
                return true;
            }
        }

        return false;
    }

    private void OnTrackingFound()
    {
        trackingStatus.text = "Tracking Found!!!";

        if (transform.childCount > 0)
        {
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in rends)
            {
                rend.enabled = true;
            }
        }
        ready = true;
    }

    private void OnTrackingLost()
    {
        trackingStatus.text = "Tracking Lost!!!";

        if(transform.childCount > 0)
        {
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            foreach(Renderer rend in rends)
            {
                rend.enabled = false;
            }
        }
    }

    private void ClearChildren()
    {
        if(transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void LoadChildren(AssetBundle assetBundle)
    {
        if(assetBundle != null)
        {
            ClearChildren();
            GameObject childObj = assetBundle.LoadAsset<GameObject>(assetBundle.GetAllAssetNames()[0]);
            GameObject newChild = Instantiate(childObj, transform.position, Quaternion.identity, transform);
            newChild.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newChild.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            newChild.GetComponent<Renderer>().enabled = true;
            newChild.GetComponent<Collider>().enabled = true;
            ready = false;
            childrenStatus.text = "Children Loaded: " + newChild.name;
        }
    }
}
