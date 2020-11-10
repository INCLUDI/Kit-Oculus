using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kit.Oculus.Interaction
{
    public class LeftHandInteractionManager : MonoBehaviour
    {
        public void LeftHandSelectionBegin()
        {
            EventManager.TriggerEvent("LeftHandInteractionBegin");
        }

        public void LeftHandSelectionEnd()
        {
            EventManager.TriggerEvent("LeftHandInteractionEnd");
        }
    }
}