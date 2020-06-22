using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAssistantTrigger : TriggerBase
{
    protected override void Trigger()
    {
        ActivityManager.instance.AssistantTriggered();
    }
}
