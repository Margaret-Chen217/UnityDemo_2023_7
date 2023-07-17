using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightLandingState : PlayerLandingState
{
    public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;
        ResetVelocity();
    }

    public override void Update()
    {
        base.Update();
        //玩家没有输入则idle
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            return;
        }
        OnMove();
    }

    /// <summary>
    /// 动画结某一帧进入idle
    /// </summary>
    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    #endregion
}