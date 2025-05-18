using UnityEngine;

public class FollowUpdate : MonoBehaviour
{
    public Transform ball;           // Drag your ball GameObject here
    
    public Vector3 offset = new Vector3(0, 0, -0.5f);

    private void Start()
    {
        //this.rotation = Quaternion.Euler(fixedRotation);

    }

    void LateUpdate()
    {
        // Follow the ball's position with optional offset
        transform.position = ball.position+offset;
        

        
    }
}
