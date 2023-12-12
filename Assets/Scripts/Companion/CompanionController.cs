using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    [SerializeField] private IState currentStateVal = default;
    [SerializeField] private bool isIdle = false;
    [SerializeField] private bool isScanning = false;
    [SerializeField] private bool isReceiving = false;
    [SerializeField] private bool isPulsing = false;

    [Header("VFX")]
    public GameObject scanSurvey;
    public GameObject pulseOrb;
    public GameObject receiverBeam;


    private CompanionStateMachine companionStateMachine;
    private Animator companionAnimator;

    public IState CurrentStateVal => currentStateVal;

    public CompanionStateMachine CompanionStateMachine => companionStateMachine;
    public Animator CompanionAnimator => companionAnimator;

    public bool IsIdle => isIdle;
    public bool IsScanning => isScanning;
    public bool IsReceiving => isReceiving;
    public bool IsPulsing => isPulsing;

    private void Awake()
    {
        companionAnimator = GetComponent<Animator>();
        companionStateMachine = new CompanionStateMachine(this);
    }

    void Start()
    {
        companionStateMachine.Initialize(companionStateMachine.idleState);
    }

    void Update()
    {
        companionStateMachine.Update();
    }

    // MOve to player input script //
    public void StartScanning()
    {
        isScanning = true;
        isPulsing = false;
        isReceiving = false;
        isIdle = false;
    }

    public void StartPulsing()
    {
        isScanning = false;
        isPulsing = true;
        isReceiving = false;
        isIdle = false;
    }

    public void StartReceiving()
    {
        isPulsing = false;
        isScanning = false;
        isReceiving = true;
        isIdle = false;
    }

    public void StarIdle()
    {
        isPulsing = false;
        isScanning = false;
        isReceiving = false;
        isIdle = true;
    }
}
