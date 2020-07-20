using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StatsManager;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager instance
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

    public void PostSession(Session session)
    {
        Debug.Log(session.activityName);
        Debug.Log(session.activityDuration);
        foreach (var v in session.userActions)
            Debug.Log(v.request);
    }
}
