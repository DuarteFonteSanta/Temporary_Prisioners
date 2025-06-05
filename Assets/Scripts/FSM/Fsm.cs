using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Fsm : MonoBehaviour
{
    [SerializeField] private State initialState;
    [SerializeField] private State currentState;
    [SerializeField] private List<Transform> waypoints;
    public List<Transform> Waypoints => waypoints;
    private Agent agent;
    private FOV fov;
    public FOV Fov => fov;
    public Agent Agent => agent;

    [SerializeField] private State patrolState;
    public State PatrolState => patrolState;

    void Awake()
    {
        agent = GetComponent<Agent>();
        fov = GetComponent<FOV>();
    }

    void Start()
    {
        if (initialState != null)
        {
            currentState = initialState;
            currentState.EnterState(this);
        }

        else
        {
       //     Debug.LogError("Initial state is not set in the FSM.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null && currentState.GetActions().Count > 0)
        {
       //     Debug.Log("Executing actions in state: " + currentState.name);
            foreach (var action in currentState.GetActions())
            {
//                Debug.Log("Executing action: " + action.name);
                action.Execute(this);
            }
        }
        Transition triggeredTransition = currentState.GetTransition(this);
        if (triggeredTransition != null) ChangeState(triggeredTransition.GetTargetState());
    }

    public void ChangeState(State newState)
    {
        if (newState != null)
        {
            currentState = newState;
      //      Debug.Log("State changed to: " + newState.name);
        }
        else
        {
      //      Debug.LogError("New state is null. Cannot change state.");
        }
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public void StartPatrolling()
    {
        if (patrolState != null && Waypoints.Count > 0)
        {
            ChangeState(patrolState);
        }
    }
}
