using UnityEditor;
using UnityEngine;

namespace UNodes
{
    [CustomPropertyDrawer(typeof(LanguageValue))]
    public class LanguageValueDrawer : PropertyDrawer
    {
        const float Spacing = 4f;
        const float LanguageWidth = 110f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var languageProperty = property.FindPropertyRelative("language");
            var valueProperty = property.FindPropertyRelative("value");

            EditorGUI.BeginProperty(position, label, property);

            var line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var content = EditorGUI.PrefixLabel(line, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            try
            {
                var languageWidth = Mathf.Min(LanguageWidth, Mathf.Max(0f, content.width * 0.4f));
                var languageRect = new Rect(content.x, content.y, languageWidth, line.height);
                var valueRect = new Rect(
                    content.x + languageWidth + Spacing,
                    content.y,
                    Mathf.Max(0f, content.width - languageWidth - Spacing),
                    line.height);

                EditorGUI.PropertyField(languageRect, languageProperty, GUIContent.none);
                EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
            }
            finally
            {
                EditorGUI.indentLevel = indent;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
