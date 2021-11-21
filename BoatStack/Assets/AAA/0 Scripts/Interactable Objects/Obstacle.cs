using UnityEngine;

public class Obstacle : MonoBehaviour, IInteractable
{
    [SerializeField] int height;
    [SerializeField] BoxCollider boxCollider;

    public void Interact()
    {
        boxCollider.enabled = false;
        Player.Instance.Obstacle(height);
    }
}