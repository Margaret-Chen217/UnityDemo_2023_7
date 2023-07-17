using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerMovingState
{
    private PlayerSprintData sprintData;
    private float startTime;

    //根据按键时常判断是否Sprint
    private bool keepSprint;

    //
    private bool shouldResetSprintState;

    public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(
        playerMovementStateMachine)
    {
        sprintData = movementData.SprintData;
    }

    #region IState Method

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

        shouldResetSprintState = true;

        startTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (keepSprint)
        {
            //Debug.Log($"Keep Sprint: {keepSprint}");
            return;
        }

        //未超过规定时间，继续sprint
        if (Time.time < startTime + sprintData.SprintToRunTime)
        {
            return;
        }

        //超过规定时间，停止sprint
        stopSprint();
    }

    public override void Exit()
    {
        base.Exit();

        if (shouldResetSprintState)
        {
            keepSprint = false;
            stateMachine.ReusableData.ShouldSprint = false;
        }
    }

    #endregion

    #region Main Method

    private void stopSprint()
    {
        //用户没有输入则进入idle
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }

        //用户输入进入跑run
        stateMachine.ChangeState(stateMachine.RunningState);
    }

    #endregion

    #region Reusable Method

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
    }


    protected override void RemoveInputActionCallbacks()
    {
        base.RemoveInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
    }

    /// <summary>
    /// 键盘按下足够长时间，进入持续冲刺状态
    /// </summary>
    protected override void OnFall()
    {
        shouldResetSprintState = false;
        base.OnFall();
    }

    #endregion

    #region Input Method

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.HardStoppingState);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        shouldResetSprintState = false;
        base.OnJumpStarted(context);
    }


    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        keepSprint = true;
        stateMachine.ReusableData.ShouldSprint = true;
    }

    #endregion
}