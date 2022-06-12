using UnityEngine;

public abstract class PlayerInputElement: MonoBehaviour
{
    protected PlayerInputs inputs = null;


    protected virtual void Awake()
    {
        inputs = GetComponent<PlayerInputs>();
    }

    protected virtual void OnEnable()
    {
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected abstract void SubscribeEvents();
    protected abstract void UnsubscribeEvents();
}
