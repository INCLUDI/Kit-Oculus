using System;
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
    public class AudioFeedback
    {
        public string audioOk;
        public string audioWrong;
    }

    [Serializable]
    public class EventParameters
    {
        public List<string> correct;
        public List<string> wrong;
        public CustomVector3 finalPosition;
        public CustomVector3 finalRotation;
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
    public class EventConfiguration
    {
        public string type;
        public List<string> objsToDeactivate;
        public List<SceneObj> objsToActivate;
        public List<string> targetsToDeactivate;
        public List<SceneObj> targetsToActivate;
        public EventParameters parameters;
        public List<string> audio;
        public AudioFeedback audioFeedback;
    }

    [Serializable]
    public class SceneConfiguration
    {
        public string sceneName;
        public List<EventConfiguration> events;
    }

}
