using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentTrack : MonoBehaviour
{
    private static PersistentTrack instance = null;
    [SerializeField] private string[] keepNames = new string[0];


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void Awake()
    {
        if(instance == null)
        {
            SetInstance();
        }
        else if(instance.ShouldReset(SceneManager.GetActiveScene().name))
        {
            Destroy(instance.gameObject);
            SetInstance();
        }
        else
            Destroy(gameObject);
    }

    private void SetInstance()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if(ShouldReset(scene.name))
        {
            instance = null;
            Destroy(gameObject);
        }
    }

    private bool ShouldReset(string sceneName)
    {
        foreach(var name in keepNames)
        {
            if(name != sceneName) continue;
            return false;
        }
        return true;
    }
}
