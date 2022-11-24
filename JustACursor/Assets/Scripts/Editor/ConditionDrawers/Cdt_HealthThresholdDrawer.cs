using Bosses.Conditions;
using UnityEditor;
using UnityEngine;

namespace Editor.ConditionDrawers
{
    //[CustomPropertyDrawer(typeof(Cdt_HealthThreshold))]
    public class Cdt_HealthThresholdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            EditorGUI.PropertyField(new Rect(position.position, position.size),
                property.FindPropertyRelative("threshold"), label);
        }
    }
}