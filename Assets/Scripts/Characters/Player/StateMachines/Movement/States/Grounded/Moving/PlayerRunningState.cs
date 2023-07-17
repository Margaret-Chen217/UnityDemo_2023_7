using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerMovingState
{
    private PlayerSprintData SprintData;
    private float startTime;

    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        SprintData = movementData.SprintData;
    }

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;
        startTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        //run->sprint->run
        if (!stateMachine.ReusableData.ShouldWalk)
        {
            return;
        }
        //walk->sprint->walk
        //过渡
        if (Time.time < startTime + SprintData.RunToWalkTime)
        {
            return;
        }
        //过度结束， 结束run
        StopRunning();
    }

    private void StopRunning()
    {
        //idle
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
        //walk
        stateMachine.ChangeState(stateMachine.WalkingState);
    }

    #endregion

    #region Input Method
    
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.MediumStoppingState);
    }

    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        //run->walk
        base.OnWalkToggleStarted(context);
        stateMachine.ChangeState(stateMachine.WalkingState);
    }

    #endregion
}