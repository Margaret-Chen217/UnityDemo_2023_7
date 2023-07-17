using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    private SlopeData slopeData;

    protected PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(
        playerMovementStateMachine)
    {
        slopeData = stateMachine.Player.ColliderUtility.SlopeData;
    }

    #region IState

    public override void Enter()
    {
        base.Enter();
        UpdateShoudSprintState();
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        FloatCapsule();
    }

    #region MainMethod

    private void UpdateShoudSprintState()
    {
        if (!stateMachine.ReusableData.ShouldSprint)
        {
            return;
        }

        //持续移动不重置should sprint
        if (stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            return;
        }

        //没有持续按键盘，重置
        stateMachine.ReusableData.ShouldSprint = false;
    }

    private void FloatCapsule()
    {
        Vector3 capsuleColliderCenterInWorldSpace =
            stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance,
                stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Collide))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }


            float distanceToFloatingPoint =
                stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y *
                stateMachine.Player.transform.localScale.y - hit.distance;
            //Debug.Log("Hit Distance: " + hit.distance);
            if (distanceToFloatingPoint == 0)
            {
                return;
            }

            float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = movementData.SlopeSpeedAngle.Evaluate(angle);
        stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;
        return slopeSpeedModifier;
    }

    #endregion

    #endregion


    #region ReusableMethod

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        stateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
        stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        stateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
        stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
    }

    protected virtual void OnMove()
    {
        //long sprint -> jump -> sprint
        //长时间的sprint后跳跃，落地会继续sprint
        if (stateMachine.ReusableData.ShouldSprint)
        {
            stateMachine.ChangeState(stateMachine.SprintingState);
            return;
        }

        //idle->walk
        if (stateMachine.ReusableData.ShouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
            return;
        }


        //idle->run
        stateMachine.ChangeState(stateMachine.RunningState);
    }
    
    
    #endregion

    #region Input Method

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        //walk->idle
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        //walk->dash
        stateMachine.ChangeState(stateMachine.DashingState);
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        //TODO: 需要先判断是否在地面上
        stateMachine.ChangeState(stateMachine.JumpingState);
    }

    #endregion
}