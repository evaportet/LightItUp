using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    //CharacterController charC;
    //Camera cam;

    [SerializeField] GameObject respawnCanvas;
    //[Header("Movement Settings")]
    //[SerializeField] float movementSpeed = 2.0f;
    //[SerializeField] float JumpHeight = 1.0f;
    //Vector3 playerVelocity;
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
    [SerializeField] float dropInterval = 5f;
    float nextDrop;
    public bool isAlive { get; private set; } = true;
    public bool isLit = true;
    public float meltModifier = 1f;
    public GameObject waxDrop;
    [SerializeField] GameObject fire;
    GameObject heat;
    float distFromSrc;
    bool inFire;
    GameObject currentWaxTrail;
    float waxBaseHeight = 1.8372e-02f;
    [SerializeField] float waxGrowthModifier;
    Vector3 prevPos;
    bool waxDropPressed = false;


    void Start()
    {
        //charC = GetComponent<CharacterController>();
        //cam = Camera.main;
        duration = maxDuration;
        nextDrop = maxDuration - dropInterval;
        Cursor.visible = false;
    }


    void Update()
    {
        //float[] inputs = GetInputs();

        #region MOVEMENT
        //Movement
        //Vector3 forward = cam.transform.forward;
        //forward.y = .0f;//we don't need y value
        //forward.Normalize();
        //Vector3 right = cam.transform.right;
        //right.y = .0f;
        //right.Normalize();

        //Vector3 movementDirection = forward * inputs[1] + right * inputs[0];
        //movementDirection.x *= movementSpeed;
        //movementDirection.z *= movementSpeed;


        //if (movementDirection != Vector3.zero)
        //{
        //    gameObject.transform.forward = movementDirection;
        //}

        //Jump
        //isGrounded = charC.isGrounded;

        //if (isGrounded && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = -.5f;
        //    hasJumped = false;
        //}

        //if (Input.GetButtonDown("Jump"))
        //{
        //    if (isGrounded)
        //    {
        //        playerVelocity.y = Jump(playerVelocity.y);
        //    }
        //    else
        //    {
        //        hasClicked = true;
        //        buffer = inputBuffer;
        //    }

        //}

        //Input Buffer Logic
        //if (hasClicked && buffer > .0f)
        //{
        //    if (isGrounded && !hasJumped)
        //    {
        //        playerVelocity.y = Jump(playerVelocity.y);
        //        hasClicked = false;
        //        buffer = .0f;
        //    }
        //    else
        //        buffer -= Time.deltaTime;
        //}

        //CoyoteTime Logic
        //if (wasGrounded && !isGrounded)
        //{
        //    inCoyote = true;
        //    coyoteTimer = coyoteTimeDuration;
        //}
        //if (inCoyote && !hasJumped)
        //{
        //    if (Input.GetButtonDown("Jump") && coyoteTimer > .0f)
        //    {
        //        playerVelocity.y = Jump(playerVelocity.y);
        //        inCoyote = false;
        //        coyoteTimer = .0f;
        //    }
        //    else
        //    {
        //        coyoteTimer -= Time.deltaTime;
        //    }
        //}

        //playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        //movementDirection.y = playerVelocity.y;
        //charC.Move(movementDirection * Time.deltaTime);
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
            Die();
        }

        if (heat != null)
        {
            distFromSrc = heat.GetComponent<HeatHazard>().DistFromSource(this.gameObject.transform.position);
            if (distFromSrc < 10f)
            {
                meltModifier = ((4 * 1) / distFromSrc) + 1;
            }
        }
        if (inFire)
        {
            meltModifier = 20f;
        }
        //if(prevPos == this.gameObject.transform.position)
        if (Input.GetButtonDown("WaxDrop"))
            waxDropPressed = true;
        if (Input.GetButtonUp("WaxDrop"))
            waxDropPressed = false;
        if (waxDropPressed && isLit)
        {
            if (currentWaxTrail == null)
            {
                Drop();
            }
            else
            {
                currentWaxTrail.transform.localScale += new Vector3(0f, Time.deltaTime * meltModifier * waxBaseHeight * waxGrowthModifier, 0f);
            }
        }

        #endregion
        prevPos = this.gameObject.transform.position;
        wasGrounded = isGrounded;

    }

    //float[] GetInputs() //[0]x axis, [1] z axis
    //{
    //    float[] inputs = new float[2];

    //    inputs[0] = Input.GetAxis("Horizontal");
    //    inputs[1] = Input.GetAxis("Vertical");

    //    return inputs;
    //}

    //float Jump(float verticalVelocity)
    //{
    //    hasJumped = true;
    //    return verticalVelocity + Mathf.Sqrt(JumpHeight * -3.0F * Physics.gravity.y);
    //}

    public void Respawn(Vector3 respawnPos)
    {
        Cursor.visible = false;
        duration = maxDuration;
        isAlive = true;
        isLit = true;
        //charC.Move(respawnPos - this.gameObject.transform.position);
        respawnCanvas.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RespawnPoint"))
        {
            RespawnManager.GetInstance().SetNewRespawnPoint(other);
        }
        else if (other.gameObject.CompareTag("AirHazard"))
        {
            Extinguish();
        }
        else if (other.gameObject.CompareTag("NPCandle"))
        {
            if (!isLit && other.gameObject.GetComponent<NPCandles>().isLit)
            {
                LightUp();
            }
            else if (isLit && !other.gameObject.GetComponent<NPCandles>().isLit)
            {
                other.gameObject.GetComponent<NPCandles>().LightUp();
            }
        }
        else if (other.gameObject.CompareTag("Fire"))
            Die();
        else if (other.gameObject.CompareTag("Heat"))
        {
            heat = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Heat"))
        {
            heat = null;
            meltModifier = 1f;
        }
        else if (other.gameObject.CompareTag("waxTrail"))
        {
            currentWaxTrail = null;
        }
    }

    void Drop()
    {
        waxDrop.transform.localScale = new Vector3(.3f, (Time.deltaTime * meltModifier) / maxDuration, .3f);
        currentWaxTrail = Instantiate(waxDrop, this.gameObject.transform.position, waxDrop.transform.rotation);
    }

    public void LightUp()
    {
        isLit = true;
        fire.SetActive(true);
    }

    private void Extinguish()
    {
        isLit = false;
        fire.SetActive(false);
    }

    private void Die()
    {
        Cursor.visible = true;
        isAlive = false;
        isLit = false;
        respawnCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
