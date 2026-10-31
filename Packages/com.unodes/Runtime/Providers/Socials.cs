using System;

namespace UNodes
{
    public static class Socials
    {
        public abstract class Provider : CommonProvider
        {
            public abstract bool IsSupportInvite { get; }
            public abstract bool IsSupportRateGame { get; }
            public abstract bool IsSupportOpenUrl { get; }

            public abstract void Invite();
            public abstract void RateGame();
            public abstract void OpenUrl(string url);
        }

        private static Provider _provider;

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;
        public static bool IsSupportInvite => _provider.IsSupportInvite;
        public static bool IsSupportRateGame => _provider.IsSupportRateGame;

        public static void Invite() => _provider.Invite();
        public static void RateGame() => _provider.RateGame();
        public static void OpenUrl(string url) => _provider.OpenUrl(url);


    }
}



