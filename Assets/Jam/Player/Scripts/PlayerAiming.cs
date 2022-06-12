using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAiming : PlayerInputElement
{
    #region //Input actions
    private InputAction aimInput = null;
    private Camera cam = null;
    [SerializeField] private Transform target = null;
    public Transform gunTransform { get => target; }
    #endregion

    #region //Aiming variables
    private Vector3 currentAim = Vector3.zero;
    #endregion

        
    #region //Monobehaviour
    protected override void Awake()
    {
        base.Awake();
        aimInput = inputs.controls.Player.Aim;
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        target.eulerAngles = currentAim;
    }
    #endregion

    #region //Input Events
    protected override void SubscribeEvents()
    {
        aimInput.performed += Aim;
    }

    protected override void UnsubscribeEvents()
    {
        aimInput.performed -= Aim;
    }

    private void Aim(InputAction.CallbackContext context)
    {
        Vector2 aim = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimVector = aim - (Vector2)target.position;
        currentAim.z = Vector2.SignedAngle(Vector2.up, aimVector);
    }
    #endregion
}
