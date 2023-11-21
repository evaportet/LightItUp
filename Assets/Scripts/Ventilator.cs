using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventilator : MonoBehaviour
{
    public float rotationSpeed = 30f;

    void Update()
    {
        float newRotation = transform.rotation.eulerAngles.y + rotationSpeed * Time.deltaTime;

        if (newRotation > 30f)
        {
            newRotation = 30f;
            rotationSpeed *= -1; 
        }

        else if (newRotation < -30f)
        {
            newRotation = -30f;
            rotationSpeed *= -1; 
        }

        transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
    }
}
