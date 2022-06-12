using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float postFadeWait = 0.2f;
    [SerializeField] private Image overlay = null;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) Outro();
    }

    public void Outro()
    {
        if(Time.timeScale == 0) return;
        StartCoroutine(FadeOut());
    }


    private IEnumerator FadeOut()
    {
        Time.timeScale = 0;
        float currentTime = 0;
        Color color = overlay.color;
        
        while(currentTime < fadeTime)
        {
            yield return null;
            currentTime += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(0, 1, currentTime / fadeTime);
            overlay.color = color;
        }
        color.a = 1;
        overlay.color = color;
        yield return new WaitForSecondsRealtime(postFadeWait);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
