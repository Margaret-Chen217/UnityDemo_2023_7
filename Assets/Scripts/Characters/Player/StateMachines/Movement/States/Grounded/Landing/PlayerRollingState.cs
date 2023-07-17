using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRollingState : PlayerLandingState
{
    private PlayerRollData rollData;

    public PlayerRollingState(PlayerMovementStateMachine playerMovementStateMachine) : base(
        playerMovementStateMachine)
    {
        rollData = movementData.RollData;
    }

    #region IState

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementSpeedModifier = rollData.SpeedModifier;

        stateMachine.ReusableData.ShouldSprint = false;
    }

    /// <summary>
    /// landing状态中若玩家没有持续输入
    /// 需要进行自动旋转
    /// </summary>
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            return;
        }

        RotateTowardsTargetRotation();
    }


    /// <summary>
    /// 向其他状态转换
    /// </summary>
    public override void OnAnimationTransitionEvent()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.MediumStoppingState);
            return;
        }

        OnMove();
    }

    #endregion


    #region Input Method

    /// <summary>
    /// 不能向JumpState转换
    /// </summary>
    /// <param name="context"></param>
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
    }

    #endregion
}