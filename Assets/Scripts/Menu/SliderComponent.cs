using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderComponent : MonoBehaviour
{
    public string parameter;
    public TextMeshProUGUI text;
    public Slider slider;
    public float multiplier;
    private string _text;

    private void Awake()
    {
        _text = text.text;
        slider.value = PlayerPrefs.GetFloat(parameter) / multiplier;
        text.text = _text.Replace("#", (slider.value * multiplier).ToString());
        slider.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(float val)
    {
        PlayerPrefs.SetFloat(parameter, val * multiplier);
        PlayerPrefs.Save();
        text.text = _text.Replace("#", (val * multiplier).ToString()); 
    }
}