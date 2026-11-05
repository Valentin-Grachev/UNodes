using System;
using UnityEngine;
using UNodes.Project;

namespace UNodes.Providers.Test
{
    public class MissingEnvProvider : Env.Provider
    {
        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();


        public override EnvKey EnvKey => EnvKey.Missing;
        public override string PlatformName => "Unknown";
        public override bool IsMobile => false;
        public override bool IsDesktop => true;
        public override SystemLanguage Language => SystemLanguage.English;
        public override DateTime ServerTimeNowUtc => DateTime.UtcNow;
        
    }
}

