using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class VirtualAssistantManager : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private TextToSpeech textToSpeech;
    private GameObject speechBubble;
    private TextMeshProUGUI speechBubbleText;

    public string voice;
    public string pitch;
    private bool speechBubbleEnabled;
    private float speechRate;
    private float speechPausesDuration;

    public Transform head;

    public static VirtualAssistantManager instance
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

            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            speechBubble = transform.GetChild(0).gameObject;
            speechBubbleText = speechBubble.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            speechBubbleEnabled = PlayerPrefs.GetInt("SpeechBubble") == 0 ? false : true;
            speechRate = PlayerPrefs.GetFloat("SpeechRate");
            speechPausesDuration = PlayerPrefs.GetFloat("SpeechPausesDuration");
        }
    }


    // Start is called before the first frame update
    private void Start()
    {
        Vector3 relativePos = Camera.allCameras[0].transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;
    }

    public void Initialize()
    {
        textToSpeech = new TextToSpeech(voice, pitch, speechRate);
        ActivityManager.instance.Initialize();
    }

    public void startTalking(string instruction, string animatorParam, UnityAction call = null)
    {
        textToSpeech.SyntetizeInstruction(instruction, audioSource, () =>
        {
            animator.SetBool(animatorParam, true);
            ActivateBubble(instruction);
        }, 
        call);
    }

    public void stopTalking(string animatorParam, UnityAction call)
    {
        animator.SetBool(animatorParam, false);
        StartCoroutine(WaitAndCallback(call));
    }

    public void ActivateBubble(string text)
    {
        speechBubble.SetActive(speechBubbleEnabled && true);
        speechBubbleText.text = text;
    }

    public void DeactivateBubble()
    {
        speechBubble.SetActive(false);
        speechBubbleText.text = "";
    }

    private IEnumerator WaitAndCallback(UnityAction call)
    {
        yield return new WaitForSeconds(speechPausesDuration);
        DeactivateBubble();
        call.Invoke();
    }
}