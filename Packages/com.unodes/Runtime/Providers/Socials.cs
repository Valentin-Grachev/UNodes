using System;
using UnityEngine;
using UNodes.Data;

namespace UNodes
{
    public static class Socials
    {
        public abstract class Provider : CommonProvider
        {
            public abstract bool IsSupportInvite { get; }
            public abstract bool IsRateGameAvailable { get; }
            public abstract bool IsSupportOpenUrl { get; }

            public abstract void Invite();
            public abstract void RateGame(Action<bool> onGameWasRated);
            public abstract void OpenUrl(string url);
        }

        public static event Action<bool> OnGameRateHandled;

        private static Provider _provider;

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsSupportInvite => _provider.IsSupportInvite;
        public static bool IsRateGameAvailable => _provider.IsRateGameAvailable && !SaveData.GameWasRated;
        public static bool IsSupportOpenUrl => _provider.IsSupportOpenUrl;


        public static void Invite() => _provider.Invite();
        public static void RateGame(Action<bool> onHandled)
        {
            if (SaveData.GameWasRated)
            {
                Debug.LogError("[Socials] Game was rated already!");
                InvokeHandledEvent(false);
                return;
            }

            if (!IsRateGameAvailable)
            {
                Debug.LogError("[Socials] Rate game is not supported!");
                InvokeHandledEvent(false);
                return;
            }

            _provider.RateGame((gameWasRated) =>
            {
                if (gameWasRated)
                {
                    SaveData.GameWasRated = true;
                    Saves.Commit();
                }

                InvokeHandledEvent(gameWasRated);
            });

            void InvokeHandledEvent(bool gameWasRated)
            {
                onHandled?.Invoke(gameWasRated);
                OnGameRateHandled?.Invoke(gameWasRated);
            }
        }
        public static void OpenUrl(string url) => _provider.OpenUrl(url);


    }
}



