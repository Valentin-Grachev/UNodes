using System;
using Cysharp.Threading.Tasks;

namespace UNodes.GamePush
{
    public class GamePushSocialsProvider : Socials.Provider
    {
        private bool _canReview;


        public override bool IsSupportInvite => false;

        public override bool IsRateGameAvailable => _canReview;

        public override bool IsSupportOpenUrl => false;

        public async override void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => YandexGamesSdk.IsInitialized);

            YandexGamesSdk.RequestCanReview((canReview) =>
            {
                _canReview = canReview;
                onInitialized?.Invoke();
            });
        }

        public override void Invite() { }

        public override void OpenUrl(string url) { }

        public override void RateGame(Action<bool> onGameWasRated) 
            => YandexGamesSdk.RequestReview(onGameWasRated);
    }
}
