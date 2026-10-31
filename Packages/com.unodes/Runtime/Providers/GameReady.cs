using System;


namespace UNodes
{
    public static class GameReady
    {

        public abstract class Provider : CommonProvider
        {
            public abstract void GameplayReady();
            public abstract void GameplayStart();
            public abstract void GameplayPause();

        }


        private static Provider _provider;

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;

        public static void GameplayReady() => _provider.GameplayReady();
        public static void GameplayStart() => _provider.GameplayStart();
        public static void GameplayPause() => _provider.GameplayPause();


    }

}


