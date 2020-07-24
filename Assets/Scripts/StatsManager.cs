﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DataModel;

public class StatsManager : MonoBehaviour
{
    private string activityName;
    private bool speechBubbleEnabled;
    private float speechRate;
    private float speechPausesDuration;

    private DateTime startActivityTime;
    private DateTime startActionTime;

    private List<UserAction> userActions;

    public static StatsManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        activityName = GameManager.instance.currentActivity.name;
        speechBubbleEnabled = PlayerPrefs.GetInt("SpeechBubble") == 0 ? false : true;
        speechRate = PlayerPrefs.GetFloat("SpeechRate");
        speechPausesDuration = PlayerPrefs.GetFloat("SpeechPausesDuration");
        userActions = new List<UserAction>();

        EventManager.StartListening("StartActivity", ActivityStarted);
        EventManager.StartListening("ActivityCompleted", ActivityCompleted);
        EventManager.StartListening("WaitForAction", ActionStarted);
    }

    private void ActivityStarted()
    {
        startActivityTime = DateTime.Now;
    }

    private void ActivityCompleted()
    {
        Session session = new Session
        {
            activityName = activityName,
            speechBubbleEnabled = speechBubbleEnabled,
            speechRate = speechRate,
            speechPausesDuration = speechPausesDuration,
            activityDuration = (DateTime.Now - startActivityTime).TotalSeconds,
            userActions = userActions
        };
        ConnectionManager.instance.PostSession(session);
    }

    private void ActionStarted()
    {
        startActionTime = DateTime.Now;
    }

    public void ActionCompleted(List<string> request, EventParameters correctParameters, ActionParameters action, bool error, int hints)
    {
        UserAction userAction = new UserAction
        {
            request = request,
            correctParameters = correctParameters,
            action = action,
            error = error,
            hints = hints,
            actionDuration = (DateTime.Now - startActionTime).TotalSeconds
        };
        userActions.Add(userAction);
    }


    [Serializable]
    public class Session
    {
        public string activityName;
        public bool speechBubbleEnabled;
        public float speechRate;
        public float speechPausesDuration;
        public double activityDuration;
        public List<UserAction> userActions;
    }

    [Serializable]
    public class UserAction
    {
        public List<string> request;
        public EventParameters correctParameters;
        public ActionParameters action;
        public bool error;
        public double actionDuration;
        public int hints;
    }

    [Serializable]
    public class ActionParameters
    {
        public string selectable;
        public string interactable;
        public string target;
        public string answer;
    }
}

