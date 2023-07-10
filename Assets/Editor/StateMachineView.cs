// 日本語対応
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

/// <summary>
/// ステートマシンを編集するためのウィンドウクラス
/// </summary>
public class StateMachineView : GraphView
{
    public new class UxmlFactory : UxmlFactory<StateMachineView, GraphView.UxmlTraits> { }
    StateMachineSO _stateMachine;
    public StateMachineView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/StateMachineEditor.uss");
        styleSheets.Add(styleSheet);
    }

    StateMachineNodeView FindNodeView(StateMachineNode node)
    {
        return GetNodeByGuid(node.GUID) as StateMachineNodeView;
    }
    internal void PopulateView(StateMachineSO stateMachine)
    {
        _stateMachine = stateMachine;
        graphViewChanged -= OnGraphVieChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphVieChanged;
        _stateMachine.Nodes.ForEach(n => CreateNodeView(n));

        _stateMachine.Nodes.ForEach(n =>
        {
            var children = _stateMachine.GetNextStates(n);
            children.ForEach(c =>
            {
                StateMachineNodeView parentView = FindNodeView(n);
                StateMachineNodeView childView = FindNodeView(c);

                Edge edge = parentView.Output.ConnectTo(childView.Input);
                AddElement(edge);
            });
        })
;
    }

    private GraphViewChange OnGraphVieChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                StateMachineNodeView nodeView = elem as StateMachineNodeView;
                if (nodeView != null)
                {
                    _stateMachine.DeleteNode(nodeView.Node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    StateMachineNodeView outNode = edge.output.node as StateMachineNodeView;
                    StateMachineNodeView inNode = edge.input.node as StateMachineNodeView;
                    _stateMachine.RemoveTo(outNode.Node, inNode.Node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                StateMachineNodeView outNode = edge.output.node as StateMachineNodeView;
                StateMachineNodeView inNode = edge.input.node as StateMachineNodeView;
                _stateMachine.AddTo(outNode.Node, inNode.Node);
            });
        }

        return graphViewChange;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        var types = TypeCache.GetTypesDerivedFrom<StateMachineNode>();

        evt.menu.AppendAction($"Create Node", a => CreateNode());
    }
    void CreateNode()
    {
        StateMachineNode node = _stateMachine.CreateNode(typeof(StateMachineNode));
        CreateNodeView(node);
    }
    void CreateNodeView(StateMachineNode node)
    {
        StateMachineNodeView nodeView = new StateMachineNodeView(node);
        AddElement(nodeView);
    }
}