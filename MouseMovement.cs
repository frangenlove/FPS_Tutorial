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
        //�������
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //��ȡ�������
        float MouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        //��X����ת�����¿���
        xRotation -= MouseY;

        //�����ӽ�ת���Ƕ�
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //��Y����ת�����ҿ���
        yRotation += MouseX;

        //������Transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
