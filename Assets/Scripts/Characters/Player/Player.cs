using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    // [field: Header("Properties")]
    // [field: SerializeField]
    // public Health Health { get; private set; }

    [field: Header("Reference")]
    [field: SerializeField]
    public PlayerSO Data { get; private set; }

    [field: Header("Collisions")]
    [field: SerializeField]
    public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField]
    public PlayerAnimationData AnimationData { get; private set; }

    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    public PlayerInput Input { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    public Animator Animator { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;
    public Transform MainCameraTransform { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInput>();
        movementStateMachine = new PlayerMovementStateMachine(this);
        MainCameraTransform = Camera.main.transform;
        AnimationData.Initialize();
        // Health.Initialize();
    }

    private void OnValidate()
    {
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
    }

    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);
    }

    private void Update()
    {
        movementStateMachine.HandleInput();
        movementStateMachine.Update();
    }

    private void FixedUpdate()
    {
        movementStateMachine.PhysicsUpdate();
    }

    /// <summary>
    /// 玩家脚底碰撞器接触地面
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        movementStateMachine.OnTriggerEnter(collider);
    }

    /// <summary>
    /// 玩家脚底碰撞器离开地面
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        movementStateMachine.OnTriggerExit(collider);
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        movementStateMachine.OnAnimationEnterEvent();
    }
    
    public void OnMovementStateAnimationExitEvent()
    {
        movementStateMachine.OnAnimationExitEvent();
    }
    
    public void OnMovementStateAnimationTransitionEvent()
    {
        movementStateMachine.OnAnimationTransitionEvent();
    }
}