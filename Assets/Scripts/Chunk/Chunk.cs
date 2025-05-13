using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class Chunk : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private Vector2 movementInput;
    private SpriteRenderer sprRend;

    private bool isGrounded;
    private bool jumpHeld;
    private bool facingLeft = false;

    [Header("Chunk Stats")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundCheckDist;
    [SerializeField] private float gravity = 1;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sprRend = GetComponentInChildren<SpriteRenderer>();
        rb.gravityScale = gravity;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);

        if (isGrounded && jumpHeld)
        {
            Jump();
        }
    }
    private void Update()
    {
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
    }
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            jumpHeld = true;
        }
        else
        {
            jumpHeld = false;
        }
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        if (ctx.ReadValue<Vector2>().x > 0)
        {
            facingLeft = false;
        } 
        if (ctx.ReadValue<Vector2>().x < 0)
        {
            facingLeft = true;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
