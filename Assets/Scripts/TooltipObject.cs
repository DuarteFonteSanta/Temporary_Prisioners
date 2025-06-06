using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TooltipObject : InteractableObject<PlayerController>
{
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private Tooltip tooltip2;

    public SlidingDoor slidingdoor;

    public override void Interact(PlayerController player)
    {
        if (slidingdoor.isLocked && Input.GetKeyDown(player.GetControl("Interact")))
        {
            if (player.controls.TryGetValue("Interact", out var value))
            {
                player.CanvasHandler.DisplayToolTip(tooltip.GetToolTipText(value.code));
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player2") || collider.CompareTag("Player"))
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player.controls.TryGetValue("Interact", out var value))
            {
                player.CanvasHandler.DisplayToolTip(tooltip.GetToolTipText(value.code));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player2") || other.CompareTag("Player"))
        {
            PlayerController player2 = other.GetComponent<PlayerController>();
            player2.CanvasHandler.ShowToolTip(false);
        }

    }
}
