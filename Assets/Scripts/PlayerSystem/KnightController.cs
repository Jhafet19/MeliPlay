using System;
using UnityEngine;

// 1. Requerimos AudioSource para que suene
[RequireComponent(typeof(AudioSource))]
public class KnightController : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public float knockbackForce = 5f;
    public float stunTime = 0.5f;
    private bool _isHurt = false; 
    
    [Header("Configuracion de Componentes")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Collider2D standingCollider;
    public Collider2D rollingCollider; 
    
    [Header("Parametros de Movimiento")]
    public float runningSpeed = 5f;
    public float jumpForce = 5f;
    
    [Header("Layer de Suelo")]
    public LayerMask groundLayer;
    [HideInInspector] public bool isGrabbingBox = false;

    [Header("Audio SFX")]
    public AudioClip jumpSound;
    public AudioClip stepSound;
    private AudioSource _audioSource; 
    
    public static KnightController Instance;
    
    private Rigidbody2D _rigidbody2D;
    private bool _isRolling = false;


    void OnEnable()
    {
        EventManager.Subscribe(GlobalEvents.OnPlayerDeath, Die);
    }

    void OnDisable()
    {
        EventManager.Unsubscribe(GlobalEvents.OnPlayerDeath, Die);
    }

    void Awake()
    {  
        Instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        animator.SetBool("isDeath", false);
    }

    void Update()
    {
        if (_isHurt) return;
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 1. DETECCIÓN DE INTENCIÓN DE RODAR
        if (verticalInput < 0 && IsTouchingTheGround())
        {
            if (!_isRolling) StartRoll(); 
        }
        else
        {
            if (_isRolling) EndRoll(); 
        }

        // 2. MOVIMIENTO Y SALTO
        if (_isRolling)
        {
            HandleRollingMovement(horizontalInput);
        }
        else
        {
            HandleNormalMovement(horizontalInput);
            if (Input.GetKeyDown(KeyCode.Space) && !isGrabbingBox) Jump();
        }
    }
    
    void StartRoll()
    {
        _isRolling = true;
        animator.SetBool("isRolling", true);
        standingCollider.enabled = false;
        rollingCollider.enabled = true;
    }

    void EndRoll()
    {
        _isRolling = false;
        animator.SetBool("isRolling", false);
        rollingCollider.enabled = false;
        standingCollider.enabled = true;
    }

    void HandleNormalMovement(float xInput)
    {
        if (xInput != 0)
        {
            spriteRenderer.flipX = xInput < 0;
            float currentSpeed = runningSpeed;
            if (isGrabbingBox) currentSpeed = runningSpeed * 0.5f;
            _rigidbody2D.linearVelocity = new Vector2(xInput * currentSpeed, _rigidbody2D.linearVelocity.y);
            animator.SetBool("isRunning", true);
        }
        else
        {
            _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocity.y);
            animator.SetBool("isRunning", false);
        }
    }

    void HandleRollingMovement(float xInput)
    {
        if (xInput != 0)
        {
             spriteRenderer.flipX = xInput < 0;
            _rigidbody2D.linearVelocity = new Vector2(xInput * runningSpeed, _rigidbody2D.linearVelocity.y);
        }
        else
        {
             _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocity.y);
        }
    }

    void Jump()
    {
        if (IsTouchingTheGround())
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            if(jumpSound != null && _audioSource != null)
                _audioSource.PlayOneShot(jumpSound);
        }
    }
    
    bool IsTouchingTheGround()
    {
        Collider2D currentCol = _isRolling ? rollingCollider : standingCollider;
        float extraHeight = 0.1f;
        
        RaycastHit2D hit = Physics2D.BoxCast(
            currentCol.bounds.center, 
            currentCol.bounds.size, 
            0f, 
            Vector2.down, 
            extraHeight, 
            groundLayer
        );

        return hit.collider != null;
    }
    
    public void GetHit(Vector2 directionOfHit)
    {
        if (_isHurt) return; 
        _isHurt = true;
        animator.SetBool("isHit", true);

        _rigidbody2D.linearVelocity = Vector2.zero; 
        Vector2 knockbackDirection = (transform.position - (Vector3)directionOfHit).normalized;
        
        Vector2 finalForce = new Vector2(knockbackDirection.x, 0.5f) * knockbackForce;
        
        _rigidbody2D.AddForce(finalForce, ForceMode2D.Impulse);

        Invoke("RecoverFromHit", stunTime);
    }
    
    void RecoverFromHit()
    {
        _isHurt = false;
        animator.SetBool("isHit", false);
    }
    
    void Die(){
        _isHurt = true;
        _rigidbody2D.linearVelocity = Vector2.zero;
        animator.SetBool("isDeath", true);
    }
    
    public void PlayStepSound()
    {
        if (!IsTouchingTheGround()) return;
        if(stepSound != null && _audioSource != null)
            _audioSource.PlayOneShot(stepSound);
    }   
}