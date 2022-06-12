using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelIntroManager : MonoBehaviour
{
    [SerializeField] private GameObject[] boxes = new GameObject[0];
    [SerializeField] private TextMeshProUGUI continueText = null;
    [SerializeField] private bool isOutro = false;
    [SerializeField, ShowIf("isOutro")] private string outroNextScene = "";
    private GameObject currentBox => boxes[currentIndex];
    private int currentIndex = 0;


    private void Awake()
    {
        foreach(var box in boxes)
            box.SetActive(false);
            
        currentBox.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) Outro();
        else if(Input.GetKeyDown(KeyCode.C)) NextBox();
    }

    private void NextBox()
    {
        if(currentIndex + 1 < boxes.Length)
        {
            currentBox.SetActive(false);
            currentIndex++;
            currentBox.SetActive(true);
            if(currentIndex == boxes.Length - 1)
            {
                continueText.text = "Press C To Finish";
                LayoutRebuilder.ForceRebuildLayoutImmediate(continueText.GetComponent<RectTransform>());
            }
        }
        else Outro();
    }

    private void Outro()
    {
        if(isOutro)
        {
            SceneManager.LoadScene(outroNextScene);
        }
        else
        {
            enabled = false;
            var sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName.Substring(0, 7));
        }
    }
}
