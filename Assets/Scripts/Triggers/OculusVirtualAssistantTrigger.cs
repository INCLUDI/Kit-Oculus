using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OculusVirtualAssistantTrigger : MonoBehaviour, IVirtualAssistantTrigger
{
    private void Start()
    {
        XRSelectableInteractable virtualAssistant = gameObject.AddComponent<XRSelectableInteractable>();
        XRInteractableEvent event_selected = new XRInteractableEvent();
        event_selected.AddListener((data) => ActivityManager.instance.AssistantTriggered());
        virtualAssistant.onSelectEnter = event_selected;

        gameObject.layer = 13;
    }
}
