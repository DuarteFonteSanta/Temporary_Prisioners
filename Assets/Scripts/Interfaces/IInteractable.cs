using UnityEngine;

public interface IInteractable<T>
{
    public abstract void Interact(T player);
}
