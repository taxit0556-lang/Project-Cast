using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float jumpingPower;
    public float CyoteTime;
    public float SlamJumpWindow;

    private float SlamJumpTime;
    private bool groundJump;
    private bool CanSlamJump;
    private bool StartSlamJump;

    Rigidbody2D rb;
    Vector2 movement;

    [Header("Wall Jump Settings")]
    public float wallJumpForceX = 8f;
    public float wallJumpForceY = 12f;
    public float wallCheckDistance = 0.6f;
    public LayerMask wallLayer;

    private bool RightOnWall;
    private bool LeftOnWall;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform GroundCheck;
    private float LastTimeGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        // -----------------------
        // Jump Buffer Input
        // -----------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // -----------------------
        // Wall Jump (PRIORITY)
        // -----------------------
        if (jumpBufferCounter > 0f && IsOnWall() && !IsGrounded())
        {
            float direction = RightOnWall ? -1 : 1;
            rb.linearVelocity = new Vector2(direction * wallJumpForceX, wallJumpForceY);

            jumpBufferCounter = 0f;
        }
        // -----------------------
        // Normal + Coyote Jump
        // -----------------------
        else if (jumpBufferCounter > 0f && (IsGrounded() || LastTimeGrounded < CyoteTime))
        {
            if (CanSlamJump)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower * 1.4f);
                CanSlamJump = false;
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            }

            jumpBufferCounter = 0f;
            groundJump = true;
        }

        // -----------------------
        // Jump Cut (variable height)
        // -----------------------
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // -----------------------
        // Down Slam
        // -----------------------
        if (Input.GetKey(KeyCode.S) && !IsGrounded())
        {
            rb.AddForce(Vector2.down * 60f, ForceMode2D.Force);
            CanSlamJump = true;
            StartSlamJump = true;
        }

        FlipPlayer();
    }

    void FixedUpdate()
    {
        if (!IsGrounded())
        {
            LastTimeGrounded += Time.deltaTime;
            SlamJumpTime = 0;

            // -----------------------
            // Wall Slide
            // -----------------------
            if (IsOnWall() && rb.linearVelocity.y < 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2f);
            }
        }
        else
        {
            LastTimeGrounded = 0;
            groundJump = false;

            // Reset slam state
            StartSlamJump = false;

            if (StartSlamJump)
            {
                SlamJumpTime += Time.deltaTime;
            }

            if (SlamJumpTime > SlamJumpWindow)
            {
                CanSlamJump = false;
            }
        }

        rb.linearVelocity = new Vector2(movement.x * MoveSpeed, rb.linearVelocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.7f, groundLayer);
    }

    void FlipPlayer()
    {
        if(movement.x != 0 )
        {
            transform.localScale = new Vector2(1.7f * movement.x, transform.localScale.y);
        }
    }
//this game is GoAtEd baby

    private bool IsOnWall()
    {
        RightOnWall = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        LeftOnWall = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);

        return RightOnWall || LeftOnWall;
    }
}