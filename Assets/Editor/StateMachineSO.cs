// 日本語対応
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StateMachineData", menuName = "ScriptableObjects/StateMachine", order = 1)]
public class StateMachineSO : ScriptableObject
{
    [SerializeField]
    private StateMachineNode _enterNode = null;

    private StateMachineNode _currentState = null;

    [SerializeField]
    private List<StateMachineNode> _nodes = new List<StateMachineNode>();
    [SerializeField]
    private List<StateMachineEdge> _edges = new List<StateMachineEdge>();

    public List<StateMachineNode> Nodes => _nodes;

    public void Update()
    {
        _currentState.Update();
    }
    public StateMachineNode CreateNode(Type type)
    {
        StateMachineNode node = ScriptableObject.CreateInstance(type) as StateMachineNode;
        node.name = type.Name;
        node.GUID = GUID.Generate().ToString();
        _nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }
    public void DeleteNode(StateMachineNode node)
    {
        _nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddTo(StateMachineNode from, StateMachineNode to)
    {
        from.NextNodes.Add(to);
    }
    public void RemoveTo(StateMachineNode from, StateMachineNode to)
    {
        from.NextNodes.Remove(to);
    }
    public List<StateMachineNode> GetNextStates(StateMachineNode from)
    {
        return from.NextNodes;
    }

    #region OpenWindow
#if UNITY_EDITOR
    // OnOpenAssetAttributeはアセットが開かれた際に呼び出されるコールバック関数。
    //[OnOpenAsset(0)]
    //public static bool OnOpenWindow(int instanceID, int line)
    //{
    //    // 開いたアセットがStateMachineSO型でなければ開かない。（リターンする。）
    //    if (EditorUtility.InstanceIDToObject(instanceID) is not StateMachineSO) return false;

    //    // ウィンドウを開く
    //    var window = EditorWindow.GetWindow<StateMachineView>();
    //    // ウィンドウの初期化を行う
    //    window.Initialize();
    //    return true;
    //}
#endif
    #endregion
}
