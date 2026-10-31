using System;
using UnityEngine;
using UNodes.Project;

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
            public abstract SystemLanguage Language { get; }
        }

        private static Provider _provider;

        public static void InitializeProvider(Provider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;


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
        public static SystemLanguage Language => _provider.Language;

    }
}

