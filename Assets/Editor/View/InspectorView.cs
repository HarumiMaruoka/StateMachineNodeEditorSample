// 日本語対応
using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    Editor _editor;

    public InspectorView()
    {

    }
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
}
