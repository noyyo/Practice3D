using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public PlayerCamera PlayerCamera { get; private set; }
    [SerializeField] private PlayerStateMachine stateMachine;


    private void Awake()
    {
        AnimationData.Initialize();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerCamera = GetComponent<PlayerCamera>();
        stateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
    }
    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }
    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
}