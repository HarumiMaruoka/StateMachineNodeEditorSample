// 日本語対応
using UnityEngine;

/// <summary>
/// ステートマシンのエッジデータを表現するクラス
/// </summary>
public class StateMachineEdge : ScriptableObject
{
    private bool[] _condition;
    private StateMachineNode _from;
    private StateMachineNode _to;
}
