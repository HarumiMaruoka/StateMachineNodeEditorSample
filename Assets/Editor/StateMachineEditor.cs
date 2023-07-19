using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class StateMachineEditor : EditorWindow
{
    private StateMachineView _stateMachineView;
    private InspectorView _inspectorView;
    private ConditionsView _conditionsView;

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
        _conditionsView = root.Q<ConditionsView>();

        _stateMachineView.OnNodeSelected = OnNodeSelectionChanged;

        OnSelectionChange();
    }
    StateMachineSO stateMachine;
    private void OnSelectionChange()
    {
        stateMachine = Selection.activeObject as StateMachineSO;

        if (stateMachine == null)
        {
            if (Selection.activeGameObject != null)
            {
                StateMachineRunner runner = Selection.activeGameObject.GetComponent<StateMachineRunner>();
                if (runner != null)
                {
                    stateMachine = runner.StateMachine;
                }
            }
        }

        if (Application.isPlaying)
        {
            if (stateMachine != null)
            {
                _stateMachineView?.PopulateView(stateMachine);
                _conditionsView?.UpdateSelection(stateMachine);
            }
        }

        if (stateMachine != null && AssetDatabase.CanOpenAssetInEditor(stateMachine.GetInstanceID()))
        {
            _stateMachineView?.PopulateView(stateMachine);
            _conditionsView?.UpdateSelection(stateMachine);
        }
    }
    void OnNodeSelectionChanged(StateMachineNodeView node)
    {
        _inspectorView.UpdateSelection(node);
    }
    private void OnGUI()
    {
        var e = Event.current;
        if (GetEventAction(e) && e.type == EventType.KeyDown && e.keyCode == KeyCode.S)
        {
            Debug.Log("Saved StateMachineEditor");
            if (stateMachine != null)
            {
                EditorUtility.SetDirty(stateMachine);
                AssetDatabase.SaveAssets();
            }
            e.Use();
        }
    }

    private bool GetEventAction(Event e)
    {
#if UNITY_EDITOR_WIN
        return e.control;
#else
    return e.command;
#endif
    }

    // OnOpenAssetAttributeはアセットが開かれた際に呼び出されるコールバック関数。
    [OnOpenAsset(0)]
    public static bool OnOpenWindow(int instanceID, int line)
    {
        // 開いたアセットがStateMachineSO型でなければ開かない。（リターンする。）
        if (EditorUtility.InstanceIDToObject(instanceID) is not StateMachineSO) return false;

        // ウィンドウを開く
        var window = EditorWindow.GetWindow<StateMachineEditor>();
        return true;
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }
    private void OnPlayModeChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }
    private void OnInspectorUpdate()
    {
        _stateMachineView?.UpdateNodeState();
    }
}