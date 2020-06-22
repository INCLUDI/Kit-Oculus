using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupManager : TriggerBase
{
    public Text header;
    public TextMeshProUGUI message;
    public TextMeshProUGUI buttonText;
    public Button button;
    private UnityAction call;

    public void ActivatePopup(string header, string message, string buttonText, bool buttonEnabled, UnityAction call = null)
    {
        this.header.text = header;
        this.message.text = message;
        this.buttonText.text = buttonText;
        button.gameObject.SetActive(buttonEnabled);
        this.call = call;

        Vector3 finalPosition = Camera.allCameras[0].transform.position + Camera.allCameras[0].transform.forward;
        Vector3 startPosition = new Vector3(finalPosition.x, finalPosition.y + 5f, finalPosition.z);
        transform.position = startPosition;
        transform.rotation = GameManager.instance.Player.transform.rotation;
        gameObject.SetActive(true);
        transform.DOMoveY(finalPosition.y, 2f);
    }

    protected override void Trigger()
    {
        call?.Invoke();
    }
}
