using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    #region //Variables
    [SerializeField] private GameObject livePrefab;
    [SerializeField] private Transform uiContainer = null;
    private List<GameObject> livesUI = new List<GameObject>();
    private DeathZone deathZone = null;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        deathZone = FindObjectOfType<DeathZone>();

        foreach(Transform child in uiContainer)
            Destroy(child.gameObject);

        for(int ii = 0; ii < deathZone.currentLives; ii++)
            livesUI.Add(Instantiate(livePrefab, uiContainer));
    }

    private void OnEnable()
    {
        deathZone.OnHit += UpdateUI;
    }

    private void OnDisable()
    {
        deathZone.OnHit -= UpdateUI;
    }
    #endregion

    #region //UI
    private void UpdateUI(int currentLives)
    {
        for(int ii = 1; ii <= livesUI.Count; ii++)
            livesUI[ii-1].SetActive(ii <= currentLives);
    }
    #endregion
}
