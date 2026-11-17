using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UNodes.Data;

namespace UNodes.YandexGames
{
    public class YandexGamesEnvProvider : Env.Provider
    {
        private Language _deviceLanguage;
        private bool _isDesktop;
        private bool _gameIsPaused = false;


        public override EnvKey EnvKey => EnvKey.YandexGames;

        public override string PlatformName => "YandexGames";

        public override bool IsMobile => !_isDesktop;

        public override bool IsDesktop => _isDesktop;

        public override Language DeviceLanguage => _deviceLanguage; 

        public override DateTime GetServerTime() 
            => YandexGamesSdk.GetServerTime();


        public override async void Initialize(Action onInitialized)
        {
            var sdkPrefab = Resources.Load<YandexGamesSdk>("YandexGamesSDK");
            var sdkObject = Instantiate(sdkPrefab, parent: null);
            sdkObject.name = "YandexGamesSDK";
            DontDestroyOnLoad(sdkObject);

            await UniTask.WaitUntil(() => YandexGamesSdk.IsInitialized);

            YandexGamesSdk.OnGameApiPauseChanged += () => SetPause(YandexGamesSdk.IsPaused);
            if (YandexGamesSdk.IsPaused) SetPause(true);


            _deviceLanguage = ConvertToLanguage(YandexGamesSdk.GetLanguageCode());
            _isDesktop = YandexGamesSdk.GetDeviceType() == "desktop";

            onInitialized?.Invoke();
        }

        private void SetPause(bool isPaused)
        {
            if (isPaused == _gameIsPaused) return;

            if (isPaused) GameTime.AddPause();
            else GameTime.RemovePause();

            AudioListener.pause = YandexGamesSdk.IsPaused;
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
