using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5f;
    float newRotation;
    [SerializeField] float minRotation=-35f;
    [SerializeField] float maxRotation=30;

    void Update()
    {
       newRotation= minRotation+Mathf.PingPong(Time.time*rotationSpeed, maxRotation-minRotation);
       transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
    }
}
