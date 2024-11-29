
using GoogleMobileAds.Api;
using System.Drawing;
using UnityEngine.UIElements;

namespace com.sun.ad
{
    [System.Serializable]
    public class BannerAd : AdImplement
    {
        private string _adKey;
        private BannerView _view;
        private AdSize _size;
        private AdPosition _position;
        private bool _isCollapsible;
        private bool _isShow;

        public bool IsShow
        {
            get
            {
                if (_view == null)
                    return false;
                return _isShow;
            }
        }

        public override void Hide()
        {
            this._view?.Hide();
            _isShow = false;
        }

        public override void Destroy()
        {
            this._view?.Destroy();
            this._view = null;
        }

        public BannerAd(AdSize size, AdPosition position, bool isCollapsible)
        {
            this._size = size;
            this._position = position;
            Type = AdObject.AdType.Banner;
            this._isCollapsible = isCollapsible;
        }

        public override void Load(int idPlacement)
        {
            if (idPlacement != AdPlacement)
                return;
            if (_isLoading)
                return;
            if (_view != null)
                return;
            base.Load(idPlacement);
            this._view = new BannerView(_adKey, _size, _position);
            AdRequest adRequest = new AdRequest();
            if (_isCollapsible)
                adRequest.Extras.Add("collapsible", "bottom");
            ListenEvent();
            this._view.LoadAd(adRequest);

            void ListenEvent()
            {
                this._view.OnAdClicked +=
                                () => ThreadDispatcher.ExecuteOnMainThread(
                                    () => this.OnClicked?.Invoke(AdPlacement, Type, null));
                this._view.OnAdPaid +=
                    (value) => ThreadDispatcher.ExecuteOnMainThread(
                        () => this.OnImpress?.Invoke(AdPlacement, Type, value));
                this._view.OnBannerAdLoaded +=
                    () => ThreadDispatcher.ExecuteOnMainThread(
                        () =>
                        {
                            _isLoading = false;
                            this.OnLoaded?.Invoke(AdPlacement, Type, null);
                            Show(AdPlacement);
                        });
                this._view.OnBannerAdLoadFailed +=
                     (error) => ThreadDispatcher.ExecuteOnMainThread(
                          () =>
                          {
                              _isLoading = false;
                              this.OnError?.Invoke(AdPlacement, Type, error.GetMessage());
                          });
            }
        }

        public override void Show(int idPlacement)
        {
            if (this._view == null)
            {
                if (!_isLoading)
                    Load(AdPlacement);
            }
            else if (!_isShow)
            {
                base.Show(idPlacement);
                this._view.Show();
                _isShow = true;
            }
        }

        public override void Init(int idPlacement, string adKey)
        {
            base.Init(idPlacement, adKey);
            this._adKey = adKey;
            Load(AdPlacement);
        }
    }
}
