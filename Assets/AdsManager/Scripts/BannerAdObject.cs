using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAdObject : MonoBehaviour
{
    [Range(0, 3)]
    public int adPlacement;
    AdService _adService;
    private void Start()
    {
        ServiceLocator.Resolve<AdService>(service =>
        {
            _adService = service as AdService;
            ShowBanner();
        });
    }

    [Button]
    public void ShowBanner()
    {
        _adService.ShowBanner(adPlacement);
    }

    [Button]
    public void HideBanner()
    {
        _adService.HideBanner();
    }

    [Button]
    public void ShowReward()
    {
        _adService.ShowRewardAd(AdPlacement.Reward);
    }
}
