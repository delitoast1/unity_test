using UnityEngine;

public class FollowPositionOnly : MonoBehaviour
{
    public Transform player;          // Reference to the player object
    public Transform cameraTransform; // Reference to the camera
    public Vector3 positionOffset = new Vector3(0f, 0f, 0f); // tweak this
    public Vector3 rotationOffset = new Vector3(0, 0, 0);          // optional fixed rotation

    void LateUpdate()
    {
        // Match the rotation of the camera (or the rotation you used in your Camera script)
        Quaternion camRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = camRotation * Quaternion.Euler(rotationOffset);

        // Stick to the player's position with a relative offset (based on facing direction)
        transform.position = player.position + camRotation * positionOffset;
    }
}
