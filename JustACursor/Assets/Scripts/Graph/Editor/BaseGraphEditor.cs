using Graph.Dialogue;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Graph.Editor
{
    [CustomNodeGraphEditor(typeof(BaseGraph))]
    public class BaseGraphEditor : NodeGraphEditor
    {
        private BaseGraph graph;
        
        public override void OnGUI()
        {
            base.OnGUI();
            
            if (graph == null) graph = target as BaseGraph;
            
            serializedObject.Update();
            
            GUI.backgroundColor = new(25, 25, 25, 0.8f);
            GUILayout.BeginVertical("Options", "window", GUILayout.Width(200), GUILayout.Height(40));

            GUI.backgroundColor = Color.grey;

            AddOption("Center Start Node", typeof(StartNode));
            AddOption("Center Stop Node", typeof(StopNode));

            GUILayout.EndVertical(); 
            serializedObject.ApplyModifiedProperties();
        }

        private void AddOption(string optionName, System.Type nodeType)
        {
            Node targetedNode = null;
            if (GUILayout.Button(optionName))
            {
                if (graph.nodes.Count == 0) Debug.LogWarning("This graph has no nodes !");
                
                foreach (Node node in graph.nodes)
                {
                    if (node.GetType() == nodeType)
                    {
                        targetedNode = node;
                        break;
                    }
                }
                
                if (targetedNode == null) targetedNode = graph.nodes[0];
                
                window.zoom = 1; 
             
                float flippedX = targetedNode.position.x >= 0 ? targetedNode.position.x * -1 : Mathf.Abs(targetedNode.position.x);
                float flippedY = targetedNode.position.y >= 0 ? targetedNode.position.y * -1 : Mathf.Abs(targetedNode.position.y);
                window.panOffset = new Vector2(flippedX, flippedY);
            }
        }
    }
}