using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoundaryManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Vector3 originalScale = other.transform.localScale;

        if (other.gameObject.tag == "Grabbable")
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(new Vector3(0, 0, 0), 0.5f));
            //sequence.AppendCallback(() => other.GetComponent<GrabbableManager>().ResetPosition());
            //sequence.Append(transform.DOScale(originalScale, 0.5f));

            transform.DORestart();
            DOTween.Play(sequence);

            other.gameObject.GetComponent<Rigidbody>().velocity = new Vector3();
        }
    }
}
