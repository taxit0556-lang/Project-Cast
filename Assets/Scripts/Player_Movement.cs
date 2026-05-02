using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 6f;
    public float jumpingPower = 12f;
    public float CyoteTime = 0.15f;

    [Header("Wall Jump")]
    public float wallJumpForceX = 10f;
    public float wallJumpForceY = 12f;
    public float wallCheckDistance = 0.6f;
    public LayerMask wallLayer;
    public float wallDetachTime = 0.15f;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.15f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform GroundCheck;

    Rigidbody2D rb;
    Vector2 movement;

    float jumpBufferCounter;
    float LastTimeGrounded;

    bool RightOnWall;
    bool LeftOnWall;

    float wallDetachTimer;

    bool isWallSticking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        // JUMP BUFFER
    
        if (Input.GetKeyDown(KeyCode.Space))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // WALL JUMP
        if (jumpBufferCounter > 0f && isWallSticking)
        {
            float inputDir = movement.x;
            float direction = (inputDir != 0) ? Mathf.Sign(inputDir) : (RightOnWall ? -1 : 1);

            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(direction * wallJumpForceX, wallJumpForceY);

            isWallSticking = false;
            wallDetachTimer = wallDetachTime;
            jumpBufferCounter = 0f;
        }
        // NORMAL JUMP
        else if (jumpBufferCounter > 0f && (IsGrounded() || LastTimeGrounded < CyoteTime))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            jumpBufferCounter = 0f;
        }

        // Jump cut
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    void FixedUpdate()
    {
        // Ground check
        if (!IsGrounded())
            LastTimeGrounded += Time.deltaTime;
        else
            LastTimeGrounded = 0;

        wallDetachTimer -= Time.deltaTime;

        bool onWall = IsOnWall();
        bool pushingIntoWall =
            (RightOnWall && movement.x > 0) ||
            (LeftOnWall && movement.x < 0);

        
        // WALL STICK STATE
       
        if (!IsGrounded() && onWall && pushingIntoWall && wallDetachTimer <= 0f)
        {
            isWallSticking = true;
        }
        else
        {
            isWallSticking = false;
        }

        if (isWallSticking)
        {
           
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;

          
            rb.position = new Vector2(rb.position.x, rb.position.y);

            return; 
        }
        else
        {
            rb.gravityScale = 1f;
        }

        // Movement
        rb.linearVelocity = new Vector2(movement.x * MoveSpeed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.7f, groundLayer);
    }

    private bool IsOnWall()
    {
        RightOnWall = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        LeftOnWall = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        return RightOnWall || LeftOnWall;
    }
}