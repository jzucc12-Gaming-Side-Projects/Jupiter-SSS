using UnityEngine;

public class LaunchUI : MonoBehaviour
{
    #region //Variables
    [SerializeField] private RectTransform negativeMask = null;
    [SerializeField] private RectTransform positiveMask = null;
    [SerializeField] private GameObject aKey = null;
    [SerializeField] private GameObject dKey = null;
    private RectTransform activeMask = null;
    private PlayerMovement playerMove = null;
    private float maxRectX = 0;
    private float rectY = 0;
    #endregion


    #region //Monobehaviour
    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
        maxRectX = positiveMask.rect.width;
        rectY = positiveMask.rect.height;
        negativeMask.gameObject.SetActive(false);
        positiveMask.gameObject.SetActive(false);
        aKey.SetActive(true);
        dKey.SetActive(true);
    }

    private void OnEnable()
    {
        playerMove.StartLaunch += ShowUI;
        playerMove.LaunchAmountChange += UpdateBar;
        playerMove.StopLaunch += HideUI;
    }

    private void OnDisable()
    {
        playerMove.StartLaunch -= ShowUI;
        playerMove.LaunchAmountChange -= UpdateBar;
        playerMove.StopLaunch -= HideUI;
    }
    #endregion

    #region //UI
    private void ShowUI(bool usePositive)
    {
        aKey.SetActive(false);
        dKey.SetActive(false);
        activeMask = usePositive ? positiveMask : negativeMask;
        activeMask.gameObject.SetActive(true);
        UpdateBar(0);
    }

    private void UpdateBar(float percentage)
    {
        var xValue = Mathf.Lerp(0, maxRectX, percentage);
        activeMask.sizeDelta = new Vector2(xValue, rectY);
    }

    private void HideUI(bool showKeys)
    {
        if(showKeys)
        {
            aKey.SetActive(true);
            dKey.SetActive(true);
        }

        if(activeMask != null)
        {
            activeMask.gameObject.SetActive(false);
            activeMask = null;
        }
    }
    #endregion
}
