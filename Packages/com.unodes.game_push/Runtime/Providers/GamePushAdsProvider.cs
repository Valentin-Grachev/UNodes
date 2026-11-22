using System;
using Cysharp.Threading.Tasks;
using GamePush;

namespace UNodes.GamePush
{
    public class GamePushAdsProvider : Ads.Provider
    {
        public override bool IsSupportRewarded => GP_Ads.IsRewardedAvailable();
        public override bool IsSupportInterstitial => GP_Ads.IsFullscreenAvailable();
        public override bool IsAdBlock => GP_Ads.IsAdblockEnabled();

        public override async void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);
            onInitialized?.Invoke();
        }

        public override void SetBanner(bool isActive)
        {
            if (isActive) GP_Ads.ShowSticky();
            else GP_Ads.CloseSticky();
        }

        public override void ShowInterstitial(Action<bool> onHandled = null)
        {
            GP_Ads.ShowFullscreen(onFullscreenClose: 
                (success) => onHandled?.Invoke(success));
        }

        public override void ShowRewarded(Action<bool> onHandled = null)
        {
            bool wasRewarded = false;

            GP_Ads.ShowRewarded(
                onRewardedReward: (rewardName) => wasRewarded = true,
                onRewardedClose: (success) => onHandled?.Invoke(wasRewarded));
        }
    }
}
