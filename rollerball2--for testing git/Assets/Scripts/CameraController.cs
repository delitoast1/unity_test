using UnityEngine;
using System.Collections;

public class CameraBehaviourScript : MonoBehaviour
{
    public GameObject player; //母球
    private Vector3 offset; //攝影機 離 母球 相對位置
    public float speed = 12.0f; //母球 初速
    public float forceSpd = 9.0f; //母球 蓄力 速度
    private float force = 0.0f; //母球 已經蓄了多少力 的大小
    public float distance = 6.0f; //攝影機 離 母球 距離 初始值
    public float xSpeed = 120.0f; //滑鼠左右移動速度
    public float ySpeed = 120.0f; //滑鼠上下移動速度
    public float yMinLimit = -20f; //滑鼠上下 轉仰角 下限
    public float yMaxLimit = 80f; //滑鼠上下 轉仰角 上限
    public float distanceMin = .5f; //滾輪 拉 攝影機 離 母球 距離下限
    public float distanceMax = 15f; //滾輪 拉 攝影機 離 母球 距離上限
    public float minlerp= 15f; 
    public float maxlerp = 15f; 
                                    //public GameObject sword;

    private Rigidbody rbody;
    float x = 0.0f;
    float y = 0.0f;
    // Use this for initialization
    void Start()
    {
        //攝影機位置 - 母球位置 = 相對位置
        offset = transform.position - player.transform.position;
        Vector3 angles = transform.eulerAngles; //攝影機角度
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
    { // 用上下限 夾值
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }


}