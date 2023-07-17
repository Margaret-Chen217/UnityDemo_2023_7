using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFallingState : PlayerAirborneState
{
    private PlayerFallData fallData;

    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        fallData = airborneData.FallData;
    }

    #region IState

    public override void Enter()
    {
        base.Enter();
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