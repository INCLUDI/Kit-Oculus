using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kit.Oculus.Interaction
{
    public class RightHandInteractionManager : MonoBehaviour
    {
        public void RightHandSelectionBegin()
        {
            EventManager.TriggerEvent("RightHandInteractionBegin");
        }

        public void RightHandSelectionEnd()
        {
            EventManager.TriggerEvent("RightHandInteractionEnd");
        }
    }
}
