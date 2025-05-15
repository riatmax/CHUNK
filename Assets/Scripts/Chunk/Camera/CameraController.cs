using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Chunk player;
    private Vector3 targetPoint = Vector3.zero;

    private bool isFalling;

    private float lookOffset;

    [Header("Camera Stats")]
    [SerializeField] private float damping;
    [SerializeField] private float maxVertOffset;
    [SerializeField] private float lookAheadSpeed;
    [SerializeField] private float lookAheadDist;
    [SerializeField] private float cameraVertOffset;
    [SerializeField] private float fallingCamDist;

    private void Awake()
    {
        player = FindAnyObjectByType<Chunk>();
    }

    private void Start()
    {
        targetPoint = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    private void LateUpdate()
    {
        if (player.isGrounded)
        {
            targetPoint.y = player.transform.position.y + cameraVertOffset;
        }

        if (transform.position.y - player.transform.position.y > maxVertOffset)
        {
            isFalling = true;
        }
        if (isFalling)
        {
            targetPoint.y = player.transform.position.y - fallingCamDist;

            if (player.isGrounded)
            {
                isFalling = false;
            }
        }

        if (player.rb.linearVelocity.x > 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, lookAheadDist, lookAheadSpeed * Time.deltaTime);
        }
        if (player.rb.linearVelocity.x < 0f)
        {
            lookOffset = Mathf.Lerp(lookOffset, -lookAheadDist, lookAheadSpeed * Time.deltaTime);
        }
        targetPoint.x = player.transform.position.x + lookOffset;

        transform.position = Vector3.Lerp(transform.position, targetPoint, damping * Time.deltaTime);
    }
}
