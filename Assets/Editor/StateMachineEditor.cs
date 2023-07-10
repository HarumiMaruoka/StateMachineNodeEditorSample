using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class StateMachineEditor : EditorWindow
{
    private StateMachineView _stateMachineView;
    private InspectorView _inspectorView;

    [MenuItem("Window/StateMachineEditor")]
    public static void OpenWindow()
    {
        StateMachineEditor wnd = GetWindow<StateMachineEditor>();
        wnd.titleContent = new GUIContent("StateMachineEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/StateMachineEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachineEditor.uss");
        root.styleSheets.Add(styleSheet);

        _stateMachineView = root.Q<StateMachineView>();
        _inspectorView = root.Q<InspectorView>();

        OnSelectionChange();
    }
    private void OnSelectionChange()
    {
        StateMachineSO stateMachine = Selection.activeObject as StateMachineSO;
        if (stateMachine != null)
        {
            _stateMachineView.PopulateView(stateMachine);
        }
    }
}