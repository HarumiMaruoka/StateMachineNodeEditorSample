// 日本語対応
using UnityEngine;

public interface IState
{
    /// <summary>  </summary>
    /// <param name="stateMachine"></param>
    /// <returns></returns>
    public StateMachineSO Init(StateMachineSO stateMachine);

    public void Enter();
    public void Update();
    public void Exit();
}
