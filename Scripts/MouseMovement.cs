using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        //隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //获取鼠标输入
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        //绕X轴旋转（上下看）
        xRotation -= MouseY;

        //限制视角转动角度
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //绕Y轴旋转（左右看）
        yRotation += MouseX;

        //运用于Transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
