// 日本語対応
using System;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// ステートマシンをエッジを表示するためのクラス
/// </summary>
public class StateMachineEdgeView : Port
{
    private StateMachineEdge _edge;
    public StateMachineEdgeView(StateMachineEdge edge, Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) :
        base(portOrientation, portDirection, portCapacity, type)
    {
        _edge = edge;
    }
}

