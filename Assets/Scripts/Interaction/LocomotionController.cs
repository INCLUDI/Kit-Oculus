using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Kit.Oculus.Interaction
{
    public class LocomotionController : MonoBehaviour
    {
        public XRController leftTeleportRay;
        public XRController rightTeleportRay;
        public InputHelpers.Button teleportActivationButton;
        public float activationTreshold = 0.1f;

        public XRRayInteractor leftInteractorRay;
        public XRRayInteractor rightInteractorRay;

        public bool EnableLeftTeleport { get; set; } = true;
        public bool EnableRightTeleport { get; set; } = true;

        // Update is called once per frame
        void Update()
        {
            if (leftTeleportRay)
            {
                bool isLeftInteractorRayHovering = leftInteractorRay.TryGetHitInfo(out _, out _, out _, out _);
                leftTeleportRay.gameObject.SetActive(EnableLeftTeleport && CheckIfActivated(leftTeleportRay) && !isLeftInteractorRayHovering);
            }

            if (rightTeleportRay)
            {
                bool isRightInteractorRayHovering = rightInteractorRay.TryGetHitInfo(out _, out _, out _, out _);
                rightTeleportRay.gameObject.SetActive(EnableRightTeleport && CheckIfActivated(rightTeleportRay) && !isRightInteractorRayHovering);
            }
        }

        public bool CheckIfActivated(XRController controller)
        {
            InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationTreshold);
            return isActivated;
        }
    }
}