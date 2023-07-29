using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: Header("Properties")]
    [field: SerializeField]
    public Health Health { get; private set; }

    [field: Header("Reference")]
    [field: SerializeField]
    public PlayerSO Data { get; private set; }

    [field: SerializeField] public PlayerOnlineInfoSO PlayerOnlineData { get; set; }

    [field: Header("Collisions")]
    [field: SerializeField]
    public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField]
    public PlayerAnimationData AnimationData { get; private set; }

    [field: Header("UserClient")]
    [field: SerializeField]
    public UserClient UserClient { get; private set; }

    public Button onlineButton;
    public GameObject nameInputField;
    public GameObject conncetButton;
    private UpdatePlayerLabel playerName;

    private bool isOnlineGame = false;
    public float onlineSendCD = 0.5f;

    [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
    public PlayerInput Input { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    public Animator Animator { get; private set; }
    private PlayerMovementStateMachine movementStateMachine;
    public Transform MainCameraTransform { get; private set; }

    public event Action<Message> HandlePlayerPosition;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInput>();
        playerName = transform.Find("PlayerName").GetComponent<UpdatePlayerLabel>();
        movementStateMachine = new PlayerMovementStateMachine(this);
        MainCameraTransform = Camera.main.transform;
        AnimationData.Initialize();
        Health.Initialize();
    }

    private void Start()
    {
        movementStateMachine.ChangeState(movementStateMachine.IdlingState);

        conncetButton.SetActive(false);
        nameInputField.SetActive(false);
        onlineButton.onClick.AddListener(delegate { OnOnlineButtonClicked(); });
        conncetButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            OnConnectButtonClicked();
        });
    }

    private void UpdatePlayerName()
    {
        PlayerOnlineData.playerName = nameInputField.transform.GetComponent<TMP_InputField>().text;
        playerName.UpdateLabel();
    }

    private void OnOnlineButtonClicked()
    {
        if (conncetButton.activeSelf)
        {
            conncetButton.SetActive(false);
            nameInputField.SetActive(false);
        }
        else
        {
            conncetButton.SetActive(true);
            nameInputField.SetActive(true);
        }
    }


    private void OnConnectButtonClicked()
    {
        UpdatePlayerName();
        UserClient.Initialze();
        isOnlineGame = true;
    }

    private void OnValidate()
    {
        ColliderUtility.Initialize(gameObject);
        ColliderUtility.CalculateCapsuleColliderDimensions();
    }


    private float onlineTimer = 0;

    private void Update()
    {
        movementStateMachine.HandleInput();
        movementStateMachine.Update();

        if (isOnlineGame)
        {
            onlineTimer += Time.deltaTime;
            if (onlineTimer > onlineSendCD)
            {
                UserClient.SendMessageToServer(UpdatePlayerPosition());
                onlineTimer = 0;
            }
        }
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

    public Message UpdatePlayerPosition()
    {
        Debug.Log($"playername: {PlayerOnlineData.playerName}");
        PlayerInfo playerInfo = new PlayerInfo(PlayerOnlineData.playerName, transform.position);
        Debug.Log($"Playerinfo: {playerInfo}");
        Message message = new Message("UpdatePosition", JsonConvert.SerializeObject(playerInfo));
        return message;
    }
}