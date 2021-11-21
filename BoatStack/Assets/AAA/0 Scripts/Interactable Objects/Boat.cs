using UnityEngine;

public class Boat : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Player.Instance.Boat(gameObject);
        Player.Instance.MakeCharacterHappy();
    }
}