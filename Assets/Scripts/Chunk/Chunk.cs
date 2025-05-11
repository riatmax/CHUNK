using UnityEngine;
using UnityEngine.InputSystem;

public class Chunk : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private Vector2 movementInput;

    private bool isGrounded;
    private bool wasGrounded;
    private bool jumpHeld;

    [Header("Chunk Stats")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundCheckDist;

    private void Start()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
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
    }
    private void OnJump(InputValue iv)
    {
        jumpHeld = iv.isPressed;
        Debug.Log(jumpHeld);
    }
    private void OnMove(InputValue iv)
    {
        movementInput = iv.Get<Vector2>();
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //jumpHeld = false;
    }
}
