using UnityEngine;

public class LockRotation:MonoBehaviour
{
    public Vector3 lockedRotationEuler = new Vector3(0, 0, 0); // Set your desired rotation

    void LateUpdate()
    {
        // Keep position from parent (Ball), but override rotation
        transform.rotation = Quaternion.Euler(lockedRotationEuler);
    }
}
