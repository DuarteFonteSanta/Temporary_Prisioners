using UnityEngine;
using System.Collections;

public class FOV : MonoBehaviour
{
    [Header("FOV Settings: ")]
    [SerializeField] protected float fovRadius;
    [Range(0, 360)][SerializeField] protected float angle;
    [SerializeField] protected float fovDelay;
    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected GameObject target;

    private Vector2 detectPosition;
    public Vector2 DetectPosition { get => detectPosition; set => detectPosition = value; }
    protected bool isEnabled = true;

    private NPCController controller;

    public float Radius => fovRadius;
    public float Angle => angle;
    public NPCController Controller => controller;

    public Vector2 TargetDirection
    {
        get
        {
            if (target != null)
                return DirectionToTarget();

            else
            {
                Debug.LogWarning($"Target is null!");
                return Vector2.zero;
            }
        }
    }

    public float TargetDistance
    {
        get
        {
            if (target != null)
                return DistanceToTarget();

            else
            {
                Debug.LogWarning($"Target is null!");
                return DistanceToTarget();
            }
        }
    }

    public bool IsEnabled { get => isEnabled; set => isEnabled = value; }

    protected void Awake()
    {
        isEnabled = true;
        if (TryGetComponent(out NPCController controller))
        {
            this.controller = controller;
        }
        else
        {
            Debug.LogError($"FOV: No NPCController component found on {gameObject.name}!");
        }
    }
    // Start is called before the first frame update
    protected void Start()
    {
        StartCoroutine(FOVRoutine(fovDelay));
    }

    protected void OnEnable()
    {
        StartCoroutine(FOVRoutine(fovDelay));
    }

    private void PerformFov()
    {
        if (target == null && isEnabled)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fovRadius, targetMask);

            if (colliders.Length != 0)
            {
                foreach (Collider2D collider in colliders)
                {
                    GameObject potentialTarget = collider.gameObject;
                    Vector2 dirToTarget = (potentialTarget.transform.position - transform.position).normalized;
                    Vector3 facingDir = controller.LastDirection;

                    // Check if the target is within the specified angle
                    if (Vector2.Angle(facingDir, dirToTarget) < angle / 2)
                    {
                        target = potentialTarget;
                        detectPosition = potentialTarget.transform.position;
                        Debug.Log("Target detected within angle!");
                        return; // Exit after finding the first valid target
                    }
                }
            }
        }
    }

    private IEnumerator FOVRoutine(float seconds)
    {
        while (true)
        {
            WaitForSeconds wait = new WaitForSeconds(seconds);
            yield return wait;
            PerformFov();
        }
    }

    private Vector2 DirectionToTarget() => (target.transform.position - transform.position).normalized;

    private float DistanceToTarget() => (target.transform.position - transform.position).magnitude;

    public GameObject GetTarget() => target;

    public void SetTarget(GameObject target) => this.target = target;
    public void ClearTarget() => target = null;
}
