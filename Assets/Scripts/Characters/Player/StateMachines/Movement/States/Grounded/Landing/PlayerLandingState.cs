using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundedState
{
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region Istate

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.LandingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.LandingParameterHash);
    }

    #endregion

    #region Input Method

    /// <summary>
    /// 防止落地进入idle
    /// </summary>
    /// <param name="context"></param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    #endregion
}