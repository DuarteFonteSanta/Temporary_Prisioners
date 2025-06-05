using UnityEngine;
[CreateAssetMenu(fileName = "New Condition", menuName = "FSM/Condition/Can See Player")]
public class CanSeePlayer : Condition
{
    public override bool CheckCondition(Fsm fsm) => fsm.Fov.GetTarget() != null && fsm.Fov.TargetDistance < fsm.Fov.Radius - 0.2f;

}
