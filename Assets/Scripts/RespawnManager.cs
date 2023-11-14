using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RespawnManager : MonoBehaviour
{
    public Vector3 lastRespawnPoint;
    Vector3 initialPosition;
    public static RespawnManager instance { get; private set; }

    public GameObject player;
    PlayerMovement pMov;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    void Start()
    {
        pMov = player.GetComponent<PlayerMovement>();
        initialPosition = player.transform.position;
        lastRespawnPoint = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (pMov.isAlive)
        {

        }
        if (Input.GetButtonDown("ResetCheat"))
        {
            RespawnPlayer();
        }
    }

    public static RespawnManager GetInstance()
    { 
        return instance;
    }

    public void SetNewRespawnPoint(Collider col)
    {
        lastRespawnPoint= col.transform.position;
        Debug.Log("respawn set");
    }

 public void RespawnPlayer()
    {
        player.gameObject.SetActive(true);
        lastRespawnPoint.y += 1;
        pMov.Respawn(lastRespawnPoint);
    }
}
