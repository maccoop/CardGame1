using com.sun.ad;
using GoogleMobileAds.Api;
using System.Collections.Generic;

public partial class AdService
{
    AdPlacement lastBanner = -1;
    public void ShowBanner(AdPlacement idPlacement)
    {
        lastBanner = idPlacement;
        if (_banners[idPlacement] == null)
        {
            var setting = _defaultSetting[idPlacement];
            _banners[idPlacement] = new BannerAd(setting.adSize, setting.adPosition, idPlacement == AdPlacement.BannerCollapsible);
            _banners[idPlacement].Init(idPlacement, BannerKey);
        }
        HideAllAd();
        _banners[idPlacement].Show(idPlacement);
    }

    internal void HideBanner()
    {
        foreach(var e in _banners)
        {
            e?.Hide();
        }
        lastBanner = -1;
    }

    internal void ShowLastBanner()
    {
        if(lastBanner != -1)
        {
            ShowBanner(lastBanner);
        }
    }

    private static Dictionary<AdPlacement, (AdSize adSize, AdPosition adPosition)> _defaultSetting = new Dictionary<AdPlacement, (AdSize, AdPosition)>
    {
        {AdPlacement.Banner, (AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom)},
        {AdPlacement.BannerTop, (AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Top)},
        {AdPlacement.BannerBottom, (AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom)},
        {AdPlacement.BannerCollapsible, (AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom)},
    };
}
