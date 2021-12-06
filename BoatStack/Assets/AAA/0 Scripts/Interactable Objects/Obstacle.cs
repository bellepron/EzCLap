using UnityEngine;

public class Obstacle : MonoBehaviour, IInteractable
{
    [SerializeField] int height;
    [SerializeField] BoxCollider boxCollider;

    public void Interact()
    {
        Player.Instance.Obstacle(height);
        boxCollider.isTrigger = false;
        // boxCollider.enabled = false;
    }
}