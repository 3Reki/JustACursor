using Bosses.Dependencies;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using ConditionType = Bosses.Dependencies.ConditionType;

namespace Editor
{
    [CustomPropertyDrawer(typeof(ResolvedPattern))]
    public class ResolvedPatternDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Vector2 propPosition = position.position;
            Vector2 defaultSize = new Vector2(position.width, EditorGUIUtility.singleLineHeight);

            property.isExpanded = EditorGUI.Foldout(new Rect(propPosition, defaultSize), property.isExpanded, label);
            propPosition.y += EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel += 1;
            EditorGUI.PropertyField(new Rect(propPosition, defaultSize),
                property.FindPropertyRelative("conditionType"), new GUIContent("Condition Type"));
            propPosition.y += EditorGUIUtility.singleLineHeight;

            float propHeight = 0;

            switch (property.FindPropertyRelative("conditionType").GetEnumValue<ConditionType>())
            {
                case ConditionType.HealthThreshold:
                    DrawConditionProperty("cdtHealthThreshold");
                    break;
                case ConditionType.CornerDistance:
                    DrawConditionProperty("cdtCornerDistance");
                    break;
                case ConditionType.CenterDistance:
                    DrawConditionProperty("cdtCenterDistance");
                    break;
                case ConditionType.BossDistance:
                    DrawConditionProperty("cdtBossDistance");
                    break;
                case ConditionType.Quarter:
                    DrawConditionProperty("cdtQuarter");
                    break;
                case ConditionType.Half:
                    DrawConditionProperty("cdtHalf");
                    break;
            }

            propPosition.y += propHeight;

            EditorGUI.PropertyField(new Rect(propPosition, defaultSize), property.FindPropertyRelative("weight"));
            propPosition.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();

            void DrawConditionProperty(string propertyName)
            {
                propHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative(propertyName), true);
                EditorGUI.PropertyField(
                    new Rect(propPosition.x, propPosition.y, defaultSize.x,
                        propHeight), property.FindPropertyRelative(propertyName), true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            float selectedPropertyHeight = 0f;

            switch (property.FindPropertyRelative("conditionType").GetEnumValue<ConditionType>())
            {
                case ConditionType.HealthThreshold:
                    selectedPropertyHeight = GetConditionPropertyHeight("cdtHealthThreshold");
                    break;
                case ConditionType.CornerDistance:
                    selectedPropertyHeight = GetConditionPropertyHeight("cdtCornerDistance");
                    break;
                case ConditionType.CenterDistance:
                    selectedPropertyHeight = GetConditionPropertyHeight("cdtCenterDistance");
                    break;
                case ConditionType.BossDistance:
                    selectedPropertyHeight = GetConditionPropertyHeight("cdtBossDistance");
                    break;
                case ConditionType.Quarter:
                    selectedPropertyHeight = GetConditionPropertyHeight("cdtQuarter");
                    break;
                case ConditionType.Half:
                    selectedPropertyHeight = GetConditionPropertyHeight("cdtHalf");
                    break;
            }

            return EditorGUIUtility.singleLineHeight * 3 + selectedPropertyHeight;

            float GetConditionPropertyHeight(string propertyName) =>
                EditorGUI.GetPropertyHeight(property.FindPropertyRelative(propertyName), true);
        }
    }
}