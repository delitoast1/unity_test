using UnityEngine;


public class FollowUpdate : MonoBehaviour
{
    public Transform ball;           // Drag your ball GameObject here
    

    void LateUpdate()
    {
        // Follow the ball's position with optional offset
        transform.position = ball.position;

        // Lock rotation to fixed values (prevents rolling/spinning with ball)
        //transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
