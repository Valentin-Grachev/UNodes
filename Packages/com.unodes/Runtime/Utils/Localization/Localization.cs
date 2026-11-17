using System;
using System.Collections.Generic;
using UnityEngine;
using UNodes.Extensions;


namespace UNodes
{
    public enum Language { English = 0, Russian = 1 }

    public static class Localization
    {
        public static event Action OnLanguageChanged;
        public static Language Language { get; private set; }
        public static IReadOnlyList<Language> SupportedLanguages => _config.SupportedLanguages;

        private static LocalizationConfig _config;
        private static Dictionary<string, Translation> _translations = new();

        private const string LANGUAGE_SAVE_KEY = "lang";


        public static void Initialize(Language deviceLanguage)
        {
            _config = Resources.Load<LocalizationConfig>("Localization");

            if (PlayerPrefs.HasKey(LANGUAGE_SAVE_KEY))
            {
                var language = PlayerPrefs.GetString(LANGUAGE_SAVE_KEY).ToEnum<Language>();
                SetLanguage(language);
            }    
            else SetLanguage(deviceLanguage);
        }

        private static void SetLanguage(Language language)
        {
            var languageIsSupported = _config.SupportedLanguages.Contains(language);
            Language = languageIsSupported ? language : _config.DefaultLanguage;
        }

        public static void ChangeLanguage(Language language)
        {
            if (!_config.SupportedLanguages.Contains(language))
                throw new Exception($"[Localization] Language {language} is not supported!");

            SetLanguage(language);
            PlayerPrefs.SetString(LANGUAGE_SAVE_KEY, language.ToString());

            OnLanguageChanged?.Invoke();
        }


        public static string GetLocalizedText(string key)
        {
            var language = Language;

            if (!_translations.ContainsKey(key))
            {
                int index = _config.Translations
                    .FindIndex(translation => translation.key == key);

                if (index < 0)
                {
                    throw new Exception($"[Localization] " +
                        $"Translation with key \"{key}\" doesn't exist!");
                }
                    
                var translation = _config.Translations[index];
                _translations.Add(key, translation);
            }

            int valueIndex = _translations[key].values.FindIndex
                (languageValue => languageValue.language == language);

            if (valueIndex < 0)
            {
                throw new Exception($"[Localization] Translation with key \"{key}\" " +
                    $"doesn't contain value for language \"{language}\"!");
            }
                
            return _translations[key].values[valueIndex].value;
        }

    }


}


