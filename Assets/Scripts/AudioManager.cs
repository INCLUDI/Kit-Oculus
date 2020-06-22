using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static DataModel;

public class AudioManager : MonoBehaviour
{

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


    public void PlayAudioAndWait(AudioSource source, AudioClip audio, UnityAction call = null)
    {
        source.PlayOneShot(audio);
        StartCoroutine(waitForAudioCompleted(audio.length, call));
    }

    private IEnumerator waitForAudioCompleted(float clipLength, UnityAction call = null)
    {
        yield return new WaitForSeconds(clipLength);
        call?.Invoke();
    }
}