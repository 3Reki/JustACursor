using Bosses.Conditions;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Editor.ConditionDrawers
{
    public class ConditionDrawer : PropertyDrawer
    {
        public virtual void Draw(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(new Rect(position.position, position.size), property, label);
        }
        
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            
            switch (property.FindPropertyRelative("conditionType").GetEnumValue<ConditionType>())
            {
                case ConditionType.HealthThreshold:
                    Draw(position, property, label);
                    //Debug.Log(property.FindPropertyRelative("condition"));
                    // EditorGUI.ObjectField(new Rect(position.x, position.y + 35, position.width, position.height),
                    //     .ser, typeof(Cdt_HealthThreshold));
                    break;
            }
            
            // if (valueType.enumValueIndex == (int)DynamicParameterSorting.Fixed)
            // {
            //     EditorGUIUtility.labelWidth -= indent*15;
            //     Draw(property, label);
            //     EditorGUIUtility.labelWidth += indent*15;
            // }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}