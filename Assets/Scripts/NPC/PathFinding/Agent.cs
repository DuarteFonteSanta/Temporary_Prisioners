using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
    public float speed = 20f;
    public float turnSpeed = 3f;
    public float turnDistance = 2f;
    public float stoppingDistance = 10f;
    public Animator animator;

    [SerializeField]
    private Vector3 spawnPosition;
    
    
    [SerializeField]
    private PlayerManager playerManager;
    
    [SerializeField]
    private float caughtDistance;
    
    public const float pathRequestUpdateTime = 0.2f;
    public const float pathUpdateMoveThreshold = 0.2f;

    private Path path;
    private Vector2 targetDestination;

    [SerializeField] private bool stop;
    [SerializeField] private Vector2 agentDestination;
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private Vector2 velocity;

    [SerializeField] private float rDist;
    [SerializeField] private Grid grid;

    [SerializeField] private PathRequestManager pathRequestManager;

    public Vector2 TargetDestination => targetDestination;
    public Path Path => path;
    public Grid Grid => grid;
    public bool agentStop { get => stop; set => stop = value; }
    public Vector2 agentVelocity => velocity;

    public float remainingDistance => CalculateRemainingDistance();


    public Vector2 destination
    {
        get => agentDestination; set => agentDestination = value;
    }

    private void Awake()
    {
        agentDestination = Vector2.zero;
        velocity = Vector2.zero;
        stop = agentVelocity.normalized.magnitude == 0 ? true : false;
    }

    private void Start()
    {
        StartCoroutine(UpdatePath());
        
    }

    private void OnEnable()
    {
        agentDestination = Vector2.zero;
        velocity = Vector2.zero;
        moveDirection = Vector2.zero;
        path = null;
        stop = agentVelocity.normalized.magnitude == 0 ? true : false;
        StartCoroutine(UpdatePath());
    }

    private void Update()
    {
        
        if ((velocity.x > 0 && transform.localScale.x < 0) ||
            (velocity.x < 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
   //     Debug.Log(velocity);
        
        
        
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            animator.SetFloat("Velocity", Mathf.Abs(velocity.x));
        }
        else
        {
            animator.SetFloat("Velocity", Mathf.Abs(velocity.y));
        }

        
        
        
        rDist = CalculateRemainingDistance();
        
        
        // If In range of player arrest him 
        if (rDist < caughtDistance)
        {
            
            // Find closest player and respawn him
            GameObject player1 = GameObject.FindGameObjectWithTag("Player");
            GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
            
            if(player1 != null && player2 != null)
            {
                float distance1 = Vector3.Distance(transform.position, player1.transform.position);
                float distance2 = Vector3.Distance(transform.position, player2.transform.position);

                if (distance1 < caughtDistance || distance2 < caughtDistance)
                {
                    if (distance1 < distance2)
                    {
                        playerManager.RespawnPlayer(1);
                    }
                    else
                    {
                        playerManager.RespawnPlayer(2);
                    }

                    transform.position = spawnPosition;
                }
            }
        }
    }

    public void OnPathFound(Vector2[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //Debug.Log($"WAYPOINTS: {waypoints.Length}");
            path = new Path(waypoints, transform.position, turnDistance, stoppingDistance);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }

        else
        {
            Debug.Log("Path is not successfull!");
            Fsm fsm = GetComponent<Fsm>();
            if (fsm != null) fsm.StartPatrolling();
        }
    }

    private IEnumerator UpdatePath()
    {
        while (true)
        {
            if (agentDestination != Vector2.zero)
            {
                if (Time.timeSinceLevelLoad < 0.3f)
                {
                    yield return new WaitForSeconds(.3f);
                }
                pathRequestManager.RequestPath(new PathRequest(transform.position, agentDestination, OnPathFound));

                float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
                Vector2 destinationOldPos = agentDestination;

                while (agentDestination != Vector2.zero)
                {
                    yield return new WaitForSeconds(pathRequestUpdateTime);

                    if ((agentDestination - destinationOldPos).sqrMagnitude > sqrMoveThreshold)
                    {
                        pathRequestManager.RequestPath(new PathRequest(transform.position, agentDestination, OnPathFound));
                        destinationOldPos = agentDestination;
                    }
                }
            }

            // Wait for a short time before checking the destination again
            yield return new WaitForSeconds(0.2f);
        }
    }

    private float CalculateRemainingDistance()
    {
        if (path == null || path.lookPoints == null || path.lookPoints.Length == 0)
        {
            return 0f;
        }

        float distance = 0f;
        Vector2 currentPosition = transform.position;

        // Calculate distance from current position to the next path point
        if (path.turnBoundaries.Length > 0)
        {
            distance = Vector2.Distance(currentPosition, path.lookPoints[path.turnBoundaries.Length - 1]);
        }

        // Add distances between all remaining path points
        for (int i = path.turnBoundaries.Length - 1; i < path.lookPoints.Length - 1; i++)
        {
            distance += Vector2.Distance(path.lookPoints[i], path.lookPoints[i + 1]);
        }

        return distance;
    }


    private IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        Vector2 previousPosition = transform.position; // Store the previous position
        float speedPercent = 1f;

        while (followingPath)
        {
            if (stop)
            {
                yield return null;
                continue;
            }

            Vector2 position = new Vector2(transform.position.x, transform.position.y);

            while (path.turnBoundaries[pathIndex].HasCrossedLine(position))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {
                CalculateRemainingDistance();

                if (pathIndex >= path.slowDownIndex && stoppingDistance > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(position) / stoppingDistance);

                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                Vector2 targetDirection = (path.lookPoints[pathIndex] - position).normalized;
                moveDirection = Vector2.Lerp(moveDirection, targetDirection, Time.deltaTime * turnSpeed);

                // Update position
                position += moveDirection * speed * speedPercent * Time.deltaTime;
                transform.position = new Vector3(position.x, position.y, transform.position.z);

                // Calculate agent velocity
                velocity = (position - previousPosition) / Time.deltaTime; // Velocity is change in position over time
                previousPosition = position; // Update previous position
            }

            yield return null;
        }

        // If not following the path, ensure velocity is zero
        velocity = Vector2.zero;
    }

    public void SetDestination(Vector2 newDestination) => agentDestination = newDestination;

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }

        // Draw a line showing the direction the object is facing
        Gizmos.color = Color.red;
        Vector3 direction = transform.right; // Assuming the character is facing right by default
        Gizmos.DrawLine(transform.position, transform.position + direction * 2);
    }
}