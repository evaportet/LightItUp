using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatHazard : MonoBehaviour
{
    [SerializeField] GameObject heatSource;
    Vector3 srcTransform;
    void Start()
    {
        srcTransform = heatSource.transform.position;
    }

    public float DistFromSource(Vector3 obj)
    {
        return Vector3.Distance(obj, srcTransform);
    }
}
