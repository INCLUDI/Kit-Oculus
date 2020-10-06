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
    protected override void Start()
    {
        EventManager.StartListening("ActivatePopup", ActivatePopup);
    }

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

    void ActivatePopup()
    {
        PopupManager.instance.ActivatePopup(
            header: "Attivita' in pausa",
            message: "Vuoi davvero tornare al menu principale?",
            button1Enabled: true,
            button1Text: "OK",
            call1: () => LoadMenuScene());
    }

    public override void ActivityReady()
    {
        EventManager.TriggerEvent("StartActivity");
    }

    public override void ActivityCompleted()
    {
        PopupManager.instance.ActivatePopup(
            header: "Attività terminata",
            message: "Congratulazioni! Hai terminato l'attivita'. Premi il pulsante OK per tornare al menu principale",
            button1Enabled: true,
            button1Text: "OK",
            call1: () => LoadMenuScene());
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