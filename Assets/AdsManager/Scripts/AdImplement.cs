using System;
using UnityEngine;
using static com.sun.ad.AdObject;

namespace com.sun.ad
{
    public abstract class AdImplement : AdObject
    {
        private AdObject.AdType _type;
        private AdEvent _onBeginLoad;
        private AdEvent _onLoaded;
        private AdEvent _onBeginShow;
        private AdEvent _onShowed;
        private AdEvent _onClicked;
        private AdEvent _onImpress;
        private AdEvent _onError;
        private AdPlacement _adPlacement;
        protected bool _isLoading;

        public AdPlacement AdPlacement => _adPlacement;
        public AdObject.AdType Type { get => _type; protected set => _type = value; }

        public AdEvent OnBeginLoad { get => _onBeginLoad; set => _onBeginLoad = value; }

        public AdEvent OnLoaded { get => _onLoaded; set => _onLoaded = value; }

        public AdEvent OnBeginShow { get => _onBeginShow; set => _onBeginShow = value; }

        public AdEvent OnShowed { get => _onShowed; set => _onShowed = value; }

        public AdEvent OnClicked { get => _onClicked; set => _onClicked = value; }

        public AdEvent OnImpress { get => _onImpress; set => _onImpress = value; }

        public AdEvent OnError { get => _onError; set => _onError = value; }

        public abstract void Hide();
        public abstract void Destroy();

        public virtual void Init(int idPlacement, string adKey)
        {
            _adPlacement = idPlacement;
            RegisterLogEvent();
        }

        private void RegisterLogEvent()
        {
            OnBeginLoad += (adPlacement, type, @object) => Debug.Log(adPlacement + " BEGIN LOAD");
            OnLoaded += (adPlacement, type, @object) => Debug.Log(adPlacement + " LOADED!");
            OnBeginShow += (adPlacement, type, @object) => Debug.Log(adPlacement + " BEGIN SHOW");
            OnShowed += (adPlacement, type, @object) => Debug.Log(adPlacement + " SHOWED!");
            OnClicked += (adPlacement, type, @object) => Debug.Log(adPlacement + " CLICKED");
            OnError += (adPlacement, type, @object) => Debug.LogError(adPlacement + " ONERROR with value" + @object);
            OnImpress += (adPlacement, type, @object) => Debug.Log(adPlacement + " INPAID with value: " + @object);
        }

        public virtual void Load(int idPlacement)
        {
            _adPlacement = idPlacement;
            OnBeginLoad?.Invoke(idPlacement, Type, null);
            _isLoading = true;
        }

        public virtual void Show(int idPlacement)
        {
            _adPlacement = idPlacement;
            OnBeginShow?.Invoke(idPlacement, Type, null);
        }
    }
}
