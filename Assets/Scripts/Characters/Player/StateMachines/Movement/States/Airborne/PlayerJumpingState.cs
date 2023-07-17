using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerAirborneState
{
    //跳跃过程中判断是否需要旋转玩家
    
    private bool shouldKeepRotating;
    private bool canStartFalling;

    private PlayerJumpData jumpData;
    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        jumpData = airborneData.JumpData;
    }

    #region IState

    public override void Enter()
    {
        base.Enter();
        //跳跃过程中不能移动
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        stateMachine.ReusableData.RotationData = jumpData.RotationData;

        stateMachine.ReusableData.MovementDecelerateForce = jumpData.DecelerationForce;
        
        //跳跃过程用户有输入，就需要旋转玩家
        shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;
        
        //Debug.Log($"shouldKeepRotating = {shouldKeepRotating}");
        Jump();
    }

    public override void Exit()
    {
        base.Exit();
        SetBaseRotationData();
        canStartFalling = false;
    }

    /// <summary>
    /// player下落速度为负，则进入falling
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (!canStartFalling && IsMovingUp(0f))
        {
            canStartFalling = true;
        }
        if (!canStartFalling || GetPlayerVerticalVelocity().y > 0)
        {
            return;
        }
        stateMachine.ChangeState(stateMachine.FallingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        //跳跃过程中旋转玩家
         if (shouldKeepRotating)
         {
             RotateTowardsTargetRotation();
         }

         if (IsMovingUp())
         {
             //Debug.Log("IsMovingUp");
             DecelerateVertically();
         }
    }

    #endregion

    #region Main Methods

    private void Jump()
    {
        //力和角色方向、地面坡度有关
        Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;

        Vector3 jumpDirection = stateMachine.Player.transform.forward;

        
        if (shouldKeepRotating)
        {
            jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            //Debug.Log(jumpDirection);
        }

        jumpForce.x *= jumpDirection.x;
        jumpForce.z *= jumpDirection.z;

        Vector3 capsuleColliderCenterInWorldSpace =
            stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, jumpData.JumpGroundRayDistance, stateMachine.Player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            //Debug.Log($"GroundAngle = {groundAngle}");
            //上坡
            if (IsMovingUp())
            {
                //Debug.Log("IsMovingUp");
                float forceModifier = jumpData.JumpForceModifierOnSlopUpwards.Evaluate(groundAngle);
                jumpForce.x *= forceModifier;
                jumpForce.z *= forceModifier;
            } 

            //下坡
            if (IsMovingDown())
            {
                //Debug.Log("IsMovingDown");
                float forceModifier = jumpData.JumpForceModifierOnSlopDownwards.Evaluate(groundAngle);
                jumpForce.y *= forceModifier;
            }
        }
        
        //防止玩家当前速度影响跳跃
        ResetVelocity();
        
        stateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
    }

    #endregion

    #region Reusable Method

    /// <summary>
    /// 跳跃状态下不重置should sprint
    /// </summary>
    protected override void ResetSprintState()
    {
        
    }

    #endregion
}