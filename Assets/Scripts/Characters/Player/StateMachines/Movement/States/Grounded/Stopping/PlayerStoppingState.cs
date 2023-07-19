using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    protected PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(
        playerMovementStateMachine)
    {
    }

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.StoppingParameterHash);
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.StoppingParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        RotateTowardsTargetRotation();
        
        if (!IsMovingHorizongtally())
        {
            return;
        }

        DecelerateHorizontally();
    }

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

    #endregion

    #region Input Method

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }

    #endregion
}