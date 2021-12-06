using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class SpeedBurst : MonoBehaviour, IInteractable
{
    // TODO: level scriptableından çek cd'yi ve güçlendirmeyi.
    public void Interact()
    {
        FindObjectOfType<SplineFollower>().followSpeed += 10;
        StartCoroutine(BackToNormalSpeed());

        gameObject.SetActive(false);
    }

    IEnumerator BackToNormalSpeed()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<SplineFollower>().followSpeed -= 10;
    }
}