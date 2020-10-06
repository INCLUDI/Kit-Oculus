using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OculusButtonTrigger : MonoBehaviour, IButtonTrigger
{
    public UnityAction Call { get; set; }

    private void Start()
    {
        gameObject.AddComponent<Button>().onClick.AddListener(Call);
    }
}
