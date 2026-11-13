using System;

namespace UNodes.Providers.Default
{
    public class DefaultAdsProvider : Ads.Provider
    {
        public override bool IsSupportRewarded => false;
        public override bool IsSupportInterstitial => false;
        public override bool IsAdBlock => false;

        public override void Initialize(Action onInitialized) 
            => onInitialized?.Invoke();

        public override void ShowInterstitial(Action<bool> onHandled = null)
            => onHandled?.Invoke(false);


        public override void ShowRewarded(Action<bool> onHandled = null)
            => onHandled?.Invoke(false);
    }
}

