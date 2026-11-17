using TMPro;
using UnityEngine;


namespace UNodes
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _key; public string Key => _key;
        private TextMeshProUGUI _text;

        private void OnEnable()
        {
            Localization.OnLanguageChanged += Display;
            Display();
        }

        private void OnDisable()
        {
            Localization.OnLanguageChanged -= Display;
        }


        private void Display()
        {
            if (_key == string.Empty)
            {
                Debug.LogWarning($"[Localized Text] ID is empty! Object \"{name}\"");
                return;
            }

            _text ??= GetComponent<TextMeshProUGUI>();
            _text.text = Localization.GetLocalizedText(_key);
        }

        public void SetKey(string key)
        {
            _key = key;
            Display();
        }
        
    }
}



