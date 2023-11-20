using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    // Speed of rotation in degrees per second
    public float rotationSpeed = 30f;

    void Update()
    {
        // Rotate the object continuously around its up axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
