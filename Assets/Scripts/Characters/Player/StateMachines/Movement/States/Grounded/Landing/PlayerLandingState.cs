using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundedState
{
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region Input Method

    /// <summary>
    /// 防止落地进入idle
    /// </summary>
    /// <param name="context"></param>
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    #endregion
}