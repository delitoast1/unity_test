using UnityEngine;

public class Setrotation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform ball;           // Drag your ball GameObject here
    public Vector3 fixedRotation = new Vector3(0, 90, 0);
    public Transform cameraTransform;

    void Start()
    {
        // Follow the ball's position with optional offset
        ball.rotation = Quaternion.Euler(fixedRotation);
        
        // Lock rotation to fixed values (prevents rolling/spinning with ball)
        //transform.rotation = Quaternion.Euler(fixedRotation);
    }
    void Update()
    {
        
    }


}
