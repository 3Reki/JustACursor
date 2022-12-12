﻿using Bosses.Instructions;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Editor
{
    [CustomEditor(typeof(InstructionSequence<>))]
    public class PatternQueueEditor : UnityEditor.Editor
    {
        private SerializedProperty resolver, instructions;
        private float patternDuration;

        private void OnEnable()
        {
            resolver = serializedObject.FindProperty("resolver");
            instructions = serializedObject.FindProperty("instructions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            patternDuration = 0;
            for (int i = 0; i < instructions.arraySize; i++)
            {
                Object objRef = instructions.GetArrayElementAtIndex(i).objectReferenceValue;
                
                if (objRef == null)
                    continue;
                
                patternDuration += new SerializedObject(objRef).FindProperty("patternDuration").floatValue;
            }

            EditorGUILayout.PropertyField(instructions);
            EditorGUILayout.PropertyField(resolver);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Pattern Duration", patternDuration);
            EditorGUI.EndDisabledGroup();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
    
    [CustomEditor(typeof(InstructionSequenceBoss))]
    public class PatternBossQueueEditor : PatternQueueEditor {}

    [CustomEditor(typeof(InstructionSequenceDrones))]
    public class PatternDronesQueueEditor : PatternQueueEditor {}
}