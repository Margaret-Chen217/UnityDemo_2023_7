using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(
        playerMovementStateMachine)
    {
    }

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);
        stateMachine.ReusableData.MovementDecelerateForce = movementData.StopData.HardDecelerationForce;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);
    }

    #endregion

    #region Reusable Method

    /// <summary>
    /// HardStoping状态中玩家继续移动， 只会进入跑步状态
    /// </summary>
    protected override void OnMove()
    {
        if(stateMachine.ReusableData.ShouldWalk){}
        {
            return;
        }
        stateMachine.ChangeState(stateMachine.RunningState);
    }

    #endregion
}