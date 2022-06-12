using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerInputElement
{
    #region //Input variables
    private InputAction moveAction = null;
    private InputAction cancelAction = null;
    private int moveValue = 0;
    private bool moveInProgress => moveValue != 0;
    #endregion

    #region //Launch Variables
    [Header("Launch Set Up")]
    [Tooltip("Percentage per second")] [SerializeField] private float barDT = 0.2f;
    private float distanceMultiplier = 4;

    [Header("Launch Curve")]
    [Tooltip("Only use curve when above this limit")] [SerializeField] private float curveLimit = 0.1f;
    [Tooltip("Percentage per second")] [SerializeField] private float launchDT = 0.08f;
    [SerializeField] private AnimationCurve launchCurve = null;

    [Header("Fixed Launch Movements")]
    [Header("Units per second, only if not using curve")] [SerializeField] private float launchDX = 0.1f;
    [Tooltip("Units per second")] [SerializeField] private float returnDX = 0.3f;
    #endregion

    #region //Events
    public event Action<bool> StartLaunch;
    public event Action<bool> StopLaunch;
    public event Action<float> LaunchAmountChange;
    #endregion

    #region //SFX
    [Header("SFX")]
    [SerializeField] private AudioSource launchSFX = null;
    #endregion


    #region //Monobehaviour
    protected override void Awake()
    {
        base.Awake();
        distanceMultiplier = Camera.main.orthographicSize * Camera.main.aspect;
        moveAction = inputs.controls.Player.Move;
        cancelAction = inputs.controls.Player.Cancel;
    }
    #endregion

    #region //Input callbacks
    protected override void SubscribeEvents()
    {
        moveAction.started += ReadMoveInput;
        moveAction.canceled += DoMove;
        cancelAction.started += CancelMove;
    }

    protected override void UnsubscribeEvents()
    {
        moveAction.started -= ReadMoveInput;
        moveAction.canceled -= DoMove;
        cancelAction.started -= CancelMove;
    }

    private void ReadMoveInput(InputAction.CallbackContext context)
    {
        int input = (int)context.ReadValue<float>();
        moveValue = input;
        StartCoroutine(SetUpLaunch(moveValue));
    }

    private void DoMove(InputAction.CallbackContext context)
    {
        if(!moveInProgress) return;
        StopLaunch?.Invoke(false);
        moveValue = 0;
    }

    private void CancelMove(InputAction.CallbackContext context)
    {
        if(!moveInProgress) return;
        moveValue = 0;
        StopAllCoroutines();
        StopLaunch?.Invoke(true);
    }
    #endregion

    #region //Launching
    private IEnumerator SetUpLaunch(int input)
    {
        StartLaunch?.Invoke(input > 0);
        float distance = 0;

        while(moveInProgress)
        {
            distance = Mathf.MoveTowards(distance, .99f, barDT * Time.deltaTime);
            LaunchAmountChange?.Invoke(distance);
            yield return null;
        }
        StartCoroutine(Launch(distance * Mathf.Sign(input)));
    }

    private IEnumerator Launch(float distance)
    {
        UnsubscribeEvents();
        launchSFX.Play();
        var absoluteDistance = Mathf.Abs(distance);
        var position = transform.position;
        var start = position.x;
        var end = distance * distanceMultiplier;
        var endTime = 1 - Mathf.Abs(distance);
        var pos = 0f;

        while(transform.position.x != end)
        {
            if(absoluteDistance >= curveLimit)
            {
                position.x = launchCurve.Evaluate(pos) * end;
                pos += launchDT / absoluteDistance * Time.deltaTime;
            }
            else
                position.x = Mathf.MoveTowards(position.x, end, launchDX * Time.deltaTime);

            Globals.gameSpeed = Mathf.Lerp(1, endTime, Mathf.Abs((position.x - start)/(end - start)));
            transform.position = position;
            // Debug.Log($"Time: {Time.timeScale}");
            yield return null;
        }

        while(transform.position.x != start)
        {
            position.x = Mathf.MoveTowards(position.x, start, returnDX * Time.deltaTime);
            Globals.gameSpeed = Mathf.Lerp(endTime, 1, Mathf.Abs((position.x - end)/(start - end)));
            transform.position = position;
            // Debug.Log($"Time: {Time.timeScale}");
            yield return null;
        }
        
        StopLaunch?.Invoke(true);
        SubscribeEvents();
        #endregion
    }
}