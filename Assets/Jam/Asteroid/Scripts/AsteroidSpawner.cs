using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    #region //Spawned Objects Variables
    [Header("Spawned Objects")]
    [SerializeField] private Asteroid prefab = null;
    [SerializeField] private int count = 0;
    private Stack<Asteroid> spawned = new Stack<Asteroid>();
    #endregion

    #region //Spawn Timing Variables
    [Header("Spawn Timing")]
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] private float spawnVariance = 0f;
    private float currentTime = 0f;
    #endregion

    #region //Velocity Variables
    [Header("Spawn Velocity")]
    [SerializeField] private Vector2 baseVelocity = Vector2.down;
    [SerializeField] private Vector2 velocityVariance = Vector2.zero;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        for(int ii = 0; ii < count; ii++)
        {
            var asteroid = GameObject.Instantiate(prefab, transform);
            spawned.Push(asteroid);
            asteroid.gameObject.SetActive(false);
        }
        currentTime = VaryValue(spawnTime, spawnVariance) + startDelay;
    }

    private void FixedUpdate()
    {
        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            var newAsteroid = spawned.Pop();
            currentTime = VaryValue(spawnTime, spawnVariance);
            var velX = VaryValue(baseVelocity.x, velocityVariance.x);
            var velY = VaryValue(baseVelocity.y, velocityVariance.y);
            newAsteroid.SetVelocity(new Vector2(velX, velY));
            newAsteroid.gameObject.SetActive(true);
            if(spawned.Count == 0) enabled = false;
        }
    }
    #endregion

    private float VaryValue(float baseValue, float variance)
    {
        return baseValue + Random.Range(-variance, variance);
    }
}