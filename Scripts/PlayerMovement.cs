using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    

    public float speed = 12;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        //地面检测
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //重置速度
        if (isGrounded && velocity.y < 0)
            velocity.y -= 2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move=transform.right * x + transform.forward * z;//（左右-红轴，前后=蓝轴）
        //Vector3 move = Camera.transform.right * x + Camera.transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //检测是否可以跳跃
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            //跳起来时的速度
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //下坠时的速度
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && lastPosition != gameObject.transform.position)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
