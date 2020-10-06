using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandInteractionManager : MonoBehaviour
{
    private void Update()
    {
        
    }

    public void LeftHandSelectionBegin()
    {
        EventManager.TriggerEvent("LeftHandInteractionBegin");
    }

    public void LeftHandSelectionEnd()
    {
        EventManager.TriggerEvent("LeftHandInteractionEnd");
    }
}
