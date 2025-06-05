using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "FSM/State")]
public class State : ScriptableObject
{
    [SerializeField] private List<Action> entryActions;
    [SerializeField] private List<Action> actions;
    [SerializeField] private List<Transition> transitions;

    public List<Action> GetEntryActions() => entryActions;

    public List<Action> GetActions() => actions;

    public List<Transition> GetTransitions() => transitions;

    public void EnterState(Fsm fsm)
    {
        if (entryActions.Count > 0)
        {
            foreach (var action in entryActions)
            {
                action.Execute(fsm);
            }
        }
    }

    public Transition GetTransition(Fsm fsm)
    {
        if (transitions.Count > 0)
        {
            foreach (var transition in transitions)
            {
                if (transition.isTriggered(fsm)) return transition;
            }
        }
        return null;
    }
}
