using System;

namespace UNodes.Providers.Missing
{
    public class MissingGameReadyProvider : GameReady.Provider
    {
        public override void GameplayReady() { }

        public override void GameplayStart() { }

        public override void GameplayPause() { }

        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();
    }
}

