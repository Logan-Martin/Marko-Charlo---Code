using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject playerCamera;

    public float walkSpeed = 12f;
    public float runSpeed = 20f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float bobbingAmount = 0.1f;
    public bool normalizeMovement = true;
    public AudioSource footsteps;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    float initCamY;
    float timer = 0;
    float camY;
    bool bobbingUp = true;

    void Start()
    {
        initCamY = playerCamera.transform.localPosition.y;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (normalizeMovement) move = Vector3.Normalize(move);

        float speed = walkSpeed;
        //if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        //{
        //    speed = runSpeed;
        //}

        controller.Move(move * speed * Time.deltaTime);

        // Bobbing
        float camX = playerCamera.transform.localPosition.x;
        float camZ = playerCamera.transform.localPosition.z;
        float previousCamY = camY;
        if (move == Vector3.zero)
        {
            timer = 0;
            camY = Mathf.Lerp(playerCamera.transform.localPosition.y, initCamY, Time.deltaTime * speed);
        }
        else
        {
            timer += Time.deltaTime * speed;
            camY = initCamY + Mathf.Sin(timer) * bobbingAmount;
        }
        playerCamera.transform.localPosition = new Vector3(camX, camY, camZ);

        // Footsteps SFX
        if (isGrounded && move != Vector3.zero)
        {
            if (camY > previousCamY)
            {
                if (!bobbingUp)
                {
                    footsteps.Play(0);
                }
                bobbingUp = true;
            }
            else
            {
                bobbingUp = false;
            }
        }

        // Jumping
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        //}

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
