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

public class GameManager : MonoBehaviour
{
    //public GameObject SdkElements;
    public GameObject Player;
    public GameObject ActivityManager;
    //public GameObject Popup;
    public GameObject StatsManager;

    public ActivityConfiguration currentActivity;

    public List<string> assistantsList = new List<string>();

    public static GameManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        //DisableVR();
    }

    //public void EnableVR()
    //{
    //    StartCoroutine(LoadDevice("cardboard", true));
    //}

    //public void DisableVR()
    //{
    //    StartCoroutine(LoadDevice("", false));
    //}

    public void LoadActivity(ActivityConfiguration activity)
    {
        currentActivity = activity;
        Addressables.LoadSceneAsync(activity.scene).Completed += SceneLoaded;
    }

    void SceneLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<SceneInstance> scenes)
    {
        //SdkElements = Instantiate(SdkElements);

        Vector3 playerPosition = new Vector3(
            currentActivity.playerTransform.position.x, 
            currentActivity.playerTransform.position.y, 
            currentActivity.playerTransform.position.z);
        Quaternion playerRotation = Quaternion.Euler(
            currentActivity.playerTransform.rotation.x, 
            currentActivity.playerTransform.rotation.y, 
            currentActivity.playerTransform.rotation.z);
        Player = Instantiate(Player, playerPosition, playerRotation);
        
        ActivityManager = Instantiate(ActivityManager);

        Vector3 assistantPosition = new Vector3(
            currentActivity.assistantTransform.position.x,
            currentActivity.assistantTransform.position.y,
            currentActivity.assistantTransform.position.z);
        Quaternion assistantRotation = Quaternion.Euler(
            currentActivity.assistantTransform.rotation.x,
            currentActivity.assistantTransform.rotation.y,
            currentActivity.assistantTransform.rotation.z);
        Vector3 assistantScale = new Vector3(
            currentActivity.assistantTransform.scale.x,
            currentActivity.assistantTransform.scale.y,
            currentActivity.assistantTransform.scale.z);
        GameObject assistant = Instantiate(Resources.Load<GameObject>
            ("VirtualAssistants/Prefabs/" + assistantsList[PlayerPrefs.GetInt("SelectedAssistantIndex")]),
            assistantPosition, assistantRotation);
        assistant.transform.localScale = assistantScale;

        //Popup = Instantiate(Popup);
        //Popup.SetActive(false);

        StatsManager = Instantiate(StatsManager);

        GameObject[] teleportationAreas = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject teleportationArea in teleportationAreas)
        {
            teleportationArea.AddComponent<TeleportationArea>();
            teleportationArea.layer = 12;
        }
    }


    //private IEnumerator LoadDevice(string newDevice, bool enable)
    //{
    //    if (string.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0)
    //    {
    //        XRSettings.LoadDeviceByName(newDevice);
    //        yield return null;
    //        XRSettings.enabled = enable;
    //        if (string.IsNullOrEmpty(newDevice))
    //        {
    //            Application.targetFrameRate = -1;
    //        }
    //        else
    //        {
    //            Application.targetFrameRate = 60;
    //            QualitySettings.vSyncCount = 0;
    //        }
    //    }
    //}

    public void ActivityReady()
    {
        //Popup.GetComponent<PopupManager>().ActivatePopup("Message", "Ready to go! Insert your phone into the headset and look the OK button!", "Let's start", true, () =>
        //{
        //    Popup.transform.DOMoveY(Popup.transform.position.y + 5f, 2f).OnComplete(() =>
        //    {
        //        Popup.SetActive(false);
        //        VirtualAssistantManager.instance.Initialize();
        //    });
        //});
        VirtualAssistantManager.instance.Initialize();
    }

    public void ActivityCompleted()
    {
        //Popup.GetComponent<PopupManager>().ActivatePopup("Message", "Activity finished! Remove your phone from the headset and look at the button to go back to the main menu", "Ok!", true, () =>
        //{
        //    SceneManager.LoadScene("MenuScene");
        //});
        SceneManager.LoadScene("MenuScene");
    }
}