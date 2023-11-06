using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float JumpHeight = 1.0f;
    Vector3 playerVelocity;
    bool isGrounded;

    CharacterController charC;
    Camera cam;

    void Start()
    {
        charC = GetComponent<CharacterController>();
        cam = Camera.main;
    }


    void Update()
    {
        float[] inputs = GetInputs();

        #region MOVEMENT
        //Movement
        Vector3 forward = cam.transform.forward;
        forward.y = .0f;//we don't need y value
        forward.Normalize();
        Vector3 right = cam.transform.right;
        right.y = .0f;
        right.Normalize();

        Vector3 movementDirection = forward * inputs[1] + right * inputs[0];
        charC.Move(movementDirection * Time.deltaTime * movementSpeed);

        if (movementDirection != Vector3.zero)
        {
            gameObject.transform.forward = movementDirection;
        }


        //Jump
        isGrounded = charC.isGrounded;
        if (isGrounded&&playerVelocity.y<0)
        {
            playerVelocity.y = -.5f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("SPace");
            Debug.Log(charC.isGrounded);
            if (isGrounded)
            { 
                playerVelocity.y += Mathf.Sqrt(JumpHeight *-3.0F* Physics.gravity.y);
            }
        }

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        charC.Move(playerVelocity * Time.deltaTime);

        #endregion
    }

    float[] GetInputs() //[0]x axis, [1] z axis
    {
        float[] inputs = new float[2];

        inputs[0] = Input.GetAxis("Horizontal");
        inputs[1] = Input.GetAxis("Vertical");

        return inputs;
    }
}
