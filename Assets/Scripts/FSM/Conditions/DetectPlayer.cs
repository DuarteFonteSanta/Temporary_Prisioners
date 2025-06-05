using UnityEngine;

[CreateAssetMenu(fileName = "New Condition", menuName = "FSM/Condition/Detect Player")]
public class DetectPlayer : Condition
{
    public override bool CheckCondition(Fsm fsm) => fsm.Fov.GetTarget() != null && fsm.Fov.TargetDistance >= fsm.Fov.Radius - 0.1f;

}