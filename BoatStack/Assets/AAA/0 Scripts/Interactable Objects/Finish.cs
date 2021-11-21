using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour, IInteractable
{
    [SerializeField] BoxCollider boxCollider;

    public void Interact()
    {
        boxCollider.enabled = false;
        Player.Instance.Win();
    }
}