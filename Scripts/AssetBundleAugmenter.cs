using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Vuforia;

public class AssetBundleAugmenter : MonoBehaviour//, ITrackableEventHandler
{
    public Text augmentStatus;
    private GameObject currTarget;

    
    //public string AssetName;
    //public int Version;
    private TrackableBehaviour mTrackableBehaviour;
    //private bool mAttached = false;
    void Start()
    {
        /*mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }*/
    }

    private void Update()
    {
        if(currTarget)
        {
            augmentStatus.text = currTarget.name + "\nChild Count: " + transform.childCount + "\nChild[0] Position: " + transform.GetChild(0).localPosition; 
        }
        else
        {
            augmentStatus.text = "N/A!!!";
        }
    }

    /*public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            if (!mAttached && mBundleInstance)
            {
                // if bundle has been loaded, let's attach it to this trackable
                mBundleInstance.transform.parent = this.transform;
                mBundleInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                mBundleInstance.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
                mBundleInstance.transform.gameObject.SetActive(true);
                mAttached = true;
            }
        }
    }*/

    public void SetObject(GameObject augmentTarget)
    {
        if(currTarget != null)
        {
            Destroy(currTarget);
        }

        if (augmentTarget != null)//!mAttached && 
        {
            currTarget = Instantiate(augmentTarget);
            currTarget.transform.parent = this.transform;
            currTarget.transform.localScale = new Vector3(1f, 1f, 1f);
            currTarget.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            currTarget.SetActive(true);
            currTarget.GetComponent<MeshRenderer>().enabled = true;
            //currTarget.GetComponent<Collider2D>
            //mAttached = true;
        }
    }
}
