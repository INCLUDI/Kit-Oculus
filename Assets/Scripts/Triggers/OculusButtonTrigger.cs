using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OculusButtonTrigger : IButtonTrigger
{
    public UnityAction Call { get; set; }
}
