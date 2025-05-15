using UnityEngine;

public class FollowUpdate : MonoBehaviour
{
    public Transform ball;           // Drag your ball GameObject here
                                     //public Vector3 offset;           // Optional: position offset relative to the ball
                                     //public Vector3 offset2;           // Optional: position offset relative to the ball
                                     //public Vector3 fixedRotationblade=new Vector3(90,0, 0);           // Optional: position offset relative to the ball
                                     //public Vector3 fixedRotationhilt=new Vector3(90,0, 0);           // Optional: position offset relative to the ball

    public Transform blade;
    public Transform hilt;
    public Vector3 offset = new Vector3(0, 0, -0.5f);

    private void Start()
    {
        //this.rotation = Quaternion.Euler(fixedRotation);

    }

    void LateUpdate()
    {
        // Follow the ball's position with optional offset
        transform.position = ball.position+offset;

        ////blade.rotation = Quaternion.Euler(fixedRotation);
        //// Lock rotation to fixed values (prevents rolling/spinning with ball)
        ////transform.rotation = Quaternion.Euler(fixedRotation);
        //blade.rotation = Quaternion.Euler(fixedRotationblade);
        //hilt.rotation = Quaternion.Euler(fixedRotationhilt);

        //hilt.position = ball.position + offset2;
    }
}
