using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBurst : MonoBehaviour, IInteractable
{
    // TODO: level scriptableından çek cd'yi ve güçlendirmeyi.
    public void Interact()
    {
        Follower.Instance.speed *= 4;
        StartCoroutine(BackToNormalSpeed());

        gameObject.SetActive(false);
    }

    IEnumerator BackToNormalSpeed()
    {
        yield return new WaitForSeconds(2);
        Follower.Instance.speed /= 4;
    }
}