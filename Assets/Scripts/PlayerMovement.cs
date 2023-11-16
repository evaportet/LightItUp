using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charC;
    Camera cam;

    [SerializeField] GameObject respawnCanvas;
    [Header("Movement Settings")]
    [SerializeField] float movementSpeed = 2.0f;
    [SerializeField] float JumpHeight = 1.0f;
    Vector3 playerVelocity;
    bool isGrounded;
    bool wasGrounded;
    bool hasJumped;
    bool hasClicked = false;
    
    //InputBuffer
    [SerializeField] float inputBuffer = 1f; //this variable will only be total time of inputBuffer
    float buffer;
    
    //CoyoteTime
    [SerializeField] float coyoteTimeDuration = .5f; //this variable will be the total time of coyote
    float coyoteTimer;
    bool inCoyote = false;

    [Header("Melting Settings")]
    [SerializeField] float maxDuration = 10f;
    [SerializeField] float duration;
    public bool isAlive { private set; get; } = true;
    public bool isLit { set; private get; } = true;
    public float meltModifier = 1f;


    void Start()
    {
        charC = GetComponent<CharacterController>();
        cam = Camera.main;
        duration = maxDuration;
        Cursor.visible = false;
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
            hasJumped = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                playerVelocity.y = Jump(playerVelocity.y);
            }
            else
            {
                hasClicked = true;
                buffer = inputBuffer;
            }
        }

        //Input Buffer Logic
        if (hasClicked && buffer > .0f)
        {
            if (isGrounded && !hasJumped)
            {
                playerVelocity.y = Jump(playerVelocity.y);
                hasClicked = false;
                buffer = .0f;
            }
            else
                buffer -= Time.deltaTime;
        }

        //CoyoteTime Logic
        if (wasGrounded && !isGrounded)
        {
            inCoyote = true;
            coyoteTimer = coyoteTimeDuration;
        }
        if (inCoyote && !hasJumped)
        {
            if (Input.GetButtonDown("Jump") && coyoteTimer > .0f)
            {
                playerVelocity.y = Jump(playerVelocity.y);
                inCoyote = false;
                coyoteTimer = .0f;
            }
            else
            {
                coyoteTimer -= Time.deltaTime;
            }
        }

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        movementDirection.y = playerVelocity.y;
        charC.Move(movementDirection * Time.deltaTime);
        wasGrounded = isGrounded;
        #endregion

        #region CANDLE MELT

        if (isLit)
        {
            duration -= Time.deltaTime * meltModifier;
            Vector3 scaleChange = new Vector3(1f, duration / maxDuration, 1f);
            this.gameObject.transform.localScale = scaleChange * 1.75f;
        }
        if (duration <= .0f)
        {
            isAlive = false;
            isLit = false;
            respawnCanvas.SetActive(true);
            this.gameObject.SetActive(false);
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

    float Jump(float verticalVelocity)
    {
        hasJumped = true;
        return verticalVelocity + Mathf.Sqrt(JumpHeight * -3.0F * Physics.gravity.y);
    }

    public void Respawn(Vector3 respawnPos)
    {
        duration = maxDuration;
        isAlive = true;
        isLit = true;
        charC.Move(respawnPos-this.gameObject.transform.position);
        respawnCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RespawnPoint"))
        {
            RespawnManager.GetInstance().SetNewRespawnPoint(other);
        }

        if (other.gameObject.CompareTag("AirHazard"))
        {
            isLit = false;

            GameObject fireObject = GameObject.FindWithTag("Fire");
            fireObject.SetActive(false);
        }
    } 
}
