using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private string sceneName = "";


    public void OnClick()
    {
        SceneManager.LoadScene(sceneName);
    }
}
