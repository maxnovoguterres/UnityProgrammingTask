using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ChangePainterAttribute))]
public class ChangePainterAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var originalColor = GUI.contentColor;
        property.serializedObject.Update();

        if (property.propertyType == SerializedPropertyType.Generic)
        {
            var hasDataChanged = false;
            var propertyEnumerator = property.Copy().GetEnumerator();
            using var propertyEnumerator1 = propertyEnumerator as IDisposable;
            while (propertyEnumerator.MoveNext())
            {
                var currentProperty = propertyEnumerator.Current as SerializedProperty;
                if (HasChange(currentProperty))
                {
                    hasDataChanged = true;
                    break;
                }
            }
            GUI.contentColor = hasDataChanged ? Color.cyan : originalColor;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        }
        else
        {
            GUI.contentColor = HasChange(property) ? Color.cyan : originalColor;
            EditorGUI.PropertyField(position, property, label);
        }

        GUI.contentColor = originalColor;
        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            var propertyEnumerator = property.Copy().GetEnumerator();
            using var propertyEnumerator1 = propertyEnumerator as IDisposable;
            while (propertyEnumerator.MoveNext())
            {
                var currentProperty = propertyEnumerator.Current as SerializedProperty;
                if (HasChange(currentProperty))
                {
                    GUI.contentColor = Color.cyan;
                    EditorGUILayout.PropertyField(currentProperty);
                    GUI.contentColor = originalColor;
                }
                else
                {
                    EditorGUILayout.PropertyField(currentProperty);
                }
            }
            EditorGUI.indentLevel--;
        }

        GUI.contentColor = originalColor;
        property.serializedObject.ApplyModifiedProperties();
    }

    private static bool HasChange(SerializedProperty currentProperty)
    {
        if (currentProperty.propertyType == SerializedPropertyType.Integer && currentProperty.intValue != default)
        {
            return true;
        }
        if (currentProperty.propertyType == SerializedPropertyType.Boolean && currentProperty.boolValue != default)
        {
            return true;
        }
        if (currentProperty.propertyType == SerializedPropertyType.Float && !Mathf.Approximately(currentProperty.floatValue, default))
        {
            return true;
        }
        if (currentProperty.propertyType == SerializedPropertyType.String && currentProperty.stringValue != default)
        {
            return true;
        }
        if (currentProperty.propertyType == SerializedPropertyType.Color && currentProperty.colorValue != default)
        {
            return true;
        }
        if (currentProperty.propertyType == SerializedPropertyType.Vector2 && currentProperty.vector2Value != default)
        {
            return true;
        }
        if (currentProperty.propertyType == SerializedPropertyType.Vector3 && currentProperty.vector3Value != default)
        {
            return true;
        }
        if (currentProperty.propertyType == SerializedPropertyType.Quaternion && currentProperty.quaternionValue != default)
        {
            return true;
        }
        return false;
    }
}
