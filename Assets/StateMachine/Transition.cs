// 日本語対応
using UnityEngine;

[System.Serializable]
public class Transition
{
    public Transition(StateMachineNode nextNode, BoolStateCondition[] stateConditions)
    {
        _nextState = nextNode; _conditions = stateConditions;
    }

    public StateMachineNode _nextState;
    [SerializeField]
    public BoolStateCondition[] _conditions;
}
