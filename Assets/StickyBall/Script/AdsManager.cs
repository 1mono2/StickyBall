using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Placement;
using GoogleMobileAds.Common;
using Facebook.Unity;
using MyUtility;

public class AdsManager : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_IOS
        int status = ATTUtili.GetTrackingAuthorizationStatus();
        Debug.Log("ATT???? = " + status);
        // ATT??????4??
        // ATTrackingManagerAuthorizationStatusNotDetermined = 0
        // ATTrackingManagerAuthorizationStatusRestricted    = 1
        // ATTrackingManagerAuthorizationStatusDenied        = 2
        // ATTrackingManagerAuthorizationStatusAuthorized    = 3
        if (status == 0)
        {
            // ATT???????\ & ?????F??????ATT???F?v???A???[?g???\??
            ATTUtili.RequestTrackingAuthorization(CallbackFunction);
        }
        else
        {
            if (status == 1 || status == 2)
            {
                // ATT?????s?????????AATT???F???K?v???????|?????[?U?[???`????
                
            }
            else if(status == 3)
            {
                FB.Init(this.OnInitComplete, this.OnHideUnity);
            }
            // Google Mobile Ads SDK ????????
            MobileAds.Initialize(initStatus =>
            {
                // AdMob???????R?[???o?b?N?????C???X???b?h???????o?????????????????????A????Update()????????????????MobileAdsEventExecutor???g?p
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    // ?o?i?[?????N?G?X?g
                    ShowAds();
                });
            });
        }

#elif UNITY_ANDROID
        MobileAds.Initialize(initStatus =>
        {
            // AdMob???????R?[???o?b?N?????C???X???b?h???????o?????????????????????A????Update()????????????????MobileAdsEventExecutor???g?p
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                // ?o?i?[?????N?G?X?g
                ShowAds();
            });
        });
#endif
    }
    private void OnHideUnity(bool isUnityShown)
    {
        throw new NotImplementedException();
    }

    private void OnInitComplete()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
                FB.Mobile.SetAdvertiserTrackingEnabled(true);
            });
        }
    }

    void CallbackFunction(int status)
    {
        Debug.Log("ATT???V???? --> " + status);
        // ATT?????????????????? Google Mobile Ads SDK ????????
        MobileAds.Initialize(initStatus => {
            // AdMob???????R?[???o?b?N?????C???X???b?h???????o?????????????????????A????Update()????????????????MobileAdsEventExecutor???g?p
            MobileAdsEventExecutor.ExecuteInUpdate(() => {
                ShowAds();
            });
        });

        // Facebook SDK
        if (status == 1 || status == 2)
        {
            // ATT?????s?????????AATT???F???K?v???????|?????[?U?[???`????

            FB.Init(this.OnInitComplete, this.OnHideUnity);
        }
        else if (status == 3)
        {

            FB.Init(this.OnInitComplete, this.OnHideUnity);
        }
    }

    void ShowAds()
    {
        // banner is shown.
        BannerAdGameObject bannerAd = MobileAds.Instance.GetAd<BannerAdGameObject>("BannerAd");
        bannerAd.LoadAd();
        bannerAd.Show();

    }

}
