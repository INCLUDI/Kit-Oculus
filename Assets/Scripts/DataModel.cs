﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataModel : MonoBehaviour
{
    [Serializable]
    public class CustomVector3
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class SceneObj
    {
        public string uid;
        public string path;
        public CustomVector3 position;
        public CustomVector3 rotation;
        public CustomVector3 scale;
    }

    [Serializable]
    public class AudioFeedback
    {
        public List<string> audio;
        public string audioOk;
        public string audioWrong;
    }

    [Serializable]
    public class EventParameters
    {
        public List<string> correctGrabbable;
        public List<string> wrongGrabbable;
        public List<string> correctTarget;
        public List<string> wrongTarget;
        public CustomVector3 finalPosition;
        public CustomVector3 finalRotation;
    }

    [Serializable]
    public class EventObjs
    {
        public List<SceneObj> grabbablesToActivate;
        public List<string> grabbablesToDeactivate;
        public List<SceneObj> targetsToActivate;
        public List<string> targetsToDeactivate;
        public List<SceneObj> othersToActivate;
        public List<string> othersToDeactivate;
    }

    [Serializable]
    public class EventConfiguration
    {
        public string type;
        public EventObjs sceneObjs;
        public EventParameters parameters;
        public AudioFeedback audioFeedback;
    }

    [Serializable]
    public class SceneConfiguration
    {
        public string sceneName;
        public List<EventConfiguration> events;
    }

}
