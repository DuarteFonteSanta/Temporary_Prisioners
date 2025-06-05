using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "FSM/Action")]
public abstract class Action : ScriptableObject
{
    public abstract void Execute(Fsm fsm);

}
