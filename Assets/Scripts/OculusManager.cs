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

        void ActivatePopup()
        {
            PopupManager.Instance.ActivatePopup(
                header: "Attivita' in pausa",
                message: "Vuoi davvero tornare al menu principale?",
                primaryButtonEnabled: true,
                secondaryButtonEnabled: true,
                primaryButtonText: "Yes",
                secondaryButtonText: "No",
                primaryCallback: () => LoadMenuScene(),
                secondaryCallback: () => PopupManager.Instance.transform.DOMoveY(PopupManager.Instance.transform.position.y + 5f, 2f).OnComplete(() => PopupManager.Instance.gameObject.SetActive(false)));
        }

        public override void LoadActivity(ActivityConfiguration activity)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;

            EventManager.TriggerEvent("ActivityLoading");
            Addressables.LoadSceneAsync(activity.scene).Completed += async (UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<SceneInstance> scenes) =>
            {
                // Instantiate the player (camera rig).
                Vector3 playerPosition = new Vector3(
                    activity.player.transform.position.x,
                    activity.player.transform.position.y + activity.player.groundOffset,
                    activity.player.transform.position.z);
                Quaternion playerRotation = Quaternion.Euler(
                    activity.player.transform.rotation.x,
                    activity.player.transform.rotation.y,
                    activity.player.transform.rotation.z);
                Player = Instantiate(Player, playerPosition, playerRotation);

                // Instantiate the ActivityManager.
                Instantiate(ActivityManager).GetComponent<ActivityManager>().LoadActivity(activity);

                // Instantiate the virtual assistant.
                Vector3 assistantPosition = new Vector3(
                    activity.virtualAssistant.transform.position.x,
                    activity.virtualAssistant.transform.position.y,
                    activity.virtualAssistant.transform.position.z);
                Quaternion assistantRotation = Quaternion.Euler(
                    activity.virtualAssistant.transform.rotation.x,
                    activity.virtualAssistant.transform.rotation.y,
                    activity.virtualAssistant.transform.rotation.z);
                Vector3 assistantScale = new Vector3(
                    activity.virtualAssistant.transform.scale.x,
                    activity.virtualAssistant.transform.scale.y,
                    activity.virtualAssistant.transform.scale.z);
                GameObject assistant = await Addressables.InstantiateAsync(PlayerPrefs.GetString("SelectedAssistant"), assistantPosition, assistantRotation).Task;
                assistant.transform.localScale = assistantScale;

                //Instantiate the StatsManager.
                Instantiate(StatsManager);

                //Instantiate the default popup and hide it from the environment.
                Instantiate(Popup).SetActive(false);

                // Add the teleportation areas.
                GameObject[] teleportationAreas = GameObject.FindGameObjectsWithTag("Ground");
                foreach (GameObject teleportationArea in teleportationAreas)
                {
                    teleportationArea.AddComponent<TeleportationArea>();
                    teleportationArea.layer = 12;
                }

                // Adjust the popup.
                GameObject popup = PopupManager.Instance.gameObject;
                popup.AddComponent<TrackedDeviceGraphicRaycaster>();
                popup.GetComponent<Canvas>().worldCamera = Camera.main;
            };
        }

        protected override void ActivityReady()
        {
            EventManager.TriggerEvent("StartActivity");
        }

        protected override void ActivityCompleted()
        {
            PopupManager.Instance.ActivatePopup(
                header: "Attività terminata",
                message: "Congratulazioni! Hai terminato l'attivita'. Premi il pulsante OK per tornare al menu principale",
                primaryButtonEnabled: true,
                primaryButtonText: "OK",
                primaryCallback: () => LoadMenuScene());
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