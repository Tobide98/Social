using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.Events;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    string iosGameID = "4136340";
    string androidGameID = "4116007";
    string GameID = "";
    string interstitialAds = "video";
    string rewardAds = "rewardedVideo";
    public UnityEvent onAdsFinished;


    void Start()
    {
#if UNITY_IOS
        GameID = iosGameID;
#else
        GameID = androidGameID;
#endif


        Advertisement.AddListener(this);
        Advertisement.Initialize(GameID, true);
    }


    private bool WatchAdId(string id)
    {
        if (Advertisement.IsReady(id))
        {
            Advertisement.Show(id);
            return true;
        }
        else
        {
            Debug.Log("Ads not ready in watchad");
            onAdsFinished.RemoveAllListeners();
            return false;
        }
        return false;
    }

    public bool IsInterstitialReady()
    {
        return Advertisement.IsReady(interstitialAds);
    }

    public bool IsRewardVideoReady()
    {
        return (Advertisement.IsReady(rewardAds));
    }


    public bool WatchInterstitial(Action callback)
    {
        onAdsFinished.AddListener(() => { callback?.Invoke(); });
        return WatchAdId(interstitialAds);
    }

    public bool WatchRewardVideo(Action callback)
    {
        onAdsFinished.AddListener(() => { callback?.Invoke(); });
        return WatchAdId(rewardAds);
    }

    public void WatchVideoAds()
    {
        WatchAdId(interstitialAds);
    }

    public void OnUnityAdsDidError(string message)
    {
        //var modal = ModalManager.Instance.AttentionOneButton(() => ModalManager.Instance.CloseModal(), () => { });
        //modal.title.text = "Voucher Diskon";
        //modal.buttonPrompts[0].text = "OK";
        // modal.messages[0].text = "Sedang tidak ada ads, coba beberapa saat lagi.";
        //Debug.LogWarning("ADS DID ERROR : " + message);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:

        Debug.Log("Unity ad finished");
        if (showResult == ShowResult.Finished)
        {           
            Debug.Log("Show Result Finished");
        }
        else if (showResult == ShowResult.Skipped)
        {
            Debug.Log("Skipped");            
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }

        if(showResult!= ShowResult.Failed)
        {
            onAdsFinished.Invoke();            
        }
        onAdsFinished.RemoveAllListeners();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //throw new System.NotImplementedException();
        Debug.Log("Ads did start");
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }
}