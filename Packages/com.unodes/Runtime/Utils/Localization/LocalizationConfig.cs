using System.Collections.Generic;
using UnityEngine;


namespace UNodes
{
    [CreateAssetMenu(menuName = "UNodes/Localization", fileName = "Localization")]
    public class LocalizationConfig : ScriptableObject
    {
        [field: SerializeField] public Language DefaultLanguage { get; private set; }
        [field: SerializeField] public List<Language> SupportedLanguages { get; private set; }
        [field: SerializeField] public List<Translation> Translations { get; private set; }

    }
}



