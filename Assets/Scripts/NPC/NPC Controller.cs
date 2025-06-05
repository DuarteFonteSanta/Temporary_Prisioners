using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private Vector2 lastDirection = Vector2.right;
    private Agent agent;
    public Agent Agent => agent;
    public Vector2 LastDirection => lastDirection;

    void Awake()
    {
        if (TryGetComponent(out Agent agent))
        {
            this.agent = agent;
        }
        else
        {
            Debug.LogError($"NPCController: No Agent component found on {gameObject.name}!");
        }
    }
    void Start()
    {

    }

    void Update()
    {
        if (!agent.agentStop) lastDirection = agent.agentVelocity.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Player2"))
        {
            gameManager.GameOver();
        }
    }
}
