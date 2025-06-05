using UnityEngine;
[CreateAssetMenu(fileName = "New Action", menuName = "FSM/Action/Chase/Chasing")]
public class Chasing : Action
{
    public override void Execute(Fsm fsm)
    {
        if (fsm.Fov.GetTarget() != null)
        {
            fsm.Agent.SetDestination(fsm.Fov.GetTarget().transform.position);
        }
    }
}
