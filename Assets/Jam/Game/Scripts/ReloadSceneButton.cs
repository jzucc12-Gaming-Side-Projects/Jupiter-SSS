using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
