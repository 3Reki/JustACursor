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
        private Cdt_HealthThreshold cdtHealthThreshold;
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height),
                property.FindPropertyRelative("conditionType"), new GUIContent("Condition Type"));
            EditorGUI.PropertyField(new Rect(position.x, position.y + 20, position.width, position.height),
                property.FindPropertyRelative("cdtHealthThreshold"), true);

            // switch (property.FindPropertyRelative("conditionType").GetEnumValue<ConditionType>())
            // {
            //     case ConditionType.HealthThreshold:
            //         EditorGUI.PropertyField(new Rect(position.x, position.y + 20, position.width, position.height),
            //             property.FindPropertyRelative("cdtHealthThreshold"), true);
            //         break;
            // }
            
            EditorGUI.EndProperty();
        }
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}