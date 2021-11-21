using UnityEngine;

public class Simit : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Player.Instance.Boat(gameObject);
    }
}