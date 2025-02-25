using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Rigidbody2D rb;
    bool facingRight = true;
    bool isGrounded;
    public LogicScript logic;
    private SpriteRenderer sr;
    

    [Header("Movement")]
    public float speed = 5f;
    float xMovement;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 2;
    public int jumpsLeft;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    public float wallSlideSpeed = 2f;
    private bool isWallSliding;

    bool isWallJumping;
    float WJDirection;
    float WJTime = 0.5f;
    float WJTimer;
    public Vector2 WJPower = new Vector2(5f, 10f);

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultip = 2f;

    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        ProcessWallSlide();
        ProcessWallJump();
        Gravity();
        
        
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(xMovement * speed, rb.linearVelocity.y);
            Flip();
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        xMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsLeft > 0)
        {
            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpPower);
                jumpsLeft--;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
                jumpsLeft--;
            }
        }

        // Wall Jump WJ
        if(context.performed && WJTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(WJDirection * WJPower.x, WJPower.y);
            WJTimer = 0;

            if (transform.localEulerAngles.x != WJDirection)
            {
                facingRight = !facingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), WJTime + 0.1f);
        }
    }

    private void ProcessWallSlide()
    {
        if (!isGrounded & WallCheck() & xMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        } 
        else 
        { 
            isWallSliding = false; 
        }
    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            WJDirection = -transform.localScale.x;
            WJTimer = WJTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (WJTimer > 0f)
        {
            WJTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(wallCheckPos.position, wallCheckSize);
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsLeft = maxJumps;
            isGrounded = true;
            
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void Gravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultip;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Flip()
    {
        if(facingRight && xMovement < 0 || !facingRight && xMovement > 0)
        {
            facingRight = !facingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyPatrolControl enemy = collision.GetComponent<EnemyPatrolControl>();
        if (enemy)
        {
            logic.AddTime(-5);
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.white;
    }

}
