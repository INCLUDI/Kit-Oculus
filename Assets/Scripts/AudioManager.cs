using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    public static AudioManager instance
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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playAudioFromString(string audioName, UnityAction call = null)
    {
        EventManager.TriggerEvent("startTalking");
        AudioClip audioClip = (AudioClip)Resources.Load(audioName);
        audioSource.PlayOneShot(audioClip);
        StartCoroutine(waitForAudioCompleted(audioClip.length, call));
    }

    private IEnumerator waitForAudioCompleted(float clipLength, UnityAction call = null)
    {
        yield return new WaitForSeconds(clipLength);
        {
            call?.Invoke();
        }
    }

}
