using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wax : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float maxDuration = 3f;
    float duration;
    public bool destroyable;
    void Start()
    {
        duration=maxDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyable)
        {
            if (duration <= 0)
                Destroy(this.gameObject);

            this.gameObject.GetComponent<MeshRenderer>().material.color =
                new Color(this.gameObject.GetComponent<MeshRenderer>().material.color.r,
                this.gameObject.GetComponent<MeshRenderer>().material.color.g,
                this.gameObject.GetComponent<MeshRenderer>().material.color.b,
                duration / maxDuration);

            duration -= Time.deltaTime;
        }
    }
}
