using Kit.Oculus.Interaction;
using Kit.StepGroupManagers;
using Kit.Triggers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kit.Oculus.Triggers
{
    public class OculusVirtualAssistantTrigger : MonoBehaviour, IVirtualAssistantTrigger
    {
        private void Start()
        {
            XRSelectableInteractable virtualAssistant = gameObject.AddComponent<XRSelectableInteractable>();
            XRInteractableEvent event_selected = new XRInteractableEvent();
            event_selected.AddListener((data) => StepGroupManagerBase.Instance.AssistantTriggered());
            virtualAssistant.onSelectEnter = event_selected;

            gameObject.layer = 13;
        }
    }
}