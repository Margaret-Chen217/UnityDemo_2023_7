using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediumStoppingState : PlayerStoppingState
{
    public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Method

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementDecelerateForce = movementData.StopData.MediumDecelerationForce;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;
    }

    #endregion
}
