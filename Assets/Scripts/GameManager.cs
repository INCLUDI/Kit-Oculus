﻿using DG.Tweening;
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

public class GameManager : PlatformManager
{
    protected override void SceneLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<SceneInstance> scenes)
    {
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

        StatsManager = Instantiate(StatsManager);

        GameObject[] teleportationAreas = GameObject.FindGameObjectsWithTag("Ground");
        foreach (GameObject teleportationArea in teleportationAreas)
        {
            teleportationArea.AddComponent<TeleportationArea>();
            teleportationArea.layer = 12;
        }
    }

    public override void ActivityReady()
    {
        VirtualAssistantManager.instance.Initialize();
    }

    public override void ActivityCompleted()
    {
        SceneManager.LoadScene("MenuScene");
    }
}