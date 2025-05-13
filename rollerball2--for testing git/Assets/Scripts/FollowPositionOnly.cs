using UnityEngine;

public class FollowPositionOnly : MonoBehaviour
{
    public Transform ball;           // Drag your ball GameObject here
    public Vector3 offset;           // Optional: position offset relative to the ball
    

    void LateUpdate()
    {
        // Follow the ball's position with optional offset
        transform.position = ball.position + offset;

        // Lock rotation to fixed values (prevents rolling/spinning with ball)
        //transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
