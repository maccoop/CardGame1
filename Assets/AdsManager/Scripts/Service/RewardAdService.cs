using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static com.sun.ad.AdObject;

public partial class AdService
{
    public void ShowRewardAd(int idPlacement)
    {
        HideAllAd();
        _reward.Show(idPlacement);
    }

    public void RegisterRewardComplete(int idPlacement, Action<bool, string> OnComplete)
    {
        _reward.OnShowed += (id, type, @object) =>
        {
            if (idPlacement != id)
            {
                return;
            }
            OnComplete.Invoke(true, "Sucess");
        };
        _reward.OnError += (id, Type, @object) =>
        {
            if (idPlacement != id)
            {
                return;
            }
            OnComplete.Invoke(false, @object as string);
        };
    }
}
