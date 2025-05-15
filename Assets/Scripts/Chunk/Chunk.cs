using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class Chunk : MonoBehaviour
{
    public Rigidbody2D rb;
    private PlayerControls playerControls;
    private Vector2 movementInput;
    private SpriteRenderer sprRend;

    public bool isGrounded;
    private bool facingLeft = false;

    private int currJump = 1;

    [Header("Chunk Stats")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundCheckDist;
    [SerializeField] private float gravity = 1;
    [SerializeField] private int maxNumJumps;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sprRend = GetComponentInChildren<SpriteRenderer>();
        rb.gravityScale = gravity;
    }
    private void FixedUpdate()
    {
        float horizontalVelocity = Mathf.Abs(movementInput.x) > 0.01f ? movementInput.x * moveSpeed : 0f;
        rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDist, LayerMask.GetMask("Ground"));
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckDist, 0), Color.green);
        if (facingLeft)
        {
            sprRend.flipX = true;
        }
        else
        {
            sprRend.flipX = false;
        }

        if (isGrounded)
        {
            currJump = 0;
        }
    }
    private void Update()
    {
       
    }
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && (isGrounded || (currJump != maxNumJumps)))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            currJump++;
        }
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        if (Mathf.Abs(movementInput.x) > 0.1f)
        {
            facingLeft = Mathf.Sign(movementInput.x) < 0;
        }
    }

   /* private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }*/
}
