using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine stateMachine;
    protected PlayerGroundedData movementData;
    protected PlayerAirborneData airborneData;


    /// <summary>
    /// 构造函数:使用stateMachine构造stateMachine
    /// </summary>
    /// <param name="playerMovementStateMachine"></param>
    public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        stateMachine = playerMovementStateMachine;
        movementData = stateMachine.Player.Data.GroundedData;
        airborneData = stateMachine.Player.Data.AirborneData;
        InitializeData();
    }

    private void InitializeData()
    {
        SetBaseRotationData();
    }


    #region IState

    public virtual void Enter()
    {
        Debug.Log($"Enter State: {GetType().Name}");
        AddInputActionCallbacks();
    }


    public virtual void Exit()
    {
        Debug.Log($"Exit State: {GetType().Name}");
        RemoveInputActionCallbacks();
    }


    public virtual void HandleInput()
    {
        ReadMovementInput();
    }


    public virtual void Update()
    {
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    public virtual void OnAnimationEnterEvent()
    {
    }

    public virtual void OnAnimationExitEvent()
    {
    }

    public virtual void OnAnimationTransitionEvent()
    {
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);
            return;
        }
    }
    
    public virtual void OnTriggerExit(Collider collider)
    {
        if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGroundExited(collider);
        }
    }



    #endregion

    #region Main Method

    private void ReadMovementInput()
    {
        //从InputSystem中获取玩家输入信息
        stateMachine.ReusableData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero ||
            stateMachine.ReusableData.MovementSpeedModifier == 0f)
        {
            return;
        }

        //获取用户输入
        Vector3 movementDirection = GetMovementInputDirection();

        //计算旋转角度
        float targetRotationYAngle = Rotate(movementDirection);
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        //计算移动速度
        float movementSpeed = GetMovementSpeed();

        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        //常数 * 常数 * 向量 向量放最后减少开销
        stateMachine.Player.Rigidbody.AddForce(
            movementSpeed * targetRotationDirection - currentPlayerHorizontalVelocity,
            ForceMode.VelocityChange);
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    private float Rotate(Vector3 direction)
    {
        //计算目标旋转角度
        var directionAngle = UpdateTargetRotation(direction);
        //平滑旋转相机
        RotateTowardsTargetRotation();
        return directionAngle;
    }


    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

        stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    /// <summary>
    /// 相机旋转角度
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.Player.MainCameraTransform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360;
        }

        return angle;
    }

    /// <summary>
    /// 根据方向计算旋转目标角度
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        if (directionAngle < 0f)
        {
            directionAngle += 360;
        }

        return directionAngle;
    }

    #endregion

    #region ReusableMethod

    protected void StartAnimation(int animationHash)
    {
        //Debug.Log($"Start Animation: {animationHash}");
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }
    
    protected void StopAnimation(int animationHash)
    {
        //Debug.Log($"StopAnimation: {animationHash}");
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }
    protected void SetBaseRotationData()
    {
        stateMachine.ReusableData.RotationData = movementData.BaseRotationData;
        stateMachine.ReusableData.TimeToReachTargetRotation =
            stateMachine.ReusableData.RotationData.TargetRotationReachTime;
    }

    protected virtual void AddInputActionCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
    }

    /// <summary>
    /// 减速玩家
    /// </summary>
    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.Player.Rigidbody.AddForce(
            -playerHorizontalVelocity * stateMachine.ReusableData.MovementDecelerateForce, ForceMode.Acceleration);
    }
    
    protected void DecelerateVertically()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        stateMachine.Player.Rigidbody.AddForce(
            -playerVerticalVelocity * stateMachine.ReusableData.MovementDecelerateForce, ForceMode.Acceleration);
    }

    protected bool IsMovingHorizongtally(float minimumMagnititude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        //水平面上
        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

        return playerHorizontalMovement.magnitude > minimumMagnititude;
        ;
    }


    protected virtual void RemoveInputActionCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
    }

    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;
        playerHorizontalVelocity.y = 0;
        return playerHorizontalVelocity;
    }

    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0f);
    }

    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
    }

    protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
    {
        float movementSpeed = movementData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;

        if (shouldConsiderSlopes)
        {
            movementSpeed *= stateMachine.ReusableData.MovementOnSlopesSpeedModifier;
        }

        return movementSpeed;
    }

    /// <summary>
    /// 平滑旋转角色
    /// </summary>
    protected void RotateTowardsTargetRotation()
    {
        //判断是否达到目标角度
        float currentYAngle = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;
        if (currentYAngle == stateMachine.ReusableData.CurrentTargetRotation.y)
        {
            return;
        }

        //Debug.Log("RotateTowardsTargetRotation");
        //计算平滑旋转角度并进行旋转
        float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.ReusableData.CurrentTargetRotation.y,
            ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y,
            stateMachine.ReusableData.TimeToReachTargetRotation.y -
            stateMachine.ReusableData.DampedTargetRotationPassedTime.y);
        stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;
        Quaternion targetRotation = Quaternion.Euler(0f, smoothYAngle, 0f);
        stateMachine.Player.Rigidbody.MoveRotation(targetRotation);
    }

    /// <summary>
    /// 计算目标旋转角度
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="shouldConsiderCameraRotation"></param>
    /// <returns></returns>
    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        //计算旋转目标角度
        //用户输入移动方向
        float directionAngle = GetDirectionAngle(direction);
        //加上相机旋转角度
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        //对currentTargetRotation赋值
        if (directionAngle != stateMachine.ReusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected void ResetVelocity()
    {
        stateMachine.Player.Rigidbody.velocity = Vector3.zero;
    }

    /// <summary> 
    /// 重置竖直方向上的速度
    /// </summary>
    protected void ResetVerticalVelocity()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        stateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;

    }

    protected virtual void OnContactWithGround(Collider collider)
    {
        
    }
    
    protected virtual void OnContactWithGroundExited(Collider collider)
    {
       
    }

    /// <summary>
    /// 判断玩家垂直移动方向
    /// </summary>
    /// <param name="minimunVelocity"></param>
    /// <returns></returns>
    protected bool IsMovingUp(float minimunVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimunVelocity;
    }
    
    protected bool IsMovingDown(float minimunVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y < -minimunVelocity;
    }

    #endregion

    #region InputMethod

    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        //参数没有使用但是也需要传输， unity才能知道我们要删除的是哪个回调
        stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
    }

    #endregion
}