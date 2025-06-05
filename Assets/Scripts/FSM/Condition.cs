using UnityEngine;
[CreateAssetMenu(fileName = "New Condition", menuName = "FSM/Condition")]
public abstract class Condition : ScriptableObject
{
    public abstract bool CheckCondition(Fsm fsm);
}
