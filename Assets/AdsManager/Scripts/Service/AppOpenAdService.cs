using com.sun.ad;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AdService
{
    public void ShowOpenAd(int idPlacement)
    {
        CheckInit();
        this._appOpen?.Show(idPlacement);
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause && _isInit)
        {
            CheckInit();
            HideAllAd();
            this._appOpen.OnShowed += ShowLastBanner;
            ShowOpenAd(AdPlacement.AppResume);
        }
    }

    private void ShowLastBanner(int idPlacement, AdObject.AdType type, object @object)
    {
        CheckInit();
        ShowLastBanner();
        this._appOpen.OnShowed -= ShowLastBanner;
    }
}
