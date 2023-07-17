using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerStateReusableData
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 0.225f;

    public float MovementOnSlopesSpeedModifier { get; set; } = 1f;

    public float MovementDecelerateForce { get; set; } = 1f;
    public bool ShouldWalk { get; set; }
    public bool ShouldSprint { get; set; }

    private Vector3 currentTargetRotation;
    private Vector3 timeToReachTargetRotation;
    private Vector3 dampedTargetRotationCurrentVelocity;
    private Vector3 dampedTargetRotationPassedTime;

    //Vector3 直接用属性的话传递的是值变量， 不能修改
    public ref Vector3 CurrentTargetRotation
    {
        get { return ref currentTargetRotation; }
    }

    public ref Vector3 TimeToReachTargetRotation
    {
        get { return ref timeToReachTargetRotation; }
    }

    public ref Vector3 DampedTargetRotationCurrentVelocity
    {
        get { return ref dampedTargetRotationCurrentVelocity; }
    }

    public ref Vector3 DampedTargetRotationPassedTime
    {
        get { return ref dampedTargetRotationPassedTime; }
    }
    
    //不需要修改，无需引用
    public Vector3 CurrentJumpForce { get; set; }

    public PlayerRotationData RotationData { get; set; }
}