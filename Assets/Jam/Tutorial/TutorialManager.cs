using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] boxes = new GameObject[0];
    [SerializeField] private GameObject extras = null;
    private GameObject currentBox => boxes[currentIndex];
    private int currentIndex = 0;
    public UnityEvent TutorialDone;


    private void Awake()
    {
        foreach(var box in boxes)
            box.SetActive(false);
            
        currentBox.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)) NextBox();
    }

    private void NextBox()
    {
        if(currentIndex + 1 < boxes.Length)
        {
            currentBox.SetActive(false);
            currentIndex++;
            currentBox.SetActive(true);
        }
        else EndTutorial();
    }

    private void EndTutorial()
    {
        extras.SetActive(false);
        currentBox.SetActive(false);
        TutorialDone?.Invoke();
    }
}
