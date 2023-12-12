using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isIdle;

    public bool IsWalking => isWalking;
    public bool IsIdle => isIdle;
    public float Speed = 0;
    public Animator PlayerAnimator => playerAnimator;

    private Vector3 lastPosition = Vector3.zero;
    private Animator playerAnimator;
    private PlayerStateMachine playerStateMachine;
    public PlayerStateMachine PlayerStateMachine => playerStateMachine;

    private void Awake()
    {
        playerStateMachine = new PlayerStateMachine(this);
        playerAnimator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        playerStateMachine.Initialize(playerStateMachine.idleState);   
    }

    void Update()
    {
        playerStateMachine.Update();
    }

    private void FixedUpdate()
    {
        Speed = (transform.position - lastPosition).magnitude; // think about changing speed scope?
        lastPosition = transform.position;
    }
}
