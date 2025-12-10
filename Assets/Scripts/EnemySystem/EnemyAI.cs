using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Configuración General")]
    public float speed = 2f;
    public float chaseSpeed = 3.5f;

    [Header("Referencias IA")]
    public Transform pointA;
    public Transform pointB;
    public Transform playerTarget;

    [Header("Sensores")]
    public float detectionRange = 4f;
    public float lostRange = 6f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    
    private IEnemyState _currentState;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    private void Start()
    {
        ChangeState(new EnemyPatrolState(this));
    }

    private void FixedUpdate()
    {
        _currentState?.Tick();
    }

    public void ChangeState(IEnemyState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
    
}
