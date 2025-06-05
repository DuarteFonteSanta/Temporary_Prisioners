using UnityEngine;
[CreateAssetMenu(fileName = "New Condition", menuName = "FSM/Condition/Target Lost")]
public class InvestigationEnded : Condition
{
    public override bool CheckCondition(Fsm fsm) => fsm.Agent.remainingDistance <= 0.1f && fsm.Fov.GetTarget() == null;
}
