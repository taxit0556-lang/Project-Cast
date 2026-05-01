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

    [Header("WallJump")]
    private bool RightOnWall;
    private bool LeftOnWall;
    public bool CanClimbRight;
    public bool CanClimbLeft;
    private float RayLength = 1f;

    [Header("GroundCheck")]
    [SerializeField]private LayerMask groundLayer;
    [SerializeField]private Transform GroundCheck;
    private float LastTimeGrounded;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");

        //normal Jump
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            groundJump = true;
        }
        if(Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            groundJump = true;
        }

        //SlamJump
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded() && CanSlamJump == true)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower * 1.4f);
            groundJump = true;
            CanSlamJump = false;
        }
        if(Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            groundJump = true;
            CanSlamJump = false;
        }
        

        //Cyo Time
        if(LastTimeGrounded < CyoteTime && !IsGrounded() && groundJump != true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            }

            if(Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }


        //DownForce
        if(Input.GetKey(KeyCode.S) && !IsGrounded())
        {
            rb.AddForce(Vector2.down * 60f,ForceMode2D.Force);
            CanSlamJump = true;
            StartSlamJump = true;
        }
    }

    void FixedUpdate()
    {
        if (!IsGrounded())
        {
            LastTimeGrounded += 1 * Time.deltaTime;

            SlamJumpTime = 0;
        }
        else
        {
            LastTimeGrounded = 0;

            groundJump = false;
            
            //Slamtimer
            if(StartSlamJump = true)
            {
                SlamJumpTime += 1 * Time.deltaTime;
            }
            if(SlamJumpTime > SlamJumpWindow)
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
}