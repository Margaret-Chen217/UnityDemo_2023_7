using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFallingState : PlayerAirborneState
{
    private PlayerFallData fallData;

    private Vector3 playerPositionOnEnter;

    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        fallData = airborneData.FallData;
    }

    #region IState

    public override void Enter()
    {
        base.Enter();
        playerPositionOnEnter = stateMachine.Player.transform.position;
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        ResetVerticalVelocity();
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        LimitVerticalVelocity();
    }

    #endregion

    #region Reusable Method

    protected override void ResetSprintState()
    {
    }

    protected override void OnContactWithGround(Collider collider)
    {
        //TODO:添加坠落伤害
        //开始falling高度减去接触地面时的高度
        float fallDistance = Mathf.Abs(playerPositionOnEnter.y - stateMachine.Player.transform.position.y);
        Debug.Log($"Fall Distance = {fallDistance}");

        //LightLanding
        if (fallDistance < fallData.MinimunDistanceToBeConsideredHardFall)
        {
            stateMachine.ChangeState(stateMachine.LightLandingState);
            return;
        }

        //HardLanding
        //玩家无输入
        //walking State : shouldwalk且不在sprint

        Debug.Log($"Movement Input = {stateMachine.ReusableData.MovementInput}");
        if (stateMachine.ReusableData.ShouldWalk && !stateMachine.ReusableData.ShouldSprint ||
            stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.HardLandingState);
            return;
        }

        //Rolling
        stateMachine.ChangeState(stateMachine.RollingState);
    }

    #endregion

    #region Main Method

    /// <summary>
    /// 限制player垂直下落速度，防止玩家脚底碰撞器以过高的速度穿过地面
    /// OnTriggerEnter无法触发
    /// </summary>
    private void LimitVerticalVelocity()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

        //判断当前玩家坠落速度是否小于限制速度
        if (playerVerticalVelocity.y >= -fallData.FallSpeedLimit)
        {
            return;
        }

        //假设玩家以-16下落，最大速度为-15，
        // (-15) - (-16) = 1
        //需要加回1才能让玩家速度为-15
        Vector3 limitVelocityDiff =
            new Vector3(0f, -fallData.FallSpeedLimit - stateMachine.Player.Rigidbody.velocity.y, 0f);

        stateMachine.Player.Rigidbody.AddForce(limitVelocityDiff, ForceMode.VelocityChange);
    }

    #endregion
}