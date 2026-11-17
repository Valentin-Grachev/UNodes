using System;
using UnityEngine;
using UNodes.Data;

namespace UNodes
{
    public static class Env
    {
        public abstract class Provider : CommonProvider
        {
            public abstract EnvKey EnvKey { get; }
            public abstract string PlatformName { get; }
            public abstract bool IsMobile { get; }
            public abstract bool IsDesktop { get; }
            public abstract Language DeviceLanguage { get; }
            public abstract DateTime GetServerTime();
            public virtual float TimeScale 
            { 
                get => Time.timeScale;
                set => Time.timeScale = value;
            }
        }

        private static Provider _provider;
        private static DateTime _initializedServerTime;
        private static float _initializedUnscaledTime;

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                _initializedServerTime = _provider.GetServerTime();
                onInitialized?.Invoke();
            });


        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static EnvKey EnvKey => _provider.EnvKey;
        public static string PlatformName => _provider.PlatformName;
        public static bool IsMobile => _provider.IsMobile;
        public static bool IsDesktop => _provider.IsDesktop;
        public static Language DeviceLanguage => _provider.DeviceLanguage;
        public static DateTime TimeNowUtc => _initializedServerTime 
            + TimeSpan.FromSeconds(Time.unscaledTime - _initializedUnscaledTime);

        public static float TimeScale
        {
            get => _provider.TimeScale;
            set => _provider.TimeScale = value;
        }

    }
}

