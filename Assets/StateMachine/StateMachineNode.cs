// 日本語対応
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンのノードデータを表現するクラス
/// </summary>
public class StateMachineNode : ScriptableObject
{
    [SerializeField]
    private string _name = null;
    public string Name { get => _name; set => _name = value; }

    [SerializeField]
    [HideInInspector]
    private string guid;
    public string GUID { get => guid; set => guid = value; }

    [SerializeField]
    private List<Transition> _nextNodes = new List<Transition>();
    public List<Transition> NextNodes => _nextNodes;

    [SerializeField]
    [HideInInspector]
    private Vector2 _position;
    public Vector2 Position { get => _position; set => _position = value; }

    [Header("実行するクラス")]
    [SerializeField, SerializeReference, SubclassSelector]
    private IState[] _states;

    public bool IsRunning { get; private set; }
    public bool IsInspected { get; set; } = false;

    private StateMachineSO _stateMachine;

    public void Initialize()
    {
        foreach (var e in _states)
        {
            e?.Init(_stateMachine);
        }
    }
    private void SetOwner(StateMachineSO owner)
    {
        _stateMachine = owner;
    }
    public virtual void Enter()
    {
        IsRunning = true;
        foreach (var e in _states) e?.Enter();
    }
    public virtual void Update()
    {
        // 毎フレーム実行する処理
        foreach (var e in _states) e?.Update();

        // ステートの遷移
        foreach (var e in _nextNodes)
        {
            foreach (var k in _stateMachine.Values)
            {
                var valueName = k.Name;
                bool isSuccess = true;
                foreach (var i in e._conditions)
                {
                    if (i.TargetName == k.Name && !k.CurrentValue)
                    {
                        isSuccess = false;
                        break;
                    }
                }
                if (isSuccess)
                {
                    _stateMachine.TransitionTo(e._nextState);
                    return;
                }
            }
        }
    }
    public virtual void Exit()
    {
        foreach (var e in _states) e?.Exit();
        IsRunning = false;
    }

    // このノードを基準としたグラフの各連結成分のクローンを取得する。
    private Dictionary<StateMachineNode, StateMachineNode> GetCloneNodes(StateMachineSO owner)
    {
        var clones = new Dictionary<StateMachineNode, StateMachineNode>();

        Stack<StateMachineNode> targets = new Stack<StateMachineNode>();

        targets.Push(this);
        while (targets.Count != 0)
        {
            var node = targets.Pop();
            if (!clones.ContainsKey(node))
            {
                var clone = Instantiate(node);
                clones.Add(node, clone);
                clone.SetOwner(owner);
            }

            for (int i = 0; i < node.NextNodes.Count; i++)
            {
                if (node.NextNodes[i]._nextState != null && !clones.ContainsKey(node.NextNodes[i]._nextState))
                {
                    targets.Push(node.NextNodes[i]._nextState);
                }
            }
        }

        return clones;
    }
    public virtual StateMachineNode Clone(StateMachineSO owner)
    {
        var clones = GetCloneNodes(owner);
        var clone = clones[this];

        foreach (var item in clones)
        {
            var nexts = new List<Transition>();
            foreach (var e in item.Key._nextNodes)
            {
                var transition = new Transition(clones[e._nextState], e._conditions);
                nexts.Add(transition);
            }
            item.Value._nextNodes = nexts;
        }

        return clone;
    }
}