using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.PhysicsRaycaster;

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
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
        UpdateShoudSprintState();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
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
        Debug.Log("Float Capsule: ");
        
        if (stateMachine.Player.ColliderUtility == null)
        {
            Debug.Log("ColliderUtility == null");
        }

        if (stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider == null)
        {
            Debug.Log("Collider == null");
        }
        
        Vector3 capsuleColliderCenterInWorldSpace =
            stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
        Debug.Log("Here1");
        //Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        //Debug.Log($"IsRayExist : {downwardsRayFromCapsuleCenter}");
        RaycastHit hit;
        Debug.Log("Here2");
        // if (Physics.Raycast(downwardsRayFromCapsuleCenter, out hit, slopeData.FloatRayDistance,
        //         stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Collide))
        if (capsuleColliderCenterInWorldSpace == null)
        {
            Debug.Log("capsuleColliderCenterInWorldSpace is null");
        }
        Debug.Log("Here3");
        if (slopeData == null)
        {
            Debug.Log("slopeData is null");
        }
        Debug.Log("Here4");
        if (stateMachine.Player.LayerData == null)
        {
            Debug.Log("layerData is null");
        }
        Debug.Log("Here5");
        if (Physics.Raycast(capsuleColliderCenterInWorldSpace, Vector3.down, out hit, slopeData.FloatRayDistance,
                stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Collide))
        {
            Debug.Log("Float Capsule : Rayhit");
            float groundAngle = Vector3.Angle(hit.normal, -Vector3.down);

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

    protected virtual void OnFall()
    {
        stateMachine.ChangeState(stateMachine.FallingState);
    }

    /// <summary>
    /// 针对两个障碍物距离很小，但是中间有缝隙，射线检测会出问题
    /// </summary>
    /// <returns></returns>
    private bool IsThereGroundUnderneat()
    {
        BoxCollider groundCheckCollider = stateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckCollider;
        Vector3 groundColliderBoxCenterInWorldSpace = groundCheckCollider.bounds.center;

        Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderBoxCenterInWorldSpace,
            stateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckColliderExtents,
            groundCheckCollider.transform.rotation, stateMachine.Player.LayerData.GroundLayer,
            QueryTriggerInteraction.Ignore);

        return overlappedGroundColliders.Length > 0;
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

    protected override void OnContactWithGroundExited(Collider collider)
    {
        Debug.Log("OnContactWithGroundExited");
        base.OnContactWithGroundExited(collider);
        Debug.Log($"Collider : {collider != null}");
        if (IsThereGroundUnderneat())
        {
            return;
        }
        Debug.Log("here1");
        //射线检测，离开地面一定距离，才认为进入Falling状态
        //角色碰撞器中心
        Debug.Log($"Player : {stateMachine.Player == null}");
        Debug.Log($"ColliderUtility : {stateMachine.Player.ColliderUtility == null}");
        Debug.Log($"CapsuleColliderData : {stateMachine.Player.ColliderUtility.CapsuleColliderData == null}");
        Debug.Log($"Collider : {stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider == null}");
        Vector3 capsuleColliderCenterInWorldSpace =
            stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
        //射线 origin:碰撞器底部 direction:down
        Ray downwardsRayFromCapsuleBottom =
            new Ray(
                capsuleColliderCenterInWorldSpace -
                stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

        //碰撞器底部发出一定长度的射线和地面层无交点，才认为是
        if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movementData.GroundToFallRayDistance,
                stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFall();
        }
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