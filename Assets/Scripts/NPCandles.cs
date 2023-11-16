using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCandles : MonoBehaviour
{
    [SerializeField] bool isLit;
    CharacterController charC;
    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.GetComponent<PlayerMovement>().isLit && !isLit)
            {
                isLit = true;
            }
            if(!collision.gameObject.GetComponent<PlayerMovement>().isLit && isLit)
            {
                collision.gameObject.GetComponent<PlayerMovement>().isLit = true;
            }
        }
    }

    public void LightUp()
    {
        isLit = true;
        GameObject fireObject = GameObject.FindWithTag("Fire");
        fireObject.SetActive(true);
    }

    public void Extinguish()
    {
        isLit = false;
        GameObject fireObject = GameObject.FindWithTag("Fire");
        fireObject.SetActive(false);
    }
}
