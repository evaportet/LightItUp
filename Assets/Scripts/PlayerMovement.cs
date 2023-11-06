using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float JumpHeight = 1.0f;
    Vector3 playerVelocity;
    bool isGrounded;

    CharacterController charC;
    Camera cam;

    bool isAlive = true;
    bool isLit = true;
    [SerializeField] float maxDuration = 10f;
    public float meltModifier = 1f;
    float duration;

    void Start()
    {
        charC = GetComponent<CharacterController>();
        cam = Camera.main;
        duration = maxDuration;
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
        movementDirection.x *= movementSpeed;
        movementDirection.z *= movementSpeed;


        if (movementDirection != Vector3.zero)
        {
            gameObject.transform.forward = movementDirection;
        }


        //Jump
        isGrounded = charC.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -.5f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("SPace");
            Debug.Log(charC.isGrounded);
            if (isGrounded)
            {
                playerVelocity.y += Mathf.Sqrt(JumpHeight * -3.0F * Physics.gravity.y);
            }
        }

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        movementDirection.y = playerVelocity.y;
        charC.Move(movementDirection * Time.deltaTime);
        #endregion

        #region CANDLE MELT

        if (isLit)
        {
            duration -=Time.deltaTime*meltModifier;
        }
        if (duration <= .0f)
        {
            isAlive = false;
            isLit = false;
        }
        if (this.gameObject.transform.localScale.y >= .0f)
        {
            Vector3 scaleChange = new Vector3(1f,duration /maxDuration, 1f);
            this.gameObject.transform.localScale = scaleChange;
        }

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
