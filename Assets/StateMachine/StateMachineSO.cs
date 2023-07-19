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
    private EntryNode _entryNode;
    public EntryNode EntryNode { get => _entryNode; set => _entryNode = value; }

    private StateMachineNode _currentState = null;

    [SerializeField]
    private List<StateMachineNode> _nodes = new List<StateMachineNode>();
    public List<StateMachineNode> Nodes => _nodes;

    [SerializeField]
    private List<BoolStateValue> _values;
    public List<BoolStateValue> Values => _values;

    public void Start()
    {
        foreach (var e in _nodes) e.Initialize();

        _currentState = _entryNode.NextNodes[0]._nextState;
        _currentState.Enter();
    }
    public void Update()
    {
        _currentState?.Update();
    }
    public void TransitionTo(StateMachineNode node)
    {
        _currentState.Exit();
        _currentState = node;
        _currentState.Enter();
    }
    public void SetValue(string name, bool value)
    {
        for (int i = 0; i < _values.Count; i++)
        {
            if (_values[i].Name == name)
            {
                _values[i].CurrentValue = value;
                return;
            }
        }
    }
    public StateMachineNode CreateNode(Type type)
    {
        StateMachineNode node = ScriptableObject.CreateInstance(type) as StateMachineNode;
        node.name = type.Name;
        node.GUID = GUID.Generate().ToString();

        Undo.RecordObject(this, "State Machine (Create Node)");
        _nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        Undo.RegisterCreatedObjectUndo(node, "State Machine (Create Node)");

        AssetDatabase.SaveAssets();
        return node;
    }
    public void DeleteNode(StateMachineNode node)
    {
        Undo.RecordObject(this, "State Machine (Delete Node)");
        _nodes.Remove(node);

        //AssetDatabase.RemoveObjectFromAsset(node);
        Undo.DestroyObjectImmediate(node);

        AssetDatabase.SaveAssets();
    }

    public void AddTo(StateMachineNode from, StateMachineNode to)
    {
        Undo.RecordObject(from, "State Machine (Add Next State)");

        if (from is EntryNode && from.NextNodes.Count == 1)
        {
            from.NextNodes.Clear();
        }

        var transition = new Transition(to, default);
        from.NextNodes.Add(transition);

        EditorUtility.SetDirty(from);
    }
    public void RemoveTo(StateMachineNode from, StateMachineNode to)
    {
        Undo.RecordObject(from, "State Machine (Remove Next State)");

        Transition t = null;
        for (int i = 0; i < from.NextNodes.Count; i++)
        {
            if (from.NextNodes[i]._nextState == to)
            {
                t = from.NextNodes[i];
                break;
            }
        }

        if (t != null) from.NextNodes.Remove(t);
        EditorUtility.SetDirty(from);
    }
    public List<StateMachineNode> GetNextStates(StateMachineNode from)
    {
        var nexts = new List<StateMachineNode>();

        foreach (var e in from.NextNodes)
        {
            if (e._nextState != null)
            {
                nexts.Add(e._nextState);

            }
        }

        return nexts;
    }

    public void Traverse(StateMachineNode node, Action<StateMachineNode> visiter)
    {
        if (node)
        {
            if (!node.IsInspected)
            {
                node.IsInspected = true;
                visiter.Invoke(node);
                var children = GetNextStates(node);
                children.ForEach(n => Traverse(n, visiter));
            }
            node.IsInspected = false;
        }
    }

    public virtual StateMachineSO Clone()
    {
        StateMachineSO clone = Instantiate(this);
        clone.EntryNode = this.EntryNode.Clone(clone) as EntryNode;
        clone._nodes = new List<StateMachineNode>();
        Traverse(clone.EntryNode, n => clone.Nodes.Add(n));
        clone._values = new List<BoolStateValue>();
        for (int i = 0; i < this._values.Count; i++)
        {
            clone._values.Add(this._values[i].Clone());
        }
        return clone;
    }
}
