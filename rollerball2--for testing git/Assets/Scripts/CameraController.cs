using UnityEngine;
using System.Collections;

public class CameraBehaviourScript : MonoBehaviour
{
    public GameObject player; //���y
    private Vector3 offset; //��v�� �� ���y �۹��m
    public float speed = 12.0f; //���y ��t
    public float forceSpd = 9.0f; //���y �W�O �t��
    private float force = 0.0f; //���y �w�g�W�F�h�֤O ���j�p
    public float distance = 6.0f; //��v�� �� ���y �Z�� ��l��
    public float xSpeed = 120.0f; //�ƹ����k���ʳt��
    public float ySpeed = 120.0f; //�ƹ��W�U���ʳt��
    public float yMinLimit = -20f; //�ƹ��W�U ����� �U��
    public float yMaxLimit = 80f; //�ƹ��W�U ����� �W��
    public float distanceMin = .5f; //�u�� �� ��v�� �� ���y �Z���U��
    public float distanceMax = 15f; //�u�� �� ��v�� �� ���y �Z���W��
    public float minlerp= 15f; 
    public float maxlerp = 15f; 
                                    //public GameObject sword;

    private Rigidbody rbody;
    float x = 0.0f;
    float y = 0.0f;
    // Use this for initialization
    void Start()
    {
        //��v����m - ���y��m = �۹��m
        offset = transform.position - player.transform.position;
        Vector3 angles = transform.eulerAngles; //��v������
        x = angles.y;
        y = angles.x;
        rbody = player.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        // Update distance based on scroll input first
        distance = Mathf.Clamp(
            distance - Input.GetAxis("Mouse ScrollWheel") * 5,
            distanceMin, distanceMax);

        // Increase sensitivity when zoomed in (closer = faster sensitivity)
        float zoomFactor = Mathf.Lerp(minlerp, maxlerp, (distance - distanceMin) / (distanceMax - distanceMin));
        float dynamicXSpeed = xSpeed * zoomFactor;
        float dynamicYSpeed = ySpeed * zoomFactor;

        // Update rotation input
        x += Input.GetAxis("Mouse X") * dynamicXSpeed * distance * 0.02f;
        y -= Input.GetAxis("Mouse Y") * dynamicYSpeed * 0.02f;
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // Apply camera transformation
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        offset = rotation * negDistance;
        transform.rotation = rotation;
        transform.position = player.transform.position + offset;
    }
    public static float ClampAngle(float angle, float min, float max)
    { // �ΤW�U�� ����
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }


}