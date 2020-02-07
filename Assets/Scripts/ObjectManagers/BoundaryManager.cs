using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoundaryManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Grabbable")
        {
            Vector3 originalScale = other.transform.localScale;
            Vector3 initialPosition = other.GetComponent<GrabbableManager>().initialPosition;
            Quaternion initialRotation = other.GetComponent<GrabbableManager>().initialRotation;

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
