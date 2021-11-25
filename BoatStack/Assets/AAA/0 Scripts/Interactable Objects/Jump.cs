using UnityEngine;

public class Jump : MonoBehaviour, IInteractable
{
    [SerializeField] BoxCollider boxCollider;
    public void Interact()
    {
        Player.Instance.Jump();
        boxCollider.enabled = false;
    }
}