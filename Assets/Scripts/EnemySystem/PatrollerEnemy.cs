using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SpriteRenderer))]
public class PatrollerEnemy : MonoBehaviour
{
 [Header("Patrol Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float waitAtPoint = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform target;
    private float waitTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            var patrol = transform.Find("PatrolPoints");
            if (patrol != null)
            {
                pointA = patrol.Find("PointA");
                pointB = patrol.Find("PointB");
            }
        }

        target = pointB != null ? pointB : pointA;
    }

    private void FixedUpdate()
    {
        if (pointA == null || pointB == null || target == null) return;

        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 dir = (target.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);

        if (dir.x != 0) sr.flipX = dir.x < 0;

        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
            waitTimer = waitAtPoint;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            Debug.Log("Player tocó al Slime. Luego conectamos vidas/checkpoint.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.08f);
            Gizmos.DrawSphere(pointB.position, 0.08f);
        }
    }
}
