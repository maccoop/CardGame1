using UnityEngine.Events;
namespace com.sun.ad
{
    public interface AdObject
    {
        public delegate void AdEvent(int idPlacement, AdType type, object @object);
        [System.Serializable]
        public enum AdType
        {
            AppOpen, Banner, Interestitial, Rewarded
        }
        public AdPlacement AdPlacement { get; }
        public AdType Type { get; }
        public void Init(int idPlacement, string adKey);
        public void Load(int idPlacement);
        public void Show(int idPlacement);
        public void Hide();
        public void Destroy();
        public AdEvent OnBeginLoad { get; set; }
        public AdEvent OnLoaded { get; set; }
        public AdEvent OnBeginShow { get; set; }
        public AdEvent OnShowed { get; set; }
        public AdEvent OnClicked { get; set; }
        public AdEvent OnImpress { get; set; }
        public AdEvent OnError { get; set; }
    }
}
