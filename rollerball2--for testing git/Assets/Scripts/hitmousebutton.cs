using UnityEngine;
using UnityEngine.UIElements;

public class hitmousebutton : MonoBehaviour
{
    Animator animator;
    //public GameObject gameobject;       
    public GameObject bladelength;      // The part that stretches
    public float growthRate = 1f;       // How fast the blade grows per second
    public float resetDelay = 1.0f;     // Time in seconds before scale resets
    private bool isGrowing = false;
    private float resetTimer = 0f;

    void Start()
    {
        //gameobject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isGrowing = true;
            resetTimer = 0f; // cancel any pending reset
        }

        if (Input.GetMouseButtonUp(0))
        {
            isGrowing = false;
            resetTimer = resetDelay; // start the reset countdown
            //gameobject.SetActive(true);
            animator.SetTrigger("PlaySwing");
        }

        if (isGrowing)
        {
            Vector3 scale = bladelength.transform.localScale;
            if (scale.z < 140)
            {
                scale.z += growthRate * Time.deltaTime;
            }
            
            
            
            
            bladelength.transform.localScale = scale;
            
        }
        else if (resetTimer > 0f)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0f)
            {
                bladelength.transform.localScale = new Vector3(10, 10, 10); // reset blade
            }
        }
    }
}
