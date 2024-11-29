using com.sun.ad;
using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AdService : MonoBehaviour, Service
{
#if UNITY_ANDROID
    [SerializeField] string _bannerKeyAndroid, _intersKeyAndroid, _rewardKeyAndroid, _openAdKeyAndroid;
#elif UNITY_IOS
    [SerializeField] string _bannerKeyIOS, _intersKeyIOS, _rewardKeyIOS, _openAdKeyIOS;
#endif
    protected BannerAd[] _banners;
    protected IntersAd _interstitial;
    protected RewardAd _reward;
    protected OpenAd _appOpen;
    bool _isInit;

#if UNITY_ANDROID
    public string BannerKey => _bannerKeyAndroid;
    public string IntersKey => _intersKeyAndroid;
    public string RewardKey => _rewardKeyAndroid;
    public string OpenAdKey => _openAdKeyAndroid;
#elif UNITY_IOS
    public string BannerKey=> _bannerKeyIOS;
    public string IntersKey => _intersKeyIOS; 
    public string RewardKey => _rewardKeyIOS;
    public string OpenAdKey => _openAdKeyIOS;
#endif


    public void Init()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("Mobile ad init success!");

            _banners = new BannerAd[4];
            _interstitial = new IntersAd();
            _reward = new RewardAd();
            _appOpen = new OpenAd();
            _appOpen.Init(AdPlacement.AppOpen, OpenAdKey);
            _reward.Init(AdPlacement.Reward, RewardKey);
            _interstitial.Init(AdPlacement.Interstitial, IntersKey);
            _isInit = true;
        });
    }

    protected void CheckInit()
    {
        if (_isInit)
            throw new System.NotImplementedException("Mobile ad is not init success!");
    }

    protected void HideAllAd()
    {
        _appOpen.Hide();
        _interstitial.Hide();
        _reward.Hide();
        foreach (var e in _banners)
        {
            e?.Hide();
        }
    }
}
