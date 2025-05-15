using UnityEngine;

public class hitmousebutton : MonoBehaviour
{
    Animator animator;
    public GameObject gameobject;       // Presumably the blade
    public GameObject bladelength;      // The part that stretches
    public float growthRate = 1f;       // How fast the blade grows per second
    private bool isGrowing = false;

    void Start()
    {
        gameobject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Start growing blade
        {
            isGrowing = true;
        }

        if (Input.GetMouseButtonUp(0)) // Stop growing, activate blade, play animation
        {
            isGrowing = false;
            gameobject.SetActive(true);
            animator.SetTrigger("PlaySwing");
           
        }

        if (isGrowing)
        {
            // Increase the localScale of the blade in the Y direction (or adjust as needed)
            Vector3 scale = bladelength.transform.localScale;
            scale.z += growthRate * Time.deltaTime;
            bladelength.transform.localScale = scale;
        }
        
    }
}
