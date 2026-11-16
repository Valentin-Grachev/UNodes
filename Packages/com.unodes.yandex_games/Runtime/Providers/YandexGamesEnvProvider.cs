using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UNodes.Data;

namespace UNodes.YandexGames
{
    public class YandexGamesEnvProvider : Env.Provider
    {
        private SystemLanguage _language;
        private bool _isDesktop;
        private bool _gameIsPaused = false;


        public override EnvKey EnvKey => EnvKey.YandexGames;

        public override string PlatformName => "YandexGames";

        public override bool IsMobile => !_isDesktop;

        public override bool IsDesktop => _isDesktop;

        public override SystemLanguage Language => _language; 

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


            _language = ConvertToLanguage(YandexGamesSdk.GetLanguageCode());
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


        private SystemLanguage ConvertToLanguage(string code)
        {
            return code?.ToLowerInvariant() switch
            {
                "af" => SystemLanguage.Afrikaans,
                "ar" => SystemLanguage.Arabic,
                "eu" => SystemLanguage.Basque,
                "be" => SystemLanguage.Belarusian,
                "bg" => SystemLanguage.Bulgarian,
                "ca" => SystemLanguage.Catalan,
                "zh" => SystemLanguage.Chinese,
                "zh-cn" => SystemLanguage.ChineseSimplified,
                "zh-tw" => SystemLanguage.ChineseTraditional,
                "cs" => SystemLanguage.Czech,
                "da" => SystemLanguage.Danish,
                "nl" => SystemLanguage.Dutch,
                "en" => SystemLanguage.English,
                "et" => SystemLanguage.Estonian,
                "fo" => SystemLanguage.Faroese,
                "fi" => SystemLanguage.Finnish,
                "fr" => SystemLanguage.French,
                "de" => SystemLanguage.German,
                "el" => SystemLanguage.Greek,
                "he" => SystemLanguage.Hebrew,
                "hu" => SystemLanguage.Hungarian,
                "is" => SystemLanguage.Icelandic,
                "id" => SystemLanguage.Indonesian,
                "it" => SystemLanguage.Italian,
                "ja" => SystemLanguage.Japanese,
                "ko" => SystemLanguage.Korean,
                "lv" => SystemLanguage.Latvian,
                "lt" => SystemLanguage.Lithuanian,
                "no" => SystemLanguage.Norwegian,
                "pl" => SystemLanguage.Polish,
                "pt" => SystemLanguage.Portuguese,
                "ro" => SystemLanguage.Romanian,
                "ru" => SystemLanguage.Russian,
                "sh" or "sr" => SystemLanguage.SerboCroatian,
                "sk" => SystemLanguage.Slovak,
                "sl" => SystemLanguage.Slovenian,
                "es" => SystemLanguage.Spanish,
                "sv" => SystemLanguage.Swedish,
                "th" => SystemLanguage.Thai,
                "tr" => SystemLanguage.Turkish,
                "uk" => SystemLanguage.Ukrainian,
                "vi" => SystemLanguage.Vietnamese,
                _ => SystemLanguage.Unknown
            };
        }

        
    }
}
