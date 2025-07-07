using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public float speed = 0;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private bool isGrounded = true;
    public float jumpForce = 5f;
    public float teleportDistance = 10f;
    [SerializeField] private float StartingHealth;
    private float health;
   [SerializeField] private GameObject outerDeflectLayer;
    public Collider deflectCollider; // Assign this in the Inspector
    public Transform deflectZoneTransform; // Assign your DeflectZone object here
    public float distanceInFront = 1.5f;   // How far in front of the player
    public float Health
    {
        get
        {
            return health;
        }
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = StartingHealth;
        rb = GetComponent<Rigidbody>();
    }

    
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        

        // Face forward in camera direction (flat)
        // Calculate position in front of the camera's forward direction, relative to the player
        Vector3 forward = cameraTransform.forward;
        //forward.y = 0f; // Optional: flatten the Y to keep deflect zone level

        deflectZoneTransform.position = transform.position + forward.normalized * distanceInFront;

        // Optional: make deflect zone face same direction as camera
        deflectZoneTransform.rotation = Quaternion.LookRotation(forward);

        //player movement
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // WASD movement vector relative to camera
        Vector3 movement = camRight * movementX + camForward * movementY;

        rb.AddForce(movement * speed);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Teleport in WASD direction (if moving)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (movement.sqrMagnitude > 0.01f)
            {
                Vector3 teleportDirection = movement.normalized;
                transform.position += teleportDirection * teleportDistance;
                rb.AddForce(teleportDirection * speed);
            }
        }
        if (Input.GetMouseButton(1))
        {
            deflectCollider.enabled = true;
        }
        else
        {
            deflectCollider.enabled = false;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) // Hit from below
        {
            isGrounded = true;
        }
    }

}
