using System;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;

namespace UNodes.GamePush
{
    public class GamePushSocialsProvider : Socials.Provider
    {
        private bool _isSupportInvite;
        private bool _isSupportOpenUrl;

        public override bool IsSupportInvite => _isSupportInvite;

        public override bool IsRateGameAvailable
            => GP_App.CanReview() && !GP_App.IsAlreadyReviewed();

        public override bool IsSupportOpenUrl => _isSupportOpenUrl;

        public async override void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);

            _isSupportInvite = GP_Socials.IsSupportsNativeInvite();
            _isSupportOpenUrl = GP_Platform.IsExternalLinksAllowed();

            onInitialized?.Invoke();
        }

        public override void Invite()
        {
            if (!_isSupportInvite) return;
            GP_Socials.Invite();
        }

        public override void OpenUrl(string url)
        {
            if (!_isSupportOpenUrl || string.IsNullOrEmpty(url)) return;
            Application.OpenURL(url);
        }

        public override void RateGame(Action<bool> onGameWasRated)
        {
            bool handled = false;

            GP_App.ReviewRequest(
                onReviewResult: rating => Handle(rating > 0),
                onReviewClose: error => Handle(false));

            void Handle(bool wasRated)
            {
                if (handled) return;
                handled = true;
                onGameWasRated?.Invoke(wasRated);
            }
        }
    }
}
