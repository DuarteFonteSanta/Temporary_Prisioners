using UnityEngine;
[CreateAssetMenu(fileName = "New Transition", menuName = "FSM/Transition")]
public class Transition : ScriptableObject
{
    [SerializeField] private Condition condition;
    [SerializeField] private State targetState;
    [SerializeField] private Action action;

    public bool isTriggered(Fsm fsm)
    {
        return condition.CheckCondition(fsm);
    }

    public State GetTargetState()
    {
        return targetState;
    }

    public Action GetAction()
    {
        return action;
    }

    public Condition GetCondition()
    {
        return condition;
    }
}
