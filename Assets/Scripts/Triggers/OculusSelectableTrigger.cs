using Kit.Oculus.Interaction;
using Kit.StepGroupManagers;
using Kit.Triggers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kit.Oculus.Triggers
{
    public class OculusSelectableTrigger : MonoBehaviour, ISelectableTrigger
    {
        private void Start()
        {
            XRSelectableInteractable selectable = gameObject.AddComponent<XRSelectableInteractable>();
            XRInteractableEvent event_selected = new XRInteractableEvent();
            event_selected.AddListener((data) => StepGroupManagerBase.Instance.CheckCorrectAction(gameObject));
            selectable.onSelectEntered = event_selected;

            if (StepGroupManagerBase.Instance.StepGroup.type != "TouchManager")
            {
                gameObject.layer = 11;
            }
        }

        void OnDisable()
        {
            Destroy(this);
        }
    }
}