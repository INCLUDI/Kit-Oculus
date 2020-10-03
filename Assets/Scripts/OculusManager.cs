using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static DataModel;

public class OculusManager : PlatformManager
{
    protected override void SceneLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<SceneInstance> scenes)
    {
        base.SceneLoaded(scenes);

        GameObject[] teleportationAreas = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject teleportationArea in teleportationAreas)
        {
            teleportationArea.AddComponent<TeleportationArea>();
            teleportationArea.layer = 12;
        }
    }

    public override void ActivityReady()
    {
        EventManager.TriggerEvent("StartActivity");
    }

    public override void ActivityCompleted()
    {
        LoadMenuScene();
    }

    public override Type SelectableTriggerType()
    {
        return typeof(OculusSelectableTrigger);
    }

    public override Type InteractableTriggerType()
    {
        return typeof(OculusInteractableTrigger);
    }

    public override Type TargetTriggerType()
    {
        return typeof(OculusTargetTrigger);
    }

    public override Type VirtualAssistantTriggerType()
    {
        return typeof(OculusVirtualAssistantTrigger);
    }

    public override Type ButtonTriggerType()
    {
        return typeof(OculusButtonTrigger);
    }
}