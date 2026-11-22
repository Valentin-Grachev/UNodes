using System;
using Cysharp.Threading.Tasks;
using GamePush;

namespace UNodes.GamePush
{
    public class GamePushMarkingProvider : Marking.Provider
    {

        public async override void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);
            onInitialized?.Invoke();
        }

        public override void GameplayReady() => GP_Game.GameReady();

        public override void GameplayStart() => GP_Game.GameplayStart();

        public override void GameplayPause() => GP_Game.GameplayStop();
    }
}
