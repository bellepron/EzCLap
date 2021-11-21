using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        PointCalculator.Instance.AddDiamond();
        gameObject.SetActive(false);
    }
}