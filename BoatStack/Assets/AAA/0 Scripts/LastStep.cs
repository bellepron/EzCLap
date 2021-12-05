using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStep : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Player.Instance.LastStep();
    }
}