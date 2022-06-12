using System;
using JZ.DISPLAY;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    #region //Lives
    [SerializeField] private int startingLives = 5;
    private int _currentLives; 
    public int currentLives 
    { 
        get => _currentLives;
        private set
        {
            _currentLives = value;
            OnHit?.Invoke(_currentLives);
        }

    }
    #endregion

    #region //Events
    public event Action<int> OnHit;
    public static event Action OnGameOver;
    #endregion

    #region //SFX
    [SerializeField] private AudioSource hitSFX = null;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        currentLives = startingLives;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        currentLives--;
        ScreenShake.CallShake(0.3f, 0.4f);

        if(currentLives == 0) 
        {
            OnGameOver?.Invoke();
        }
        else
            hitSFX.Play();
    }
    #endregion
}
