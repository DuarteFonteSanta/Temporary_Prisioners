using UnityEngine;

[CreateAssetMenu(fileName = "New Action Guarding", menuName = "FSM/Action/Patrol/StartGuarding")]
public class StartGuarding : Action
{
    public override void Execute(Fsm fsm)
    {
        Agent agent = fsm.Agent;
        Vector2 newPosition = fsm.Waypoints[Random.Range(0, fsm.Waypoints.Count)].position;
        agent.SetDestination(newPosition);
        agent.agentStop = false;
    }
}
