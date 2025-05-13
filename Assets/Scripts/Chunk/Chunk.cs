using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class Chunk : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private Vector2 movementInput;
    private SpriteRenderer sprRend;

    private CinemachineCamera[] camContainer;
    private bool isGrounded;
    private bool jumpHeld;
    private bool facingLeft = false;

    [Header("Chunk Stats")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundCheckDist;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        sprRend = GetComponentInChildren<SpriteRenderer>();
        camContainer = GetComponentInChildren<CameraContainer>().Cameras;
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
            camContainer[0].Priority = 0;
            camContainer[1].Priority = 1;
        }
        else
        {
            sprRend.flipX = false;
            camContainer[1].Priority = 0;
            camContainer[0].Priority = 1;
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
