using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UNodes
{
    [CustomPropertyDrawer(typeof(LanguageValue))]
    public class LanguageValueDrawer : PropertyDrawer
    {
        const float Spacing = 4f;
        const float LanguageWidth = 110f;
        const float HeightEpsilon = 1f;

        static readonly Dictionary<string, float> ValueWidths = new Dictionary<string, float>();
        static GUIStyle wrappedTextStyle;
        static bool repaintScheduled;

        static GUIStyle WrappedTextStyle
        {
            get
            {
                if (wrappedTextStyle == null)
                {
                    wrappedTextStyle = new GUIStyle(EditorStyles.textField)
                    {
                        wordWrap = true,
                        alignment = TextAnchor.UpperLeft,
                        fixedHeight = 0f,
                        stretchHeight = true
                    };
                }

                return wrappedTextStyle;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var languageProperty = property.FindPropertyRelative("language");
            var valueProperty = property.FindPropertyRelative("value");

            EditorGUI.BeginProperty(position, label, property);

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var labelLine = new Rect(position.x, position.y, position.width, lineHeight);
            var content = EditorGUI.PrefixLabel(labelLine, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            try
            {
                var languageWidth = Mathf.Min(LanguageWidth, Mathf.Max(0f, content.width * 0.4f));
                var languageRect = new Rect(content.x, content.y, languageWidth, lineHeight);
                var valueRect = new Rect(
                    content.x + languageWidth + Spacing,
                    content.y,
                    Mathf.Max(0f, content.width - languageWidth - Spacing),
                    Mathf.Max(lineHeight, position.height));

                EditorGUI.PropertyField(languageRect, languageProperty, GUIContent.none);
                DrawValue(property, valueProperty, valueRect);
            }
            finally
            {
                EditorGUI.indentLevel = indent;
                EditorGUI.showMixedValue = false;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var valueProperty = property.FindPropertyRelative("value");
            if (valueProperty == null)
                return EditorGUIUtility.singleLineHeight;

            ValueWidths.TryGetValue(CacheKey(property), out var width);
            if (width <= 1f)
                width = EstimateValueWidth(label);

            return CalcTextHeight(valueProperty.stringValue, width);
        }

        static void DrawValue(SerializedProperty property, SerializedProperty valueProperty, Rect valueRect)
        {
            if (valueRect.width > 1f)
            {
                var key = CacheKey(property);
                if (!ValueWidths.TryGetValue(key, out var cachedWidth) || Mathf.Abs(cachedWidth - valueRect.width) > HeightEpsilon)
                {
                    ValueWidths[key] = valueRect.width;
                    ScheduleInspectorRepaint();
                }
            }

            EditorGUI.showMixedValue = valueProperty.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            var text = EditorGUI.TextArea(valueRect, valueProperty.stringValue, WrappedTextStyle);
            if (!EditorGUI.EndChangeCheck())
                return;

            valueProperty.stringValue = text;
            if (Mathf.Abs(CalcTextHeight(text, valueRect.width) - valueRect.height) > HeightEpsilon)
                ScheduleInspectorRepaint();
        }

        static float CalcTextHeight(string text, float width)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            if (width <= 1f)
                return lineHeight;

            var measured = text ?? string.Empty;
            if (measured.EndsWith("\n"))
                measured += " ";

            var height = WrappedTextStyle.CalcHeight(new GUIContent(measured), width);
            return Mathf.Max(lineHeight, height);
        }

        static float EstimateValueWidth(GUIContent label)
        {
            var totalWidth = EditorGUIUtility.currentViewWidth;
            var labelHasContent = label != null && (!string.IsNullOrEmpty(label.text) || label.image != null);
            var contentWidth = labelHasContent
                ? totalWidth - EditorGUIUtility.labelWidth
                : totalWidth - EditorGUI.indentLevel * 15f;
            var languageWidth = Mathf.Min(LanguageWidth, Mathf.Max(0f, contentWidth * 0.4f));
            return Mathf.Max(1f, contentWidth - languageWidth - Spacing);
        }

        static string CacheKey(SerializedProperty property)
        {
            var target = property.serializedObject.targetObject;
            var id = target != null ? target.GetInstanceID() : 0;
            return id + ":" + property.propertyPath;
        }

        static void ScheduleInspectorRepaint()
        {
            if (repaintScheduled)
                return;

            repaintScheduled = true;
            EditorApplication.delayCall += () =>
            {
                repaintScheduled = false;
                var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
                foreach (var window in windows)
                {
                    if (window != null && window.GetType().Name == "InspectorWindow")
                        window.Repaint();
                }
            };
        }
    }
}
