using System;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;
using UNodes.Data;

namespace UNodes.GamePush
{
    public class GamePushEnvProvider : Env.Provider
    {
        private Language _deviceLanguage;
        private bool _isDesktop;
        private bool _gameIsPaused = false;


        public override EnvKey EnvKey => EnvKey.GamePush;

        public override string PlatformName => "GamePush";

        public override bool IsMobile => !_isDesktop;

        public override bool IsDesktop => _isDesktop;

        public override Language DeviceLanguage => _deviceLanguage;

        public override DateTime GetServerTime()
            => GP_Server.Time();


        public override async void Initialize(Action onInitialized)
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);

            GP_Game.OnPause += OnGamePause;
            GP_Game.OnResume += OnGameResume;
            if (GP_Game.IsPaused()) SetPause(true);

            _deviceLanguage = ConvertToLanguage(GP_Language.CurrentISO());
            _isDesktop = GP_Device.IsDesktop();

            onInitialized?.Invoke();
        }

        private void OnDestroy()
        {
            GP_Game.OnPause -= OnGamePause;
            GP_Game.OnResume -= OnGameResume;
        }

        private void OnGamePause() => SetPause(true);

        private void OnGameResume() => SetPause(false);

        private void SetPause(bool isPaused)
        {
            if (isPaused == _gameIsPaused) return;

            if (isPaused) GameTime.AddPause();
            else GameTime.RemovePause();

            AudioListener.pause = isPaused;
            _gameIsPaused = isPaused;
        }


        private Language ConvertToLanguage(string code)
        {
            return code?.ToLowerInvariant() switch
            {
                "ru" => Language.Russian,
                "en" => Language.English,

                _ => Language.English
            };
        }
    }
}
