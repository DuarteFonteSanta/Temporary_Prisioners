using UnityEngine;
[CreateAssetMenu(fileName = "New Action", menuName = "FSM/Action/Investigate/Investigate")]
public class Investigate : Action
{
    public override void Execute(Fsm fsm)
    {
        fsm.Agent.SetDestination(fsm.Fov.DetectPosition);
        //fsm.Fov.ClearTarget();
    }
}
