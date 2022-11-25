using Bosses;
using Bosses.Conditions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

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
                    DrawHealth();
                    break;
                case ConditionType.Test:
                    DrawTest();
                    break;
            }
            propPosition.y += propHeight;
            
            EditorGUI.PropertyField(new Rect(propPosition, defaultSize), property.FindPropertyRelative("pattern"));
            propPosition.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
            
            void DrawHealth()
            {
                propHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cdtHealthThreshold"), true);
                EditorGUI.PropertyField(
                    new Rect(propPosition.x, propPosition.y, defaultSize.x,
                        propHeight), property.FindPropertyRelative("cdtHealthThreshold"), true);
            }

            void DrawTest()
            {
                propHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cdtTest"), true);
                EditorGUI.PropertyField(
                    new Rect(propPosition.x, propPosition.y, defaultSize.x,
                        propHeight), property.FindPropertyRelative("cdtTest"), true);
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
                    selectedPropertyHeight =
                        EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cdtHealthThreshold"), true);
                    break;
                case ConditionType.Test:
                    selectedPropertyHeight =
                        EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cdtTest"), true);
                    break;
            }

            return EditorGUIUtility.singleLineHeight * 3 + selectedPropertyHeight;
        }

        
    }
}