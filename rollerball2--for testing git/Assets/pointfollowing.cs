using UnityEngine;

public class pointfollowing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform ball;           // Drag your ball GameObject here
                                     //public Vector3 offset;           // Optional: position offset relative to the ball
                                     //public Vector3 offset2;           // Optional: position offset relative to the ball
                                     //public Vector3 fixedRotationblade=new Vector3(90,0, 0);           // Optional: position offset relative to the ball
                                     //public Vector3 fixedRotationhilt=new Vector3(90,0, 0);           // Optional: position offset relative to the ball

    [SerializeField]
    private float leftOffset = 0.3f;
    [SerializeField]
    private float forwardOffset = 0.5f;
    [SerializeField]
    private float upOffset = 0.0f;

    void LateUpdate()
    {
        // Use camera's Y rotation to match its horizontal orbit
        float cameraYaw = Camera.main.transform.eulerAngles.y;
        Quaternion cameraFlatRotation = Quaternion.Euler(0f, cameraYaw, 0f);

        // Calculate offset using camera rotation, not ball's
        Vector3 relativeOffset =
            cameraFlatRotation * (-Vector3.right * leftOffset) +
            cameraFlatRotation * Vector3.forward * forwardOffset +
            Vector3.up * upOffset;

        // Final position
        transform.position = ball.position + relativeOffset;
    }


}
