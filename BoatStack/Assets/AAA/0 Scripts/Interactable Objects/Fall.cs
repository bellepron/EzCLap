using UnityEngine;

public class Fall : MonoBehaviour, IInteractable
{
    [SerializeField] BoxCollider boxCollider;
    public void Interact()
    {
        Player.Instance.Fall();
        boxCollider.enabled = false;
    }
}