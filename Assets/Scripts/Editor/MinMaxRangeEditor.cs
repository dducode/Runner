using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MinMaxRange))]
public class MinMaxRangeEditor : PropertyDrawer
{
    const int LINE_COUNT = 2;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * LINE_COUNT;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var minValueProperty = property.FindPropertyRelative("minValue");
        var maxValueProperty = property.FindPropertyRelative("maxValue");

        var minLimitProperty = property.FindPropertyRelative("minLimit");
        var maxLimitProperty = property.FindPropertyRelative("maxLimit");

        using (var propertyScope = new EditorGUI.PropertyScope(position, label, property))
        {
            var sliderRect = EditorGUI.PrefixLabel(position, label);
            var lineHeight = position.height / LINE_COUNT;
            sliderRect.height = lineHeight;

            var valuesRect = sliderRect;
            valuesRect.y += sliderRect.height;
            valuesRect.width /= 2.0f;

            var minValueRect = valuesRect;
            var maxValueRect = valuesRect;
            maxValueRect.x += minValueRect.width;

            var minValue = minValueProperty.floatValue;
            var maxValue = maxValueProperty.floatValue;

            EditorGUI.BeginChangeCheck();

            EditorGUI.MinMaxSlider(
                sliderRect,
                ref minValue,
                ref maxValue,
                minLimitProperty.floatValue,
                maxLimitProperty.floatValue
            );

            minValue = EditorGUI.FloatField(minValueRect, minValue);
            maxValue = EditorGUI.FloatField(maxValueRect, maxValue);

            var isChanged = EditorGUI.EndChangeCheck();

            if (isChanged)
            {
                minValueProperty.floatValue = minValue;
                maxValueProperty.floatValue = maxValue;
            }
        }
    }
}
