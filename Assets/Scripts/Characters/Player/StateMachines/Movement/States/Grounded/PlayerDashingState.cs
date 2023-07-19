using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerGroundedState
{
    private PlayerDashData dashData;

    private float startTime;

    private int consecutiveDashedUsed;

    private bool shouldKeepRotating;

    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        dashData = movementData.DashData;
    }
    //moving状态到dash：增加speedmodifier
    //idle状态到dash：addforce

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.DashParameterHash);
        stateMachine.ReusableData.MovementSpeedModifier = dashData.SpeedModifier;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

        stateMachine.ReusableData.RotationData = dashData.RotationData;

        Dash();

        shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

        UpdateConsecutiveDashes();
        startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.DashParameterHash);
        SetBaseRotationData();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!shouldKeepRotating)
        {
            return;
        }

        RotateTowardsTargetRotation();
    }

    public override void OnAnimationTransitionEvent()
    {
        //静止状态
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);
            return;
        }

        //运动状态
        stateMachine.ChangeState(stateMachine.SprintingState);
    }

    #endregion


    #region Main Method

    private void UpdateConsecutiveDashes()
    {
        //不是连续冲刺
        if (!IsConsecutive())
        {
            consecutiveDashedUsed = 0;
        }

        consecutiveDashedUsed++;

        //连续冲刺
        if (consecutiveDashedUsed >= dashData.ConsecutiveDashesLimitAmount)
        {
            consecutiveDashedUsed = 0;

            stateMachine.Player.Input.DisableActionFor(stateMachine.Player.Input.PlayerActions.Dash,
                dashData.DashLimitReachedCooldown);
        }
    }

    /// <summary>
    /// 判断是否为连续冲刺
    /// </summary>
    /// <returns></returns>
    private bool IsConsecutive()
    {
        return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;
    }

    private void Dash()
    {
        //idling状态到dash: 设置速度
        Vector3 dashDirection = stateMachine.Player.transform.forward;

        dashDirection.y = 0f;

        UpdateTargetRotation(dashDirection, false);

        //moving状态到dash：增加speedmodifier
        if (stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            UpdateTargetRotation(GetMovementInputDirection());

            dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
        }

        stateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
    }

    #endregion

    #region Reusable Method

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
    }


    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
    }

    #endregion

    #region Input Method

    /// <summary>
    /// 覆盖GroundedState中的方法， 防止进入Idle
    /// </summary>
    /// <param name="context"></param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        shouldKeepRotating = true;
    }

    #endregion
}