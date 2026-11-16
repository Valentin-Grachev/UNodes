using System;
using Cysharp.Threading.Tasks;

namespace UNodes.YandexGames
{
    public class YandexGamesMarkingProvider : Marking.Provider
    {

        public async override void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => YandexGamesSdk.IsInitialized);
            onInitialized?.Invoke();
        }

        public override void GameplayReady() => YandexGamesSdk.GameReady();

        public override void GameplayStart() => YandexGamesSdk.GameplayStart();

        public override void GameplayPause() => YandexGamesSdk.GameplayStop();
    }
}
