using System;
using Cysharp.Threading.Tasks;

namespace UNodes.YandexGames
{
    public class YandexGamesAdsProvider : Ads.Provider
    {
        public override bool IsSupportRewarded => true;
        public override bool IsSupportInterstitial => true;
        public override bool IsAdBlock => false;

        public override async void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => YandexGamesSdk.IsInitialized);
            onInitialized?.Invoke();
        }

        public override void SetBanner(bool isActive)
        {
            if (isActive) YandexGamesSdk.ShowBanner();
            else YandexGamesSdk.HideBanner();
        }

        public override void ShowInterstitial(Action<bool> onHandled = null)
        {
            YandexGamesSdk.ShowInterstitial
                (onClosed: (wasShown) => onHandled?.Invoke(wasShown));
        }

        public override void ShowRewarded(Action<bool> onHandled = null)
        {
            bool wasRewarded = false;

            YandexGamesSdk.ShowRewarded(
                onRewarded: () => wasRewarded = true,
                onClosed: (wasShown) => onHandled?.Invoke(wasRewarded));
        }
    }
}
