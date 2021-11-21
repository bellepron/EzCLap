using UnityEngine;

public class Jump : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Player.Instance.Jump();
    }
}