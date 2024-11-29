using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sun.ad
{
    public class RewardAd : AdImplement
    {
        string _adKey;
        RewardedAd _rewardedAd;
        bool _forceShow;

        public override void Hide()
        {
        }

        public override void Destroy()
        {
            _rewardedAd?.Destroy();
            Load(this.AdPlacement);
        }

        public override void Load(int idPlacement)
        {
            base.Load(idPlacement);
            AdRequest request = new();
            RewardedAd.Load(_adKey, request, OnLoadedCallBack);
        }

        private void OnLoadedCallBack(RewardedAd ad, LoadAdError error)
        {
            _isLoading = false;
            if (error != null || ad == null)
            {
                this.OnError?.Invoke(AdPlacement, Type, error.GetMessage());
                return;
            }
            this._rewardedAd = ad;
            this.OnLoaded?.Invoke(AdPlacement, Type, null);
            ListenEvent();
            if(_forceShow)
            {
                this._rewardedAd.Show(OnCompleted);
            }
            _forceShow = false;

            void ListenEvent()
            {
                this._rewardedAd.OnAdClicked +=
                                () => ThreadDispatcher.ExecuteOnMainThread(
                                    () => this.OnClicked?.Invoke(AdPlacement, Type, null));
                this._rewardedAd.OnAdPaid +=
                    (value) => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnImpress?.Invoke(AdPlacement, Type, value));
                this._rewardedAd.OnAdFullScreenContentFailed +=
                    (value) => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnError?.Invoke(AdPlacement, Type, value.GetMessage()));
                this._rewardedAd.OnAdFullScreenContentClosed +=
                    () => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnError?.Invoke(AdPlacement, Type, "Client Closed!"));
            }
        }

        public override void Show(int idPlacement)
        {
            base.Show(idPlacement);
            _forceShow = true;
            if (_isLoading)
            {
                return;
            }
            _rewardedAd.Show(OnCompleted);
        }

        private void OnCompleted(Reward reward)
        {
            OnShowed?.Invoke(AdPlacement, Type, reward.Type);
            _forceShow = false;
            Load(AdPlacement);
        }

        public override void Init(int idPlacement, string adKey)
        {
            base.Init(idPlacement, adKey);
            this._adKey = adKey;
            this.Type = AdObject.AdType.Rewarded;
            Load(AdPlacement);
        }
    }
}
