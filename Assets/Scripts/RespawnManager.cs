using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RespawnManager : MonoBehaviour
{
    Vector3 lastRespawnPoint;
    Vector3 initialPosition;

    PlayerMovement player;
    void Start()
    {
        initialPosition = player.transform.position;
        lastRespawnPoint = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
