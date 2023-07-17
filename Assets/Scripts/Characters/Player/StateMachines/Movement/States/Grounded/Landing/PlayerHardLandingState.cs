using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHardLandingState : PlayerLandingState
{
    public PlayerHardLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState

    public override void Enter()
    {
        base.Enter();
        stateMachine.Player.Input.PlayerActions.Movement.Disable();
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        ResetVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Player.Input.PlayerActions.Movement.Enable();
    }

    public override void OnAnimationExitEvent()
    {
        stateMachine.Player.Input.PlayerActions.Movement.Enable();
    }

    /// <summary>
    /// 动画结某一帧进入idle
    /// </summary>
    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    #endregion

    #region Reusable Method

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStarted;
    }



    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStarted;
    }

    protected override void OnMove()
    {
        if (stateMachine.ReusableData.ShouldWalk)
        {
            return;
        }
        
        stateMachine.ChangeState(stateMachine.RunningState);
    }

    #endregion

    #region Input Method

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }

    #endregion
}
