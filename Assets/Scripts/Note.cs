using UnityEngine;

public class Note : TooltipObject
{
    [SerializeField]
    private MessageSO message;
    public override void Interact(PlayerController player)
    {
        player.CanvasHandler.DisplayMessage(message.message);
    }
}
