// 日本語対応
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StateMachineNode))]
public class StateMachineNodePropertyDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_name"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_states"));
    }
}
