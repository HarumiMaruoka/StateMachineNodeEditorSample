// 日本語対応
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステートマシンのノードデータを表現するクラス
/// </summary>
[CreateAssetMenu()]
public class StateMachineNode : ScriptableObject
{
    public string GUID { get; set; }

    [SerializeField]
    private List<StateMachineNode> _nextNodes = new List<StateMachineNode>();
    public List<StateMachineNode> NextNodes => _nextNodes;

    [SerializeField]
    private Vector2 _position;
    public Vector2 Position { get => _position; set => _position = value; }

    public void Start()
    {

    }
    public void Update()
    {

    }
    public void Exit()
    {

    }
}
