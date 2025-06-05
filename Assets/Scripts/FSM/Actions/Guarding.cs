using UnityEngine;

[CreateAssetMenu(fileName = "New Action Guarding", menuName = "FSM/Action/Guarding")]
public class Guarding : Action
{
    public override void Execute(Fsm fsm)
    {
        Agent agent = fsm.Agent;
        if (agent.remainingDistance < 0.2f) requestPosition(fsm);
    }

    public void requestPosition(Fsm fsm)
    {
        int randomIndex = Random.Range(0, fsm.Waypoints.Count);
        Vector2 newPosition = fsm.Waypoints[randomIndex].position;
        fsm.Agent.SetDestination(newPosition);
        fsm.Agent.agentStop = false;
//        Debug.Log("Requested Position: " + newPosition);
    }
}
