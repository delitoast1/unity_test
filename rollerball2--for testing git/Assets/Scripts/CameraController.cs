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
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit); // ���� ���� �ɥ��d��
                                                     //¶ Y �b �O ¶�y��A¶ X �b �O �ɥ�
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.Clamp( // ���� �u�� �� ���� ���ʽd��
            distance - Input.GetAxis("Mouse ScrollWheel") * 5,
            distanceMin, distanceMax);
            // (�u Z �b �e�Ჾ�ʡ^

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            offset = rotation * negDistance; //�̷s���סA�Z�� ���s��۹��m
            transform.rotation = rotation; // ��v�� �s����
                                           //sword.transform.rotation = rotation;

        }
        // ��v���s��m = �s�۹��m + ���y��m
        transform.position = player.transform.position+offset;
        /* if (Input.GetButton("W")) // ���ƹ��k�� ���� �W�O
         {
             force += Time.deltaTime * forceSpd; // �j�p�M�ɶ�������
         }
         else if (Input.GetMouseButtonUp(1)) // ���ƹ��k�� ��} �o�g
         {
             //�����ݪ���V the direction of camera(eye)�G
             // Camera.main.transform.forward
             Vector3 movement = Camera.main.transform.forward;
             movement.y = 0.0f; // no vertical movement ���W�U����
                                //�O�q�Ҧ� impulse:�ĤO�Aspeed�G��t�j�p
             rbody.AddForce(movement * speed * force, ForceMode.Impulse);
             force = 0.0f; // �O�q�κ��k 0�A�ǳƤU�����s�W�O
         }*/
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