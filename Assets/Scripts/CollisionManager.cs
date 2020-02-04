using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private ActivityManager activityManager;

    private void Start()
    {
        activityManager = GameObject.Find("ActivityManager").GetComponent<ActivityManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ciao");
        activityManager.checkCorrectObject(other.gameObject);
    }
}
