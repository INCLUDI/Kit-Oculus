using Kit.Oculus.Interaction;
using Kit.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kit.Oculus.Triggers
{
    public class OculusSelectableTrigger : MonoBehaviour, ISelectableTrigger
    {
        private void Start()
        {
            XRSelectableInteractable selectable = gameObject.AddComponent<XRSelectableInteractable>();
            XRInteractableEvent event_selected = new XRInteractableEvent();
            event_selected.AddListener((data) => ActivityManager.instance.checkCorrectAction(gameObject));
            selectable.onSelectEnter = event_selected;

            if (ActivityManager.instance.CurrentStepGroup.type != "TouchManager")
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