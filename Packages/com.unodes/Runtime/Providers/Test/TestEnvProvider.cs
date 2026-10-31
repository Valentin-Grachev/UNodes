using System;
using UnityEngine;
using UNodes.Project;

namespace UNodes.Providers.Test
{
    public class TestEnvProvider : Env.Provider
    {
        private enum DeviceType { Desktop, Mobile }


        [SerializeField] private string _platformName = "Test";
        [SerializeField] private DeviceType _deviceType;
        [SerializeField] private SystemLanguage _language = SystemLanguage.English;

        public override EnvKey EnvKey => EnvKey.Test;
        public override string PlatformName => _platformName;
        public override bool IsMobile => _deviceType == DeviceType.Mobile;
        public override bool IsDesktop => _deviceType == DeviceType.Desktop;
        public override SystemLanguage Language => _language;

        public override void Initialize(Action onInitialized)
            => onInitialized?.Invoke();
    }
}

