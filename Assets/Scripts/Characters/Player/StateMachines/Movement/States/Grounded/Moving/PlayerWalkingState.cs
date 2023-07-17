using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerMovingState
{
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementSpeedModifier = movementData.WalkData.SpeedModifier;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
    }

    #endregion
    
    #region Input Method

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.LightStoppingState);
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        //walk->run
        base.OnWalkToggleStarted(context);
        stateMachine.ChangeState(stateMachine.RunningState);
    }

    #endregion
}