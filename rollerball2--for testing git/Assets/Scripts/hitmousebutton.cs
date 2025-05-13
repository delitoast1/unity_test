using UnityEngine;

public class hitmousebutton : MonoBehaviour
{
    Animator animator;
    public GameObject gameobject;

    void Start()
    {
        gameobject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = Left Mouse Button
        {
            gameobject.SetActive(true);
            animator.SetTrigger("PlaySwing");
        }
        if (Input.GetMouseButtonUp(0))
        {
            gameobject.SetActive(false);
        }
        
    }
}
