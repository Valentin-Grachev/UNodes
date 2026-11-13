using System;
using UnityEngine;
using UNodes.Data;

namespace UNodes.Providers.Default
{
    public class DefaultEnvProvider : Env.Provider
    {
        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();


        public override EnvKey EnvKey => EnvKey.Default;
        public override string PlatformName => "Default";
        public override bool IsMobile => false;
        public override bool IsDesktop => true;
        public override SystemLanguage Language => SystemLanguage.English;
        public override DateTime ServerTimeNowUtc => DateTime.UtcNow;
        
    }
}

