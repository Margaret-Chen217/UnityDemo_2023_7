using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightStoppingState : PlayerStoppingState
{
    public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(
        playerMovementStateMachine)
    {
    }

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementDecelerateForce = movementData.StopData.LightDecelerationForce;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
    }

    #endregion
}