using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualAssistantComponentFiller : MonoBehaviour
{
    public Image image;
    public Text assistantName;

    public void Fill(Sprite sprite)
    {
        image.sprite = sprite;
        assistantName.text = sprite.name;
    }
}
