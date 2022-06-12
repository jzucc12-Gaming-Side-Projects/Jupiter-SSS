using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    #region //UI components
    private Button button = null;
    #endregion

    #region //Level info
    [SerializeField] private bool isTutorial = false;
    [HideIf("isTutorial"), SerializeField] int levelNumber = 0;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        if(isTutorial) return;
        button = GetComponent<Button>();
        if(levelNumber < 2) return;
        if(PlayerPrefs.GetInt($"Level {levelNumber - 1}", 0) == 1) return;
        button.interactable = false;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "???";
    }
    #endregion

    #region //Pointer methods
    public void OnClick()
    {
        var go = FindObjectOfType<PersistentTrack>().gameObject;
        Destroy(go);
        if(isTutorial) SceneManager.LoadScene("Tutorial");
        else SceneManager.LoadScene($"Level {levelNumber} Intro");
    }
    #endregion
}