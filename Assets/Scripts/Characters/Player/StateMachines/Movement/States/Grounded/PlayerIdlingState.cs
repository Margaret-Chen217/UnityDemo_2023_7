using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region Istate Method
    public override void Enter()
    {
        base.Enter(); //打印状态机类型
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        //设置jumpForce
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;
        ResetVelocity();
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            return;
        }

        OnMove();
    }
    #endregion
}