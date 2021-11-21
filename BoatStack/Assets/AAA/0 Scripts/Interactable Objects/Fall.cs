using UnityEngine;

public class Fall : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Player.Instance.Fall();
    }
}