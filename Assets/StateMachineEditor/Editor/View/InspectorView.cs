// 日本語対応
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    Editor _editor;

    public InspectorView()
    { }

    internal void UpdateSelection(StateMachineNodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(_editor);
        _editor = Editor.CreateEditor(nodeView.Node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (_editor.target)
            {
                _editor.OnInspectorGUI();
            }
        });
        Add(container);
    }
    internal void UpdateSelection(Edge edge)
    {
        // EdgeからNodeViewを取得する
        var inputNodeView = edge.input.node as StateMachineNodeView;
        if (inputNodeView == null) return;
        var outputNodeView = edge.output.node as StateMachineNodeView;
        if (outputNodeView == null) return;

        // NodeViewからNodeを取得する
        var inputNode = inputNodeView.Node;
        if (inputNode == null) return;
        var outputNode = outputNodeView.Node;
        if (outputNode == null) return;

        if (outputNode is EntryNode)
        {
            Clear();
            UnityEngine.Object.DestroyImmediate(_editor);
            return;
        }

        Transition transition = null;
        int transitionIndex = -1;

        for (int i = 0; i < outputNode.NextNodes.Count; i++)
        {
            if (outputNode.NextNodes[i]._nextState == inputNode)
            {
                transition = outputNode.NextNodes[i];
                transitionIndex = i;
                break;
            }
        }

        if (transitionIndex == -1) return;
        if (transition == null) return;

        Clear();

        UnityEngine.Object.DestroyImmediate(_editor);
        _editor = Editor.CreateEditor(outputNode);

        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (_editor.target)
            {
                _editor.serializedObject.Update();
                EditorGUILayout.PropertyField(
                    _editor.serializedObject.FindProperty("_nextNodes").
                    GetArrayElementAtIndex(transitionIndex).
                    FindPropertyRelative("_conditions"));
                _editor.serializedObject.ApplyModifiedProperties();
            }
        });
        Add(container);
    }
}
