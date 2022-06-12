using System;
using System.Collections;
using JZ.POOL;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : PlayerInputElement
{
    #region //Other Inputs
    private PlayerAiming aiming = null;
    #endregion

    #region //Weapon variables
    [Header("Base Weapon")]
    [SerializeField] private Transform spawnPoint = null;
    [SerializeField] private int maxClip = 6;
    private int _currentClip;
    private int currentClip 
    { 
        get => _currentClip; 
        set 
        {
            _currentClip = value;
            ChangeAmmoCount?.Invoke(value); 
        }
    }
    public event Action<int> ChangeAmmoCount;

    [Header("Firing")]
    [SerializeField] private float fireDelayTime = 1f;
    [SerializeField] private float projectileVelocity = 1f;

    [Header("Reloading")]
    [SerializeField] private float reloadTime = 1f;

    #endregion

    #region //Input Actions
    private InputAction fireAction = null;
    private InputAction reloadAction = null;
    #endregion

    #region //Pool
    [SerializeField] private ComponentPool<Rigidbody2D> bulletPoolRef = null;
    private ComponentPool<Rigidbody2D> bulletPool = null;
    #endregion

    #region //SFX
    [Header("SFX")]
    [SerializeField] private AudioSource reloadSFX = null;
    [SerializeField] private AudioSource fireSFX = null;
    [SerializeField] private AudioClip[] fireClips = new AudioClip[0];
    #endregion


    #region //Monobehaviour
    protected override void Awake()
    {
        base.Awake();
        bulletPool = new ComponentPool<Rigidbody2D>(bulletPoolRef);
        fireAction = inputs.controls.Player.Fire;
        reloadAction = inputs.controls.Player.Reload;
        aiming = GetComponent<PlayerAiming>();
    }

    private void Start()
    {
        currentClip = maxClip;
    }
    #endregion

    #region //Input Callbacks
    protected override void SubscribeEvents()
    {
        fireAction.started += OnFireInput;
        reloadAction.started += OnReloadInput;
    }

    protected override void UnsubscribeEvents()
    {
        fireAction.started -= OnFireInput;
        reloadAction.started -= OnReloadInput;
    }

    private void OnFireInput(InputAction.CallbackContext context)
    {
        if(currentClip == 0) StartCoroutine(Reload());
        else StartCoroutine(Fire());
    }

    private void OnReloadInput(InputAction.CallbackContext context)
    {
        StartCoroutine(Reload());
    }
    #endregion

    #region //Firing and reloading
    private IEnumerator Fire()
    {
        UnsubscribeEvents();
        var bullet = bulletPool.GetObject();
        bullet.transform.position = spawnPoint.position;
        bullet.transform.eulerAngles = aiming.gunTransform.eulerAngles;
        bullet.velocity = projectileVelocity * aiming.gunTransform.up;
        currentClip--;

        var clipNo = UnityEngine.Random.Range(0, fireClips.Length);
        fireSFX.clip = fireClips[clipNo];
        fireSFX.Play();

        yield return new WaitForSeconds(fireDelayTime);
        SubscribeEvents();
    }

    private IEnumerator Reload()
    {
        UnsubscribeEvents();
        currentClip = maxClip;
        reloadSFX.Play();
        yield return new WaitForSeconds(reloadTime);
        SubscribeEvents();
    }

    #endregion

    #region //Getters
    public int GetMaxClip() => maxClip;
    #endregion
}