using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UNodes
{
    [CustomPropertyDrawer(typeof(EnableValue<>))]
    public class EnableValueDrawer : PropertyDrawer
    {
        const float ToggleWidth = 16f;
        const float Spacing = 4f;

        static Gradient defaultGradient;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enabledProperty = property.FindPropertyRelative("enabled");
            var valueProperty = property.FindPropertyRelative("value");

            EditorGUI.BeginProperty(position, label, property);

            var lineHeight = EditorGUIUtility.singleLineHeight;
            var firstLine = new Rect(position.x, position.y, position.width, lineHeight);
            var content = EditorGUI.PrefixLabel(firstLine, GUIUtility.GetControlID(FocusType.Passive), label);
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            try
            {
                var toggleRect = new Rect(content.x, content.y, ToggleWidth, lineHeight);
                var fieldRect = new Rect(
                    content.x + ToggleWidth + Spacing,
                    content.y,
                    Mathf.Max(0f, content.width - ToggleWidth - Spacing),
                    lineHeight);

                if (valueProperty != null)
                    ResetUnlessDefault(valueProperty, enabledProperty.boolValue, enabledProperty.hasMultipleDifferentValues);

                EditorGUI.showMixedValue = enabledProperty.hasMultipleDifferentValues;
                EditorGUI.BeginChangeCheck();
                var enabled = EditorGUI.Toggle(toggleRect, enabledProperty.boolValue);
                if (EditorGUI.EndChangeCheck())
                {
                    enabledProperty.boolValue = enabled;
                    if (!enabled && valueProperty != null)
                        ResetProperty(valueProperty);
                }

                EditorGUI.showMixedValue = false;

                if (valueProperty != null && !enabledProperty.hasMultipleDifferentValues && enabledProperty.boolValue)
                {
                    fieldRect.height = EditorGUI.GetPropertyHeight(valueProperty, GUIContent.none, true);
                    EditorGUI.PropertyField(fieldRect, valueProperty, GUIContent.none, true);
                }
            }
            finally
            {
                EditorGUI.showMixedValue = false;
                EditorGUI.indentLevel = indent;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lineHeight = EditorGUIUtility.singleLineHeight;
            var enabledProperty = property.FindPropertyRelative("enabled");
            if (enabledProperty == null || enabledProperty.hasMultipleDifferentValues || !enabledProperty.boolValue)
                return lineHeight;

            var valueProperty = property.FindPropertyRelative("value");
            if (valueProperty == null)
                return lineHeight;

            return Mathf.Max(lineHeight, EditorGUI.GetPropertyHeight(valueProperty, GUIContent.none, true));
        }

        static void ResetUnlessDefault(SerializedProperty valueProperty, bool enabled, bool enabledIsMixed)
        {
            if (enabledIsMixed || enabled || IsDefault(valueProperty))
                return;

            ResetProperty(valueProperty);
        }

        static void ResetProperty(SerializedProperty property)
        {
            if (IsDefault(property))
                return;

            if (property.propertyType != SerializedPropertyType.String && property.isArray)
            {
                property.ClearArray();
                return;
            }

            if (property.isFixedBuffer)
            {
                for (var i = 0; i < property.fixedBufferSize; i++)
                    ResetProperty(property.GetFixedBufferElementAtIndex(i));
                return;
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.Character:
                case SerializedPropertyType.Enum:
                    if (property.type == "long")
                        property.longValue = 0L;
                    else
                        property.intValue = 0;
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = false;
                    break;
                case SerializedPropertyType.Float:
                    if (property.type == "double")
                        property.doubleValue = 0d;
                    else
                        property.floatValue = 0f;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = null;
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = default;
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = null;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = default;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = default;
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = default;
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = default;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    property.animationCurveValue = new AnimationCurve();
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = default;
                    break;
                case SerializedPropertyType.Quaternion:
                    property.quaternionValue = default;
                    break;
                case SerializedPropertyType.ExposedReference:
                    property.exposedReferenceValue = null;
                    break;
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = default;
                    break;
                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = default;
                    break;
                case SerializedPropertyType.RectInt:
                    property.rectIntValue = default;
                    break;
                case SerializedPropertyType.BoundsInt:
                    property.boundsIntValue = default;
                    break;
                case SerializedPropertyType.ManagedReference:
                    property.managedReferenceValue = null;
                    break;
                case SerializedPropertyType.Hash128:
                    property.hash128Value = default;
                    break;
                case SerializedPropertyType.Generic:
                    ResetChildren(property);
                    break;
            }
        }

        static bool IsDefault(SerializedProperty property)
        {
            if (property.hasMultipleDifferentValues)
                return false;

            if (property.propertyType != SerializedPropertyType.String && property.isArray)
                return property.arraySize == 0;

            if (property.isFixedBuffer)
            {
                for (var i = 0; i < property.fixedBufferSize; i++)
                {
                    if (!IsDefault(property.GetFixedBufferElementAtIndex(i)))
                        return false;
                }

                return true;
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.LayerMask:
                case SerializedPropertyType.ArraySize:
                case SerializedPropertyType.Character:
                case SerializedPropertyType.Enum:
                    return property.type == "long" ? property.longValue == 0L : property.intValue == 0;
                case SerializedPropertyType.Boolean:
                    return property.boolValue == false;
                case SerializedPropertyType.Float:
                    return property.type == "double" ? property.doubleValue == 0d : property.floatValue == 0f;
                case SerializedPropertyType.String:
                    return string.IsNullOrEmpty(property.stringValue);
                case SerializedPropertyType.Color:
                    return property.colorValue == default;
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue == null;
                case SerializedPropertyType.Vector2:
                    return property.vector2Value == default;
                case SerializedPropertyType.Vector3:
                    return property.vector3Value == default;
                case SerializedPropertyType.Vector4:
                    return property.vector4Value == default;
                case SerializedPropertyType.Rect:
                    return property.rectValue == default;
                case SerializedPropertyType.AnimationCurve:
                    var curve = property.animationCurveValue;
                    return curve == null || curve.length == 0;
                case SerializedPropertyType.Bounds:
                    return property.boundsValue == default;
                case SerializedPropertyType.Quaternion:
                    return property.quaternionValue == default;
                case SerializedPropertyType.ExposedReference:
                    return property.exposedReferenceValue == null;
                case SerializedPropertyType.Vector2Int:
                    return property.vector2IntValue == default;
                case SerializedPropertyType.Vector3Int:
                    return property.vector3IntValue == default;
                case SerializedPropertyType.BoundsInt:
                    return property.boundsIntValue == default;
                case SerializedPropertyType.ManagedReference:
                    return property.managedReferenceValue == null;
                case SerializedPropertyType.Hash128:
                    return property.hash128Value.Equals(default(Hash128));
                case SerializedPropertyType.Generic:
                    return ChildrenAreDefault(property);
                default:
                    return true;
            }
        }

        static void ResetChildren(SerializedProperty property)
        {
            var paths = new List<string>();
            ForEachChild(property, child => paths.Add(child.propertyPath));

            var serializedObject = property.serializedObject;
            foreach (var path in paths)
            {
                var child = serializedObject.FindProperty(path);
                if (child != null)
                    ResetProperty(child);
            }
        }

        static bool ChildrenAreDefault(SerializedProperty property)
        {
            var areDefault = true;
            ForEachChild(property, child =>
            {
                if (!IsDefault(child))
                    areDefault = false;
            });
            return areDefault;
        }

        static void ForEachChild(SerializedProperty property, System.Action<SerializedProperty> action)
        {
            var iterator = property.Copy();
            var end = iterator.GetEndProperty();
            var enterChildren = true;
            while (iterator.Next(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
            {
                enterChildren = false;
                action(iterator);
            }
        }

        static bool GradientIsDefault(Gradient gradient)
        {
            if (gradient == null)
                return true;

            defaultGradient ??= new Gradient();
            return gradient.mode == defaultGradient.mode
                && KeysMatch(gradient.colorKeys, defaultGradient.colorKeys)
                && KeysMatch(gradient.alphaKeys, defaultGradient.alphaKeys);
        }

        static bool KeysMatch(GradientColorKey[] keys, GradientColorKey[] defaults)
        {
            if (keys.Length != defaults.Length)
                return false;

            for (var i = 0; i < keys.Length; i++)
            {
                if (keys[i].color != defaults[i].color || keys[i].time != defaults[i].time)
                    return false;
            }

            return true;
        }

        static bool KeysMatch(GradientAlphaKey[] keys, GradientAlphaKey[] defaults)
        {
            if (keys.Length != defaults.Length)
                return false;

            for (var i = 0; i < keys.Length; i++)
            {
                if (keys[i].alpha != defaults[i].alpha || keys[i].time != defaults[i].time)
                    return false;
            }

            return true;
        }
    }
}
