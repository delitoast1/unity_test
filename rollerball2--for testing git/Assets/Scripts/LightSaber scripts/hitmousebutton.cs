using UnityEngine;
using UnityEngine.UIElements;

public class hitmousebutton : MonoBehaviour
{
    Animator animator;
    public GameObject bladelength;         // The part that stretches
    public float growthRate = 1f;          // Growth per second
    public float resetDelay = 1.0f;        // Time before blade resets
    public float maxLength = 10f;

    private CapsuleCollider bladeCollider;
    private Vector3 initialScale;
    private float initialHeight;
    private Vector3 initialCenter;

    private bool isGrowing = false;
    private float resetTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        bladeCollider = bladelength.GetComponent<CapsuleCollider>();

        // Ensure it grows along the Z axis
        bladeCollider.direction = 2;

        // Store initial values
        initialScale = bladelength.transform.localScale;
        initialHeight = bladeCollider.height;
        initialCenter = bladeCollider.center;
    }

    void Update()
    {
        // Start growing on mouse down
        if (Input.GetMouseButtonDown(0))
        {
            isGrowing = true;
            resetTimer = 0f;
        }

        // Stop growing on mouse up
        if (Input.GetMouseButtonUp(0))
        {
            isGrowing = false;
            resetTimer = resetDelay;
            animator.SetTrigger("PlaySwing");
        }

        if (isGrowing)
        {
            Vector3 scale = bladelength.transform.localScale;

            // Grow only if under max length
            if (scale.z < maxLength)
            {
                scale.z += growthRate * Time.deltaTime;  // Framerate-independent
                bladelength.transform.localScale = scale;

                float scaleFactor = scale.z / initialScale.z;

                // Adjust collider height and center
                //bladeCollider.height = initialHeight * scaleFactor;
                //bladeCollider.center = new Vector3(
                //    initialCenter.x,
                //    initialCenter.y,
                //    initialCenter.z + (bladeCollider.height - initialHeight) / 2f
                //);
            }
        }
        else if (resetTimer > 0f)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
            {
                // Reset to initial size
                bladelength.transform.localScale = initialScale;
                bladeCollider.height = initialHeight;
                bladeCollider.center = initialCenter;
            }
        }
    }
}
