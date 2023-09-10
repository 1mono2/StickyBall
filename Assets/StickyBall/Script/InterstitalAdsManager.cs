using System;
using UnityEngine;
using UnityEngine.Events;
using MyUtility;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;
using GoogleMobileAds.Common;

public class InterstitalAdsManager : SingletonMonoBehaviour<InterstitalAdsManager>
{

    protected override bool DontDestroy => true;

    InterstitialAd interstitialAds;

    [SerializeField] string AndroidAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField] string iOSAdUnitId = "ca-app-pub-3940256099942544/4411468910";


    [System.Serializable]
    public class HanderInterstitialAds : UnityEvent { }
    public HanderInterstitialAds OnAdLoaded;
    public HanderInterstitialAds OnAdFailedToLoad;
    public HanderInterstitialAds OnAdOpening;
    public HanderInterstitialAds OnAdClosed;
    public HanderInterstitialAds OnAdLeavingApplication;

    void Start()
    {
        // Don't worry ATT
        LoadInterstitialAds();
    }

    public void LoadInterstitialAds()
    {
#if UNITY_ANDROID
        string adUnitId = AndroidAdUnitId;
#elif UNITY_IPHONE
     string adUnitId = iOSAdUnitId;
#else
     string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitialAds = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitialAds.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAds.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAds.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAds.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitialAds.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialAds.LoadAd(request);
    }

    public void ShowInterstitialAds()
    {
        if (this.interstitialAds.IsLoaded())
        {
            this.interstitialAds.Show();
        }
    }


    void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
        OnAdLoaded.Invoke();
    }

    void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
        OnAdFailedToLoad.Invoke();
    }

    void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
        OnAdOpening.Invoke();
    }

    void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        OnAdClosed.Invoke();
        Destroy();
        LoadInterstitialAds();
    }

    void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
        OnAdLeavingApplication.Invoke();
    }

    void Destroy()
    {
        this.interstitialAds.Destroy();
    }


}
