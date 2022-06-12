using UnityEngine;

public class PlaceInReferenceToCamera : MonoBehaviour
{
    #region //Positioning variables
    private enum HAlign
    {
        left,
        center,
        right
    }
    [SerializeField] private HAlign horizontalAlignment = HAlign.center;
    private enum VAlign
    {
        top,
        center,
        bottom
    }
    [SerializeField] private VAlign verticalAlignment = VAlign.center;
    [SerializeField] private Vector2 offset = Vector2.zero;
    #endregion

    #region //Camera variables
    private float myAR = 0;
    private Camera cam = null;
    #endregion

    #region //Monobehaviour
    private void OnValidate()
    {
        if(Camera.main == null) return;
        PositionObject(Camera.main);
    }   

    private void Update()
    {
        if (cam == null)
            cam = Camera.main;

        if(cam != null)
        {
            if (myAR == cam.aspect) return;
            PositionObject(cam);
        }
    }
    #endregion

    #region //Positioning
    public void PositionObject(Camera _cam)
    {
        myAR = _cam.aspect;
        transform.position = new Vector2(GetXPos(_cam), GetYPos(_cam));
    }

    private float GetXPos(Camera _cam)
    {
        float origin = _cam.transform.position.x;
        float xOffset = offset.x;

        switch(horizontalAlignment)
        {
            case HAlign.left:
                xOffset -= _cam.orthographicSize * myAR;
                break;

            case HAlign.right:
                xOffset += _cam.orthographicSize * myAR;
                break;

            case HAlign.center:
            default:
                break;
        }

        return origin + xOffset;
    }

    private float GetYPos(Camera _cam)
    {
        float origin = _cam.transform.position.y;
        float yOffset = offset.y;

        switch(verticalAlignment)
        {
            case VAlign.top:
                yOffset += _cam.orthographicSize;
                break;

            case VAlign.bottom:
                yOffset -= _cam.orthographicSize;
                break;

            case VAlign.center:
            default:
                break;
        }

        return origin + yOffset;
    }
    #endregion
}