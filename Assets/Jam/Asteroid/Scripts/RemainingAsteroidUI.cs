using UnityEngine;

public class RemainingAsteroidUI : MonoBehaviour
{
    [SerializeField] private Transform progressFill = null;
    private LevelManager manager = null;
    private int maxAsteroids = 0;


    #region //Monobehaviour
    private void Awake()
    {
        manager = FindObjectOfType<LevelManager>();
    }

    private void OnEnable()
    {
        manager.AsteroidCountChange += UpdateUI;
    }

    private void OnDisable()
    {
        manager.AsteroidCountChange -= UpdateUI;
    }

    private void Start()
    {
        maxAsteroids = manager.GetAsteroidsRemaining();
        UpdateUI(maxAsteroids);
    }
    #endregion
    
    #region //UI
    private void UpdateUI(int newCount)
    {
        float percentageLeft = (float)newCount / (float)maxAsteroids;
        progressFill.localScale = new Vector3(percentageLeft, 1, 1);
    }
    #endregion
}
