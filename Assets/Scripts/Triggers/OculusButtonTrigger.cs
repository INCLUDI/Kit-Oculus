using Kit.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kit.Oculus.Triggers
{
    public class OculusButtonTrigger : MonoBehaviour, IButtonTrigger
    {
        public UnityAction Call { get; set; }

        private void Start()
        {
            gameObject.AddComponent<Button>().onClick.AddListener(Call);
        }
    }
}