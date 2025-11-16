using UnityEngine;
using UnityEngine.InputSystem;

public class CapsulePlayerMove : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 8f;              // move speed

    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private bool isGrounded = true;

    [Header("Jump Settings")]
    public float jumpForce = 5f;

    [Tooltip("Time after leaving ground where jump is still allowed")]
    public float coyoteTime = 0.15f;
    [Tooltip("Time before landing where a jump press will be buffered")]
    public float jumpBufferTime = 0.15f;

    private float coyoteTimer = 0f;
    private float jumpBufferTimer = 0f;

    [Header("Teleport")]
    public float teleportDistance = 10f;

    [Header("Health")]
    [SerializeField] private float StartingHealth = 100f;
    private float health;

    public float distanceInFront = 1.5f;   // kept for your use

    public float Health
    {
        get => health;
        set
        {
            health = value;
            Debug.Log(health);

            if (health <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        Health = StartingHealth;
        rb = GetComponent<Rigidbody>();

        // Prevent tipping over
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;

        // Optional: auto-assign main camera
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // New Input System callback
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // ---------- COYOTE TIME & JUMP BUFFER ----------
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        jumpBufferTimer -= Time.deltaTime;

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // remember jump press
            jumpBufferTimer = jumpBufferTime;
        }

        // perform jump if allowed
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            // reset vertical so jumps always same strength
            Vector3 vel = rb.linearVelocity;
            vel.y = 0f;
            rb.linearVelocity = vel;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            // clear timers so we don't double jump
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // ---------- TELEPORT ----------
        Vector3 moveDir = GetCameraRelativeDirection();
        if (Keyboard.current != null &&
            Keyboard.current.leftShiftKey.wasPressedThisFrame &&
            moveDir.sqrMagnitude > 0.01f)
        {
            Vector3 teleportDirection = moveDir.normalized;
            transform.position += teleportDirection * teleportDistance;

            // optional little push after teleport
            rb.linearVelocity = new Vector3(
                teleportDirection.x * speed,
                rb.linearVelocity.y,
                teleportDirection.z * speed
            );
        }
    }

    void FixedUpdate()
    {
        if (cameraTransform == null) return;

        // ---------- MOVEMENT USING VELOCITY ----------
        Vector3 moveDir = GetCameraRelativeDirection();

        // desired horizontal velocity
        Vector3 vel = rb.linearVelocity;
        Vector3 horizontal = moveDir * speed;

        vel.x = horizontal.x;
        vel.z = horizontal.z;

        rb.linearVelocity = vel;
    }

    // Camera-relative WASD direction on XZ plane
    Vector3 GetCameraRelativeDirection()
    {
        if (cameraTransform == null) return Vector3.zero;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = camRight * movementX + camForward * movementY;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        return move;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check all contacts to detect "floor" collisions
        foreach (var contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }
}
