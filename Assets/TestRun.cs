// 日本語対応
using UnityEngine;
using UnityEngine.UI;

public class TestRun : MonoBehaviour
{
    [SerializeField, StateMachinePropety]
    private string value1;
    [SerializeField, StateMachinePropety]
    private string value2;
    [SerializeField, StateMachinePropety]
    private string value3;

    [SerializeField]
    private Text _currentStateText;
    [SerializeField]
    private Text _value1Text;
    [SerializeField]
    private Text _value2Text;
    [SerializeField]
    private Text _value3Text;

    StateMachineRunner stateMachineRunner;

    private void Start()
    {
        stateMachineRunner = GetComponent<StateMachineRunner>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            stateMachineRunner.StateMachine.SetValue(value1, !stateMachineRunner.StateMachine.GetValue(value1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stateMachineRunner.StateMachine.SetValue(value2, !stateMachineRunner.StateMachine.GetValue(value2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            stateMachineRunner.StateMachine.SetValue(value3, !stateMachineRunner.StateMachine.GetValue(value3));
        }
        _currentStateText.text = "Current State = " + stateMachineRunner.StateMachine.CurrentState.Name;
        _value1Text.text = "Value1 = " + stateMachineRunner.StateMachine.GetValue(value1);
        _value2Text.text = "Value2 = " + stateMachineRunner.StateMachine.GetValue(value2);
        _value3Text.text = "Value3 = " + stateMachineRunner.StateMachine.GetValue(value3);
    }
}
