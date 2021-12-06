using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Player.Instance.Multiplier();
    }
}