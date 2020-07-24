using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableTrigger : TriggerBase
{
    private void OnTriggerEnter(Collider other)
    {
        ActivityManager.instance.checkCorrectAction(gameObject);
    }
}
