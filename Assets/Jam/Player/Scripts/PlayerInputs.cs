using UnityEngine;

public class PlayerInputs: MonoBehaviour
{
    public PlayerControls controls = null;


    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        Pause.OnPause += Paused;
        LevelManager.LevelEnd += TurnOff;
    }

    private void OnDisable()
    {
        controls.Disable();
        Pause.OnPause -= Paused;
        LevelManager.LevelEnd -= TurnOff;
    }

    private void Paused(bool isPaused)
    {
        if(isPaused) controls.Disable();
        else controls.Enable();
    }

    private void TurnOff() => enabled = false;
}
