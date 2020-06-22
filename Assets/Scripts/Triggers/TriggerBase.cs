using UnityEngine;

public abstract class TriggerBase : MonoBehaviour
{
    protected bool _gvrStatus = false;
    protected float _gvrTimer = 0;
    protected float _totalTime = 1.5f;
    protected bool _gazeComplete = false;

    /// <summary>
    /// Function to check the timer and know when the gaze is completed
    /// </summary>
    protected void Update()
    {
        // Gaze Timer logic
        if (_gvrStatus)
        {
            _gvrTimer += Time.deltaTime;
        }

        // Accept/Reject feedback logic
        if (_gvrTimer > _totalTime && _gazeComplete != true)
        {
            Trigger();
            _gazeComplete = true;
        }
    }

    /// <summary>
    /// Function listener on event trigger from the unity editor
    /// Need to be added to each interactable object
    /// </summary>
    public void gvrOn()
    {
        // Gaze Timer logic
        _gvrStatus = true;
    }

    /// <summary>
    /// Function listener on event trigger from the unity editor
    /// Need to be added to each interactable object
    /// </summary>
    public void gvrOff()
    {
        // Gaze Timer logic
        _gvrStatus = false;
        _gvrTimer = 0;

        // Accept/Reject feedback logic
        _gazeComplete = false;
    }

    protected abstract void Trigger();
}