using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region //Level variables
    [SerializeField] private bool testMode = false;
    private List<Asteroid> asteroidCatalogue = new List<Asteroid>();
    public static Action LevelEnd;
    public static event Action<int> OnVictory;
    public static event Action<bool> Defeat;
    public event Action<int> AsteroidCountChange;
    private int currentLevel = 0;
    private bool cometDeath = false;
    private bool lost = false;
    #endregion

    #region //Music and SFX
    [SerializeField] private AudioSource mainLevelMusic = null;
    [SerializeField] private AudioSource lastLevelMusic = null;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        Time.timeScale = 1;
        Globals.gameSpeed = 1;
        asteroidCatalogue.Clear();
        
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Tutorial" || sceneName == "Test Level") currentLevel = 0;
        else currentLevel = int.Parse(sceneName.Substring(sceneName.Length - 1));

        if(currentLevel == 5) lastLevelMusic.Play();
        else mainLevelMusic.Play();
    }

    private void OnEnable()
    {
        if(testMode) return;
        Asteroid.OnAwake += RegisterAsteroid;
        DeathZone.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        if(testMode) return;
        Asteroid.OnAwake -= RegisterAsteroid;
        DeathZone.OnGameOver -= GameOver;
    }
    #endregion

    #region //Registering
    private void RegisterAsteroid(Asteroid asteroid)
    {
        asteroidCatalogue.Add(asteroid);
        asteroid.OnDestroy += AsteroidDestroyed;
        asteroid.OnCometDeath += CometDeath;
    }

    private void AsteroidDestroyed(Asteroid destroyed)
    {
        asteroidCatalogue.Remove(destroyed);
        destroyed.OnDestroy -= AsteroidDestroyed;
        AsteroidCountChange?.Invoke(asteroidCatalogue.Count);
        destroyed.OnCometDeath -= CometDeath;
        if(asteroidCatalogue.Count == 0) StartCoroutine(Victory());
    }
    #endregion

    #region //Victory and defeat
    private IEnumerator Victory()
    {
        LevelEnd?.Invoke();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if(lost) yield break;
        mainLevelMusic.Stop();
        lastLevelMusic.Stop();
        Time.timeScale = 0;
        PlayerPrefs.SetInt($"Level {currentLevel}", 1);
        OnVictory?.Invoke(currentLevel);
    }

    private void CometDeath()
    {
        cometDeath = true;
        GameOver();
    }

    private void GameOver()
    {
        LevelEnd?.Invoke();
        Defeat?.Invoke(cometDeath);
        Time.timeScale = 0;
        StopAllCoroutines();
        mainLevelMusic.Stop();
        lastLevelMusic.Stop();
        lost = true;
    }
    #endregion

    #region //Getters
    public int GetAsteroidsRemaining() => asteroidCatalogue.Count;
    #endregion
}