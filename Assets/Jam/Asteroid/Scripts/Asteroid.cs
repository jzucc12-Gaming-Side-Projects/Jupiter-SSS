using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region //Asteroid
    [Header("Base Asteroid")]
    [SerializeField] private LayerMask bulletLayer = -1;
    [SerializeField] private LayerMask planetLayer = -1;
    [SerializeField] private LayerMask shredderLayer = -1;
    [SerializeField] private LayerMask deathLayer = -1;
    [SerializeField] private Transform body = null;
    protected Vector2 velocity = Vector2.zero;
    public static event System.Action<Asteroid> OnAwake;
    public event System.Action<Asteroid> OnDestroy;
    private Collider2D myCollider = null;
    #endregion

    #region //Multi-hit
    [Header("Multi-hit")]
    [SerializeField] private SpriteRenderer sr = null;
    [SerializeField] private Sprite[] sprites = new Sprite[0];
    private int _currentIndex = 0;
    private int currentIndex 
    { 
        get => _currentIndex;
        set
        {
            _currentIndex = value;
            if(_currentIndex >= sprites.Length) return;
            sr.sprite = sprites[sprites.Length - 1 - _currentIndex];
            Color effectColor = effectColors[effectColors.Length - 1 - _currentIndex];
            var main = bulletDeathParticles.main;
            main.startColor = effectColor;
            main = playerDamageParticles.main;
            main.startColor = effectColor;
            if(isComet) trail.startColor = effectColor;
        }
    }
    #endregion

    #region //Split Asteroid
    [Header("Split Asteroid")]
    [SerializeField] private Transform remainderContainer = null;
    [SerializeField] private Vector2 baseSplitBoost = Vector2.zero;
    [SerializeField] private Vector2 velocityVariance = Vector2.zero;
    private List<Asteroid> remainders = new List<Asteroid>();
    #endregion

    #region //Comet
    [Header("Comet")]
    [SerializeField] private bool isComet = false;
    [SerializeField] private TrailRenderer trail = null;
    public event System.Action OnCometDeath;
    #endregion

    #region //SFX & VFX
    [Header("SFX & VFX")]
    [SerializeField] private AudioSource destroyedByPlayerSFX = null; 
    [SerializeField] private ParticleSystem bulletDeathParticles = null;
    [SerializeField] private ParticleSystem playerDamageParticles = null;
    [SerializeField] private Color[] effectColors = new Color[0];
    #endregion

    #region //Monobehaviour
    protected virtual void Awake() 
    { 
        OnAwake?.Invoke(this);
        myCollider = GetComponent<Collider2D>();
        currentIndex = 0;

        foreach(Asteroid remainder in remainderContainer.GetComponentsInChildren<Asteroid>())
        {
            remainders.Add(remainder);
            remainder.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity * Globals.gameSpeed * Time.deltaTime);
        if(velocity.x == 0) return;
        var dAngle = velocity.y / velocity.x;
        var angularLimit = 3;
        if(Mathf.Abs(dAngle) > angularLimit) dAngle = angularLimit * Mathf.Sign(dAngle);
        body.Rotate(0, 0, dAngle * Globals.gameSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int layer = 1 << other.gameObject.layer;
        if(layer == bulletLayer.value) 
        {
            destroyedByPlayerSFX.Play();
            BulletHit();
            other.gameObject.SetActive(false);
        }
        else if(layer == planetLayer.value)
        {
            if(isComet) OnCometDeath?.Invoke();
            destroyedByPlayerSFX.Play();
            StartCoroutine(Destroyed());
        }
        else if(layer == shredderLayer.value) 
        {
            destroyedByPlayerSFX.Play();
            StartCoroutine(Destroyed());
        }
        else if(layer == deathLayer.value)
        {
            playerDamageParticles.Play();
            StartCoroutine(Destroyed());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        velocity.x *= -1;   
    }
    #endregion

    #region //Asteroid methods
    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    protected virtual void BulletHit()
    {
        if(++currentIndex >= sprites.Length)
        {
            bool flip = false;
            foreach(var remainder in remainders)
            {
                remainder.transform.position = transform.position;
                remainder.transform.parent = transform.parent;
                var newVel = velocity + baseSplitBoost;
                newVel.x += Random.Range(-velocityVariance.x, velocityVariance.x);
                newVel.y += Random.Range(-velocityVariance.y, velocityVariance.y);
                if(flip) newVel.x *= -1;
                flip = !flip;
                remainder.SetVelocity(newVel);
                remainder.gameObject.SetActive(true);
            }

            bulletDeathParticles.Play();
            StartCoroutine(Destroyed());
        }
    }

    private IEnumerator Destroyed()
    {
        velocity = Vector2.zero;
        if(trail != null) trail.enabled = false;
        myCollider.enabled = false;
        sr.enabled = false;
        OnDestroy?.Invoke(this);
        yield return new WaitUntil(() => 
        {
            if(bulletDeathParticles.isPlaying) return false;
            if(playerDamageParticles.isPlaying) return false;
            if(destroyedByPlayerSFX.isPlaying) return false;
            return true;
        });
        gameObject.SetActive(false);
    }
    #endregion
}
