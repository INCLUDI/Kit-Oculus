using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoundaryTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            OculusInteractableTrigger interactableTrigger = other.GetComponent<OculusInteractableTrigger>();
            Vector3 originalScale = interactableTrigger.initialScale;
            Vector3 initialPosition = interactableTrigger.initialPosition;
            Quaternion initialRotation = interactableTrigger.initialRotation;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(other.transform.DOScale(new Vector3(0, 0, 0), 0.5f));
            sequence.AppendCallback(() => other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3());
            sequence.AppendCallback(() => other.transform.position = initialPosition);
            sequence.AppendCallback(() => other.transform.rotation = initialRotation);
            sequence.Append(other.transform.DOScale(originalScale, 0.5f));

            DOTween.Play(sequence);
        }
    }
}
