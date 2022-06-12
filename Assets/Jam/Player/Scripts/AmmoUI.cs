using System.Collections.Generic;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    #region //Variables
    [SerializeField] private GameObject uiPrefab = null;
    private List<GameObject> prefabs = new List<GameObject>();
    private PlayerShooter shooter = null;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        shooter = FindObjectOfType<PlayerShooter>();
        foreach(Transform child in transform)
            Destroy(child.gameObject);


        for(int ii = 0; ii < shooter.GetMaxClip(); ii++)
            prefabs.Add(Instantiate(uiPrefab, transform));
    }

    private void OnEnable()
    {
        shooter.ChangeAmmoCount += UpdateUI;
    }

    private void OnDisable()
    {
        shooter.ChangeAmmoCount += UpdateUI;
    }
    #endregion

    #region //UI
    private void UpdateUI(int count)
    {
        for(int ii = 1; ii <= prefabs.Count; ii++)
            prefabs[ii-1].SetActive(ii <= count);
    }
    #endregion
}
