using UnityEngine;

public class Lever : TooltipObject
{
    [SerializeField] private SlidingDoor door;

    public override void Interact(PlayerController player)
    {
        door.unlockDoor();
    }
}