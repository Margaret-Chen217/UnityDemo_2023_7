using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        ResetSprintState();
        StartAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);
    }

    #endregion

    #region Reusable Methods

    /// <summary>
    /// Player接触地面， 转换到idle状态
    /// </summary>
    /// <param name="collider"></param>
    protected override void OnContactWithGround(Collider collider)
    {
        stateMachine.ChangeState(stateMachine.LightLandingState);
    }

    /// <summary>
    /// 空中状态重置shouldsprint变量
    /// 跳跃状态重写此方法
    /// </summary>
    protected virtual void ResetSprintState()
    {
        stateMachine.ReusableData.ShouldSprint = false;
    }

    #endregion
}