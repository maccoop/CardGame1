using GoogleMobileAds.Api;
using UnityEngine;

namespace com.sun.ad
{
    [System.Serializable]
    public class OpenAd : AdImplement
    {
        string _adKey;
        AppOpenAd _openAd;
        bool _forceShow;
        float timeNextAd = 0;
        const float timeDelay = 10;

        public override void Hide()
        {
            _openAd?.Destroy();
            _openAd = null;
        }

        public override void Destroy()
        {
            _openAd?.Destroy();
            _openAd = null;
            Load(AdPlacement);
        }

        public override void Load(int idPlacement)
        {
            if (_isLoading)
                return;
            base.Load(idPlacement);
            AdRequest request = new();
            AppOpenAd.Load(_adKey, request, OnLoadedCallBack);
        }

        private void OnLoadedCallBack(AppOpenAd ad, LoadAdError error)
        {
            _isLoading = false;
            if (error != null || ad == null)
            {
                this.OnError?.Invoke(AdPlacement, Type, error.GetMessage());
                return;
            }
            this._openAd = ad;
            this.OnLoaded?.Invoke(AdPlacement, Type, null);
            ListenerEvent();
            if (_forceShow)
                Show(AdPlacement);
            _forceShow = false;

            void ListenerEvent()
            {
                this._openAd.OnAdClicked +=
                    () => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnClicked?.Invoke(AdPlacement, Type, null));
                this._openAd.OnAdPaid +=
                    (value) => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnImpress?.Invoke(AdPlacement, Type, value));
                this._openAd.OnAdFullScreenContentClosed +=
                    () => ThreadDispatcher.ExecuteOnMainThread(
                        () =>
                        {
                            this.OnShowed?.Invoke(AdPlacement, Type, null); 
                            _forceShow = false;
                            Destroy();
                        });
            }
        }

        public override void Show(int idPlacement)
        {
            if (Time.time < timeNextAd)
                return;
            _forceShow = true;
            if (_isLoading)
                return;
            if(_openAd == null)
            {
                Load(idPlacement);
                return;
            }
            base.Show(idPlacement);
            _openAd.Show();
            timeNextAd = Time.time + timeDelay;
        }

        public override void Init(int idPlacement, string adKey)
        {
            base.Init(idPlacement, adKey);
            this._adKey = adKey;
            this.Type = AdObject.AdType.AppOpen;
            _forceShow = true;
            Load(AdPlacement);
        }
    }
}
