using System;
using UnityEngine;

public class KnightController : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float runningSpeed = 5f;
    public float jumpForce = 5f;
    public static KnightController Instance;
    
    private Rigidbody2D _rigidbody2D;
    private float _horizontalInput;

    void Awake()
    {  
        Instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = _horizontalInput;
        if (!(horizontalInput == 0))
        {
            spriteRenderer.flipX = horizontalInput < 0f;
            
            var targetVelocityX = horizontalInput * runningSpeed;
            _rigidbody2D.linearVelocity = new Vector2(targetVelocityX, _rigidbody2D.linearVelocity.y);
            animator.SetBool("isRunning", true);
        }else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

}
