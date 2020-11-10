using DG.Tweening;
using Kit.Oculus.Triggers;
using Kit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Android;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using static Kit.DataModel;

namespace Kit.Oculus
{
    public class OculusManager : PlatformManager
    {
        protected override void Start()
        {
            base.Start();
            EventManager.StartListening("ActivatePopup", ActivatePopup);

            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
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

            GameObject popup = PopupManager.instance.gameObject;
            popup.AddComponent<TrackedDeviceGraphicRaycaster>();
            popup.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        void ActivatePopup()
        {
            PopupManager.instance.ActivatePopup(
                header: "Attivita' in pausa",
                message: "Vuoi davvero tornare al menu principale?",
                button1Enabled: true,
                button2Enabled: true,
                button1Text: "Yes",
                button2Text: "No",
                call1: () => LoadMenuScene(),
                call2: () => PopupManager.instance.transform.DOMoveY(PopupManager.instance.transform.position.y + 5f, 2f).OnComplete(() => PopupManager.instance.gameObject.SetActive(false)));
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
}