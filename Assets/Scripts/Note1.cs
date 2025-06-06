using UnityEngine;

public class Note1 : TooltipObject
{
    [SerializeField]
    private GameWin gamewin;
    public override void Interact(PlayerController player)
    {
        gamewin.Win2();
    }
}
