using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace com.sun.ad
{
    public class IntersAd : AdImplement
    {
        string _adKey;
        InterstitialAd _interstitialAd;
        bool _forceShow;
        private float timeNextAd;
        private float timeDelay;

        public override void Hide()
        {

        }

        public override void Destroy()
        {
            _interstitialAd?.Destroy();
            Load(this.AdPlacement);
        }

        public override void Load(int idPlacement)
        {
            base.Load(idPlacement);
            if (_isLoading)
                return;
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }
            AdRequest request = new();
            InterstitialAd.Load(_adKey, request, OnLoadedCallBack);
        }

        private void OnLoadedCallBack(InterstitialAd ad, LoadAdError error)
        {
            _isLoading = false;
            if (error != null || ad == null)
            {
                this.OnError?.Invoke(AdPlacement, Type, error.GetMessage());
                return;
            }
            this._interstitialAd = ad;
            ListenerEvent();
            if (_forceShow)
                Show(AdPlacement);
            _forceShow = false;

            void ListenerEvent()
            {
                this.OnLoaded?.Invoke(AdPlacement, Type, null);
                this._interstitialAd.OnAdClicked +=
                    () => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnClicked?.Invoke(AdPlacement, Type, null));
                this._interstitialAd.OnAdPaid +=
                    (value) => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnImpress?.Invoke(AdPlacement, Type, value));
                this._interstitialAd.OnAdFullScreenContentClosed +=
                    () => ThreadDispatcher.ExecuteOnMainThread(
                        () =>
                        {
                            this.OnShowed?.Invoke(AdPlacement, Type, null);
                            Load(AdPlacement);
                        });
            }
        }



        public override void Show(int idPlacement)
        {
            if (Time.time < timeNextAd)
                return;
            base.Show(idPlacement);
            _forceShow = true;
            if (_isLoading)
            {
                return;
            }
            if (!_interstitialAd.CanShowAd())
            {
                Load(AdPlacement);
                return;
            }
            timeNextAd = Time.time + timeDelay;
            _interstitialAd.Show();
        }

        public override void Init(int idPlacement, string adKey)
        {
            base.Init(idPlacement, adKey);
            this._adKey = adKey;
            this.Type = AdObject.AdType.Interestitial;
            Load(AdPlacement);
        }
    }
}
