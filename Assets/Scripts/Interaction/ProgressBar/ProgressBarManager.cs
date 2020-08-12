using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YAProgressBar;
using DG.Tweening;

public class ProgressBarManager : MonoBehaviour
{
    private LinearProgressBar progressBar;
    private Material[] materials;

    private Transform leftHand;
    private Transform rightHand;

    private bool isGrabbingLeft;
    private bool isGrabbingRight;

    private int maxValue;
    public int MaxValue
    {
        get => maxValue;
        set
        {
            maxValue = value;
            progressBar.ElementsCount = value * 100;
        }
    }

    public float currentValue;
    public float CurrentValue 
    {
        get => currentValue;
        set
        {
            currentValue = value;
            progressBar.FillAmount = value / MaxValue;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponent<LinearProgressBar>();
        materials = GetComponent<Renderer>().materials;

        leftHand = GameObject.Find("Left Hand").transform;
        rightHand = GameObject.Find("Right Hand").transform;

        EventManager.StartListening("LeftHandInteractionBegin", LeftHandInteractionBegin);
        EventManager.StartListening("LeftHandInteractionEnd", LeftHandInteractionEnd);
        EventManager.StartListening("RightHandInteractionBegin", RightHandInteractionBegin);
        EventManager.StartListening("RightHandInteractionEnd", RightHandInteractionEnd);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbingLeft)
        {
            transform.position = new Vector3(leftHand.position.x, leftHand.position.y + 0.5f, leftHand.position.y);
            transform.rotation = Quaternion.Euler(leftHand.position.x, leftHand.position.y, leftHand.position.y);
        }
        if (isGrabbingRight)
        {
            transform.position = new Vector3(rightHand.position.x, rightHand.position.y + 0.5f, rightHand.position.y);
            transform.rotation = Quaternion.Euler(rightHand.position.x, rightHand.position.y, rightHand.position.y);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening("LeftHandInteractionBegin", LeftHandInteractionBegin);
        EventManager.StopListening("LeftHandInteractionEnd", LeftHandInteractionEnd);
        EventManager.StopListening("RightHandInteractionBegin", RightHandInteractionBegin);
        EventManager.StopListening("RightHandInteractionEnd", RightHandInteractionEnd);
    }

    private void LeftHandInteractionBegin()
    {
        isGrabbingLeft = true;
        foreach (Material material in materials)
        {
            material.DOFade(1f, 0.5f);
        }
    }

    private void LeftHandInteractionEnd()
    {
        isGrabbingLeft = false;
        foreach (Material material in materials)
        {
            material.DOFade(0f, 1f);
        }
    }

    private void RightHandInteractionBegin()
    {
        isGrabbingRight = true;
        foreach (Material material in materials)
        {
            material.DOFade(1f, 0.5f);
        }
    }

    private void RightHandInteractionEnd()
    {
        isGrabbingRight = false;
        foreach (Material material in materials)
        {
            material.DOFade(0f, 1f);
        }
    }
}
