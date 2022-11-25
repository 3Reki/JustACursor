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
        private bool showProperty;

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Vector2 propPosition = position.position;
            Vector2 defaultSize = new Vector2(position.width, EditorGUIUtility.singleLineHeight);

            showProperty = EditorGUI.Foldout(new Rect(propPosition, defaultSize), showProperty, "Resolved Pattern");
            propPosition.y += EditorGUIUtility.singleLineHeight;

            if (!showProperty)
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
            if (!showProperty)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            return EditorGUIUtility.singleLineHeight * 2 +
                   EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cdtHealthThreshold"), true);
        }

        
    }
}