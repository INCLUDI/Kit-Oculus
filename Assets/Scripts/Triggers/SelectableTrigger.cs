﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectableTrigger : TriggerBase
{
    private void Start()
    {
        XRSelectableInteractable selectable = gameObject.AddComponent<XRSelectableInteractable>();
        XRInteractableEvent event_selected = new XRInteractableEvent();
        event_selected.AddListener((data) => ActivityManager.instance.checkCorrectAction(gameObject));
        selectable.onSelectEnter = event_selected;

        if (ActivityManager.instance.CurrentEventGroup.type == "TouchManager")
        {
            gameObject.layer = 2;
        }
    }
}
