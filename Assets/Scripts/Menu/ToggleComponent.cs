using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleComponent : MonoBehaviour
{
    public string parameter;
    public Toggle toggle;

    private void Awake()
    {
        toggle.isOn = PlayerPrefs.GetInt(parameter) == 0 ? false : true;
        toggle.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(bool val)
    {
        PlayerPrefs.SetInt(parameter, val == false ? 0 : 1);
        PlayerPrefs.Save();
    }
}