using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    #region //UI
    [SerializeField] private GameObject victoryUI = null;
    [SerializeField] private GameObject lastLevelWinUI = null;
    [SerializeField] private GameObject defeatUI = null;
    [SerializeField] private GameObject cometDefeatUI = null;
    #endregion
    

    #region //Monobehaviour
    private void Awake()
    {
        victoryUI.SetActive(false);
        lastLevelWinUI.SetActive(false);
        defeatUI.SetActive(false);
        cometDefeatUI.SetActive(false);
    }

    private void OnEnable()
    {
        LevelManager.Defeat += GameOverUI;
        LevelManager.OnVictory += VictoryUI;
    }

    private void OnDisable()
    {
        LevelManager.Defeat -= GameOverUI;
        LevelManager.OnVictory -= VictoryUI;
    }
    #endregion

    #region //UI
    private void GameOverUI(bool fromComet)
    {
        if(fromComet) cometDefeatUI.SetActive(true);
        else defeatUI.SetActive(true);
    }

    private void VictoryUI(int levelNo)
    {
        if(levelNo != 5) victoryUI.SetActive(true);
        else lastLevelWinUI.SetActive(true);
    }
    #endregion
}
