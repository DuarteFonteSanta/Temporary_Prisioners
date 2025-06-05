using UnityEngine;

public class InteractableObject<T> : MonoBehaviour, IInteractable<T>
{
    public virtual void Interact(T player)
    {

    }
}
