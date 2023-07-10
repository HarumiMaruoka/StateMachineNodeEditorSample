// 日本語対応
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// ステートマシンのノードエディタを表示するためのクラス
/// </summary>
public class StateMachineNodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<StateMachineNodeView> OnNodeSelected;

    private StateMachineNode _node;
    public StateMachineNode Node => _node;

    private Port _input;
    private Port _output;

    public Port Input => _input;
    public Port Output => _output;

    public StateMachineNodeView(StateMachineNode node)
    {
        this._node = node;
        this.title = node.name;

        viewDataKey = _node.GUID;

        style.left = node.Position.x;
        style.top = node.Position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    private void CreateOutputPorts()
    {
        _input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));

        if (_input != null)
        {
            _input.portName = "";
            inputContainer.Add(_input);
        }
    }

    private void CreateInputPorts()
    {
        _output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));

        if (_output != null)
        {
            _output.portName = "";
            outputContainer.Add(_output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        _node.Position = new Vector2(newPos.xMin, newPos.y);
    }
    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }
}
